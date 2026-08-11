namespace TmdbPlus;

/// <summary>Configuration for a <see cref="TmdbClient"/>.</summary>
public sealed class TmdbOptions
{
    /// <summary>The v4 read access token (a bearer JWT). The only credential TMDB still accepts.</summary>
    public string? ReadAccessToken { get; set; }

    /// <summary>
    /// Resolves the token per request, taking precedence over <see cref="ReadAccessToken"/>.
    /// For tokens that rotate at runtime.
    /// </summary>
    public Func<string?>? TokenResolver { get; set; }

    /// <summary>Sent as <c>language</c> when a call does not override it.</summary>
    public string? DefaultLanguage { get; set; } = "en-US";

    /// <summary>Sent as <c>region</c> when a call does not override it.</summary>
    public string? DefaultRegion { get; set; }

    public Uri BaseAddress { get; set; } = new("https://api.themoviedb.org/");
}
