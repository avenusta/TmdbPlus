using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The reference areas: collections, companies, networks, keywords, credits, reviews, genres,
// certifications, configuration, and watch providers.

// ---------------------------------------------------------------------------
// Configuration
// ---------------------------------------------------------------------------

/// <summary>
/// Image base URLs and the size lists TMDB serves. A full image URL is
/// <c>{SecureBaseUrl}{size}{file_path}</c>.
/// </summary>
public class TmdbConfiguration : ITmdbConfiguration
{
    [JsonPropertyName("images")] public ImageConfiguration? Images { get; set; }

    /// <summary>Keys TMDB will accept in a change list.</summary>
    [JsonPropertyName("change_keys")] public IList<string>? ChangeKeys { get; set; }
}

public class ImageConfiguration : IImageConfiguration
{
    [JsonPropertyName("base_url")] public string? BaseUrl { get; set; }
    [JsonPropertyName("secure_base_url")] public string? SecureBaseUrl { get; set; }
    [JsonPropertyName("backdrop_sizes")] public IList<string>? BackdropSizes { get; set; }
    [JsonPropertyName("logo_sizes")] public IList<string>? LogoSizes { get; set; }
    [JsonPropertyName("poster_sizes")] public IList<string>? PosterSizes { get; set; }
    [JsonPropertyName("profile_sizes")] public IList<string>? ProfileSizes { get; set; }
    [JsonPropertyName("still_sizes")] public IList<string>? StillSizes { get; set; }
}

public class CountryInfo : ICountryInfo
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
    [JsonPropertyName("native_name")] public string? NativeName { get; set; }
}

public class LanguageInfo : ILanguageInfo
{
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

public class TimezoneInfo : ITimezoneInfo
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("zones")] public IList<string>? Zones { get; set; }
}

/// <summary>The jobs TMDB recognises, grouped by department.</summary>
public class DepartmentJobs : IDepartmentJobs
{
    [JsonPropertyName("department")] public string? Department { get; set; }
    [JsonPropertyName("jobs")] public IList<string>? Jobs { get; set; }
}

// ---------------------------------------------------------------------------
// Certifications
// ---------------------------------------------------------------------------

/// <summary>
/// Certifications keyed by country code. A raw string key, not an enum: TMDB adds regions and
/// uses non-ISO codes (issue #7).
/// </summary>
public class CertificationsResponse : ICertificationsResponse
{
    [JsonPropertyName("certifications")]
    public IDictionary<string, IList<Certification>>? Certifications { get; set; }
}

public class Certification : ICertification
{
    [JsonPropertyName("certification")] public string? Value { get; set; }
    [JsonPropertyName("meaning")] public string? Meaning { get; set; }
    [JsonPropertyName("order")] public int? Order { get; set; }
}

// ---------------------------------------------------------------------------
// Genres
// ---------------------------------------------------------------------------

public class GenreList : IGenreList<Genre>
{
    [JsonPropertyName("genres")] public IList<Genre>? Genres { get; set; }
}

// ---------------------------------------------------------------------------
// Collections
// ---------------------------------------------------------------------------

public class CollectionDetails : ICollectionDetails<MovieSummary>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("parts")] public IList<MovieSummary>? Parts { get; set; }
}

public class CollectionImages : ICollectionImages
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("backdrops")] public IList<ImageInfo>? Backdrops { get; set; }
    [JsonPropertyName("posters")] public IList<ImageInfo>? Posters { get; set; }
}

public class CollectionTranslations : ICollectionTranslations
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("translations")] public IList<CollectionTranslation>? Translations { get; set; }
}

public class CollectionTranslation : ICollectionTranslation
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
    [JsonPropertyName("data")] public CollectionTranslationData? Data { get; set; }
}

public class CollectionTranslationData : ICollectionTranslationData
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
}

// ---------------------------------------------------------------------------
// Companies and networks — same shape, different endpoints
// ---------------------------------------------------------------------------

public class CompanyDetails : ICompanyDetails<CompanySummary>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("headquarters")] public string? Headquarters { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
    [JsonPropertyName("logo_path")] public string? LogoPath { get; set; }
    [JsonPropertyName("origin_country")] public string? OriginCountry { get; set; }
    [JsonPropertyName("parent_company")] public CompanySummary? ParentCompany { get; set; }
}

