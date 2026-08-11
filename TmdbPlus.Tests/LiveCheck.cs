using Microsoft.Extensions.DependencyInjection;
using TmdbPlus;
using TmdbPlus.Models;

// Exercises what fixtures cannot: DI registration, the bearer auth handler, real URL building,
// and the error path. Skips when no token is present, so the self-check still runs offline.

static class LiveCheck
{
    public static async Task RunAsync(Action<bool, string> check)
    {
        var token = ReadToken();
        if (token is null)
        {
            Console.WriteLine("\nLive check SKIPPED (no TMDB_READ_ACCESS_TOKEN in env or .env).");
            return;
        }

        Console.WriteLine("\nLive check against api.themoviedb.org:");

        var client = new ServiceCollection()
            .AddTmdb(o => o.ReadAccessToken = token)
            .Services.BuildServiceProvider()
            .GetRequiredService<ITmdbClient>();

        // 1. Details with two appends -- the headline feature, end to end.
        var movie = await client.Movies.GetAsync(603, MovieAppend.Credits | MovieAppend.WatchProviders);
        check(movie.Id == 603, "movie 603 should come back");
        check(movie.Credits?.Cast is { Count: > 0 }, "appended credits should be populated");
        check(movie.WatchProviders?.Results is { Count: > 0 }, "appended watch/providers should be populated");
        check(movie.Videos is null, "a block that was NOT appended stays null");
        Console.WriteLine($"  details:   {movie.Title} ({movie.ReleaseDate}), " +
                          $"{movie.Credits!.Cast!.Count} cast, {movie.WatchProviders!.Results!.Count} regions");

        // 2. A paged endpoint.
        var popular = await client.Movies.GetPopularAsync(page: 2);
        check(popular.Page == 2, "page parameter should reach the API");
        check(popular.Results is { Count: > 0 }, "results should be populated");
        Console.WriteLine($"  paging:    page {popular.Page}/{popular.TotalPages}, {popular.Results!.Count} results");

        // 2b. Movie reviews: a paged sub-resource whose items carry timestamps, so this covers
        //     both the page envelope and the DateTimeOffset converter on real data.
        var reviews = await client.Movies.GetReviewsAsync(603);
        check(reviews.Page == 1, "reviews should default to page 1");
        check(reviews.Results is { Count: > 0 }, "The Matrix should have reviews");
        check(reviews.TotalResults >= reviews.Results!.Count, "total should cover the page");

        var review = reviews.Results[0];
        check(!string.IsNullOrEmpty(review.Id), "a review carries an id");
        check(!string.IsNullOrEmpty(review.Author), "a review carries an author");
        check(!string.IsNullOrEmpty(review.Content), "a review carries content");
        check(review.CreatedAt is not null, "created_at should parse into DateTimeOffset");
        check(review.CreatedAt <= DateTimeOffset.UtcNow, "created_at should not be in the future");
        Console.WriteLine($"  reviews:   {reviews.TotalResults} total, first by {review.Author} " +
                          $"on {review.CreatedAt:yyyy-MM-dd} ({review.Content!.Length} chars)");

        // The full review endpoint resolves the same id with author details attached.
        var full = await client.Reviews.GetAsync(review.Id!);
        check(full.Id == review.Id, "the review endpoint should resolve the same review");
        check(full.MediaId == 603, "the review should point back at the movie");
        Console.WriteLine($"  review:    {full.Id} -> media {full.MediaId} \"{full.MediaTitle}\", " +
                          $"rating {full.AuthorDetails?.Rating?.ToString() ?? "none"}");

        // 3. TV: the three-level nesting, and aggregate credits (roles, not a single character).
        var series = await client.Tv.GetAsync(1396, TvSeriesAppend.AggregateCredits | TvSeriesAppend.ContentRatings);
        check(series.Id == 1396, "series 1396 should come back");
        check(series.Seasons is { Count: > 0 }, "seasons should be listed");
        check(series.AggregateCredits?.Cast is { Count: > 0 }, "aggregate credits should be populated");
        check(series.ContentRatings?.Results is { Count: > 0 }, "content ratings should be populated");
        var lead = series.AggregateCredits!.Cast![0];
        check(lead.Roles is { Count: > 0 }, "an aggregate cast member carries roles");
        Console.WriteLine($"  tv:        {series.Name}, {series.NumberOfSeasons} seasons, " +
                          $"{lead.Name} in {lead.TotalEpisodeCount} eps as {lead.Roles![0].Character}");

        var season = await client.Tv.Seasons.GetAsync(1396, 1);
        check(season.Episodes is { Count: > 0 }, "season should carry episodes");
        var episode = await client.Tv.Episodes.GetAsync(1396, 1, 1, TvEpisodeAppend.Credits);
        check(episode.EpisodeNumber == 1, "episode number should round-trip");
        check(episode.Credits?.GuestStars is not null, "episode credits carry guest stars");
        Console.WriteLine($"  tv nested: S1 has {season.Episodes!.Count} eps, " +
                          $"S1E1 = \"{episode.Name}\" ({episode.EpisodeType})");

        // 4. People: combined credits mix movies and series in one list, so media_type is the
        //    only thing telling them apart. This is where MediaType has to be right.
        var person = await client.People.GetAsync(287, PersonAppend.CombinedCredits);
        check(person.Id == 287, "person 287 should come back");
        check(person.CombinedCredits?.Cast is { Count: > 0 }, "combined credits should be populated");

        var credits = person.CombinedCredits!.Cast!;
        var movies = credits.Count(c => c.MediaType.Value == MediaType.Movie);
        var shows = credits.Count(c => c.MediaType.Value == MediaType.Tv);
        check(movies > 0 && shows > 0, "combined credits should span both media types");
        check(credits.All(c => c.DisplayName is not null), "every credit resolves a title or name");
        Console.WriteLine($"  people:    {person.Name} ({person.Birthday}), " +
                          $"{credits.Count} credits = {movies} movies + {shows} tv");

        // 5. Search, discover, find: query building under real filters.
        var multi = await client.Search.MultiAsync("matrix");
        check(multi.Results is { Count: > 0 }, "multi search should return results");
        check(multi.Results!.Any(r => r.MediaType.Value == MediaType.Movie), "multi should include movies");
        check(multi.Results.All(r => r.MediaType.IsKnown), "every multi result has a known media type");
        Console.WriteLine($"  search:    \"matrix\" -> {multi.TotalResults} results, " +
                          $"types: {string.Join('/', multi.Results.Select(r => r.MediaType.Value).Distinct())}");

        // Discover with several filters at once -- the dotted parameter names have to survive.
        var discovered = await client.Discover.MoviesAsync(new DiscoverMovieOptions
        {
            SortBy = MovieSortBy.VoteAverageDesc,
            VoteCountFrom = 5000,
            PrimaryReleaseDateFrom = new DateOnly(1990, 1, 1),
            PrimaryReleaseDateTo = new DateOnly(1999, 12, 31),
        });
        check(discovered.Results is { Count: > 0 }, "discover should return results");
        var years = discovered.Results!.Where(m => m.ReleaseDate is not null)
                                      .Select(m => m.ReleaseDate!.Value.Year).ToList();
        check(years.All(y => y is >= 1990 and <= 1999), "the date filter must actually apply");
        Console.WriteLine($"  discover:  top 90s film = {discovered.Results[0].Title} " +
                          $"({discovered.Results[0].VoteAverage:0.0}), all {years.Count} in range");

        var found = await client.Find.ByExternalIdAsync("tt0133093");
        check(found.MovieResults is { Count: > 0 }, "find by imdb id should resolve");
        check(found.MovieResults![0].Id == 603, "tt0133093 is The Matrix");
        Console.WriteLine($"  find:      tt0133093 -> {found.MovieResults[0].Title}");

        var trending = await client.Trending.AllAsync(TimeWindow.Week);
        check(trending.Results is { Count: > 0 }, "trending should return results");
        Console.WriteLine($"  trending:  {trending.Results!.Count} this week, " +
                          $"top = {trending.Results[0].DisplayName}");

        // 6. Auth and writes. A guest session needs no user credentials, so the whole write path
        //    (POST body, DELETE, the status envelope) is exercisable without an account.
        var validated = await client.Authentication.ValidateAsync();
        check(validated.Success == true, "the configured token should validate");

        var guest = await client.Authentication.CreateGuestSessionAsync();
        check(guest.Success && !string.IsNullOrEmpty(guest.GuestSessionId), "guest session should be issued");

        var rated = await client.Movies.RateAsync(603, 8.5, guest.Session);
        check(rated.StatusCode == 1, $"rating should succeed, got status_code {rated.StatusCode}");

        var unrated = await client.Movies.DeleteRatingAsync(603, guest.Session);
        check(unrated.StatusCode == 13, $"delete should succeed, got status_code {unrated.StatusCode}");
        Console.WriteLine($"  writes:    guest session ok, rate -> {rated.StatusCode}, " +
                          $"delete -> {unrated.StatusCode}");

        // An invalid rating must surface TMDB's reason, not a bare 400.
        try
        {
            await client.Movies.RateAsync(603, 99, guest.Session);
            check(false, "an out-of-range rating should have thrown");
        }
        catch (TmdbApiException ex)
        {
            check(ex.StatusCode == 18, $"expected status_code 18, got {ex.StatusCode}");
            Console.WriteLine($"  validation: rating 99 -> status_code {ex.StatusCode}: {ex.StatusMessage}");
        }

        // 7. Reference areas, including the shapes that are neither a page nor an object:
        //    /configuration/countries returns a bare array, certifications a keyed map.
        var config = await client.Configuration.GetAsync();
        check(config.Images?.SecureBaseUrl is not null, "image config should carry a base url");
        check(config.Images!.PosterSizes is { Count: > 0 }, "poster sizes should be listed");

        var countries = await client.Configuration.GetCountriesAsync();
        check(countries is { Count: > 0 }, "countries deserialize from a bare top-level array");

        var certs = await client.Certifications.GetMovieCertificationsAsync();
        check(certs.Certifications is { Count: > 0 }, "certifications should be keyed by country");
        check(certs.Certifications!.ContainsKey("US"), "US certifications should be present");
        Console.WriteLine($"  config:    {config.Images.SecureBaseUrl} " +
                          $"({config.Images.PosterSizes!.Count} poster sizes), {countries!.Count} countries");
        Console.WriteLine($"  certs:     {certs.Certifications.Count} countries, " +
                          $"US has {certs.Certifications["US"].Count} ratings");

        var collection = await client.Collections.GetAsync(10);
        check(collection.Parts is { Count: > 0 }, "a collection should carry its parts");

        var credit = await client.Credits.GetAsync("52fe4232c3a36847f800b579");
        check(credit.Person is not null, "a credit resolves its person");

        var genres = await client.Genres.GetMovieGenresAsync();
        check(genres.Genres is { Count: > 0 }, "movie genres should be listed");
        Console.WriteLine($"  reference: {collection.Name} ({collection.Parts!.Count} films), " +
                          $"credit -> {credit.Person!.Name}, {genres.Genres!.Count} genres");

        // 8. v4. Reading a public list needs no user token, so the shape and the /4 routing are
        //    checkable here. The write path needs a user-approved access token, which cannot be
        //    obtained without a browser -- so it is exercised only as far as the request token.
        var v4List = await client.V4Lists.GetAsync(1);
        check(v4List.Id == 1, "v4 list 1 should come back");
        check(v4List.Results is { Count: > 0 }, "a v4 list carries its items");
        check(v4List.CreatedBy?.Username is not null, "a v4 list names its owner");
        Console.WriteLine($"  v4 list:   \"{v4List.Name}\" by {v4List.CreatedBy!.Username}, " +
                          $"{v4List.TotalResults} items, public={v4List.Public}");

        // The mixed-media claim: v4 lists can hold movies AND series, which v3 cannot.
        var kinds = v4List.Results!.Select(r => r.MediaType.Value).Distinct().ToList();
        check(kinds.All(k => k != MediaType.Unknown), "every v4 item has a known media type");
        Console.WriteLine($"  v4 mixed:  item types = {string.Join('/', kinds)}");

        // item_status answers 404 for an absent item rather than success:false, so the "is it on
        // the list" question is asked through the exception. Both directions are checked.
        var present = v4List.Results[0];
        var onList = await client.V4Lists.GetItemStatusAsync(1, present.MediaType.Value, present.Id);
        check(onList.Success, "an item that IS on the list reports success");

        try
        {
            await client.V4Lists.GetItemStatusAsync(1, MediaType.Movie, 550);
            check(false, "an absent item should 404");
        }
        catch (TmdbApiException ex)
        {
            check(ex.StatusCode == 34, $"absent item should be status_code 34, got {ex.StatusCode}");
        }
        Console.WriteLine($"  v4 status: \"{present.DisplayName}\" on list = {onList.Success}, " +
                          $"absent item -> 404/34");

        // Step 1 of the v4 auth flow works with the application token alone.
        var v4Token = await client.V4Authentication.CreateRequestTokenAsync();
        check(v4Token.Success, $"v4 request token should be issued: {v4Token.StatusMessage}");
        check(!string.IsNullOrEmpty(v4Token.RequestToken), "a request token should come back");
        Console.WriteLine($"  v4 auth:   request token issued ({v4Token.RequestToken![..8]}...), " +
                          $"approval needs a browser");

        // 9. The error path: TmdbApiException must carry TMDB's status_code, not lose it.
        try
        {
            await client.Movies.GetAsync(999999999);
            check(false, "a bogus id should have thrown");
        }
        catch (TmdbApiException ex)
        {
            check(ex.StatusCode == 34, $"expected status_code 34, got {ex.StatusCode}");
            Console.WriteLine($"  errors:    {(int)ex.HttpStatus} -> status_code {ex.StatusCode}: {ex.StatusMessage}");
        }
    }

    /// <summary>Env var first, then the gitignored .env at the repo root.</summary>
    static string? ReadToken()
    {
        var fromEnv = Environment.GetEnvironmentVariable("TMDB_READ_ACCESS_TOKEN");
        if (!string.IsNullOrWhiteSpace(fromEnv)) return fromEnv;

        var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".env");
        if (!File.Exists(path)) return null;

        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split('=', 2);
            if (parts.Length == 2 && parts[0].Trim() == "TMDB_READ_ACCESS_TOKEN")
                return parts[1].Trim();
        }
        return null;
    }
}
