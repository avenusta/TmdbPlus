using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

/// <summary>The <c>/person</c> endpoints.</summary>
public interface IPeopleEndpoints
{
    Task<PersonDetails> GetAsync(int personId, PersonAppend append = PersonAppend.None,
        string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int personId, PersonAppend append = PersonAppend.None,
        string? language = null, CancellationToken cancellationToken = default);

    /// <summary>Movies and series in one list, discriminated by each entry's media type.</summary>
    Task<CombinedCredits> GetCombinedCreditsAsync(int personId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetCombinedCreditsAsync"/>
    Task<T> GetCombinedCreditsAsync<T>(int personId, string? language = null, CancellationToken cancellationToken = default);

    Task<PersonMovieCredits> GetMovieCreditsAsync(int personId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetMovieCreditsAsync"/>
    Task<T> GetMovieCreditsAsync<T>(int personId, string? language = null, CancellationToken cancellationToken = default);

    Task<PersonTvCredits> GetTvCreditsAsync(int personId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTvCreditsAsync"/>
    Task<T> GetTvCreditsAsync<T>(int personId, string? language = null, CancellationToken cancellationToken = default);

    Task<PersonImages> GetImagesAsync(int personId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int personId, CancellationToken cancellationToken = default);

    Task<PagedResult<TaggedImage>> GetTaggedImagesAsync(int personId, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTaggedImagesAsync"/>
    Task<PagedResult<T>> GetTaggedImagesAsync<T>(int personId, int? page = null, CancellationToken cancellationToken = default);

    Task<PersonExternalIds> GetExternalIdsAsync(int personId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetExternalIdsAsync"/>
    Task<T> GetExternalIdsAsync<T>(int personId, CancellationToken cancellationToken = default);

    Task<PersonTranslations> GetTranslationsAsync(int personId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTranslationsAsync"/>
    Task<T> GetTranslationsAsync<T>(int personId, CancellationToken cancellationToken = default);

    Task<ChangesResult> GetChangesAsync(int personId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetChangesAsync"/>
    Task<T> GetChangesAsync<T>(int personId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<PersonSummary>> GetPopularAsync(string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetPopularAsync"/>
    Task<PagedResult<T>> GetPopularAsync<T>(string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PersonDetails> GetLatestAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetLatestAsync"/>
    Task<T> GetLatestAsync<T>(CancellationToken cancellationToken = default);
}

internal sealed class PeopleEndpoints(TmdbClient client) : IPeopleEndpoints
{
    public Task<PersonDetails> GetAsync(int personId, PersonAppend append = PersonAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PersonDetails>($"3/person/{personId}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<T> GetAsync<T>(int personId, PersonAppend append = PersonAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<CombinedCredits> GetCombinedCreditsAsync(int personId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<CombinedCredits>($"3/person/{personId}/combined_credits", client.Language(language), cancellationToken);

    public Task<T> GetCombinedCreditsAsync<T>(int personId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}/combined_credits", client.Language(language), cancellationToken);

    public Task<PersonMovieCredits> GetMovieCreditsAsync(int personId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PersonMovieCredits>($"3/person/{personId}/movie_credits", client.Language(language), cancellationToken);

    public Task<T> GetMovieCreditsAsync<T>(int personId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}/movie_credits", client.Language(language), cancellationToken);

    public Task<PersonTvCredits> GetTvCreditsAsync(int personId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PersonTvCredits>($"3/person/{personId}/tv_credits", client.Language(language), cancellationToken);

    public Task<T> GetTvCreditsAsync<T>(int personId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}/tv_credits", client.Language(language), cancellationToken);

    public Task<PersonImages> GetImagesAsync(int personId, CancellationToken cancellationToken = default)
        => client.GetAsync<PersonImages>($"3/person/{personId}/images", new QueryString(), cancellationToken);

    public Task<T> GetImagesAsync<T>(int personId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}/images", new QueryString(), cancellationToken);

    public Task<PagedResult<TaggedImage>> GetTaggedImagesAsync(int personId, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TaggedImage>>($"3/person/{personId}/tagged_images", new QueryString()
            .Add("page", page), cancellationToken);

    public Task<PagedResult<T>> GetTaggedImagesAsync<T>(int personId, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/person/{personId}/tagged_images", new QueryString()
            .Add("page", page), cancellationToken);

    public Task<PersonExternalIds> GetExternalIdsAsync(int personId, CancellationToken cancellationToken = default)
        => client.GetAsync<PersonExternalIds>($"3/person/{personId}/external_ids", new QueryString(), cancellationToken);

    public Task<T> GetExternalIdsAsync<T>(int personId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}/external_ids", new QueryString(), cancellationToken);

    public Task<PersonTranslations> GetTranslationsAsync(int personId, CancellationToken cancellationToken = default)
        => client.GetAsync<PersonTranslations>($"3/person/{personId}/translations", new QueryString(), cancellationToken);

    public Task<T> GetTranslationsAsync<T>(int personId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}/translations", new QueryString(), cancellationToken);

    public Task<ChangesResult> GetChangesAsync(int personId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ChangesResult>($"3/person/{personId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);

    public Task<T> GetChangesAsync<T>(int personId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/person/{personId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);

    public Task<PagedResult<PersonSummary>> GetPopularAsync(string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<PersonSummary>>("3/person/popular", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetPopularAsync<T>(string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/person/popular", client.Page(language, page), cancellationToken);

    public Task<PersonDetails> GetLatestAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<PersonDetails>("3/person/latest", new QueryString(), cancellationToken);

    public Task<T> GetLatestAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/person/latest", new QueryString(), cancellationToken);
}
