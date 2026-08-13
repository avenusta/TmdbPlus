using TmdbPlus.Auth;
using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

/// <summary>The <c>/tv</c> series endpoints. Seasons and episodes hang off this one.</summary>
public interface ITvEndpoints
{
    /// <summary>Season-level endpoints.</summary>
    ITvSeasonEndpoints Seasons { get; }

    /// <summary>Episode-level endpoints.</summary>
    ITvEpisodeEndpoints Episodes { get; }

    Task<TvSeriesDetails> GetAsync(int seriesId, TvSeriesAppend append = TvSeriesAppend.None,
        string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int seriesId, TvSeriesAppend append = TvSeriesAppend.None,
        string? language = null, CancellationToken cancellationToken = default);

    Task<Credits> GetCreditsAsync(int seriesId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetCreditsAsync"/>
    Task<T> GetCreditsAsync<T>(int seriesId, string? language = null, CancellationToken cancellationToken = default);

    Task<AggregateCredits> GetAggregateCreditsAsync(int seriesId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAggregateCreditsAsync"/>
    Task<T> GetAggregateCreditsAsync<T>(int seriesId, string? language = null, CancellationToken cancellationToken = default);

    Task<Images> GetImagesAsync(int seriesId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int seriesId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);

    Task<ResultsOf<Video>> GetVideosAsync(int seriesId, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetVideosAsync"/>
    Task<ResultsOf<T>> GetVideosAsync<T>(int seriesId, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default);

    Task<TvKeywords> GetKeywordsAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetKeywordsAsync"/>
    Task<T> GetKeywordsAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<TvExternalIds> GetExternalIdsAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetExternalIdsAsync"/>
    Task<T> GetExternalIdsAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<TvAlternativeTitles> GetAlternativeTitlesAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAlternativeTitlesAsync"/>
    Task<T> GetAlternativeTitlesAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<TvTranslations> GetTranslationsAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTranslationsAsync"/>
    Task<T> GetTranslationsAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<ResultsOf<ContentRating>> GetContentRatingsAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetContentRatingsAsync"/>
    Task<ResultsOf<T>> GetContentRatingsAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<ResultsOf<EpisodeGroupSummary>> GetEpisodeGroupsAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetEpisodeGroupsAsync"/>
    Task<ResultsOf<T>> GetEpisodeGroupsAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<ResultsOf<ScreenedTheatrically>> GetScreenedTheatricallyAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetScreenedTheatricallyAsync"/>
    Task<ResultsOf<T>> GetScreenedTheatricallyAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<ResultsMap<CountryWatchProviders>> GetWatchProvidersAsync(int seriesId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetWatchProvidersAsync"/>
    Task<ResultsMap<T>> GetWatchProvidersAsync<T>(int seriesId, CancellationToken cancellationToken = default);

    Task<ChangesResult> GetChangesAsync(int seriesId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetChangesAsync"/>
    Task<T> GetChangesAsync<T>(int seriesId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> GetRecommendationsAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetRecommendationsAsync"/>
    Task<PagedResult<T>> GetRecommendationsAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> GetSimilarAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetSimilarAsync"/>
    Task<PagedResult<T>> GetSimilarAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<Review>> GetReviewsAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetReviewsAsync"/>
    Task<PagedResult<T>> GetReviewsAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<ListSummary>> GetListsAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetListsAsync"/>
    Task<PagedResult<T>> GetListsAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<TvSeriesPage> GetAiringTodayAsync(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAiringTodayAsync"/>
    Task<T> GetAiringTodayAsync<T>(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default);

    Task<TvSeriesPage> GetOnTheAirAsync(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetOnTheAirAsync"/>
    Task<T> GetOnTheAirAsync<T>(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default);

    Task<TvSeriesPage> GetPopularAsync(string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetPopularAsync"/>
    Task<T> GetPopularAsync<T>(string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<TvSeriesPage> GetTopRatedAsync(string? language = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTopRatedAsync"/>
    Task<T> GetTopRatedAsync<T>(string? language = null, int? page = null, CancellationToken cancellationToken = default);

    Task<TvSeriesDetails> GetLatestAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetLatestAsync"/>
    Task<T> GetLatestAsync<T>(CancellationToken cancellationToken = default);

    Task<AccountStates> GetAccountStatesAsync(int seriesId, AnySession session, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAccountStatesAsync"/>
    Task<T> GetAccountStatesAsync<T>(int seriesId, AnySession session, CancellationToken cancellationToken = default);

    Task<TmdbStatusResponse> RateAsync(int seriesId, double value, AnySession session, CancellationToken cancellationToken = default);
    Task<TmdbStatusResponse> DeleteRatingAsync(int seriesId, AnySession session, CancellationToken cancellationToken = default);

    /// <summary>Episode groups are fetched by their own string id, not by series.</summary>
    Task<EpisodeGroupDetails> GetEpisodeGroupAsync(string episodeGroupId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetEpisodeGroupAsync"/>
    Task<T> GetEpisodeGroupAsync<T>(string episodeGroupId, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/tv/{id}/season/{n}</c> endpoints.</summary>
public interface ITvSeasonEndpoints
{
    Task<TvSeasonDetails> GetAsync(int seriesId, int seasonNumber, TvSeasonAppend append = TvSeasonAppend.None,
        string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int seriesId, int seasonNumber, TvSeasonAppend append = TvSeasonAppend.None,
        string? language = null, CancellationToken cancellationToken = default);

    Task<Credits> GetCreditsAsync(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetCreditsAsync"/>
    Task<T> GetCreditsAsync<T>(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default);

    Task<AggregateCredits> GetAggregateCreditsAsync(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAggregateCreditsAsync"/>
    Task<T> GetAggregateCreditsAsync<T>(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default);

    Task<Images> GetImagesAsync(int seriesId, int seasonNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int seriesId, int seasonNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);

    Task<ResultsOf<Video>> GetVideosAsync(int seriesId, int seasonNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetVideosAsync"/>
    Task<ResultsOf<T>> GetVideosAsync<T>(int seriesId, int seasonNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default);

    Task<TvExternalIds> GetExternalIdsAsync(int seriesId, int seasonNumber, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetExternalIdsAsync"/>
    Task<T> GetExternalIdsAsync<T>(int seriesId, int seasonNumber, CancellationToken cancellationToken = default);

    Task<TvTranslations> GetTranslationsAsync(int seriesId, int seasonNumber, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTranslationsAsync"/>
    Task<T> GetTranslationsAsync<T>(int seriesId, int seasonNumber, CancellationToken cancellationToken = default);

    Task<ResultsMap<CountryWatchProviders>> GetWatchProvidersAsync(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetWatchProvidersAsync"/>
    Task<ResultsMap<T>> GetWatchProvidersAsync<T>(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default);

    Task<AccountStates> GetAccountStatesAsync(int seriesId, int seasonNumber, AnySession session, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAccountStatesAsync"/>
    Task<T> GetAccountStatesAsync<T>(int seriesId, int seasonNumber, AnySession session, CancellationToken cancellationToken = default);

    /// <summary>Keyed by the season's own id, not by series and season number.</summary>
    Task<ChangesResult> GetChangesAsync(int seasonId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetChangesAsync"/>
    Task<T> GetChangesAsync<T>(int seasonId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/tv/{id}/season/{n}/episode/{n}</c> endpoints.</summary>
public interface ITvEpisodeEndpoints
{
    Task<TvEpisodeDetails> GetAsync(int seriesId, int seasonNumber, int episodeNumber,
        TvEpisodeAppend append = TvEpisodeAppend.None, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int seriesId, int seasonNumber, int episodeNumber,
        TvEpisodeAppend append = TvEpisodeAppend.None, string? language = null, CancellationToken cancellationToken = default);

    Task<EpisodeCredits> GetCreditsAsync(int seriesId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetCreditsAsync"/>
    Task<T> GetCreditsAsync<T>(int seriesId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default);

    Task<Images> GetImagesAsync(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);

    Task<ResultsOf<Video>> GetVideosAsync(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetVideosAsync"/>
    Task<ResultsOf<T>> GetVideosAsync<T>(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default);

    Task<TvExternalIds> GetExternalIdsAsync(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetExternalIdsAsync"/>
    Task<T> GetExternalIdsAsync<T>(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default);

    Task<TvTranslations> GetTranslationsAsync(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTranslationsAsync"/>
    Task<T> GetTranslationsAsync<T>(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default);

    Task<AccountStates> GetAccountStatesAsync(int seriesId, int seasonNumber, int episodeNumber, AnySession session, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAccountStatesAsync"/>
    Task<T> GetAccountStatesAsync<T>(int seriesId, int seasonNumber, int episodeNumber, AnySession session, CancellationToken cancellationToken = default);

    Task<TmdbStatusResponse> RateAsync(int seriesId, int seasonNumber, int episodeNumber, double value, AnySession session, CancellationToken cancellationToken = default);
    Task<TmdbStatusResponse> DeleteRatingAsync(int seriesId, int seasonNumber, int episodeNumber, AnySession session, CancellationToken cancellationToken = default);

    /// <summary>Keyed by the episode's own id, not by series/season/episode number.</summary>
    Task<ChangesResult> GetChangesAsync(int episodeId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetChangesAsync"/>
    Task<T> GetChangesAsync<T>(int episodeId, CancellationToken cancellationToken = default);
}

internal sealed class TvEndpoints(TmdbClient client) : ITvEndpoints
{
    public ITvSeasonEndpoints Seasons { get; } = new TvSeasonEndpoints(client);
    public ITvEpisodeEndpoints Episodes { get; } = new TvEpisodeEndpoints(client);

    public Task<TvSeriesDetails> GetAsync(int seriesId, TvSeriesAppend append = TvSeriesAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<TvSeriesDetails>($"3/tv/{seriesId}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<T> GetAsync<T>(int seriesId, TvSeriesAppend append = TvSeriesAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<Credits> GetCreditsAsync(int seriesId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<Credits>($"3/tv/{seriesId}/credits", client.Language(language), cancellationToken);

    public Task<T> GetCreditsAsync<T>(int seriesId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/credits", client.Language(language), cancellationToken);

    public Task<AggregateCredits> GetAggregateCreditsAsync(int seriesId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<AggregateCredits>($"3/tv/{seriesId}/aggregate_credits", client.Language(language), cancellationToken);

    public Task<T> GetAggregateCreditsAsync<T>(int seriesId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/aggregate_credits", client.Language(language), cancellationToken);

    public Task<Images> GetImagesAsync(int seriesId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<Images>($"3/tv/{seriesId}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<T> GetImagesAsync<T>(int seriesId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<ResultsOf<Video>> GetVideosAsync(int seriesId, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<Video>>($"3/tv/{seriesId}/videos", new QueryString()
            .Add("language", language ?? client.DefaultLanguage)
            .Add("include_video_language", includeVideoLanguage), cancellationToken);

    public Task<ResultsOf<T>> GetVideosAsync<T>(int seriesId, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/tv/{seriesId}/videos", new QueryString()
            .Add("language", language ?? client.DefaultLanguage)
            .Add("include_video_language", includeVideoLanguage), cancellationToken);

    public Task<TvKeywords> GetKeywordsAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<TvKeywords>($"3/tv/{seriesId}/keywords", new QueryString(), cancellationToken);

    public Task<T> GetKeywordsAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/keywords", new QueryString(), cancellationToken);

    public Task<TvExternalIds> GetExternalIdsAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<TvExternalIds>($"3/tv/{seriesId}/external_ids", new QueryString(), cancellationToken);

    public Task<T> GetExternalIdsAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/external_ids", new QueryString(), cancellationToken);

    public Task<TvAlternativeTitles> GetAlternativeTitlesAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<TvAlternativeTitles>($"3/tv/{seriesId}/alternative_titles", new QueryString(), cancellationToken);

    public Task<T> GetAlternativeTitlesAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/alternative_titles", new QueryString(), cancellationToken);

    public Task<TvTranslations> GetTranslationsAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<TvTranslations>($"3/tv/{seriesId}/translations", new QueryString(), cancellationToken);

    public Task<T> GetTranslationsAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/translations", new QueryString(), cancellationToken);

    public Task<ResultsOf<ContentRating>> GetContentRatingsAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<ContentRating>>($"3/tv/{seriesId}/content_ratings", new QueryString(), cancellationToken);

    public Task<ResultsOf<T>> GetContentRatingsAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/tv/{seriesId}/content_ratings", new QueryString(), cancellationToken);

    public Task<ResultsOf<EpisodeGroupSummary>> GetEpisodeGroupsAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<EpisodeGroupSummary>>($"3/tv/{seriesId}/episode_groups", new QueryString(), cancellationToken);

    public Task<ResultsOf<T>> GetEpisodeGroupsAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/tv/{seriesId}/episode_groups", new QueryString(), cancellationToken);

    public Task<ResultsOf<ScreenedTheatrically>> GetScreenedTheatricallyAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<ScreenedTheatrically>>($"3/tv/{seriesId}/screened_theatrically", new QueryString(), cancellationToken);

    public Task<ResultsOf<T>> GetScreenedTheatricallyAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/tv/{seriesId}/screened_theatrically", new QueryString(), cancellationToken);

    public Task<ResultsMap<CountryWatchProviders>> GetWatchProvidersAsync(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsMap<CountryWatchProviders>>($"3/tv/{seriesId}/watch/providers", new QueryString(), cancellationToken);

    public Task<ResultsMap<T>> GetWatchProvidersAsync<T>(int seriesId, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsMap<T>>($"3/tv/{seriesId}/watch/providers", new QueryString(), cancellationToken);

    public Task<ChangesResult> GetChangesAsync(int seriesId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ChangesResult>($"3/tv/{seriesId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);

    public Task<T> GetChangesAsync<T>(int seriesId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> GetRecommendationsAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>($"3/tv/{seriesId}/recommendations", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetRecommendationsAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/tv/{seriesId}/recommendations", client.Page(language, page), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> GetSimilarAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>($"3/tv/{seriesId}/similar", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetSimilarAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/tv/{seriesId}/similar", client.Page(language, page), cancellationToken);

    public Task<PagedResult<Review>> GetReviewsAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<Review>>($"3/tv/{seriesId}/reviews", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetReviewsAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/tv/{seriesId}/reviews", client.Page(language, page), cancellationToken);

    public Task<PagedResult<ListSummary>> GetListsAsync(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<ListSummary>>($"3/tv/{seriesId}/lists", client.Page(language, page), cancellationToken);

    public Task<PagedResult<T>> GetListsAsync<T>(int seriesId, string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/tv/{seriesId}/lists", client.Page(language, page), cancellationToken);

    public Task<TvSeriesPage> GetAiringTodayAsync(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default)
        => client.GetAsync<TvSeriesPage>("3/tv/airing_today", client.Page(language, page).Add("timezone", timezone), cancellationToken);

    public Task<T> GetAiringTodayAsync<T>(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/tv/airing_today", client.Page(language, page).Add("timezone", timezone), cancellationToken);

    public Task<TvSeriesPage> GetOnTheAirAsync(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default)
        => client.GetAsync<TvSeriesPage>("3/tv/on_the_air", client.Page(language, page).Add("timezone", timezone), cancellationToken);

    public Task<T> GetOnTheAirAsync<T>(string? language = null, int? page = null, string? timezone = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/tv/on_the_air", client.Page(language, page).Add("timezone", timezone), cancellationToken);

    public Task<TvSeriesPage> GetPopularAsync(string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<TvSeriesPage>("3/tv/popular", client.Page(language, page), cancellationToken);

    public Task<T> GetPopularAsync<T>(string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/tv/popular", client.Page(language, page), cancellationToken);

    public Task<TvSeriesPage> GetTopRatedAsync(string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<TvSeriesPage>("3/tv/top_rated", client.Page(language, page), cancellationToken);

    public Task<T> GetTopRatedAsync<T>(string? language = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/tv/top_rated", client.Page(language, page), cancellationToken);

    public Task<TvSeriesDetails> GetLatestAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<TvSeriesDetails>("3/tv/latest", new QueryString(), cancellationToken);

    public Task<T> GetLatestAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/tv/latest", new QueryString(), cancellationToken);

    public Task<AccountStates> GetAccountStatesAsync(int seriesId, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<AccountStates>($"3/tv/{seriesId}/account_states", session.ToQuery(), cancellationToken);

    public Task<T> GetAccountStatesAsync<T>(int seriesId, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/account_states", session.ToQuery(), cancellationToken);

    public Task<TmdbStatusResponse> RateAsync(int seriesId, double value, AnySession session, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/tv/{seriesId}/rating", session.ToQuery(), new RatingBody(value), cancellationToken);

    public Task<TmdbStatusResponse> DeleteRatingAsync(int seriesId, AnySession session, CancellationToken cancellationToken = default)
        => client.DeleteAsync<TmdbStatusResponse>($"3/tv/{seriesId}/rating", session.ToQuery(), cancellationToken);

    public Task<EpisodeGroupDetails> GetEpisodeGroupAsync(string episodeGroupId, CancellationToken cancellationToken = default)
        => client.GetAsync<EpisodeGroupDetails>($"3/tv/episode_group/{episodeGroupId}", new QueryString(), cancellationToken);

    public Task<T> GetEpisodeGroupAsync<T>(string episodeGroupId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/episode_group/{episodeGroupId}", new QueryString(), cancellationToken);
}

internal sealed class TvSeasonEndpoints(TmdbClient client) : ITvSeasonEndpoints
{
    public Task<TvSeasonDetails> GetAsync(int seriesId, int seasonNumber, TvSeasonAppend append = TvSeasonAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<TvSeasonDetails>($"3/tv/{seriesId}/season/{seasonNumber}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<T> GetAsync<T>(int seriesId, int seasonNumber, TvSeasonAppend append = TvSeasonAppend.None,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<Credits> GetCreditsAsync(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<Credits>($"3/tv/{seriesId}/season/{seasonNumber}/credits", client.Language(language), cancellationToken);

    public Task<T> GetCreditsAsync<T>(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/credits", client.Language(language), cancellationToken);

    public Task<AggregateCredits> GetAggregateCreditsAsync(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<AggregateCredits>($"3/tv/{seriesId}/season/{seasonNumber}/aggregate_credits", client.Language(language), cancellationToken);

    public Task<T> GetAggregateCreditsAsync<T>(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/aggregate_credits", client.Language(language), cancellationToken);

    public Task<Images> GetImagesAsync(int seriesId, int seasonNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<Images>($"3/tv/{seriesId}/season/{seasonNumber}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<T> GetImagesAsync<T>(int seriesId, int seasonNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<ResultsOf<Video>> GetVideosAsync(int seriesId, int seasonNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<Video>>($"3/tv/{seriesId}/season/{seasonNumber}/videos", new QueryString()
            .Add("language", language ?? client.DefaultLanguage)
            .Add("include_video_language", includeVideoLanguage), cancellationToken);

    public Task<ResultsOf<T>> GetVideosAsync<T>(int seriesId, int seasonNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/tv/{seriesId}/season/{seasonNumber}/videos", new QueryString()
            .Add("language", language ?? client.DefaultLanguage)
            .Add("include_video_language", includeVideoLanguage), cancellationToken);

    public Task<TvExternalIds> GetExternalIdsAsync(int seriesId, int seasonNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<TvExternalIds>($"3/tv/{seriesId}/season/{seasonNumber}/external_ids", new QueryString(), cancellationToken);

    public Task<T> GetExternalIdsAsync<T>(int seriesId, int seasonNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/external_ids", new QueryString(), cancellationToken);

    public Task<TvTranslations> GetTranslationsAsync(int seriesId, int seasonNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<TvTranslations>($"3/tv/{seriesId}/season/{seasonNumber}/translations", new QueryString(), cancellationToken);

    public Task<T> GetTranslationsAsync<T>(int seriesId, int seasonNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/translations", new QueryString(), cancellationToken);

    public Task<ResultsMap<CountryWatchProviders>> GetWatchProvidersAsync(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsMap<CountryWatchProviders>>($"3/tv/{seriesId}/season/{seasonNumber}/watch/providers", client.Language(language), cancellationToken);

    public Task<ResultsMap<T>> GetWatchProvidersAsync<T>(int seriesId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsMap<T>>($"3/tv/{seriesId}/season/{seasonNumber}/watch/providers", client.Language(language), cancellationToken);

    public Task<AccountStates> GetAccountStatesAsync(int seriesId, int seasonNumber, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<AccountStates>($"3/tv/{seriesId}/season/{seasonNumber}/account_states", session.ToQuery(), cancellationToken);

    public Task<T> GetAccountStatesAsync<T>(int seriesId, int seasonNumber, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/account_states", session.ToQuery(), cancellationToken);

    public Task<ChangesResult> GetChangesAsync(int seasonId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ChangesResult>($"3/tv/season/{seasonId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);

    public Task<T> GetChangesAsync<T>(int seasonId, DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/season/{seasonId}/changes", new QueryString()
            .Add("start_date", startDate).Add("end_date", endDate).Add("page", page), cancellationToken);
}

internal sealed class TvEpisodeEndpoints(TmdbClient client) : ITvEpisodeEndpoints
{
    public Task<TvEpisodeDetails> GetAsync(int seriesId, int seasonNumber, int episodeNumber,
        TvEpisodeAppend append = TvEpisodeAppend.None, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<TvEpisodeDetails>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<T> GetAsync<T>(int seriesId, int seasonNumber, int episodeNumber,
        TvEpisodeAppend append = TvEpisodeAppend.None, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}", new QueryString()
            .Add("append_to_response", append.ToQueryValue())
            .Add("language", language ?? client.DefaultLanguage), cancellationToken);

    public Task<EpisodeCredits> GetCreditsAsync(int seriesId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<EpisodeCredits>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/credits", client.Language(language), cancellationToken);

    public Task<T> GetCreditsAsync<T>(int seriesId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/credits", client.Language(language), cancellationToken);

    public Task<Images> GetImagesAsync(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<Images>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<T> GetImagesAsync<T>(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<ResultsOf<Video>> GetVideosAsync(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<Video>>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/videos", new QueryString()
            .Add("language", language ?? client.DefaultLanguage)
            .Add("include_video_language", includeVideoLanguage), cancellationToken);

    public Task<ResultsOf<T>> GetVideosAsync<T>(int seriesId, int seasonNumber, int episodeNumber, string? language = null, string? includeVideoLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<ResultsOf<T>>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/videos", new QueryString()
            .Add("language", language ?? client.DefaultLanguage)
            .Add("include_video_language", includeVideoLanguage), cancellationToken);

    public Task<TvExternalIds> GetExternalIdsAsync(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<TvExternalIds>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/external_ids", new QueryString(), cancellationToken);

    public Task<T> GetExternalIdsAsync<T>(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/external_ids", new QueryString(), cancellationToken);

    public Task<TvTranslations> GetTranslationsAsync(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<TvTranslations>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/translations", new QueryString(), cancellationToken);

    public Task<T> GetTranslationsAsync<T>(int seriesId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/translations", new QueryString(), cancellationToken);

    public Task<AccountStates> GetAccountStatesAsync(int seriesId, int seasonNumber, int episodeNumber, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<AccountStates>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/account_states", session.ToQuery(), cancellationToken);

    public Task<T> GetAccountStatesAsync<T>(int seriesId, int seasonNumber, int episodeNumber, AnySession session, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/account_states", session.ToQuery(), cancellationToken);

    public Task<TmdbStatusResponse> RateAsync(int seriesId, int seasonNumber, int episodeNumber, double value, AnySession session, CancellationToken cancellationToken = default)
        => client.PostAsync<TmdbStatusResponse>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/rating", session.ToQuery(), new RatingBody(value), cancellationToken);

    public Task<TmdbStatusResponse> DeleteRatingAsync(int seriesId, int seasonNumber, int episodeNumber, AnySession session, CancellationToken cancellationToken = default)
        => client.DeleteAsync<TmdbStatusResponse>($"3/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}/rating", session.ToQuery(), cancellationToken);

    public Task<ChangesResult> GetChangesAsync(int episodeId, CancellationToken cancellationToken = default)
        => client.GetAsync<ChangesResult>($"3/tv/episode/{episodeId}/changes", new QueryString(), cancellationToken);

    public Task<T> GetChangesAsync<T>(int episodeId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/tv/episode/{episodeId}/changes", new QueryString(), cancellationToken);
}
