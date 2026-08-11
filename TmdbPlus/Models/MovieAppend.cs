namespace TmdbPlus.Models;

/// <summary>Blocks that can be appended to a movie details request.</summary>
[Flags]
public enum MovieAppend
{
    None = 0,
    AlternativeTitles = 1 << 0,
    Changes = 1 << 1,
    Credits = 1 << 2,
    ExternalIds = 1 << 3,
    Images = 1 << 4,
    Keywords = 1 << 5,
    Lists = 1 << 6,
    Recommendations = 1 << 7,
    ReleaseDates = 1 << 8,
    Reviews = 1 << 9,
    Similar = 1 << 10,
    Translations = 1 << 11,
    Videos = 1 << 12,
    WatchProviders = 1 << 13,

    All = AlternativeTitles | Changes | Credits | ExternalIds | Images | Keywords | Lists
        | Recommendations | ReleaseDates | Reviews | Similar | Translations | Videos | WatchProviders,
}

internal static class MovieAppendExtensions
{
    /// <summary>
    /// Explicit switch rather than reflected attributes: this value goes in a query string, where
    /// <c>JsonStringEnumMemberName</c> does not apply and <c>ToString()</c> would yield
    /// "WatchProviders" instead of "watch/providers". Issue #3.
    /// </summary>
    internal static string ToQueryValue(this MovieAppend appends)
    {
        if (appends == MovieAppend.None) return string.Empty;

        var parts = new List<string>(14);
        if (appends.HasFlag(MovieAppend.AlternativeTitles)) parts.Add("alternative_titles");
        if (appends.HasFlag(MovieAppend.Changes)) parts.Add("changes");
        if (appends.HasFlag(MovieAppend.Credits)) parts.Add("credits");
        if (appends.HasFlag(MovieAppend.ExternalIds)) parts.Add("external_ids");
        if (appends.HasFlag(MovieAppend.Images)) parts.Add("images");
        if (appends.HasFlag(MovieAppend.Keywords)) parts.Add("keywords");
        if (appends.HasFlag(MovieAppend.Lists)) parts.Add("lists");
        if (appends.HasFlag(MovieAppend.Recommendations)) parts.Add("recommendations");
        if (appends.HasFlag(MovieAppend.ReleaseDates)) parts.Add("release_dates");
        if (appends.HasFlag(MovieAppend.Reviews)) parts.Add("reviews");
        if (appends.HasFlag(MovieAppend.Similar)) parts.Add("similar");
        if (appends.HasFlag(MovieAppend.Translations)) parts.Add("translations");
        if (appends.HasFlag(MovieAppend.Videos)) parts.Add("videos");
        if (appends.HasFlag(MovieAppend.WatchProviders)) parts.Add("watch/providers");
        return string.Join(',', parts);
    }
}
