# TmdbPlus

Typed .NET wrapper for the TMDB API. Classes and interfaces provided for each request/response for easy mapping

```
dotnet add package TmdbPlus
```

## Setup

```csharp
services.AddTmdb(token);
```

Takes a [read access token](https://www.themoviedb.org/settings/api). Returns `IHttpClientBuilder`

```csharp
services.AddTmdb(token).AddStandardResilienceHandler();
```

The client is stateless, sessions are a parameter

## Append To Response

TMDB has an `append_to_response` param for combining requests into one.
Here, `append_to_response` is a `[Flags]` enum. Appended blocks come back as nullable properties, non-null when you asked for them:

```csharp
var movie = await tmdb.Movies.GetAsync(550, MovieAppend.Credits | MovieAppend.Videos);

Console.WriteLine(movie.Title);

foreach (var cast in movie.Credits!.Cast)
    Console.WriteLine($"{cast.Name} as {cast.Character}");
```

`{MovieAppend}.All` grabs everything.

Appendable endpoints:

| Endpoint | Enum |
|---|---|
| `Movies.GetAsync` | `MovieAppend` |
| `Tv.GetAsync` | `TvSeriesAppend` |
| `Tv.Seasons.GetAsync` | `TvSeasonAppend` |
| `Tv.Episodes.GetAsync` | `TvEpisodeAppend` |
| `People.GetAsync` | `PersonAppend` |

## Good to know

- Every call has a generic twin: `tmdb.Tv.GetAsync<TmdbSeriesDetails>(1396, …)` deserializes into any class implementing `ITvSeriesDetails<…>`
- **Paging** - TMDB only serves pages 1–500; past that it 400s with `status_code` 22
- **Retry / throttling** - add `.AddStandardResilienceHandler()`. TMDB's limit is ~40 req/s and "could change at any time"
- **Rate limits** - ~50 req/s and 20 connections per IP

## Credits

Based on [TMDbLib](https://github.com/jellyfin/TMDbLib) by Michael Bisbjerg, maintained by Jellyfin.

## License

[MIT](LICENSE).

This product uses the TMDB API but is not endorsed or certified by TMDB.
