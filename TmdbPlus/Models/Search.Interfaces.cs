using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract for the response types in Search.cs: settable members a consuming app can
// implement on its own EF entities, so mapping is written once per interface (issue #4).
//
// A nested collection is generic in its element type ONLY where that element is entity-like --
// something a consumer would persist as its own row. IList<T> is not covariant and EF cannot map
// an explicit interface implementation, so those would otherwise force a shadow property.
// Block wrappers and envelopes stay concrete: a consumer stores the keywords, not the wrapper.
//
// Hand-maintained: keep in sync with the response classes in the matching .cs file.

public interface ICollectionSummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    string? OriginalLanguage { get; set; }
    string? OriginalName { get; set; }
    bool Adult { get; set; }
}

public interface ICompanySummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? LogoPath { get; set; }
    string? OriginCountry { get; set; }
}

public interface IFindResults<TMovieResults, TTvResults, TPersonResults, TTvSeasonResults, TTvEpisodeResults>
    where TMovieResults : IMovieSummary
    where TTvResults : ITvSeriesSummary
    where TPersonResults : IPersonSummary<CombinedCastCredit>
    where TTvSeasonResults : ISeasonSummary
    where TTvEpisodeResults : IEpisodeSummary
{
    IList<TMovieResults>? MovieResults { get; set; }
    IList<TTvResults>? TvResults { get; set; }
    IList<TPersonResults>? PersonResults { get; set; }
    IList<TTvSeasonResults>? TvSeasonResults { get; set; }
    IList<TTvEpisodeResults>? TvEpisodeResults { get; set; }
}

public interface IMultiSearchResult<TKnownFor>
    where TKnownFor : ICombinedCastCredit
{
    int Id { get; set; }
    bool Adult { get; set; }
    double Popularity { get; set; }
    TmdbEnum<MediaType> MediaType { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    string? OriginalLanguage { get; set; }
    IList<int>? GenreIds { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    string? Title { get; set; }
    string? OriginalTitle { get; set; }
    bool? Video { get; set; }
    DateOnly? ReleaseDate { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    IList<string>? OriginCountry { get; set; }
    DateOnly? FirstAirDate { get; set; }
    int? Gender { get; set; }
    string? KnownForDepartment { get; set; }
    string? ProfilePath { get; set; }
    IList<TKnownFor>? KnownFor { get; set; }
}

