using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract for the response types in V4.cs: settable members a consuming app can
// implement on its own EF entities, so mapping is written once per interface (issue #4).
//
// A nested collection is generic in its element type ONLY where that element is entity-like --
// something a consumer would persist as its own row. IList<T> is not covariant and EF cannot map
// an explicit interface implementation, so those would otherwise force a shadow property.
// Block wrappers and envelopes stay concrete: a consumer stores the keywords, not the wrapper.
//
// Hand-maintained: keep in sync with the response classes in the matching .cs file.

public interface IV4AccessToken
{
    bool Success { get; set; }
    int StatusCode { get; set; }
    string? StatusMessage { get; set; }
    string? AccessToken { get; set; }
    string? AccountId { get; set; }
}

public interface IV4CreateListResponse
{
    int Id { get; set; }
}

public interface IV4ItemResult
{
    int MediaId { get; set; }
    string? MediaType { get; set; }
    bool Success { get; set; }
}

public interface IV4ItemStatus
{
    int Id { get; set; }
    string? MediaType { get; set; }
    int MediaId { get; set; }
    bool Success { get; set; }
    int StatusCode { get; set; }
    string? StatusMessage { get; set; }
}

public interface IV4ListDetails<TResults>
    where TResults : IMultiSearchResult<CombinedCastCredit>
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    bool Public { get; set; }
    long? Revenue { get; set; }
    int? Runtime { get; set; }
    string? SortBy { get; set; }
    string? Iso639_1 { get; set; }
    string? Iso3166_1 { get; set; }
    double? AverageRating { get; set; }
    string? BackdropPath { get; set; }
    string? PosterPath { get; set; }
    V4ListOwner? CreatedBy { get; set; }
    int Page { get; set; }
    IList<TResults>? Results { get; set; }
    int TotalPages { get; set; }
    int TotalResults { get; set; }
}

public interface IV4ListItemsResponse<TResults>
    where TResults : IV4ItemResult
{
    IList<TResults>? Results { get; set; }
}

public interface IV4ListOwner
{
    string? GravatarHash { get; set; }
    string? Name { get; set; }
    string? Username { get; set; }
    string? AvatarPath { get; set; }
}

public interface IV4ListSummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    bool Public { get; set; }
    int NumberOfItems { get; set; }
    int Featured { get; set; }
    long? Revenue { get; set; }
    int? Runtime { get; set; }
    int? SortBy { get; set; }
    string? Iso639_1 { get; set; }
    string? Iso3166_1 { get; set; }
    double? AverageRating { get; set; }
    string? BackdropPath { get; set; }
    string? PosterPath { get; set; }
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}

public interface IV4RequestToken
{
    bool Success { get; set; }
    int StatusCode { get; set; }
    string? StatusMessage { get; set; }
    string? RequestToken { get; set; }
}

public interface IV4StatusResponse
{
    bool Success { get; set; }
    int StatusCode { get; set; }
    string? StatusMessage { get; set; }
}

