using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

public interface IAccountDetails
{
    int Id { get; set; }
    string? Username { get; set; }
    string? Name { get; set; }
    bool IncludeAdult { get; set; }
    string? Iso639_1 { get; set; }
    string? Iso3166_1 { get; set; }
}

public class AccountDetails : IAccountDetails
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("include_adult")] public bool IncludeAdult { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("avatar")] public AccountAvatar? Avatar { get; set; }
}

public class AccountAvatar
{
    [JsonPropertyName("gravatar")] public GravatarInfo? Gravatar { get; set; }
    [JsonPropertyName("tmdb")] public TmdbAvatarInfo? Tmdb { get; set; }
}

public class GravatarInfo
{
    [JsonPropertyName("hash")] public string? Hash { get; set; }
}

public class TmdbAvatarInfo
{
    [JsonPropertyName("avatar_path")] public string? AvatarPath { get; set; }
}

/// <summary>How an account's own lists are sorted. Only created/date ordering is accepted.</summary>
public enum AccountSortBy
{
    CreatedAtAsc = 0,
    CreatedAtDesc,
}

internal static class AccountSortByExtensions
{
    internal static string ToWire(this AccountSortBy s)
        => s == AccountSortBy.CreatedAtDesc ? "created_at.desc" : "created_at.asc";
}

// ---------------------------------------------------------------------------
// Request bodies. The spec declares every one as RAW_BODY: string, so these shapes come from
// TMDbLib and were confirmed live (issue #8).
// ---------------------------------------------------------------------------

/// <summary>Marks a movie or series as a favourite, or clears the mark.</summary>
public sealed class FavoriteRequest
{
    [JsonPropertyName("media_type")] public string MediaType { get; set; } = "movie";
    [JsonPropertyName("media_id")] public int MediaId { get; set; }
    [JsonPropertyName("favorite")] public bool Favorite { get; set; }
}

/// <summary>Adds a movie or series to the watchlist, or removes it.</summary>
public sealed class WatchlistRequest
{
    [JsonPropertyName("media_type")] public string MediaType { get; set; } = "movie";
    [JsonPropertyName("media_id")] public int MediaId { get; set; }
    [JsonPropertyName("watchlist")] public bool Watchlist { get; set; }
}

// ---------------------------------------------------------------------------
// Authentication
// ---------------------------------------------------------------------------

/// <summary>A request token, the first step of the v3 login flow.</summary>
public class RequestToken
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("request_token")] public string? Token { get; set; }

    [JsonPropertyName("expires_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? ExpiresAt { get; set; }
}

/// <summary>A user session, the result of approving a request token.</summary>
public class SessionResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("session_id")] public string? SessionId { get; set; }

    /// <summary>Convenience: the same id as the typed session the write ops take.</summary>
    [JsonIgnore] public Auth.UserSession Session => new(SessionId ?? string.Empty);
}

/// <summary>A guest session. Needs no user credentials and can rate (issue #8).</summary>
public class GuestSessionResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("guest_session_id")] public string? GuestSessionId { get; set; }

    [JsonPropertyName("expires_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? ExpiresAt { get; set; }

    [JsonIgnore] public Auth.GuestSession Session => new(GuestSessionId ?? string.Empty);
}

// ---------------------------------------------------------------------------
// Lists (v3)
// ---------------------------------------------------------------------------

public class ListDetails
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("item_count")] public int ItemCount { get; set; }
    [JsonPropertyName("favorite_count")] public int FavoriteCount { get; set; }
    [JsonPropertyName("created_by")] public string? CreatedBy { get; set; }

    /// <summary>List items are movies; v3 lists cannot hold series.</summary>
    [JsonPropertyName("items")] public IList<MovieSummary>? Items { get; set; }

    [JsonPropertyName("page")] public int? Page { get; set; }
    [JsonPropertyName("total_pages")] public int? TotalPages { get; set; }
    [JsonPropertyName("total_results")] public int? TotalResults { get; set; }
}

/// <summary>Whether a given movie is already on a list.</summary>
public class ListItemStatus
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("item_present")] public bool ItemPresent { get; set; }
}

/// <summary>The response to creating a list, which carries the new id.</summary>
public class CreateListResponse : TmdbStatusResponse
{
    [JsonPropertyName("list_id")] public int ListId { get; set; }
}

public sealed class CreateListRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }

    /// <summary>TMDB requires this; it defaults to English rather than the client's language.</summary>
    [JsonPropertyName("language")] public string Language { get; set; } = "en";
}

public sealed class ListItemRequest
{
    [JsonPropertyName("media_id")] public int MediaId { get; set; }
}
