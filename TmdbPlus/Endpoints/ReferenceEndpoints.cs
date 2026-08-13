using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

/// <summary>The <c>/configuration</c> endpoints.</summary>
public interface IConfigurationEndpoints
{
    /// <summary>Image base URLs and size lists — needed to build any image URL.</summary>
    Task<TmdbConfiguration> GetAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(CancellationToken cancellationToken = default);

    Task<IList<CountryInfo>> GetCountriesAsync(string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetCountriesAsync"/>
    Task<IList<T>> GetCountriesAsync<T>(string? language = null, CancellationToken cancellationToken = default);

    Task<IList<LanguageInfo>> GetLanguagesAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetLanguagesAsync"/>
    Task<IList<T>> GetLanguagesAsync<T>(CancellationToken cancellationToken = default);

    Task<IList<TimezoneInfo>> GetTimezonesAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTimezonesAsync"/>
    Task<IList<T>> GetTimezonesAsync<T>(CancellationToken cancellationToken = default);

    Task<IList<DepartmentJobs>> GetJobsAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetJobsAsync"/>
    Task<IList<T>> GetJobsAsync<T>(CancellationToken cancellationToken = default);

    Task<IList<string>> GetPrimaryTranslationsAsync(CancellationToken cancellationToken = default);
}

/// <summary>The <c>/certification</c> endpoints.</summary>
public interface ICertificationEndpoints
{
    Task<CertificationsResponse> GetMovieCertificationsAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetMovieCertificationsAsync"/>
    Task<T> GetMovieCertificationsAsync<T>(CancellationToken cancellationToken = default);

