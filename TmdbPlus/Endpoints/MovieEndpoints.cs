using TmdbPlus.Auth;
using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

/// <summary>The <c>/movie</c> endpoints.</summary>
public interface IMovieEndpoints
{
    Task<MovieDetails> GetAsync(int movieId, MovieAppend append = MovieAppend.None,
        string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int movieId, MovieAppend append = MovieAppend.None,
        string? language = null, CancellationToken cancellationToken = default);

    Task<Credits> GetCreditsAsync(int movieId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetCreditsAsync"/>
    Task<T> GetCreditsAsync<T>(int movieId, string? language = null, CancellationToken cancellationToken = default);

    Task<Images> GetImagesAsync(int movieId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int movieId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);

    Task<ResultsOf<Video>> GetVideosAsync(int movieId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetVideosAsync"/>
    Task<ResultsOf<T>> GetVideosAsync<T>(int movieId, string? language = null, CancellationToken cancellationToken = default);

    Task<MovieKeywords> GetKeywordsAsync(int movieId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetKeywordsAsync"/>
    Task<T> GetKeywordsAsync<T>(int movieId, CancellationToken cancellationToken = default);

    Task<MovieExternalIds> GetExternalIdsAsync(int movieId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetExternalIdsAsync"/>
    Task<T> GetExternalIdsAsync<T>(int movieId, CancellationToken cancellationToken = default);

    Task<MovieAlternativeTitles> GetAlternativeTitlesAsync(int movieId, string? country = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAlternativeTitlesAsync"/>
    Task<T> GetAlternativeTitlesAsync<T>(int movieId, string? country = null, CancellationToken cancellationToken = default);

    Task<MovieTranslations> GetTranslationsAsync(int movieId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTranslationsAsync"/>
    Task<T> GetTranslationsAsync<T>(int movieId, CancellationToken cancellationToken = default);

    Task<ResultsOf<CountryReleaseDates>> GetReleaseDatesAsync(int movieId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetReleaseDatesAsync"/>
    Task<ResultsOf<T>> GetReleaseDatesAsync<T>(int movieId, CancellationToken cancellationToken = default);

    Task<ResultsMap<CountryWatchProviders>> GetWatchProvidersAsync(int movieId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetWatchProvidersAsync"/>
    Task<ResultsMap<T>> GetWatchProvidersAsync<T>(int movieId, CancellationToken cancellationToken = default);

    Task<ChangesResult> GetChangesAsync(int movieId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetChangesAsync"/>
    Task<T> GetChangesAsync<T>(int movieId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetRecommendationsAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetRecommendationsAsync"/>
    Task<PagedResult<T>> GetRecommendationsAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetSimilarAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetSimilarAsync"/>
    Task<PagedResult<T>> GetSimilarAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<Review>> GetReviewsAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetReviewsAsync"/>
    Task<PagedResult<T>> GetReviewsAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<ListSummary>> GetListsAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetListsAsync"/>
    Task<PagedResult<T>> GetListsAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<DatedMoviePage> GetNowPlayingAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetNowPlayingAsync"/>
    Task<T> GetNowPlayingAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);

    Task<DatedMoviePage> GetUpcomingAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetUpcomingAsync"/>
    Task<T> GetUpcomingAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetPopularAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetPopularAsync"/>
    Task<PagedResult<T>> GetPopularAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetTopRatedAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTopRatedAsync"/>
    Task<PagedResult<T>> GetTopRatedAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default);

    Task<MovieDetails> GetLatestAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetLatestAsync"/>
    Task<T> GetLatestAsync<T>(CancellationToken cancellationToken = default);

    /// <summary>Accepts a user session or a guest session.</summary>
    Task<AccountStates> GetAccountStatesAsync(int movieId, AnySession session, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAccountStatesAsync"/>
    Task<T> GetAccountStatesAsync<T>(int movieId, AnySession session, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rates a movie, 0.5–10.0 in 0.5 steps. Out-of-range values are rejected by TMDB with
    /// HTTP 400 and <c>status_code 18</c> (issue #8). Accepts a guest session.
    /// </summary>
    Task<TmdbStatusResponse> RateAsync(int movieId, double value, AnySession session, CancellationToken cancellationToken = default);

    /// <summary>Idempotent: deleting an absent rating still returns success (issue #8).</summary>
    Task<TmdbStatusResponse> DeleteRatingAsync(int movieId, AnySession session, CancellationToken cancellationToken = default);
}

internal sealed class MovieEndpoints(TmdbClient client) : IMovieEndpoints
{
    public Task<MovieDetails> GetAsync(int movieId, MovieAppend append = MovieAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<MovieDetails>($"3/movie/{movieId}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<T> GetAsync<T>(int movieId, MovieAppend append = MovieAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<Credits> GetCreditsAsync(int movieId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<Credits>($"3/movie/{movieId}/credits", client.Language(language), cancellationToken);

    public Task<T> GetCreditsAsync<T>(int movieId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/credits", client.Language(language), cancellationToken);

    public Task<Images> GetImagesAsync(int movieId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<Images>($"3/movie/{movieId}/images", new QueryString()
            .Add("language", language)
            .Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<T> GetImagesAsync<T>(int movieId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/images", new QueryString()
            .Add("language", language)
            .Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<ResultsOf<Video>> GetVideosAsync(int movieId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<Video>>($"3/movie/{movieId}/videos", client.Language(language), cancellationToken);

    public Task<ResultsOf<T>> GetVideosAsync<T>(int movieId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/movie/{movieId}/videos", client.Language(language), cancellationToken);

    public Task<MovieKeywords> GetKeywordsAsync(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<MovieKeywords>($"3/movie/{movieId}/keywords", new QueryString(), cancellationToken);

    public Task<T> GetKeywordsAsync<T>(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/keywords", new QueryString(), cancellationToken);

    public Task<MovieExternalIds> GetExternalIdsAsync(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<MovieExternalIds>($"3/movie/{movieId}/external_ids", new QueryString(), cancellationToken);

    public Task<T> GetExternalIdsAsync<T>(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/external_ids", new QueryString(), cancellationToken);

    public Task<MovieAlternativeTitles> GetAlternativeTitlesAsync(int movieId, string? country = null, CancellationToken cancellationToken = default)
        => client.GetAsync<MovieAlternativeTitles>($"3/movie/{movieId}/alternative_titles", new QueryString()
            .Add("country", country), cancellationToken);

    public Task<T> GetAlternativeTitlesAsync<T>(int movieId, string? country = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/alternative_titles", new QueryString()
            .Add("country", country), cancellationToken);

    public Task<MovieTranslations> GetTranslationsAsync(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<MovieTranslations>($"3/movie/{movieId}/translations", new QueryString(), cancellationToken);

    public Task<T> GetTranslationsAsync<T>(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/translations", new QueryString(), cancellationToken);

    public Task<ResultsOf<CountryReleaseDates>> GetReleaseDatesAsync(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<CountryReleaseDates>>($"3/movie/{movieId}/release_dates", new QueryString(), cancellationToken);

    public Task<ResultsOf<T>> GetReleaseDatesAsync<T>(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/movie/{movieId}/release_dates", new QueryString(), cancellationToken);

    public Task<ResultsMap<CountryWatchProviders>> GetWatchProvidersAsync(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsMap<CountryWatchProviders>>($"3/movie/{movieId}/watch/providers", new QueryString(), cancellationToken);

    public Task<ResultsMap<T>> GetWatchProvidersAsync<T>(int movieId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsMap<T>>($"3/movie/{movieId}/watch/providers", new QueryString(), cancellationToken);

    public Task<ChangesResult> GetChangesAsync(int movieId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ChangesResult>($"3/movie/{movieId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);

    public Task<T> GetChangesAsync<T>(int movieId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetRecommendationsAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/movie/{movieId}/recommendations", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetRecommendationsAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/movie/{movieId}/recommendations", client.Page(language, page), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetSimilarAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/movie/{movieId}/similar", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetSimilarAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/movie/{movieId}/similar", client.Page(language, page), cancellationToken);

    public Task<PagedResult<Review>> GetReviewsAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<Review>>($"3/movie/{movieId}/reviews", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetReviewsAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/movie/{movieId}/reviews", client.Page(language, page), cancellationToken);

    public Task<PagedResult<ListSummary>> GetListsAsync(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<ListSummary>>($"3/movie/{movieId}/lists", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetListsAsync<T>(int movieId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/movie/{movieId}/lists", client.Page(language, page), cancellationToken);

    public Task<DatedMoviePage> GetNowPlayingAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<DatedMoviePage>("3/movie/now_playing", client.Page(language, page, region), cancellationToken);

    public Task<T> GetNowPlayingAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/movie/now_playing", client.Page(language, page, region), cancellationToken);

    public Task<DatedMoviePage> GetUpcomingAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<DatedMoviePage>("3/movie/upcoming", client.Page(language, page, region), cancellationToken);

    public Task<T> GetUpcomingAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/movie/upcoming", client.Page(language, page, region), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetPopularAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>("3/movie/popular", client.Page(language, page, region), cancellationToken);

    public Task<PagedResult<T>> GetPopularAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/movie/popular", client.Page(language, page, region), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetTopRatedAsync(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>("3/movie/top_rated", client.Page(language, page, region), cancellationToken);

    public Task<PagedResult<T>> GetTopRatedAsync<T>(string? language = null, int? page = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/movie/top_rated", client.Page(language, page, region), cancellationToken);

    public Task<MovieDetails> GetLatestAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<MovieDetails>("3/movie/latest", new QueryString(), cancellationToken);

    public Task<T> GetLatestAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/movie/latest", new QueryString(), cancellationToken);

    public Task<AccountStates> GetAccountStatesAsync(int movieId, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<AccountStates>($"3/movie/{movieId}/account_states",
            session.ToQuery(), cancellationToken);

    public Task<T> GetAccountStatesAsync<T>(int movieId, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/movie/{movieId}/account_states",
            session.ToQuery(), cancellationToken);

    public Task<TmdbStatusResponse> RateAsync(int movieId, double value, AnySession session, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/movie/{movieId}/rating",
            session.ToQuery(), new RatingBody(value), cancellationToken);

    public Task<TmdbStatusResponse> DeleteRatingAsync(int movieId, AnySession session, CancellationToken cancellationToken = default)
        => client.DeleteAsync<TmdbStatusResponse>($"3/movie/{movieId}/rating",
            session.ToQuery(), cancellationToken);

}

/// <summary>
/// The rating payload. The spec declares every request body as <c>RAW_BODY: string</c>, so this
/// shape came from TMDbLib and was confirmed live (issue #8).
/// </summary>
internal sealed record RatingBody(
    [property: System.Text.Json.Serialization.JsonPropertyName("value")] double Value);
