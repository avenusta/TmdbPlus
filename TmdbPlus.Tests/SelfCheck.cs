using System.Text.Json;
using TmdbPlus;
using TmdbPlus.Auth;
using TmdbPlus.Json;
using TmdbPlus.Models;

// Self-check against the real fixture corpus in audit/fixtures. Asserts the load-bearing
// behaviour: append blocks bind flat, TMDB's "" dates do not throw, unknown enum values degrade
// instead of blowing up, and `rated` reads through both of its wire shapes.

static class SelfCheck
{
    static readonly string Fixtures =
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "audit", "fixtures");

    static JsonSerializerOptions Json => TmdbClient.Json;

    static T Load<T>(string file)
        => JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(Fixtures, file)), Json)!;

    static async Task Main()
    {
        MovieDetailsBindsCoreFields();
        AppendBlocksBindFlat();
        WatchProvidersKeyBinds();
        EmptyDateDoesNotThrow();
        UnknownEnumDegrades();
        NumericEnumRejectsOutOfRange();
        RatedIsPolymorphic();
        QueryStringSkipsAbsentValues();
        AppendFlagsProduceWireNames();
        SessionsCarryTheRightParameter();

        await LiveCheck.RunAsync(Check);

        Console.WriteLine("\nAll checks passed.");
    }

    /// <summary>Debug.Assert compiles out in Release; this never does.</summary>
    static void Check(bool condition, string because)
    {
        if (!condition) throw new Exception("FAILED: " + because);
    }

    static void MovieDetailsBindsCoreFields()
    {
        var m = Load<MovieDetails>("append_movie-details_credits.json");
        Check(m.Id > 0, "id should bind");
        Check(!string.IsNullOrEmpty(m.Title), "title should bind");
        // The undocumented field the generated DTOs drop entirely.
        Console.WriteLine($"  details:   #{m.Id} {m.Title} status={m.Status} softcore={m.Softcore}");
    }

    static void AppendBlocksBindFlat()
    {
        // The whole point of the flat layout: an appended block lands on the detail type itself.
        var m = Load<MovieDetails>("append_movie-details_credits.json");
        Check(m.Credits is not null, "credits append must bind flat onto MovieDetails");
        Check(m.Credits!.Cast is { Count: > 0 }, "cast should be populated");

        var crew = m.Credits.Crew!;
        var mapped = crew.Count(c => c.Department.IsKnown);
        Check(mapped > 0, "some departments should map");
        // Raw text survives even when the value is unmapped -- the TmdbEnum<T> contract.
        Check(crew.All(c => c.Department.Raw is not null), "raw wire text must be kept");
        Console.WriteLine($"  appends:   {m.Credits.Cast!.Count} cast, {crew.Count} crew, {mapped} departments mapped");

        var images = Load<MovieDetails>("append_movie-details_images.json");
        Check(images.Images is not null, "images append must bind");
        Check(images.Credits is null, "an unrequested block stays null");

        var videos = Load<MovieDetails>("append_movie-details_videos.json");
        Check(videos.Videos?.Results is not null, "videos append must bind");
        Console.WriteLine($"  unasked:   credits null when not appended = {images.Credits is null}");
    }

    static void WatchProvidersKeyBinds()
    {
        // "watch/providers" is not a valid C# identifier -- the one block that needs the attribute.
        var m = Load<MovieDetails>("append_movie-details_watch_providers.json");
        Check(m.WatchProviders?.Results is { Count: > 0 }, "watch/providers must bind");
        var gb = m.WatchProviders!.Results!.ContainsKey("GB");
        Console.WriteLine($"  wp key:    {m.WatchProviders.Results.Count} regions, GB present = {gb}");
    }

    static void EmptyDateDoesNotThrow()
    {
        // TMDB sends "" for an absent date; the built-in converter throws on it.
        var m = JsonSerializer.Deserialize<MovieDetails>("""{"id":1,"release_date":""}""", Json)!;
        Check(m.ReleaseDate is null, "empty date must read as null, not throw");

        var ok = JsonSerializer.Deserialize<MovieDetails>("""{"id":1,"release_date":"1999-03-30"}""", Json)!;
        Check(ok.ReleaseDate == new DateOnly(1999, 3, 30), "a real date must still parse");
        Console.WriteLine($"  dates:     \"\" -> null, \"1999-03-30\" -> {ok.ReleaseDate}");
    }

    static void UnknownEnumDegrades()
    {
        var v = JsonSerializer.Deserialize<Video>(
            """{"site":"Dailymotion","type":"Interview"}""", Json)!;
        Check(v.Site.Value == VideoSite.Unknown, "unknown site degrades to Unknown");
        Check(v.Site.Raw == "Dailymotion", "raw site text is kept");
        Check(v.Type.Value == VideoType.Unknown, "unknown type degrades to Unknown");

        var known = JsonSerializer.Deserialize<Video>("""{"site":"YouTube","type":"Trailer"}""", Json)!;
        Check(known.Site.Value == VideoSite.YouTube, "known site maps");
        VideoType asEnum = known.Type;   // implicit conversion keeps switching ergonomic
        Check(asEnum == VideoType.Trailer, "implicit conversion works");
        Console.WriteLine($"  enums:     Dailymotion -> {v.Site}, YouTube -> {known.Site}");
    }

    static void NumericEnumRejectsOutOfRange()
    {
        // A plain numeric enum silently casts 99 to (ReleaseType)99. It must not.
        var good = JsonSerializer.Deserialize<ReleaseDateEntry>("""{"type":3}""", Json)!;
        Check(good.Type.Value == ReleaseType.Theatrical, "3 -> Theatrical");

        var bad = JsonSerializer.Deserialize<ReleaseDateEntry>("""{"type":99}""", Json)!;
        Check(bad.Type.Value == ReleaseType.Unknown, "99 must NOT silently cast");
        Console.WriteLine($"  numeric:   3 -> {good.Type.Value}, 99 -> {bad.Type.Value} (raw {bad.Type.Raw})");
    }

    static void RatedIsPolymorphic()
    {
        // Same key, two shapes: false when unrated, {"value": n} when rated.
        var unrated = JsonSerializer.Deserialize<AccountStates>("""{"id":1,"rated":false}""", Json)!;
        Check(unrated.Rating is null, "rated:false -> null");

        var rated = JsonSerializer.Deserialize<AccountStates>("""{"id":1,"rated":{"value":7.5}}""", Json)!;
        Check(rated.Rating == 7.5, "rated:{value} -> the number");
        Console.WriteLine($"  rated:     false -> null, {{value:7.5}} -> {rated.Rating}");
    }

    static void QueryStringSkipsAbsentValues()
    {
        var q = new QueryString()
            .Add("language", "en-US").Add("page", (int?)null).Add("region", (string?)null).ToString();
        Check(q == "?language=en-US", $"absent values must be skipped, got '{q}'");

        var two = new QueryString().Add("a", "1").Add("b", "2").ToString();
        Check(two == "?a=1&b=2", $"separator should be &, got '{two}'");

        var empty = new QueryString().ToString();
        Check(empty == "", "no params -> no '?'");
        Console.WriteLine($"  query:     '{q}', '{two}', '(empty)'");
    }

    static void AppendFlagsProduceWireNames()
    {
        var wire = (MovieAppend.Credits | MovieAppend.WatchProviders).ToQueryValue();
        Check(wire == "credits,watch/providers", $"got '{wire}'");
        Check(MovieAppend.None.ToQueryValue() == "", "None -> empty");

        var all = MovieAppend.All.ToQueryValue();
        Check(all.Split(',').Length == 14, "All should list 14 blocks");
        Console.WriteLine($"  append:    '{wire}', All = {all.Split(',').Length} blocks");
    }

    static void SessionsCarryTheRightParameter()
    {
        AnySession user = new UserSession("u123");
        AnySession guest = new GuestSession("g456");
        Check(user.UserSessionId == "u123" && user.GuestSessionId is null, "user session");
        Check(guest.GuestSessionId == "g456" && guest.UserSessionId is null, "guest session");
        Console.WriteLine($"  sessions:  user -> session_id, guest -> guest_session_id");
    }
}
