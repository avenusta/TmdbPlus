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

        // 3. The error path: TmdbApiException must carry TMDB's status_code, not lose it.
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
