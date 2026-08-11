using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

// Mutable classes, not records: init-only breaks EF change tracking, and `with` yields a second
// instance with the same key (issue #4). Nullability is from audit/nullability_decisions.json --
// TMDbLib's prior wins, live observation only adds nullability, no evidence => nullable.

/// <summary>A page of results. Named so the <c>page</c> property keeps its natural name.</summary>
public class PagedResult<T> : IPagedResult<T>
{
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("results")] public IList<T>? Results { get; set; }
    [JsonPropertyName("total_pages")] public int TotalPages { get; set; }
    [JsonPropertyName("total_results")] public int TotalResults { get; set; }
}

/// <summary>A block shaped <c>{ "id": n, "results": [...] }</c>.</summary>
public class ResultsOf<T>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("results")] public IList<T>? Results { get; set; }
}

/// <summary>
/// A block shaped <c>{ "results": { "GB": {...} } }</c>. Raw string keys, not an enum: the audit
/// found <c>CA-QC</c>, which is not ISO 3166-1, and TMDB adds regions over time (issue #7).
/// </summary>
public class ResultsMap<T>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("results")] public IDictionary<string, T>? Results { get; set; }
}

public class Genre : IGenre
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

public class ProductionCompany : IProductionCompany
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("logo_path")] public string? LogoPath { get; set; }
    [JsonPropertyName("origin_country")] public string? OriginCountry { get; set; }
}

public class ProductionCountry : IProductionCountry
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

public class SpokenLanguage : ISpokenLanguage
{
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
}

public class CollectionRef : ICollectionRef
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
}

public class Keyword : IKeyword
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

public class ImageInfo : IImageInfo
{
    [JsonPropertyName("file_path")] public string? FilePath { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("aspect_ratio")] public double AspectRatio { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
}

public class Images : IImages<ImageInfo>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("backdrops")] public IList<ImageInfo>? Backdrops { get; set; }
    [JsonPropertyName("logos")] public IList<ImageInfo>? Logos { get; set; }
    [JsonPropertyName("posters")] public IList<ImageInfo>? Posters { get; set; }
}

public class CastMember : ICastMember
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("character")] public string? Character { get; set; }
    [JsonPropertyName("order")] public int? Order { get; set; }
    [JsonPropertyName("cast_id")] public int? CastId { get; set; }
    [JsonPropertyName("credit_id")] public string? CreditId { get; set; }
    [JsonPropertyName("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }
    [JsonPropertyName("gender")] public int Gender { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
}

public class CrewMember : ICrewMember
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("job")] public string? Job { get; set; }

    /// <summary>
    /// Carries both the mapped value and TMDB's raw text, so an unmapped department is never
    /// lost. Implicitly converts to <see cref="CreditDepartment"/> for switching and comparison.
    /// </summary>
    [JsonPropertyName("department")]
    [JsonConverter(typeof(TmdbEnumValueConverter<CreditDepartment>))]
    public TmdbEnum<CreditDepartment> Department { get; set; }

    [JsonPropertyName("credit_id")] public string? CreditId { get; set; }
    [JsonPropertyName("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }
    [JsonPropertyName("gender")] public int Gender { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
}

public class Credits : ICredits<CastMember, CrewMember>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("cast")] public IList<CastMember>? Cast { get; set; }
    [JsonPropertyName("crew")] public IList<CrewMember>? Crew { get; set; }
}

public class Video : IVideo
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("key")] public string? Key { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("site")]
    [JsonConverter(typeof(TmdbEnumValueConverter<VideoSite>))]
    public TmdbEnum<VideoSite> Site { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(TmdbEnumValueConverter<VideoType>))]
    public TmdbEnum<VideoType> Type { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("official")] public bool Official { get; set; }

    [JsonPropertyName("published_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? PublishedAt { get; set; }

    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
}

public class AlternativeTitle : IAlternativeTitle
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
}

public class TranslationData : ITranslationData
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("tagline")] public string? Tagline { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
    [JsonPropertyName("runtime")] public int? Runtime { get; set; }
}

public class Translation : ITranslation<TranslationData>
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
    [JsonPropertyName("data")] public TranslationData? Data { get; set; }
}

public class Review : IReview
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("author")] public string? Author { get; set; }
    [JsonPropertyName("content")] public string? Content { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }

    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class WatchProvider : IWatchProvider
{
    [JsonPropertyName("provider_id")] public int ProviderId { get; set; }
    [JsonPropertyName("provider_name")] public string? ProviderName { get; set; }
    [JsonPropertyName("logo_path")] public string? LogoPath { get; set; }
    [JsonPropertyName("display_priority")] public int DisplayPriority { get; set; }
}

public class CountryWatchProviders : ICountryWatchProviders<WatchProvider>
{
    [JsonPropertyName("link")] public string? Link { get; set; }
    [JsonPropertyName("flatrate")] public IList<WatchProvider>? Flatrate { get; set; }
    [JsonPropertyName("buy")] public IList<WatchProvider>? Buy { get; set; }
    [JsonPropertyName("rent")] public IList<WatchProvider>? Rent { get; set; }
    [JsonPropertyName("ads")] public IList<WatchProvider>? Ads { get; set; }
    [JsonPropertyName("free")] public IList<WatchProvider>? Free { get; set; }
}

public class ChangeItem : IChangeItem
{
    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonPropertyName("action")]
    [JsonConverter(typeof(TmdbEnumValueConverter<ChangeAction>))]
    public TmdbEnum<ChangeAction> Action { get; set; }

    [JsonPropertyName("time")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? Time { get; set; }

    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
}

public class ChangeGroup : IChangeGroup<ChangeItem>
{
    [JsonPropertyName("key")] public string? Key { get; set; }
    [JsonPropertyName("items")] public IList<ChangeItem>? Items { get; set; }
}

public class AccountStates : IAccountStates
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("favorite")] public bool Favorite { get; set; }
    [JsonPropertyName("watchlist")] public bool Watchlist { get; set; }

    [JsonPropertyName("rated")]
    [JsonConverter(typeof(TmdbRatingConverter))]
    public double? Rating { get; set; }
}

public class ListSummary : IListSummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("item_count")] public int ItemCount { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("list_type")] public string? ListType { get; set; }
    [JsonPropertyName("favorite_count")] public int FavoriteCount { get; set; }
}