    Task<CertificationsResponse> GetTvCertificationsAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTvCertificationsAsync"/>
    Task<T> GetTvCertificationsAsync<T>(CancellationToken cancellationToken = default);
}

/// <summary>The <c>/genre</c> endpoints.</summary>
public interface IGenreEndpoints
{
    Task<GenreList> GetMovieGenresAsync(string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetMovieGenresAsync"/>
    Task<T> GetMovieGenresAsync<T>(string? language = null, CancellationToken cancellationToken = default);

    Task<GenreList> GetTvGenresAsync(string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTvGenresAsync"/>
    Task<T> GetTvGenresAsync<T>(string? language = null, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/collection</c> endpoints.</summary>
public interface ICollectionEndpoints
{
    Task<CollectionDetails> GetAsync(int collectionId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int collectionId, string? language = null, CancellationToken cancellationToken = default);

    Task<CollectionImages> GetImagesAsync(int collectionId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int collectionId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default);

    Task<CollectionTranslations> GetTranslationsAsync(int collectionId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTranslationsAsync"/>
    Task<T> GetTranslationsAsync<T>(int collectionId, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/company</c> endpoints.</summary>
public interface ICompanyEndpoints
{
    Task<CompanyDetails> GetAsync(int companyId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int companyId, CancellationToken cancellationToken = default);

    Task<AlternativeNames> GetAlternativeNamesAsync(int companyId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAlternativeNamesAsync"/>
    Task<T> GetAlternativeNamesAsync<T>(int companyId, CancellationToken cancellationToken = default);

    Task<LogoImages> GetImagesAsync(int companyId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int companyId, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/network</c> endpoints — the same shape as companies.</summary>
public interface INetworkEndpoints
{
    Task<Network> GetAsync(int networkId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int networkId, CancellationToken cancellationToken = default);

    Task<AlternativeNames> GetAlternativeNamesAsync(int networkId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAlternativeNamesAsync"/>
    Task<T> GetAlternativeNamesAsync<T>(int networkId, CancellationToken cancellationToken = default);

    Task<LogoImages> GetImagesAsync(int networkId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetImagesAsync"/>
    Task<T> GetImagesAsync<T>(int networkId, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/keyword</c> endpoints.</summary>
public interface IKeywordEndpoints
{
    Task<KeywordDetails> GetAsync(int keywordId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(int keywordId, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> GetMoviesAsync(int keywordId, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetMoviesAsync"/>
    Task<PagedResult<T>> GetMoviesAsync<T>(int keywordId, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/credit</c> endpoint — resolve a credit id from any credits block.</summary>
public interface ICreditEndpoints
{
    Task<CreditDetails> GetAsync(string creditId, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(string creditId, string? language = null, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/review</c> endpoint.</summary>
public interface IReviewEndpoints
{
    Task<ReviewDetails> GetAsync(string reviewId, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetAsync"/>
    Task<T> GetAsync<T>(string reviewId, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/watch/providers</c> endpoints.</summary>
public interface IWatchProviderEndpoints
{
    Task<WatchProviderRegions> GetRegionsAsync(string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetRegionsAsync"/>
    Task<T> GetRegionsAsync<T>(string? language = null, CancellationToken cancellationToken = default);

    Task<WatchProviderList> GetMovieProvidersAsync(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetMovieProvidersAsync"/>
    Task<T> GetMovieProvidersAsync<T>(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default);

    Task<WatchProviderList> GetTvProvidersAsync(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTvProvidersAsync"/>
    Task<T> GetTvProvidersAsync<T>(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default);
}

/// <summary>The site-wide <c>/changes</c> lists — what changed across all of TMDB.</summary>
public interface IChangesEndpoints
{
    Task<PagedResult<ChangedItem>> GetMoviesAsync(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetMoviesAsync"/>
    Task<PagedResult<T>> GetMoviesAsync<T>(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<ChangedItem>> GetTvAsync(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetTvAsync"/>
    Task<PagedResult<T>> GetTvAsync<T>(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<ChangedItem>> GetPeopleAsync(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="GetPeopleAsync"/>
    Task<PagedResult<T>> GetPeopleAsync<T>(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default);
}

internal sealed class ConfigurationEndpoints(TmdbClient client) : IConfigurationEndpoints
{
    public Task<TmdbConfiguration> GetAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<TmdbConfiguration>("3/configuration", new QueryString(), cancellationToken);

    public Task<T> GetAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/configuration", new QueryString(), cancellationToken);

    public Task<IList<CountryInfo>> GetCountriesAsync(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<IList<CountryInfo>>("3/configuration/countries", client.Language(language), cancellationToken);

    public Task<IList<T>> GetCountriesAsync<T>(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<IList<T>>("3/configuration/countries", client.Language(language), cancellationToken);

    public Task<IList<LanguageInfo>> GetLanguagesAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<IList<LanguageInfo>>("3/configuration/languages", new QueryString(), cancellationToken);

    public Task<IList<T>> GetLanguagesAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<IList<T>>("3/configuration/languages", new QueryString(), cancellationToken);

    public Task<IList<TimezoneInfo>> GetTimezonesAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<IList<TimezoneInfo>>("3/configuration/timezones", new QueryString(), cancellationToken);

    public Task<IList<T>> GetTimezonesAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<IList<T>>("3/configuration/timezones", new QueryString(), cancellationToken);

    public Task<IList<DepartmentJobs>> GetJobsAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<IList<DepartmentJobs>>("3/configuration/jobs", new QueryString(), cancellationToken);

    public Task<IList<T>> GetJobsAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<IList<T>>("3/configuration/jobs", new QueryString(), cancellationToken);

    public Task<IList<string>> GetPrimaryTranslationsAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<IList<string>>("3/configuration/primary_translations", new QueryString(), cancellationToken);
}

internal sealed class CertificationEndpoints(TmdbClient client) : ICertificationEndpoints
{
    public Task<CertificationsResponse> GetMovieCertificationsAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<CertificationsResponse>("3/certification/movie/list", new QueryString(), cancellationToken);

    public Task<T> GetMovieCertificationsAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/certification/movie/list", new QueryString(), cancellationToken);

    public Task<CertificationsResponse> GetTvCertificationsAsync(CancellationToken cancellationToken = default)
        => client.GetAsync<CertificationsResponse>("3/certification/tv/list", new QueryString(), cancellationToken);

    public Task<T> GetTvCertificationsAsync<T>(CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/certification/tv/list", new QueryString(), cancellationToken);
}

internal sealed class GenreEndpoints(TmdbClient client) : IGenreEndpoints
{
    public Task<GenreList> GetMovieGenresAsync(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<GenreList>("3/genre/movie/list", client.Language(language), cancellationToken);

    public Task<T> GetMovieGenresAsync<T>(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/genre/movie/list", client.Language(language), cancellationToken);

    public Task<GenreList> GetTvGenresAsync(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<GenreList>("3/genre/tv/list", client.Language(language), cancellationToken);

    public Task<T> GetTvGenresAsync<T>(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/genre/tv/list", client.Language(language), cancellationToken);
}

internal sealed class CollectionEndpoints(TmdbClient client) : ICollectionEndpoints
{
    public Task<CollectionDetails> GetAsync(int collectionId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<CollectionDetails>($"3/collection/{collectionId}", client.Language(language), cancellationToken);

    public Task<T> GetAsync<T>(int collectionId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/collection/{collectionId}", client.Language(language), cancellationToken);

    public Task<CollectionImages> GetImagesAsync(int collectionId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<CollectionImages>($"3/collection/{collectionId}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<T> GetImagesAsync<T>(int collectionId, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/collection/{collectionId}/images", new QueryString()
            .Add("language", language).Add("include_image_language", includeImageLanguage), cancellationToken);

    public Task<CollectionTranslations> GetTranslationsAsync(int collectionId, CancellationToken cancellationToken = default)
        => client.GetAsync<CollectionTranslations>($"3/collection/{collectionId}/translations", new QueryString(), cancellationToken);

    public Task<T> GetTranslationsAsync<T>(int collectionId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/collection/{collectionId}/translations", new QueryString(), cancellationToken);
}

internal sealed class CompanyEndpoints(TmdbClient client) : ICompanyEndpoints
{
    public Task<CompanyDetails> GetAsync(int companyId, CancellationToken cancellationToken = default)
        => client.GetAsync<CompanyDetails>($"3/company/{companyId}", new QueryString(), cancellationToken);

    public Task<T> GetAsync<T>(int companyId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/company/{companyId}", new QueryString(), cancellationToken);

    public Task<AlternativeNames> GetAlternativeNamesAsync(int companyId, CancellationToken cancellationToken = default)
        => client.GetAsync<AlternativeNames>($"3/company/{companyId}/alternative_names", new QueryString(), cancellationToken);

    public Task<T> GetAlternativeNamesAsync<T>(int companyId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/company/{companyId}/alternative_names", new QueryString(), cancellationToken);

    public Task<LogoImages> GetImagesAsync(int companyId, CancellationToken cancellationToken = default)
        => client.GetAsync<LogoImages>($"3/company/{companyId}/images", new QueryString(), cancellationToken);

    public Task<T> GetImagesAsync<T>(int companyId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/company/{companyId}/images", new QueryString(), cancellationToken);
}

internal sealed class NetworkEndpoints(TmdbClient client) : INetworkEndpoints
{
    public Task<Network> GetAsync(int networkId, CancellationToken cancellationToken = default)
        => client.GetAsync<Network>($"3/network/{networkId}", new QueryString(), cancellationToken);

    public Task<T> GetAsync<T>(int networkId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/network/{networkId}", new QueryString(), cancellationToken);

    public Task<AlternativeNames> GetAlternativeNamesAsync(int networkId, CancellationToken cancellationToken = default)
        => client.GetAsync<AlternativeNames>($"3/network/{networkId}/alternative_names", new QueryString(), cancellationToken);

    public Task<T> GetAlternativeNamesAsync<T>(int networkId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/network/{networkId}/alternative_names", new QueryString(), cancellationToken);

    public Task<LogoImages> GetImagesAsync(int networkId, CancellationToken cancellationToken = default)
        => client.GetAsync<LogoImages>($"3/network/{networkId}/images", new QueryString(), cancellationToken);

    public Task<T> GetImagesAsync<T>(int networkId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/network/{networkId}/images", new QueryString(), cancellationToken);
}

internal sealed class KeywordEndpoints(TmdbClient client) : IKeywordEndpoints
{
    public Task<KeywordDetails> GetAsync(int keywordId, CancellationToken cancellationToken = default)
        => client.GetAsync<KeywordDetails>($"3/keyword/{keywordId}", new QueryString(), cancellationToken);

    public Task<T> GetAsync<T>(int keywordId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/keyword/{keywordId}", new QueryString(), cancellationToken);

    public Task<PagedResult<MovieSummary>> GetMoviesAsync(int keywordId, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/keyword/{keywordId}/movies",
            client.Page(language, page).Add("include_adult", includeAdult), cancellationToken);

    public Task<PagedResult<T>> GetMoviesAsync<T>(int keywordId, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/keyword/{keywordId}/movies",
            client.Page(language, page).Add("include_adult", includeAdult), cancellationToken);
}

internal sealed class CreditEndpoints(TmdbClient client) : ICreditEndpoints
{
    public Task<CreditDetails> GetAsync(string creditId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<CreditDetails>($"3/credit/{creditId}", client.Language(language), cancellationToken);

    public Task<T> GetAsync<T>(string creditId, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/credit/{creditId}", client.Language(language), cancellationToken);
}

internal sealed class ReviewEndpoints(TmdbClient client) : IReviewEndpoints
{
    public Task<ReviewDetails> GetAsync(string reviewId, CancellationToken cancellationToken = default)
        => client.GetAsync<ReviewDetails>($"3/review/{reviewId}", new QueryString(), cancellationToken);

    public Task<T> GetAsync<T>(string reviewId, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/review/{reviewId}", new QueryString(), cancellationToken);
}

internal sealed class WatchProviderEndpoints(TmdbClient client) : IWatchProviderEndpoints
{
    public Task<WatchProviderRegions> GetRegionsAsync(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<WatchProviderRegions>("3/watch/providers/regions", client.Language(language), cancellationToken);

    public Task<T> GetRegionsAsync<T>(string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/watch/providers/regions", client.Language(language), cancellationToken);

    public Task<WatchProviderList> GetMovieProvidersAsync(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default)
        => client.GetAsync<WatchProviderList>("3/watch/providers/movie",
            client.Language(language).Add("watch_region", watchRegion), cancellationToken);

    public Task<T> GetMovieProvidersAsync<T>(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/watch/providers/movie",
            client.Language(language).Add("watch_region", watchRegion), cancellationToken);

    public Task<WatchProviderList> GetTvProvidersAsync(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default)
        => client.GetAsync<WatchProviderList>("3/watch/providers/tv",
            client.Language(language).Add("watch_region", watchRegion), cancellationToken);

    public Task<T> GetTvProvidersAsync<T>(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>("3/watch/providers/tv",
            client.Language(language).Add("watch_region", watchRegion), cancellationToken);
}

internal sealed class ChangesEndpoints(TmdbClient client) : IChangesEndpoints
{
    public Task<PagedResult<ChangedItem>> GetMoviesAsync(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<ChangedItem>>("3/movie/changes", Window(startDate, endDate, page), cancellationToken);

    public Task<PagedResult<T>> GetMoviesAsync<T>(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/movie/changes", Window(startDate, endDate, page), cancellationToken);

    public Task<PagedResult<ChangedItem>> GetTvAsync(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<ChangedItem>>("3/tv/changes", Window(startDate, endDate, page), cancellationToken);

    public Task<PagedResult<T>> GetTvAsync<T>(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/tv/changes", Window(startDate, endDate, page), cancellationToken);

    public Task<PagedResult<ChangedItem>> GetPeopleAsync(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<ChangedItem>>("3/person/changes", Window(startDate, endDate, page), cancellationToken);

    public Task<PagedResult<T>> GetPeopleAsync<T>(DateOnly? startDate = null, DateOnly? endDate = null, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/person/changes", Window(startDate, endDate, page), cancellationToken);

    static QueryString Window(DateOnly? startDate, DateOnly? endDate, int? page) => new QueryString()
        .Add("start_date", startDate).Add("end_date", endDate).Add("page", page);
}
