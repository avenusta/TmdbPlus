using TmdbPlus.Auth;
using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

/// <summary>
/// The <c>/account</c> endpoints. Every one is session-scoped, so the session is an explicit
/// parameter rather than client state (issue #5).
/// </summary>
public interface IAccountEndpoints
{
    Task<AccountDetails> GetAsync(int accountId, UserSession session, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetFavoriteMoviesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> GetFavoriteTvAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetWatchlistMoviesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> GetWatchlistTvAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetRatedMoviesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> GetRatedTvAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvEpisodeDetails>> GetRatedTvEpisodesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<ListSummary>> GetListsAsync(int accountId, UserSession session, int? page = null, CancellationToken cancellationToken = default);

    Task<TmdbStatusResponse> SetFavoriteAsync(int accountId, UserSession session, MediaType mediaType, int mediaId,
        bool favorite, CancellationToken cancellationToken = default);

    Task<TmdbStatusResponse> SetWatchlistAsync(int accountId, UserSession session, MediaType mediaType, int mediaId,
        bool watchlist, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/guest_session</c> endpoints — what a guest has rated.</summary>
public interface IGuestSessionEndpoints
{
    Task<PagedResult<MovieSummary>> GetRatedMoviesAsync(GuestSession session, string? language = null,
        int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> GetRatedTvAsync(GuestSession session, string? language = null,
        int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvEpisodeDetails>> GetRatedTvEpisodesAsync(GuestSession session, string? language = null,
        int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/authentication</c> endpoints — the v3 login flow.</summary>
public interface IAuthenticationEndpoints
{
    /// <summary>Confirms the configured token is accepted.</summary>
    Task<TmdbStatusResponse> ValidateAsync(CancellationToken cancellationToken = default);

    /// <summary>Step 1: get a request token for the user to approve.</summary>
    Task<RequestToken> CreateRequestTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>Step 2: exchange an approved request token for a session.</summary>
    Task<SessionResponse> CreateSessionAsync(string requestToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a request token with a username and password, skipping the browser step.
    /// TMDB discourages this; prefer the redirect flow where a browser is available.
    /// </summary>
    Task<RequestToken> ValidateWithLoginAsync(string username, string password, string requestToken,
        CancellationToken cancellationToken = default);

    /// <summary>Exchanges a v4 access token for a v3 session.</summary>
    Task<SessionResponse> CreateSessionFromV4TokenAsync(string accessToken, CancellationToken cancellationToken = default);

    /// <summary>A session that can rate without any user credentials (issue #8).</summary>
    Task<GuestSessionResponse> CreateGuestSessionAsync(CancellationToken cancellationToken = default);

    Task<TmdbStatusResponse> DeleteSessionAsync(UserSession session, CancellationToken cancellationToken = default);
}

internal sealed class AccountEndpoints(TmdbClient client) : IAccountEndpoints
{
    public Task<AccountDetails> GetAsync(int accountId, UserSession session, CancellationToken cancellationToken = default)
        => client.GetAsync<AccountDetails>($"3/account/{accountId}", Session(session), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetFavoriteMoviesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/account/{accountId}/favorite/movies",
            Listing(session, language, page, sortBy), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> GetFavoriteTvAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>($"3/account/{accountId}/favorite/tv",
            Listing(session, language, page, sortBy), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetWatchlistMoviesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/account/{accountId}/watchlist/movies",
            Listing(session, language, page, sortBy), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> GetWatchlistTvAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>($"3/account/{accountId}/watchlist/tv",
            Listing(session, language, page, sortBy), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetRatedMoviesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/account/{accountId}/rated/movies",
            Listing(session, language, page, sortBy), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> GetRatedTvAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>($"3/account/{accountId}/rated/tv",
            Listing(session, language, page, sortBy), cancellationToken);

    public Task<PagedResult<TvEpisodeDetails>> GetRatedTvEpisodesAsync(int accountId, UserSession session,
        string? language = null, int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvEpisodeDetails>>($"3/account/{accountId}/rated/tv/episodes",
            Listing(session, language, page, sortBy), cancellationToken);

    public Task<PagedResult<ListSummary>> GetListsAsync(int accountId, UserSession session, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<ListSummary>>($"3/account/{accountId}/lists",
            Session(session).Add("page", page), cancellationToken);

    public Task<TmdbStatusResponse> SetFavoriteAsync(int accountId, UserSession session, MediaType mediaType, int mediaId,
        bool favorite, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/account/{accountId}/favorite", Session(session),
            new FavoriteRequest { MediaType = Wire(mediaType), MediaId = mediaId, Favorite = favorite }, cancellationToken);

    public Task<TmdbStatusResponse> SetWatchlistAsync(int accountId, UserSession session, MediaType mediaType, int mediaId,
        bool watchlist, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/account/{accountId}/watchlist", Session(session),
            new WatchlistRequest { MediaType = Wire(mediaType), MediaId = mediaId, Watchlist = watchlist }, cancellationToken);

    static QueryString Session(UserSession session) => new QueryString().Add("session_id", session.SessionId);

    QueryString Listing(UserSession session, string? language, int? page, AccountSortBy? sortBy)
        => client.Page(language, page)
            .Add("session_id", session.SessionId)
            .Add("sort_by", sortBy?.ToWire());

    /// <summary>Favourite and watchlist accept only <c>movie</c> and <c>tv</c>.</summary>
    static string Wire(MediaType t) => t == MediaType.Tv ? "tv" : "movie";
}

internal sealed class GuestSessionEndpoints(TmdbClient client) : IGuestSessionEndpoints
{
    public Task<PagedResult<MovieSummary>> GetRatedMoviesAsync(GuestSession session, string? language = null,
        int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/guest_session/{session.GuestSessionId}/rated/movies",
            Listing(language, page, sortBy), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> GetRatedTvAsync(GuestSession session, string? language = null,
        int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>($"3/guest_session/{session.GuestSessionId}/rated/tv",
            Listing(language, page, sortBy), cancellationToken);

    public Task<PagedResult<TvEpisodeDetails>> GetRatedTvEpisodesAsync(GuestSession session, string? language = null,
        int? page = null, AccountSortBy? sortBy = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvEpisodeDetails>>($"3/guest_session/{session.GuestSessionId}/rated/tv/episodes",
            Listing(language, page, sortBy), cancellationToken);

    QueryString Listing(string? language, int? page, AccountSortBy? sortBy)
        => client.Page(language, page).Add("sort_by", sortBy?.ToWire());
}

internal sealed class AuthenticationEndpoints(TmdbClient client) : IAuthenticationEndpoints
{
    public Task<TmdbStatusResponse> ValidateAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<TmdbStatusResponse>("3/authentication", new QueryString(), cancellationToken);

    public Task<RequestToken> CreateRequestTokenAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<RequestToken>("3/authentication/token/new", new QueryString(), cancellationToken);

    public Task<SessionResponse> CreateSessionAsync(string requestToken, CancellationToken cancellationToken = default)
        => client.PostAsync<SessionResponse>("3/authentication/session/new", new QueryString(),
            new { request_token = requestToken }, cancellationToken);

    public Task<RequestToken> ValidateWithLoginAsync(string username, string password, string requestToken,
        CancellationToken cancellationToken = default)
        => client.PostAsync<RequestToken>("3/authentication/token/validate_with_login", new QueryString(),
            new { username, password, request_token = requestToken }, cancellationToken);

    public Task<SessionResponse> CreateSessionFromV4TokenAsync(string accessToken, CancellationToken cancellationToken = default)
        => client.PostAsync<SessionResponse>("3/authentication/session/convert/4", new QueryString(),
            new { access_token = accessToken }, cancellationToken);

    public Task<GuestSessionResponse> CreateGuestSessionAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<GuestSessionResponse>("3/authentication/guest_session/new", new QueryString(), cancellationToken);

    public Task<TmdbStatusResponse> DeleteSessionAsync(UserSession session, CancellationToken cancellationToken = default)
        => client.DeleteAsync<TmdbStatusResponse>("3/authentication/session", new QueryString(),
            cancellationToken, new { session_id = session.SessionId });
}