public class AlternativeNames : IAlternativeNames
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("results")] public IList<AlternativeName>? Results { get; set; }
}

public class AlternativeName : IAlternativeName
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
}

/// <summary>Companies and networks have logos only — no posters or backdrops.</summary>
public class LogoImages : ILogoImages
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("logos")] public IList<ImageInfo>? Logos { get; set; }
}

// ---------------------------------------------------------------------------
// Keywords, credits, reviews
// ---------------------------------------------------------------------------

public class KeywordDetails : IKeywordDetails
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

/// <summary>
/// A single credit, resolved from a credit id. Carries the person, the media they worked on,
/// and — for TV — the specific episodes and seasons.
/// </summary>
public class CreditDetails : ICreditDetails<PersonSummary, CreditMedia>
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("job")] public string? Job { get; set; }

    [JsonPropertyName("department")]
    [JsonConverter(typeof(TmdbEnumValueConverter<CreditDepartment>))]
    public TmdbEnum<CreditDepartment>? Department { get; set; }

    [JsonPropertyName("credit_type")] public string? CreditType { get; set; }

    [JsonPropertyName("media_type")]
    [JsonConverter(typeof(TmdbEnumValueConverter<MediaType>))]
    public TmdbEnum<MediaType> MediaType { get; set; }

    [JsonPropertyName("person")] public PersonSummary? Person { get; set; }
    [JsonPropertyName("media")] public CreditMedia? Media { get; set; }
}

public class CreditMedia : ICreditMedia<EpisodeSummary, SeasonSummary>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("original_title")] public string? OriginalTitle { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("character")] public string? Character { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }

    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? FirstAirDate { get; set; }

    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? ReleaseDate { get; set; }

    [JsonPropertyName("episodes")] public IList<EpisodeSummary>? Episodes { get; set; }
    [JsonPropertyName("seasons")] public IList<SeasonSummary>? Seasons { get; set; }

    [JsonIgnore] public string? DisplayName => Title ?? Name;
}

/// <summary>A review with its author's details, which the embedded review shape omits.</summary>
public class ReviewDetails : IReviewDetails
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("author")] public string? Author { get; set; }
    [JsonPropertyName("content")] public string? Content { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("media_id")] public int MediaId { get; set; }
    [JsonPropertyName("media_title")] public string? MediaTitle { get; set; }

    [JsonPropertyName("media_type")]
    [JsonConverter(typeof(TmdbEnumValueConverter<MediaType>))]
    public TmdbEnum<MediaType> MediaType { get; set; }

    [JsonPropertyName("author_details")] public ReviewAuthor? AuthorDetails { get; set; }

    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class ReviewAuthor : IReviewAuthor
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("avatar_path")] public string? AvatarPath { get; set; }
    [JsonPropertyName("rating")] public double? Rating { get; set; }
}

// ---------------------------------------------------------------------------
// Watch providers
// ---------------------------------------------------------------------------

public class WatchProviderRegions : IWatchProviderRegions
{
    [JsonPropertyName("results")] public IList<WatchProviderRegion>? Results { get; set; }
}

public class WatchProviderRegion : IWatchProviderRegion
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
    [JsonPropertyName("native_name")] public string? NativeName { get; set; }
}

public class WatchProviderList : IWatchProviderList<WatchProviderDetails>
{
    [JsonPropertyName("results")] public IList<WatchProviderDetails>? Results { get; set; }
}

public class WatchProviderDetails : IWatchProviderDetails
{
    [JsonPropertyName("provider_id")] public int? ProviderId { get; set; }
    [JsonPropertyName("provider_name")] public string? ProviderName { get; set; }
    [JsonPropertyName("logo_path")] public string? LogoPath { get; set; }
    [JsonPropertyName("display_priority")] public int? DisplayPriority { get; set; }

    /// <summary>Display priority varies by country, keyed by the raw region code.</summary>
    [JsonPropertyName("display_priorities")] public IDictionary<string, int>? DisplayPriorities { get; set; }
}

// ---------------------------------------------------------------------------
// Changes (the site-wide lists)
// ---------------------------------------------------------------------------

/// <summary>An id that changed in the requested window, with whether it is adult.</summary>
public class ChangedItem : IChangedItem
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool? Adult { get; set; }
}
