using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

// v4 is entirely hand-written -- the OpenAPI spec covers only v3 (issue #9 / the map's notes).
//
// Every user-scoped v4 call takes the user's access token as an explicit parameter, for the same
// reason v3 sessions are explicit (issue #5): the client stays immutable and singleton-safe. The
// token overrides the configured application token for that one request.

/// <summary>The v4 <c>/auth</c> flow.</summary>
public interface IV4AuthenticationEndpoints
{
    /// <summary>
    /// Step 1: create a request token. Send the user to
    /// <c>https://www.themoviedb.org/auth/access?request_token={token}</c> to approve it.
    /// </summary>
    Task<V4RequestToken> CreateRequestTokenAsync(string? redirectTo = null, CancellationToken cancellationToken = default);

    /// <summary>Step 2: exchange an approved request token for a user access token.</summary>
    Task<V4AccessToken> CreateAccessTokenAsync(string requestToken, CancellationToken cancellationToken = default);

    /// <summary>Invalidates a user access token.</summary>
    Task<V4StatusResponse> DeleteAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default);
}

/// <summary>
/// The v4 <c>/account</c> endpoints. Keyed by the string <c>account_object_id</c> from the access
/// token response, not the v3 integer account id.
/// </summary>
public interface IV4AccountEndpoints
{
    Task<V4PagedResult<V4ListSummary>> GetListsAsync(string accountObjectId, string accessToken, int? page = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<MovieSummary>> GetFavoriteMoviesAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<TvSeriesSummary>> GetFavoriteTvAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<MovieSummary>> GetWatchlistMoviesAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<TvSeriesSummary>> GetWatchlistTvAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<MovieSummary>> GetRatedMoviesAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<TvSeriesSummary>> GetRatedTvAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<MovieSummary>> GetMovieRecommendationsAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, CancellationToken cancellationToken = default);
    Task<V4PagedResult<TvSeriesSummary>> GetTvRecommendationsAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// The v4 <c>/list</c> endpoints — TMDB's recommended list API. Unlike v3 these hold movies and
/// series in one list, can be private, and carry a per-item comment.
/// </summary>
public interface IV4ListEndpoints
{
    /// <summary>Reading a public list needs no user token.</summary>
    Task<V4ListDetails> GetAsync(int listId, int? page = null, string? language = null,
        string? sortBy = null, string? accessToken = null, CancellationToken cancellationToken = default);

    Task<V4CreateListResponse> CreateAsync(string accessToken, string name, string? description = null,
        bool isPublic = true, string language = "en", string? country = null, CancellationToken cancellationToken = default);

    Task<V4StatusResponse> UpdateAsync(int listId, string accessToken, V4UpdateListRequest update, CancellationToken cancellationToken = default);
    Task<V4StatusResponse> DeleteAsync(int listId, string accessToken, CancellationToken cancellationToken = default);
    Task<V4StatusResponse> ClearAsync(int listId, string accessToken, CancellationToken cancellationToken = default);

    /// <summary>Adds items in bulk; the response reports success per item.</summary>
    Task<V4ListItemsResponse> AddItemsAsync(int listId, string accessToken, IEnumerable<V4ListItem> items, CancellationToken cancellationToken = default);

    /// <summary>Updates the per-item comments.</summary>
    Task<V4ListItemsResponse> UpdateItemsAsync(int listId, string accessToken, IEnumerable<V4ListItem> items, CancellationToken cancellationToken = default);

    Task<V4ListItemsResponse> RemoveItemsAsync(int listId, string accessToken, IEnumerable<V4ListItem> items, CancellationToken cancellationToken = default);

    /// <summary>
    /// Whether an item is on the list. TMDB answers <b>404 with status_code 34</b> when it is
    /// not, rather than <c>success: false</c>, so an absent item surfaces as
    /// <see cref="TmdbApiException"/>.
    /// </summary>
    Task<V4ItemStatus> GetItemStatusAsync(int listId, MediaType mediaType, int mediaId, CancellationToken cancellationToken = default);
}

internal sealed class V4AuthenticationEndpoints(TmdbClient client) : IV4AuthenticationEndpoints
{
    public Task<V4RequestToken> CreateRequestTokenAsync(string? redirectTo = null, CancellationToken cancellationToken = default)
        => client.PostAsync<V4RequestToken>("4/auth/request_token", new QueryString(),
            new { redirect_to = redirectTo }, cancellationToken);

    public Task<V4AccessToken> CreateAccessTokenAsync(string requestToken, CancellationToken cancellationToken = default)
        => client.PostAsync<V4AccessToken>("4/auth/access_token", new QueryString(),
            new { request_token = requestToken }, cancellationToken);

    public Task<V4StatusResponse> DeleteAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default)
        => client.DeleteAsync<V4StatusResponse>("4/auth/access_token", new QueryString(), cancellationToken,
            new { access_token = accessToken });
}

