using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract for the response types in People.cs: settable members a consuming app can
// implement on its own EF entities, so mapping is written once per interface (issue #4).
//
// A nested collection is generic in its element type ONLY where that element is entity-like --
// something a consumer would persist as its own row. IList<T> is not covariant and EF cannot map
// an explicit interface implementation, so those would otherwise force a shadow property.
// Envelope properties are generic in the envelope type via a non-generic marker (issue #18).
//
// Hand-maintained: keep in sync with the response classes in the matching .cs file.

public interface ICombinedCastCredit
{
    int Id { get; set; }
    bool Adult { get; set; }
    bool Softcore { get; set; }
    bool Video { get; set; }
    double Popularity { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    TmdbEnum<MediaType> MediaType { get; set; }
    string? Character { get; set; }
    string? CreditId { get; set; }
    int? Order { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    string? OriginalLanguage { get; set; }
    IList<int>? GenreIds { get; set; }
    string? Title { get; set; }
    string? OriginalTitle { get; set; }
    DateOnly? ReleaseDate { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    int? EpisodeCount { get; set; }
    IList<string>? OriginCountry { get; set; }
    DateOnly? FirstAirDate { get; set; }
}

public interface ICombinedCreditsBase
{
    int Id { get; set; }
}

public interface ICombinedCredits<TCast, TCrew> : ICombinedCreditsBase
    where TCast : ICombinedCastCredit
    where TCrew : ICombinedCrewCredit
{
    IList<TCast>? Cast { get; set; }
    IList<TCrew>? Crew { get; set; }
}

public interface ICombinedCrewCredit
{
    int Id { get; set; }
    bool Adult { get; set; }
    bool Softcore { get; set; }
    bool Video { get; set; }
    double Popularity { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    TmdbEnum<MediaType> MediaType { get; set; }
    string? Job { get; set; }
    TmdbEnum<CreditDepartment>? Department { get; set; }
    string? CreditId { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    string? OriginalLanguage { get; set; }
    IList<int>? GenreIds { get; set; }
    string? Title { get; set; }
    string? OriginalTitle { get; set; }
    DateOnly? ReleaseDate { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    int? EpisodeCount { get; set; }
    IList<string>? OriginCountry { get; set; }
    DateOnly? FirstAirDate { get; set; }
}

public interface IPersonExternalIds
{
    int Id { get; set; }
    string? ImdbId { get; set; }
    string? WikidataId { get; set; }
    string? FacebookId { get; set; }
    string? InstagramId { get; set; }
    string? TwitterId { get; set; }
    string? TiktokId { get; set; }
    string? YoutubeId { get; set; }
    string? FreebaseId { get; set; }
    string? FreebaseMid { get; set; }
    int? TvrageId { get; set; }
}

public interface IPersonImagesBase
{
    int Id { get; set; }
}

public interface IPersonImages<TImage> : IPersonImagesBase where TImage : IImageInfo
{
    IList<TImage>? Profiles { get; set; }
}

public interface IPersonMovieCreditsBase
{
    int Id { get; set; }
}

public interface IPersonMovieCredits<TCast, TCrew> : IPersonMovieCreditsBase
    where TCast : ICombinedCastCredit
    where TCrew : ICombinedCrewCredit
{
    IList<TCast>? Cast { get; set; }
    IList<TCrew>? Crew { get; set; }
}

public interface IPersonSummary<TKnownFor>
    where TKnownFor : ICombinedCastCredit
{
    int Id { get; set; }
    bool Adult { get; set; }
    int Gender { get; set; }
    double Popularity { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    string? KnownForDepartment { get; set; }
    string? ProfilePath { get; set; }
    IList<TKnownFor>? KnownFor { get; set; }
}

public interface IPersonTranslation
{
    string? Iso3166_1 { get; set; }
    string? Iso639_1 { get; set; }
    string? Name { get; set; }
    string? EnglishName { get; set; }
    PersonTranslationData? Data { get; set; }
}

public interface IPersonTranslationData
{
    string? Biography { get; set; }
}

public interface IPersonTranslations
{
    int Id { get; set; }
    IList<PersonTranslation>? Translations { get; set; }
}

public interface IPersonTvCreditsBase
{
    int Id { get; set; }
}

public interface IPersonTvCredits<TCast, TCrew> : IPersonTvCreditsBase
    where TCast : ICombinedCastCredit
    where TCrew : ICombinedCrewCredit
{
    IList<TCast>? Cast { get; set; }
    IList<TCrew>? Crew { get; set; }
}

public interface ITaggedImage<TMedia>
    where TMedia : ICombinedCastCredit
{
    string? Id { get; set; }
    string? FilePath { get; set; }
    int Width { get; set; }
    int Height { get; set; }
    double AspectRatio { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    string? Iso639_1 { get; set; }
    string? ImageType { get; set; }
    TmdbEnum<MediaType> MediaType { get; set; }
    TMedia? Media { get; set; }
}

