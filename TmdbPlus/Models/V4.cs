using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

// The v4 surface. No audit data covers this -- every path in ops.json is v3 -- so these shapes
// come from TMDB's v4 documentation, and nullability follows the same standing rule: no prior
// and no observed non-null means nullable.
//
// v4 lists are TMDB's recommended standard: they hold movies AND series in one list, support
// private lists, per-item comments, and richer sorting. v3 lists do none of that.

/// <summary>
/// A v4 page. Unlike v3 it carries <c>total_pages</c>/<c>total_results</c> alongside its own
/// per-endpoint fields, so it stays separate from <see cref="PagedResult{T}"/>.
/// </summary>
public class V4PagedResult<T>
{
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("results")] public IList<T>? Results { get; set; }
    [JsonPropertyName("total_pages")] public int TotalPages { get; set; }
    [JsonPropertyName("total_results")] public int TotalResults { get; set; }
}

// ---------------------------------------------------------------------------
// Authentication (v4 is a three-step flow, unlike v3's token/session pair)
// ---------------------------------------------------------------------------

/// <summary>Step 1: a request token the user then approves in a browser.</summary>
public class V4RequestToken
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("status_code")] public int StatusCode { get; set; }
    [JsonPropertyName("status_message")] public string? StatusMessage { get; set; }
    [JsonPropertyName("request_token")] public string? RequestToken { get; set; }
}

/// <summary>
/// Step 2: the access token, exchanged for an approved request token. This is a user-scoped
/// bearer token — distinct from the application read access token the client is configured with.
/// </summary>
public class V4AccessToken
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("status_code")] public int StatusCode { get; set; }
    [JsonPropertyName("status_message")] public string? StatusMessage { get; set; }
    [JsonPropertyName("access_token")] public string? AccessToken { get; set; }

    /// <summary>The account id v4 endpoints are keyed by — a string, not the v3 integer id.</summary>
    [JsonPropertyName("account_id")] public string? AccountId { get; set; }
}

/// <summary>The shared v4 response envelope.</summary>
public class V4StatusResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("status_code")] public int StatusCode { get; set; }
    [JsonPropertyName("status_message")] public string? StatusMessage { get; set; }
}

// ---------------------------------------------------------------------------
// Lists
// ---------------------------------------------------------------------------

/// <summary>
/// A v4 list. Items may be movies or series, so each carries its own media type.
/// </summary>
public class V4ListDetails
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("public")] public bool Public { get; set; }
    [JsonPropertyName("revenue")] public long? Revenue { get; set; }
    [JsonPropertyName("runtime")] public int? Runtime { get; set; }
    [JsonPropertyName("sort_by")] public string? SortBy { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("average_rating")] public double? AverageRating { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("comments")] public IDictionary<string, string?>? Comments { get; set; }
    [JsonPropertyName("created_by")] public V4ListOwner? CreatedBy { get; set; }
    [JsonPropertyName("object_ids")] public IDictionary<string, string>? ObjectIds { get; set; }

    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("results")] public IList<MultiSearchResult>? Results { get; set; }
    [JsonPropertyName("total_pages")] public int TotalPages { get; set; }
    [JsonPropertyName("total_results")] public int TotalResults { get; set; }
}

public class V4ListOwner
{
    [JsonPropertyName("gravatar_hash")] public string? GravatarHash { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("avatar_path")] public string? AvatarPath { get; set; }
}

/// <summary>A list in the account's own listing, without its items.</summary>
public class V4ListSummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("public")] public bool Public { get; set; }
    [JsonPropertyName("number_of_items")] public int NumberOfItems { get; set; }
    [JsonPropertyName("featured")] public int Featured { get; set; }
    [JsonPropertyName("revenue")] public long? Revenue { get; set; }
    [JsonPropertyName("runtime")] public int? Runtime { get; set; }
    [JsonPropertyName("sort_by")] public int? SortBy { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("average_rating")] public double? AverageRating { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }

    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(TmdbDateTimeOffsetConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
}

/// <summary>The response to creating a list, carrying the new id.</summary>
public class V4CreateListResponse : V4StatusResponse
{
    [JsonPropertyName("id")] public int Id { get; set; }
}

/// <summary>Per-item results, so a partial failure names the item that failed.</summary>
public class V4ListItemsResponse : V4StatusResponse
{
    [JsonPropertyName("results")] public IList<V4ItemResult>? Results { get; set; }
}

public class V4ItemResult
{
    [JsonPropertyName("media_id")] public int MediaId { get; set; }
    [JsonPropertyName("media_type")] public string? MediaType { get; set; }
    [JsonPropertyName("success")] public bool Success { get; set; }
}

/// <summary>Whether a given item is on a v4 list.</summary>
public class V4ItemStatus
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("media_type")] public string? MediaType { get; set; }
    [JsonPropertyName("media_id")] public int MediaId { get; set; }
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("status_code")] public int StatusCode { get; set; }
    [JsonPropertyName("status_message")] public string? StatusMessage { get; set; }
}

// ---------------------------------------------------------------------------
// Request bodies
// ---------------------------------------------------------------------------

/// <summary>An item to add to, update in, or remove from a v4 list.</summary>
public sealed class V4ListItem
{
    public V4ListItem() { }

    public V4ListItem(MediaType mediaType, int mediaId, string? comment = null)
    {
        MediaType = mediaType == Models.MediaType.Tv ? "tv" : "movie";
        MediaId = mediaId;
        Comment = comment;
    }

    [JsonPropertyName("media_type")] public string MediaType { get; set; } = "movie";
    [JsonPropertyName("media_id")] public int MediaId { get; set; }

    /// <summary>Per-item note. A v4 feature with no v3 equivalent.</summary>
    [JsonPropertyName("comment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Comment { get; set; }
}

public sealed class V4CreateListRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("iso_639_1")] public string Iso639_1 { get; set; } = "en";
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("public")] public bool Public { get; set; } = true;
}

public sealed class V4UpdateListRequest
{
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("public")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Public { get; set; }

    [JsonPropertyName("sort_by")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SortBy { get; set; }
}
