using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract for the response types in Movies.cs: settable members a consuming app can
// implement on its own EF entities, so mapping is written once per interface (issue #4).
//
// A nested collection is generic in its element type ONLY where that element is entity-like --
// something a consumer would persist as its own row. IList<T> is not covariant and EF cannot map
// an explicit interface implementation, so those would otherwise force a shadow property.
// Envelope properties are generic in the envelope type via a non-generic marker (issue #18).
//
// Hand-maintained: keep in sync with the response classes in the matching .cs file.

public interface IChangesResult
{
    IList<ChangeGroup>? Changes { get; set; }
}

public interface ICountryReleaseDates
{
    string? Iso3166_1 { get; set; }
    IList<ReleaseDateEntry>? ReleaseDates { get; set; }
}

public interface IDateRange
{
    DateOnly? Minimum { get; set; }
    DateOnly? Maximum { get; set; }
}

public interface IDatedMoviePage
{
    DateRange? Dates { get; set; }
}

public interface IMovieAlternativeTitles
{
    int Id { get; set; }
    IList<AlternativeTitle>? Titles { get; set; }
}

public interface IMovieChangeEntry
{
    int Id { get; set; }
    bool? Adult { get; set; }
}

public interface IMovieExternalIds
{
    int Id { get; set; }
    string? ImdbId { get; set; }
    string? WikidataId { get; set; }
    string? FacebookId { get; set; }
    string? InstagramId { get; set; }
    string? TwitterId { get; set; }
}

public interface IMovieKeywordsBase
{
    int Id { get; set; }
}

public interface IMovieKeywords<TKeywords> : IMovieKeywordsBase
    where TKeywords : IKeyword
{
    IList<TKeywords>? Keywords { get; set; }
}

public interface IMovieTranslations
{
    int Id { get; set; }
    IList<Translation>? Translations { get; set; }
}

public interface IReleaseDateEntry
{
    string? Certification { get; set; }
    DateTimeOffset? ReleaseDate { get; set; }
    TmdbEnum<ReleaseType> Type { get; set; }
    string? Note { get; set; }
    string? Iso639_1 { get; set; }
    IList<string>? Descriptors { get; set; }
}

