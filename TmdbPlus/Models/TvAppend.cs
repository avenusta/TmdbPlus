namespace TmdbPlus.Models;

// Three separate flag sets, because the three levels accept different blocks. Notably `changes`
// is appendable on a series but NOT on a season or episode -- TMDB rejects it there (issue #6),
// so it is simply absent from those enums rather than being a runtime error.

/// <summary>Blocks that can be appended to a TV series details request.</summary>
[Flags]
public enum TvSeriesAppend
{
    None = 0,
    AggregateCredits = 1 << 0,
    AlternativeTitles = 1 << 1,
    Changes = 1 << 2,
    ContentRatings = 1 << 3,
    Credits = 1 << 4,
    EpisodeGroups = 1 << 5,
    ExternalIds = 1 << 6,
    Images = 1 << 7,
    Keywords = 1 << 8,
    Lists = 1 << 9,
    Recommendations = 1 << 10,
    Reviews = 1 << 11,
    ScreenedTheatrically = 1 << 12,
    Similar = 1 << 13,
    Translations = 1 << 14,
    Videos = 1 << 15,
    WatchProviders = 1 << 16,

    All = AggregateCredits | AlternativeTitles | Changes | ContentRatings | Credits | EpisodeGroups
        | ExternalIds | Images | Keywords | Lists | Recommendations | Reviews | ScreenedTheatrically
        | Similar | Translations | Videos | WatchProviders,
}

/// <summary>Blocks that can be appended to a TV season details request.</summary>
[Flags]
public enum TvSeasonAppend
{
    None = 0,
    AggregateCredits = 1 << 0,
    Credits = 1 << 1,
    ExternalIds = 1 << 2,
    Images = 1 << 3,
    Translations = 1 << 4,
    Videos = 1 << 5,
    WatchProviders = 1 << 6,

    All = AggregateCredits | Credits | ExternalIds | Images | Translations | Videos | WatchProviders,
}

/// <summary>Blocks that can be appended to a TV episode details request.</summary>
[Flags]
public enum TvEpisodeAppend
{
    None = 0,
    Credits = 1 << 0,
    ExternalIds = 1 << 1,
    Images = 1 << 2,
    Translations = 1 << 3,
    Videos = 1 << 4,

    All = Credits | ExternalIds | Images | Translations | Videos,
}

internal static class TvAppendExtensions
{
    internal static string ToQueryValue(this TvSeriesAppend appends)
    {
        if (appends == TvSeriesAppend.None) return string.Empty;

        var parts = new List<string>(17);
        if (appends.HasFlag(TvSeriesAppend.AggregateCredits)) parts.Add("aggregate_credits");
        if (appends.HasFlag(TvSeriesAppend.AlternativeTitles)) parts.Add("alternative_titles");
        if (appends.HasFlag(TvSeriesAppend.Changes)) parts.Add("changes");
        if (appends.HasFlag(TvSeriesAppend.ContentRatings)) parts.Add("content_ratings");
        if (appends.HasFlag(TvSeriesAppend.Credits)) parts.Add("credits");
        if (appends.HasFlag(TvSeriesAppend.EpisodeGroups)) parts.Add("episode_groups");
        if (appends.HasFlag(TvSeriesAppend.ExternalIds)) parts.Add("external_ids");
        if (appends.HasFlag(TvSeriesAppend.Images)) parts.Add("images");
        if (appends.HasFlag(TvSeriesAppend.Keywords)) parts.Add("keywords");
        if (appends.HasFlag(TvSeriesAppend.Lists)) parts.Add("lists");
        if (appends.HasFlag(TvSeriesAppend.Recommendations)) parts.Add("recommendations");
        if (appends.HasFlag(TvSeriesAppend.Reviews)) parts.Add("reviews");
        if (appends.HasFlag(TvSeriesAppend.ScreenedTheatrically)) parts.Add("screened_theatrically");
        if (appends.HasFlag(TvSeriesAppend.Similar)) parts.Add("similar");
        if (appends.HasFlag(TvSeriesAppend.Translations)) parts.Add("translations");
        if (appends.HasFlag(TvSeriesAppend.Videos)) parts.Add("videos");
        if (appends.HasFlag(TvSeriesAppend.WatchProviders)) parts.Add("watch/providers");
        return string.Join(',', parts);
    }

    internal static string ToQueryValue(this TvSeasonAppend appends)
    {
        if (appends == TvSeasonAppend.None) return string.Empty;

        var parts = new List<string>(7);
        if (appends.HasFlag(TvSeasonAppend.AggregateCredits)) parts.Add("aggregate_credits");
        if (appends.HasFlag(TvSeasonAppend.Credits)) parts.Add("credits");
        if (appends.HasFlag(TvSeasonAppend.ExternalIds)) parts.Add("external_ids");
        if (appends.HasFlag(TvSeasonAppend.Images)) parts.Add("images");
        if (appends.HasFlag(TvSeasonAppend.Translations)) parts.Add("translations");
        if (appends.HasFlag(TvSeasonAppend.Videos)) parts.Add("videos");
        if (appends.HasFlag(TvSeasonAppend.WatchProviders)) parts.Add("watch/providers");
        return string.Join(',', parts);
    }

    internal static string ToQueryValue(this TvEpisodeAppend appends)
    {
        if (appends == TvEpisodeAppend.None) return string.Empty;

        var parts = new List<string>(5);
        if (appends.HasFlag(TvEpisodeAppend.Credits)) parts.Add("credits");
        if (appends.HasFlag(TvEpisodeAppend.ExternalIds)) parts.Add("external_ids");
        if (appends.HasFlag(TvEpisodeAppend.Images)) parts.Add("images");
        if (appends.HasFlag(TvEpisodeAppend.Translations)) parts.Add("translations");
        if (appends.HasFlag(TvEpisodeAppend.Videos)) parts.Add("videos");
        return string.Join(',', parts);
    }
}
