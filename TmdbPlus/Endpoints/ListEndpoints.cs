using TmdbPlus.Auth;
using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

/// <summary>
/// The v3 <c>/list</c> endpoints. Reading a list needs no session; every mutation does.
/// v3 lists hold movies only.
/// </summary>
public interface IListEndpoints
{
    Task<ListDetails> GetAsync(int listId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    /// <summary>Whether a movie is already on the list.</summary>
    Task<ListItemStatus> GetItemStatusAsync(int listId, int movieId, string? language = null, CancellationToken cancellationToken = default);

    Task<CreateListResponse> CreateAsync(UserSession session, string name, string? description = null,
        string language = "en", CancellationToken cancellationToken = default);

    Task<TmdbStatusResponse> AddItemAsync(int listId, UserSession session, int movieId, CancellationToken cancellationToken = default);
    Task<TmdbStatusResponse> RemoveItemAsync(int listId, UserSession session, int movieId, CancellationToken cancellationToken = default);

    /// <summary>Removes every item. TMDB requires the confirmation flag.</summary>
    Task<TmdbStatusResponse> ClearAsync(int listId, UserSession session, CancellationToken cancellationToken = default);

    Task<TmdbStatusResponse> DeleteAsync(int listId, UserSession session, CancellationToken cancellationToken = default);
}

internal sealed class ListEndpoints(TmdbClient client) : IListEndpoints
{
    public Task<ListDetails> GetAsync(int listId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ListDetails>($"3/list/{listId}", client.Page(language, page), cancellationToken);

    public Task<ListItemStatus> GetItemStatusAsync(int listId, int movieId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ListItemStatus>($"3/list/{listId}/item_status", client.Language(language)
            .Add("movie_id", movieId), cancellationToken);

    public Task<CreateListResponse> CreateAsync(UserSession session, string name, string? description = null,
        string language = "en", CancellationToken cancellationToken = default)
        => client.PostAsync<CreateListResponse>("3/list", Session(session),
            new CreateListRequest { Name = name, Description = description, Language = language }, cancellationToken);

    public Task<TmdbStatusResponse> AddItemAsync(int listId, UserSession session, int movieId, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/list/{listId}/add_item", Session(session),
            new ListItemRequest { MediaId = movieId }, cancellationToken);

    public Task<TmdbStatusResponse> RemoveItemAsync(int listId, UserSession session, int movieId, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/list/{listId}/remove_item", Session(session),
            new ListItemRequest { MediaId = movieId }, cancellationToken);

    public Task<TmdbStatusResponse> ClearAsync(int listId, UserSession session, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/list/{listId}/clear",
            Session(session).Add("confirm", true), null, cancellationToken);

    public Task<TmdbStatusResponse> DeleteAsync(int listId, UserSession session, CancellationToken cancellationToken = default)
        => client.DeleteAsync<TmdbStatusResponse>($"3/list/{listId}", Session(session), cancellationToken);

    static QueryString Session(UserSession session) => new QueryString().Add("session_id", session.SessionId);
}