internal sealed class V4AccountEndpoints(TmdbClient client) : IV4AccountEndpoints
{
    public Task<V4PagedResult<V4ListSummary>> GetListsAsync(string accountObjectId, string accessToken, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<V4ListSummary>>($"4/account/{accountObjectId}/lists",
            new QueryString().Add("page", page), cancellationToken, accessToken);

    public Task<V4PagedResult<MovieSummary>> GetFavoriteMoviesAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<MovieSummary>>($"4/account/{accountObjectId}/movie/favorites",
            Listing(page, language, sortBy), cancellationToken, accessToken);

    public Task<V4PagedResult<TvSeriesSummary>> GetFavoriteTvAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<TvSeriesSummary>>($"4/account/{accountObjectId}/tv/favorites",
            Listing(page, language, sortBy), cancellationToken, accessToken);

    public Task<V4PagedResult<MovieSummary>> GetWatchlistMoviesAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<MovieSummary>>($"4/account/{accountObjectId}/movie/watchlist",
            Listing(page, language, sortBy), cancellationToken, accessToken);

    public Task<V4PagedResult<TvSeriesSummary>> GetWatchlistTvAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<TvSeriesSummary>>($"4/account/{accountObjectId}/tv/watchlist",
            Listing(page, language, sortBy), cancellationToken, accessToken);

    public Task<V4PagedResult<MovieSummary>> GetRatedMoviesAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<MovieSummary>>($"4/account/{accountObjectId}/movie/rated",
            Listing(page, language, sortBy), cancellationToken, accessToken);

    public Task<V4PagedResult<TvSeriesSummary>> GetRatedTvAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, string? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<TvSeriesSummary>>($"4/account/{accountObjectId}/tv/rated",
            Listing(page, language, sortBy), cancellationToken, accessToken);

    public Task<V4PagedResult<MovieSummary>> GetMovieRecommendationsAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<MovieSummary>>($"4/account/{accountObjectId}/movie/recommendations",
            Listing(page, language, null), cancellationToken, accessToken);

    public Task<V4PagedResult<TvSeriesSummary>> GetTvRecommendationsAsync(string accountObjectId, string accessToken, int? page = null, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4PagedResult<TvSeriesSummary>>($"4/account/{accountObjectId}/tv/recommendations",
            Listing(page, language, null), cancellationToken, accessToken);

    QueryString Listing(int? page, string? language, string? sortBy) => new QueryString()
        .Add("page", page)
        .Add("language", language ?? client.DefaultLanguage)
        .Add("sort_by", sortBy);
}

internal sealed class V4ListEndpoints(TmdbClient client) : IV4ListEndpoints
{
    public Task<V4ListDetails> GetAsync(int listId, int? page = null, string? language = null,
        string? sortBy = null, string? accessToken = null, CancellationToken cancellationToken = default)
        => client.GetAsync<V4ListDetails>($"4/list/{listId}", new QueryString()
            .Add("page", page)
            .Add("language", language ?? client.DefaultLanguage)
            .Add("sort_by", sortBy), cancellationToken, accessToken);

    public Task<V4CreateListResponse> CreateAsync(string accessToken, string name, string? description = null,
        bool isPublic = true, string language = "en", string? country = null, CancellationToken cancellationToken = default)
        => client.PostAsync<V4CreateListResponse>("4/list", new QueryString(), new V4CreateListRequest
        {
            Name = name,
            Description = description,
            Public = isPublic,
            Iso639_1 = language,
            Iso3166_1 = country,
        }, cancellationToken, accessToken);

    public Task<V4StatusResponse> UpdateAsync(int listId, string accessToken, V4UpdateListRequest update, CancellationToken cancellationToken = default)
        => client.PutAsync<V4StatusResponse>($"4/list/{listId}", new QueryString(), update, cancellationToken, accessToken);

    public Task<V4StatusResponse> DeleteAsync(int listId, string accessToken, CancellationToken cancellationToken = default)
        => client.DeleteAsync<V4StatusResponse>($"4/list/{listId}", new QueryString(), cancellationToken, null, accessToken);

    public Task<V4StatusResponse> ClearAsync(int listId, string accessToken, CancellationToken cancellationToken = default)
        => client.GetAsync<V4StatusResponse>($"4/list/{listId}/clear", new QueryString(), cancellationToken, accessToken);

    public Task<V4ListItemsResponse> AddItemsAsync(int listId, string accessToken, IEnumerable<V4ListItem> items, CancellationToken cancellationToken = default)
        => client.PostAsync<V4ListItemsResponse>($"4/list/{listId}/items", new QueryString(),
            new { items = items.ToArray() }, cancellationToken, accessToken);

    public Task<V4ListItemsResponse> UpdateItemsAsync(int listId, string accessToken, IEnumerable<V4ListItem> items, CancellationToken cancellationToken = default)
        => client.PutAsync<V4ListItemsResponse>($"4/list/{listId}/items", new QueryString(),
            new { items = items.ToArray() }, cancellationToken, accessToken);

    public Task<V4ListItemsResponse> RemoveItemsAsync(int listId, string accessToken, IEnumerable<V4ListItem> items, CancellationToken cancellationToken = default)
        => client.DeleteAsync<V4ListItemsResponse>($"4/list/{listId}/items", new QueryString(), cancellationToken,
            new { items = items.ToArray() }, accessToken);

    public Task<V4ItemStatus> GetItemStatusAsync(int listId, MediaType mediaType, int mediaId, CancellationToken cancellationToken = default)
        => client.GetAsync<V4ItemStatus>($"4/list/{listId}/item_status", new QueryString()
            .Add("media_type", mediaType == MediaType.Tv ? "tv" : "movie")
            .Add("media_id", mediaId), cancellationToken);
}
