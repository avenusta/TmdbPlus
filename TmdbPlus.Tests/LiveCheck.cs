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

        // 7. The error path: TmdbApiException must carry TMDB's status_code, not lose it.
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
