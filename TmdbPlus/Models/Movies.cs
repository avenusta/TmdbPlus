using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

// Nullability from audit/nullability_decisions.json, entry "/3/movie/{movie_id}".

public interface IMovieDetails
{
    int Id { get; set; }
    bool Adult { get; set; }
    long Budget { get; set; }
    long Revenue { get; set; }
    double Popularity { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    bool Video { get; set; }

    /// <summary>Undocumented, present in every response (issue #6).</summary>
    bool Softcore { get; set; }

    string? Title { get; set; }
    string? OriginalTitle { get; set; }
    string? OriginalLanguage { get; set; }
    string? Overview { get; set; }
    string? Tagline { get; set; }
    TmdbEnum<MediaStatus> Status { get; set; }
    string? Homepage { get; set; }
    string? ImdbId { get; set; }
    string? BackdropPath { get; set; }
    string? PosterPath { get; set; }
    DateOnly? ReleaseDate { get; set; }
    int? Runtime { get; set; }
    IList<string>? OriginCountry { get; set; }
}

/// <summary>
/// A movie, with one nullable property per append block. A block is <c>null</c> unless it was
/// requested: the caller null-checks what it asked for (issue #3). The blocks sit flat on the
/// type rather than under a nested <c>Appends</c> object -- STJ cannot bind parent-level keys
/// into a nested object without a converter per type plus double deserialization.
/// </summary>
public class MovieDetails : IMovieDetails
{
    // --- core: always present ---
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("budget")] public long Budget { get; set; }
    [JsonPropertyName("revenue")] public long Revenue { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
    [JsonPropertyName("video")] public bool Video { get; set; }
    [JsonPropertyName("softcore")] public bool Softcore { get; set; }

    // --- nullable per the audit ---
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("original_title")] public string? OriginalTitle { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("tagline")] public string? Tagline { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
    [JsonPropertyName("imdb_id")] public string? ImdbId { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("runtime")] public int? Runtime { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(TmdbEnumValueConverter<MediaStatus>))]
    public TmdbEnum<MediaStatus> Status { get; set; }

    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? ReleaseDate { get; set; }

    [JsonPropertyName("origin_country")] public IList<string>? OriginCountry { get; set; }
    [JsonPropertyName("genres")] public IList<Genre>? Genres { get; set; }
    [JsonPropertyName("production_companies")] public IList<ProductionCompany>? ProductionCompanies { get; set; }
    [JsonPropertyName("production_countries")] public IList<ProductionCountry>? ProductionCountries { get; set; }
    [JsonPropertyName("spoken_languages")] public IList<SpokenLanguage>? SpokenLanguages { get; set; }

    /// <summary>Null for a standalone film, populated for a franchise entry.</summary>
    [JsonPropertyName("belongs_to_collection")] public CollectionRef? BelongsToCollection { get; set; }

    // --- append blocks: null unless requested ---
    [JsonPropertyName("credits")] public Credits? Credits { get; set; }
    [JsonPropertyName("images")] public Images? Images { get; set; }
    [JsonPropertyName("videos")] public ResultsOf<Video>? Videos { get; set; }
    [JsonPropertyName("keywords")] public MovieKeywords? Keywords { get; set; }
    [JsonPropertyName("release_dates")] public ResultsOf<CountryReleaseDates>? ReleaseDates { get; set; }
    [JsonPropertyName("alternative_titles")] public MovieAlternativeTitles? AlternativeTitles { get; set; }
    [JsonPropertyName("external_ids")] public MovieExternalIds? ExternalIds { get; set; }
    [JsonPropertyName("translations")] public MovieTranslations? Translations { get; set; }
    [JsonPropertyName("changes")] public ChangesResult? Changes { get; set; }
    [JsonPropertyName("recommendations")] public PagedResult<MovieSummary>? Recommendations { get; set; }
    [JsonPropertyName("similar")] public PagedResult<MovieSummary>? Similar { get; set; }
    [JsonPropertyName("reviews")] public PagedResult<Review>? Reviews { get; set; }
    [JsonPropertyName("lists")] public PagedResult<ListSummary>? Lists { get; set; }

    /// <summary>The only append whose JSON key is not a valid C# identifier.</summary>
    [JsonPropertyName("watch/providers")] public ResultsMap<CountryWatchProviders>? WatchProviders { get; set; }
}

public interface IMovieSummary
{
    int Id { get; set; }
    bool Adult { get; set; }
    string? Title { get; set; }
    string? OriginalTitle { get; set; }
    string? OriginalLanguage { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    DateOnly? ReleaseDate { get; set; }
    double Popularity { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    bool Video { get; set; }
    IList<int>? GenreIds { get; set; }
}

/// <summary>The trimmed movie shape returned by list, search, and discovery endpoints.</summary>
public class MovieSummary : IMovieSummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("original_title")] public string? OriginalTitle { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }

    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? ReleaseDate { get; set; }

    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
    [JsonPropertyName("video")] public bool Video { get; set; }
    [JsonPropertyName("genre_ids")] public IList<int>? GenreIds { get; set; }
}

/// <summary>
/// <c>now_playing</c> and <c>upcoming</c> add a date range to the usual page shape.
/// </summary>
public class DatedMoviePage : PagedResult<MovieSummary>
{
    [JsonPropertyName("dates")] public DateRange? Dates { get; set; }
}

public class DateRange
{
    [JsonPropertyName("minimum")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? Minimum { get; set; }

    [JsonPropertyName("maximum")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? Maximum { get; set; }
}

// --- block wrappers -------------------------------------------------------
// TMDB's block shapes are inconsistent -- some wrap in a redundant key, some paginate, some are
// flat. The wrappers are kept rather than flattened, so the library's shape and TMDB's own docs
// keep lining up (issue #3).

public class MovieKeywords
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("keywords")] public IList<Keyword>? Keywords { get; set; }
}

public class MovieAlternativeTitles
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("titles")] public IList<AlternativeTitle>? Titles { get; set; }
}

public class MovieTranslations
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("translations")] public IList<Translation>? Translations { get; set; }
}

public class ChangesResult
{
    [JsonPropertyName("changes")] public IList<ChangeGroup>? Changes { get; set; }
}

public class MovieExternalIds
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("imdb_id")] public string? ImdbId { get; set; }
    [JsonPropertyName("wikidata_id")] public string? WikidataId { get; set; }
    [JsonPropertyName("facebook_id")] public string? FacebookId { get; set; }
    [JsonPropertyName("instagram_id")] public string? InstagramId { get; set; }
    [JsonPropertyName("twitter_id")] public string? TwitterId { get; set; }
}

public class CountryReleaseDates
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("release_dates")] public IList<ReleaseDateEntry>? ReleaseDates { get; set; }
}

public class ReleaseDateEntry
{
    [JsonPropertyName("certification")] public string? Certification { get; set; }

    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? ReleaseDate { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(TmdbEnumValueConverter<ReleaseType>))]
    public TmdbEnum<ReleaseType> Type { get; set; }

    [JsonPropertyName("note")] public string? Note { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("descriptors")] public IList<string>? Descriptors { get; set; }
}

/// <summary>The <c>/movie/latest</c> shape -- a full movie with no appends.</summary>
public class MovieChangeEntry
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool? Adult { get; set; }
}
