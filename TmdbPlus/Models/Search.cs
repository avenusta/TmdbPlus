using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

/// <summary>
/// One result from <c>/search/multi</c> or a trending "all" list, which mix movies, series, and
/// people in a single array. Only <c>media_type</c> says which. Rather than a polymorphic
/// converter, every field sits on the one type: the alternative costs a converter plus a second
/// deserialization pass, for a shape callers still have to branch on.
/// </summary>
public class MultiSearchResult : IMultiSearchResult<CombinedCastCredit>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }

    [JsonPropertyName("media_type")]
    [JsonConverter(typeof(TmdbEnumValueConverter<MediaType>))]
    public TmdbEnum<MediaType> MediaType { get; set; }

    // Movie and TV shared.
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("genre_ids")] public IList<int>? GenreIds { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }

    // Movie only.
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("original_title")] public string? OriginalTitle { get; set; }
    [JsonPropertyName("video")] public bool? Video { get; set; }

    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? ReleaseDate { get; set; }

    // TV only.
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("origin_country")] public IList<string>? OriginCountry { get; set; }

    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? FirstAirDate { get; set; }

    // Person only.
    [JsonPropertyName("gender")] public int? Gender { get; set; }
    [JsonPropertyName("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }
    [JsonPropertyName("known_for")] public IList<CombinedCastCredit>? KnownFor { get; set; }

    /// <summary>The title, series name, or person name — whichever this result carries.</summary>
    [JsonIgnore] public string? DisplayName => Title ?? Name;
}

/// <summary>A collection as returned by search and by the collection endpoints.</summary>
public class CollectionSummary : ICollectionSummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
}

public class CompanySummary : ICompanySummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("logo_path")] public string? LogoPath { get; set; }
    [JsonPropertyName("origin_country")] public string? OriginCountry { get; set; }
}

/// <summary>
/// The <c>/find/{external_id}</c> response — one array per media type, since an external id can
/// match at any level.
/// </summary>
public class FindResults : IFindResults<MovieSummary, TvSeriesSummary, PersonSummary, SeasonSummary, EpisodeSummary>
{
    [JsonPropertyName("movie_results")] public IList<MovieSummary>? MovieResults { get; set; }
    [JsonPropertyName("tv_results")] public IList<TvSeriesSummary>? TvResults { get; set; }
    [JsonPropertyName("person_results")] public IList<PersonSummary>? PersonResults { get; set; }
    [JsonPropertyName("tv_season_results")] public IList<SeasonSummary>? TvSeasonResults { get; set; }
    [JsonPropertyName("tv_episode_results")] public IList<EpisodeSummary>? TvEpisodeResults { get; set; }
}

/// <summary>External databases <c>/find</c> can look up an id in.</summary>
public enum ExternalSource
{
    Imdb = 0,
    Tvdb,
    Wikidata,
    Facebook,
    Instagram,
    Twitter,
    Tiktok,
    Youtube,
}

internal static class ExternalSourceExtensions
{
    internal static string ToWire(this ExternalSource s) => s switch
    {
        ExternalSource.Imdb => "imdb_id",
        ExternalSource.Tvdb => "tvdb_id",
        ExternalSource.Wikidata => "wikidata_id",
        ExternalSource.Facebook => "facebook_id",
        ExternalSource.Instagram => "instagram_id",
        ExternalSource.Twitter => "twitter_id",
        ExternalSource.Tiktok => "tiktok_id",
        ExternalSource.Youtube => "youtube_id",
        _ => "imdb_id",
    };
}

/// <summary>The window a trending list covers.</summary>
public enum TimeWindow
{
    Day = 0,
    Week,
}

internal static class TimeWindowExtensions
{
    internal static string ToWire(this TimeWindow w) => w == TimeWindow.Week ? "week" : "day";
}
