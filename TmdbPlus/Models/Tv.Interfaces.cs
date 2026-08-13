using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract for the response types in Tv.cs: settable members a consuming app can
// implement on its own EF entities, so mapping is written once per interface (issue #4).
//
// A nested collection is generic in its element type ONLY where that element is entity-like --
// something a consumer would persist as its own row. IList<T> is not covariant and EF cannot map
// an explicit interface implementation, so those would otherwise force a shadow property.
// Envelope properties are generic in the envelope type via a non-generic marker (issue #18).
//
// Hand-maintained: keep in sync with the response classes in the matching .cs file.

public interface IAggregateCastMember<TRoles>
    where TRoles : IAggregateRole
{
    int Id { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    string? KnownForDepartment { get; set; }
    string? ProfilePath { get; set; }
    int Gender { get; set; }
    bool Adult { get; set; }
    double Popularity { get; set; }
    int? Order { get; set; }
    int TotalEpisodeCount { get; set; }
    IList<TRoles>? Roles { get; set; }
}

public interface IAggregateCreditsBase
{
    int Id { get; set; }
}

public interface IAggregateCredits<TCast, TCrew> : IAggregateCreditsBase
    where TCast : IAggregateCastMember<AggregateRole>
    where TCrew : IAggregateCrewMember<AggregateJob>
{
    IList<TCast>? Cast { get; set; }
    IList<TCrew>? Crew { get; set; }
}

public interface IAggregateCrewMember<TJobs>
    where TJobs : IAggregateJob
{
    int Id { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    string? KnownForDepartment { get; set; }
    string? ProfilePath { get; set; }
    int Gender { get; set; }
    bool Adult { get; set; }
    double Popularity { get; set; }
    int TotalEpisodeCount { get; set; }
    TmdbEnum<CreditDepartment>? Department { get; set; }
    IList<TJobs>? Jobs { get; set; }
}

public interface IAggregateJob
{
    string? CreditId { get; set; }
    string? Job { get; set; }
    int EpisodeCount { get; set; }
}

public interface IAggregateRole
{
    string? CreditId { get; set; }
    string? Character { get; set; }
    int EpisodeCount { get; set; }
}

public interface IContentRating
{
    string? Iso3166_1 { get; set; }
    string? Rating { get; set; }
    IList<string>? Descriptors { get; set; }
}

public interface IEpisodeCreditsBase : ICreditsBase;

public interface IEpisodeCredits<TGuestStars> : IEpisodeCreditsBase
    where TGuestStars : ICastMember
{
    IList<TGuestStars>? GuestStars { get; set; }
}

public interface IEpisodeGroup<TEpisodes>
    where TEpisodes : ITvEpisodeDetailsBase
{
    string? Id { get; set; }
    string? Name { get; set; }
    int? Order { get; set; }
    bool Locked { get; set; }
    IList<TEpisodes>? Episodes { get; set; }
}

public interface IEpisodeGroupDetails<TNetwork, TGroups>
    where TNetwork : INetwork
    where TGroups : IEpisodeGroup<TvEpisodeDetails>
{
    string? Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    int EpisodeCount { get; set; }
    int GroupCount { get; set; }
    int? Type { get; set; }
    TNetwork? Network { get; set; }
    IList<TGroups>? Groups { get; set; }
}

public interface IEpisodeGroupSummary<TNetwork>
    where TNetwork : INetwork
{
    string? Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    int EpisodeCount { get; set; }
    int GroupCount { get; set; }
    int? Type { get; set; }
    TNetwork? Network { get; set; }
}

public interface IEpisodeSummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Overview { get; set; }
    string? StillPath { get; set; }
    string? ProductionCode { get; set; }
    int EpisodeNumber { get; set; }
    int SeasonNumber { get; set; }
    int ShowId { get; set; }
    int? Runtime { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    TmdbEnum<EpisodeType>? EpisodeType { get; set; }
    DateOnly? AirDate { get; set; }
}

public interface INetwork
{
    int Id { get; set; }
    string? Name { get; set; }
    string? LogoPath { get; set; }
    string? OriginCountry { get; set; }
    string? Headquarters { get; set; }
    string? Homepage { get; set; }
}

public interface IScreenedTheatrically
{
    int Id { get; set; }
    int EpisodeNumber { get; set; }
    int SeasonNumber { get; set; }
}

public interface ISeasonSummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    int SeasonNumber { get; set; }
    int EpisodeCount { get; set; }
    double VoteAverage { get; set; }
    DateOnly? AirDate { get; set; }
}

public interface ISeriesCreator
{
    int Id { get; set; }
    string? CreditId { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    int Gender { get; set; }
    string? ProfilePath { get; set; }
}

public interface ITvAlternativeTitles
{
    int Id { get; set; }
    IList<AlternativeTitle>? Results { get; set; }
}

public interface ITvExternalIds
{
    int Id { get; set; }
    string? ImdbId { get; set; }
    int? TvdbId { get; set; }
    int? TvrageId { get; set; }
    string? WikidataId { get; set; }
    string? FacebookId { get; set; }
    string? InstagramId { get; set; }
    string? TwitterId { get; set; }
    string? FreebaseMid { get; set; }
    string? FreebaseId { get; set; }
}

public interface ITvKeywordsBase
{
    int Id { get; set; }
}

public interface ITvKeywords<TResults> : ITvKeywordsBase
    where TResults : IKeyword
{
    IList<TResults>? Results { get; set; }
}

public interface ITvTranslation
{
    string? Iso3166_1 { get; set; }
    string? Iso639_1 { get; set; }
    string? Name { get; set; }
    string? EnglishName { get; set; }
    TvTranslationData? Data { get; set; }
}

public interface ITvTranslationData
{
    string? Name { get; set; }
    string? Overview { get; set; }
    string? Tagline { get; set; }
    string? Homepage { get; set; }
}

public interface ITvTranslations
{
    int Id { get; set; }
    IList<TvTranslation>? Translations { get; set; }
}

