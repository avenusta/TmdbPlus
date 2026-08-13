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

    static async Task Main(string[] args)
    {
        // The v4 write check is opt-in: it mutates a real account and pauses for a browser
        // approval, so it never runs as part of the ordinary check pass.
        if (args.Contains("v4writes"))
        {
            await V4WriteCheck.RunAsync(Check);
            Console.WriteLine("\nv4 write check passed.");
            return;
        }

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
        BareInterfaceImplementerBinds();
        NonDerivableWireNamesStillBind();

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
        // Departments store the wire string, so comparison goes through the lookup constants.
        var mapped = crew.Count(c => c.Department == CreditDepartment.VisualEffects
                                  || c.Department == CreditDepartment.Directing
                                  || c.Department == CreditDepartment.Writing);
        Check(mapped > 0, "known departments should compare equal to the constants");
        // The wire text is the stored value -- nothing is dropped, mapped or not.
        Check(crew.All(c => c.Department is not null), "raw wire text must be kept");
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
        // Vocabularies store the wire string, so an unrecognised value is simply a string that
        // matches no constant -- it is never coerced and never throws.
        var v = JsonSerializer.Deserialize<Video>(
            """{"site":"Dailymotion","type":"Interview"}""", Json)!;
        Check(v.Site == "Dailymotion", "unknown site is kept verbatim");
        Check(v.Site != VideoSite.YouTube && v.Site != VideoSite.Vimeo, "unknown site matches no constant");
        Check(v.Type == "Interview", "unknown type is kept verbatim");

        var known = JsonSerializer.Deserialize<Video>("""{"site":"YouTube","type":"Trailer"}""", Json)!;
        Check(known.Site == VideoSite.YouTube, "known site compares equal to the constant");
        Check(known.Type == VideoType.Trailer, "known type compares equal to the constant");
        Console.WriteLine($"  enums:     Dailymotion -> {v.Site}, YouTube -> {known.Site}");
    }

    static void NumericEnumRejectsOutOfRange()
    {
        // Release type is numeric on the wire and stores as int -- an unlisted number is kept
        // as-is rather than coerced, and matches none of the constants.
        var good = JsonSerializer.Deserialize<ReleaseDateEntry>("""{"type":3}""", Json)!;
        Check(good.Type == ReleaseType.Theatrical, "3 -> Theatrical");

        var bad = JsonSerializer.Deserialize<ReleaseDateEntry>("""{"type":99}""", Json)!;
        Check(bad.Type == 99, "99 is kept verbatim");
        Check(bad.Type != ReleaseType.Theatrical, "99 matches no known release type");
        Console.WriteLine($"  numeric:   3 -> {good.Type}, 99 -> {bad.Type}");
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

    /// <summary>
    /// The contract's whole point: a consumer's own entity implements the interface and binds. It
    /// carries no <c>[JsonPropertyName]</c> — attributes do not cross an interface — so this fails
    /// on every multi-word property unless the shared options set a naming policy. Every other check
    /// here uses the library's own attributed classes and passes either way, which is exactly how
    /// this went unnoticed.
    /// </summary>
    static void BareInterfaceImplementerBinds()
    {
        var season = Load<BareSeason>("tv-season-details_0.json");
        Check(season.Episodes is { Count: > 0 }, "episodes must bind on a bare implementer");
        var episodes = season.Episodes!;

        // The reported symptom: ten episodes, all EpisodeNumber = 0, no exception.
        Check(episodes.All(e => e.EpisodeNumber != 0), "episode_number must bind on a bare implementer");
        Check(episodes.Any(e => e.AirDate is not null), "air_date must bind on a bare implementer");
        Check(episodes.Any(e => e.EpisodeType is not null), "episode_type must bind on a bare implementer");
        // Single-word names bind by exact match, not by the policy -- assert them so a future
        // change to case handling cannot quietly take them away.
        Check(episodes.All(e => !string.IsNullOrEmpty(e.Name)), "name must bind on a bare implementer");

        var first = episodes[0];
        Console.WriteLine($"  contract:  bare implementer bound {episodes.Count} episodes, "
            + $"first = E{first.EpisodeNumber} '{first.Name}' {first.AirDate} ({first.EpisodeType})");
    }

    /// <summary>
    /// The six wire names the naming policy cannot derive. Their <c>[JsonPropertyName]</c> must win
    /// over the policy, so these are the properties that break if the "redundant" attributes are
    /// ever bulk-deleted — the policy would silently rename them and bind nothing.
    /// </summary>
    static void NonDerivableWireNamesStillBind()
    {
        // Iso3166_1 / Iso639_1: SnakeCaseLower emits iso3166_1, missing the underscore TMDB sends.
        var releases = Load<MovieDetails>("append_movie-details_release_dates.json").ReleaseDates!;
        var group = releases.Results!.First(g => !string.IsNullOrEmpty(g.Iso3166_1));
        Check(group.Iso3166_1!.Length == 2, $"iso_3166_1 must bind, got '{group.Iso3166_1}'");

        var translations = Load<MovieDetails>("append_movie-details_translations.json").Translations!;
        var t = translations.Translations!.First();
        Check(!string.IsNullOrEmpty(t.Iso639_1), "iso_639_1 must bind");
        Check(!string.IsNullOrEmpty(t.Iso3166_1), "iso_3166_1 must bind on a translation");

        // InternalId: leading underscore, which no policy produces.
        var season = Load<TvSeasonDetails>("tv-season-details_0.json");
        Check(!string.IsNullOrEmpty(season.InternalId), "_id must bind to InternalId");

        // WatchProviders: contains '/', not a legal identifier.
        var wp = Load<MovieDetails>("append_movie-details_watch_providers.json");
        Check(wp.WatchProviders?.Results is { Count: > 0 }, "watch/providers must bind");

        // Value: a deliberate rename away from the wire name.
        var certs = Load<CertificationsResponse>("certification-movie-list_0.json");
        var us = certs.Certifications!["US"];
        Check(us.Any(c => !string.IsNullOrEmpty(c.Value)), "certification must bind to Value");

        // Token: the other deliberate rename.
        var token = Load<RequestToken>("authentication-create-request-token_0.json");
        Check(!string.IsNullOrEmpty(token.Token), "request_token must bind to Token");

        Console.WriteLine($"  exceptions: iso_3166_1={group.Iso3166_1}, iso_639_1={t.Iso639_1}, "
            + $"_id={season.InternalId}, watch/providers={wp.WatchProviders!.Results!.Count} regions, "
            + $"certification={us.First(c => !string.IsNullOrEmpty(c.Value)).Value}, request_token ok");
    }
}

/// <summary>Stands in for a consumer's EF entity: satisfies the contract, declares no attributes.</summary>
file sealed class BareEpisode : ITvEpisodeDetailsBase
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Overview { get; set; }
    public string? StillPath { get; set; }
    public int EpisodeNumber { get; set; }
    public int SeasonNumber { get; set; }
    public int? Runtime { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string? EpisodeType { get; set; }
    public DateOnly? AirDate { get; set; }
    public string? ProductionCode { get; set; }
    public int ShowId { get; set; }
}

file sealed class BareSeason
{
    public IList<BareEpisode>? Episodes { get; set; }
}
