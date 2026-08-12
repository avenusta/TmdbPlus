using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The schema contract for the response types in Account.cs: settable members a consuming app can
// implement on its own EF entities, so mapping is written once per interface (issue #4).
//
// A nested collection is generic in its element type ONLY where that element is entity-like --
// something a consumer would persist as its own row. IList<T> is not covariant and EF cannot map
// an explicit interface implementation, so those would otherwise force a shadow property.
// Block wrappers and envelopes stay concrete: a consumer stores the keywords, not the wrapper.
//
// Hand-maintained: keep in sync with the response classes in the matching .cs file.

public interface IAccountAvatar
{
    GravatarInfo? Gravatar { get; set; }
    TmdbAvatarInfo? Tmdb { get; set; }
}

public interface ICreateListResponse
{
    int ListId { get; set; }
}

public interface IGravatarInfo
{
    string? Hash { get; set; }
}

public interface IGuestSessionResponse
{
    bool Success { get; set; }
    string? GuestSessionId { get; set; }
    DateTimeOffset? ExpiresAt { get; set; }
}

public interface IListDetails<TItems>
    where TItems : IMovieSummary
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Description { get; set; }
    string? PosterPath { get; set; }
    string? Iso639_1 { get; set; }
    int ItemCount { get; set; }
    int FavoriteCount { get; set; }
    string? CreatedBy { get; set; }
    IList<TItems>? Items { get; set; }
    int? Page { get; set; }
    int? TotalPages { get; set; }
    int? TotalResults { get; set; }
}

public interface IListItemStatus
{
    string? Id { get; set; }
    bool ItemPresent { get; set; }
}

public interface IRequestToken
{
    bool Success { get; set; }
    string? Token { get; set; }
    DateTimeOffset? ExpiresAt { get; set; }
}

public interface ISessionResponse
{
    bool Success { get; set; }
    string? SessionId { get; set; }
}

public interface ITmdbAvatarInfo
{
    string? AvatarPath { get; set; }
}

