using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract. TmdbPlus defines these; a consuming app makes its own EF entities satisfy
// them, so mapping is written once per interface rather than once per type (issue #4).
// Members are SETTABLE -- init-only breaks EF change tracking. Nested collections are IList<T>
// generic in the element type, because IList<T> is not covariant and EF cannot map an explicit
// interface implementation.

public interface IPagedResult<T>
{
    int Page { get; set; }
    IList<T>? Results { get; set; }
    int TotalPages { get; set; }
    int TotalResults { get; set; }
}

public interface IGenre
{
    int Id { get; set; }
    string? Name { get; set; }
}

public interface IProductionCompany
{
    int Id { get; set; }
    string? Name { get; set; }
    string? LogoPath { get; set; }
    string? OriginCountry { get; set; }
}

public interface IProductionCountry
{
    string? Iso3166_1 { get; set; }
    string? Name { get; set; }
}

public interface ISpokenLanguage
{
    string? Iso639_1 { get; set; }
    string? Name { get; set; }
    string? EnglishName { get; set; }
}

public interface ICollectionRef
{
    int Id { get; set; }
    string? Name { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
}

public interface IKeyword
{
    int Id { get; set; }
    string? Name { get; set; }
}

public interface IImageInfo
{
    string? FilePath { get; set; }
    int Width { get; set; }
    int Height { get; set; }
    double AspectRatio { get; set; }
    string? Iso639_1 { get; set; }
    string? Iso3166_1 { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
}

public interface IImages<TImage> where TImage : IImageInfo
{
    int Id { get; set; }
    IList<TImage>? Backdrops { get; set; }
    IList<TImage>? Logos { get; set; }
    IList<TImage>? Posters { get; set; }
}

public interface ICastMember
{
    int Id { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    string? Character { get; set; }
    int? Order { get; set; }
    int? CastId { get; set; }
    string? CreditId { get; set; }
    string? KnownForDepartment { get; set; }
    string? ProfilePath { get; set; }
    int Gender { get; set; }
    bool Adult { get; set; }
    double Popularity { get; set; }
}

public interface ICrewMember
{
    int Id { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }

    /// <summary>Stays a string: 94 distinct values appeared in one film (issue #10).</summary>
    string? Job { get; set; }

    /// <summary>Mapped value plus TMDB's raw text; converts implicitly to the enum.</summary>
    TmdbEnum<CreditDepartment> Department { get; set; }

    string? CreditId { get; set; }
    string? KnownForDepartment { get; set; }
    string? ProfilePath { get; set; }
    int Gender { get; set; }
    bool Adult { get; set; }
    double Popularity { get; set; }
}

public interface ICredits<TCast, TCrew>
    where TCast : ICastMember
    where TCrew : ICrewMember
{
    int Id { get; set; }
    IList<TCast>? Cast { get; set; }
    IList<TCrew>? Crew { get; set; }
}

public interface IVideo
{
    string? Id { get; set; }
    string? Key { get; set; }
    string? Name { get; set; }
    TmdbEnum<VideoSite> Site { get; set; }
    TmdbEnum<VideoType> Type { get; set; }
    int Size { get; set; }
    bool Official { get; set; }
    DateTimeOffset? PublishedAt { get; set; }
    string? Iso639_1 { get; set; }
    string? Iso3166_1 { get; set; }
}

public interface IAlternativeTitle
{
    string? Iso3166_1 { get; set; }
    string? Title { get; set; }

    /// <summary>Free text, not a vocabulary -- stays a string (issue #10).</summary>
    string? Type { get; set; }
}

public interface ITranslationData
{
    string? Title { get; set; }
    string? Overview { get; set; }
    string? Tagline { get; set; }
    string? Homepage { get; set; }
    int? Runtime { get; set; }
}

public interface ITranslation<TData> where TData : ITranslationData
{
    string? Iso3166_1 { get; set; }
    string? Iso639_1 { get; set; }
    string? Name { get; set; }
    string? EnglishName { get; set; }
    TData? Data { get; set; }
}

public interface IReview
{
    string? Id { get; set; }
    string? Author { get; set; }
    string? Content { get; set; }
    string? Url { get; set; }
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}

public interface IWatchProvider
{
    int ProviderId { get; set; }
    string? ProviderName { get; set; }
    string? LogoPath { get; set; }
    int DisplayPriority { get; set; }
}

public interface ICountryWatchProviders<TProvider> where TProvider : IWatchProvider
{
    string? Link { get; set; }
    IList<TProvider>? Flatrate { get; set; }
    IList<TProvider>? Buy { get; set; }
    IList<TProvider>? Rent { get; set; }
    IList<TProvider>? Ads { get; set; }
    IList<TProvider>? Free { get; set; }
}

public interface IChangeItem
{
    string? Id { get; set; }
    TmdbEnum<ChangeAction> Action { get; set; }
    DateTimeOffset? Time { get; set; }
    string? Iso639_1 { get; set; }
    string? Iso3166_1 { get; set; }
}

public interface IChangeGroup<TItem> where TItem : IChangeItem
{
    string? Key { get; set; }
    IList<TItem>? Items { get; set; }
}

public interface IAccountStates
{
    int Id { get; set; }
    bool Favorite { get; set; }
    bool Watchlist { get; set; }

    /// <summary>Polymorphic on the wire: <c>false</c> or <c>{"value": n}</c> (issue #7).</summary>
    double? Rating { get; set; }
}

public interface IListSummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    string? PosterPath { get; set; }
    int ItemCount { get; set; }
    string? Iso639_1 { get; set; }
    string? ListType { get; set; }
    int FavoriteCount { get; set; }
}
