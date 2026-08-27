using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract for the response types in Reference.cs: settable members a consuming app can
// implement on its own EF entities, so mapping is written once per interface (issue #4).
//
// A nested collection is generic in its element type ONLY where that element is entity-like --
// something a consumer would persist as its own row. IList<T> is not covariant and EF cannot map
// an explicit interface implementation, so those would otherwise force a shadow property.
// Block wrappers and envelopes stay concrete: a consumer stores the keywords, not the wrapper.
//
// Hand-maintained: keep in sync with the response classes in the matching .cs file.

public interface IAlternativeName
{
    string? Name { get; set; }
    string? Type { get; set; }
}

public interface IAlternativeNames
{
    int Id { get; set; }
    IList<AlternativeName>? Results { get; set; }
}

public interface ICertification
{
    string? Value { get; set; }
    string? Meaning { get; set; }
    int? Order { get; set; }
}

public interface ICertificationsResponse
{

}

public interface IChangedItem
{
    int Id { get; set; }
    bool? Adult { get; set; }
}

public interface ICollectionDetails<TParts>
    where TParts : IMovieSummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    IList<TParts>? Parts { get; set; }
}

public interface ICollectionImages
{
    int Id { get; set; }
    IList<ImageInfo>? Backdrops { get; set; }
    IList<ImageInfo>? Posters { get; set; }
}

public interface ICollectionTranslation
{
    string? Iso3166_1 { get; set; }
    string? Iso639_1 { get; set; }
    string? Name { get; set; }
    string? EnglishName { get; set; }
    CollectionTranslationData? Data { get; set; }
}

public interface ICollectionTranslationData
{
    string? Title { get; set; }
    string? Overview { get; set; }
    string? Homepage { get; set; }
}

public interface ICollectionTranslations
{
    int Id { get; set; }
    IList<CollectionTranslation>? Translations { get; set; }
}

public interface ICompanyDetails<TParentCompany>
    where TParentCompany : ICompanySummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    string? Headquarters { get; set; }
    string? Homepage { get; set; }
    string? LogoPath { get; set; }
    string? OriginCountry { get; set; }
    TParentCompany? ParentCompany { get; set; }
}

public interface ICountryInfo
{
    string? Iso3166_1 { get; set; }
    string? EnglishName { get; set; }
    string? NativeName { get; set; }
}

public interface ICreditDetails<TPerson, TMedia>
    where TPerson : IPersonSummaryBase
    where TMedia : ICreditMediaBase
{
    string? Id { get; set; }
    string? Job { get; set; }
    string? Department { get; set; }
    string? CreditType { get; set; }
    string? MediaType { get; set; }
    TPerson? Person { get; set; }
    TMedia? Media { get; set; }
}

public interface ICreditMediaBase
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Title { get; set; }
    string? OriginalName { get; set; }
    string? OriginalTitle { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    string? Character { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    bool Adult { get; set; }
    DateOnly? FirstAirDate { get; set; }
    DateOnly? ReleaseDate { get; set; }
}

public interface ICreditMedia<TEpisodes, TSeasons> : ICreditMediaBase
    where TEpisodes : IEpisodeSummary
    where TSeasons : ISeasonSummary
{
    IList<TEpisodes>? Episodes { get; set; }
    IList<TSeasons>? Seasons { get; set; }
}

public interface IDepartmentJobs
{
    string? Department { get; set; }
    IList<string>? Jobs { get; set; }
}

public interface IGenreList<TGenres>
    where TGenres : IGenre
{
    IList<TGenres>? Genres { get; set; }
}

public interface IImageConfiguration
{
    string? BaseUrl { get; set; }
    string? SecureBaseUrl { get; set; }
    IList<string>? BackdropSizes { get; set; }
    IList<string>? LogoSizes { get; set; }
    IList<string>? PosterSizes { get; set; }
    IList<string>? ProfileSizes { get; set; }
    IList<string>? StillSizes { get; set; }
}

public interface IKeywordDetails
{
    int Id { get; set; }
    string? Name { get; set; }
}

public interface ILanguageInfo
{
    string? Iso639_1 { get; set; }
    string? EnglishName { get; set; }
    string? Name { get; set; }
}

public interface ILogoImages
{
    int Id { get; set; }
    IList<ImageInfo>? Logos { get; set; }
}

public interface IReviewAuthor
{
    string? Name { get; set; }
    string? Username { get; set; }
    string? AvatarPath { get; set; }
    double? Rating { get; set; }
}

public interface IReviewDetails
{
    string? Id { get; set; }
    string? Author { get; set; }
    string? Content { get; set; }
    string? Url { get; set; }
    string? Iso639_1 { get; set; }
    int MediaId { get; set; }
    string? MediaTitle { get; set; }
    string? MediaType { get; set; }
    ReviewAuthor? AuthorDetails { get; set; }
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}

public interface ITimezoneInfo
{
    string? Iso3166_1 { get; set; }
    IList<string>? Zones { get; set; }
}

public interface ITmdbConfiguration
{
    ImageConfiguration? Images { get; set; }
    IList<string>? ChangeKeys { get; set; }
}

public interface IWatchProviderDetails
{
    int? ProviderId { get; set; }
    string? ProviderName { get; set; }
    string? LogoPath { get; set; }
    int? DisplayPriority { get; set; }
}

public interface IWatchProviderList<TResults>
    where TResults : IWatchProviderDetails
{
    IList<TResults>? Results { get; set; }
}

public interface IWatchProviderRegion
{
    string? Iso3166_1 { get; set; }
    string? EnglishName { get; set; }
    string? NativeName { get; set; }
}

public interface IWatchProviderRegions
{
    IList<WatchProviderRegion>? Results { get; set; }
}

