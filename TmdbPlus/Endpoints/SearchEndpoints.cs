using TmdbPlus.Models;

namespace TmdbPlus.Endpoints;

/// <summary>The <c>/search</c> endpoints.</summary>
public interface ISearchEndpoints
{
    Task<PagedResult<MovieSummary>> MoviesAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, int? year = null, int? primaryReleaseYear = null,
        CancellationToken cancellationToken = default);
    /// <inheritdoc cref="MoviesAsync"/>
    Task<PagedResult<T>> MoviesAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, int? year = null, int? primaryReleaseYear = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> TvAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, int? year = null, int? firstAirDateYear = null,
        CancellationToken cancellationToken = default);
    /// <inheritdoc cref="TvAsync"/>
    Task<PagedResult<T>> TvAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, int? year = null, int? firstAirDateYear = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<PersonSummary>> PeopleAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="PeopleAsync"/>
    Task<PagedResult<T>> PeopleAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default);

    /// <summary>Movies, series, and people in one list — branch on each result's media type.</summary>
    Task<PagedResult<MultiSearchResult>> MultiAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="MultiAsync"/>
    Task<PagedResult<T>> MultiAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default);

    Task<PagedResult<CollectionSummary>> CollectionsAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="CollectionsAsync"/>
    Task<PagedResult<T>> CollectionsAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, CancellationToken cancellationToken = default);

    Task<PagedResult<CompanySummary>> CompaniesAsync(string query, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="CompaniesAsync"/>
    Task<PagedResult<T>> CompaniesAsync<T>(string query, int? page = null, CancellationToken cancellationToken = default);

    Task<PagedResult<Keyword>> KeywordsAsync(string query, int? page = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="KeywordsAsync"/>
    Task<PagedResult<T>> KeywordsAsync<T>(string query, int? page = null, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/discover</c> endpoints.</summary>
public interface IDiscoverEndpoints
{
    Task<PagedResult<MovieSummary>> MoviesAsync(DiscoverMovieOptions? options = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="MoviesAsync"/>
    Task<PagedResult<T>> MoviesAsync<T>(DiscoverMovieOptions? options = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> TvAsync(DiscoverTvOptions? options = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="TvAsync"/>
    Task<PagedResult<T>> TvAsync<T>(DiscoverTvOptions? options = null, CancellationToken cancellationToken = default);
}

/// <summary>The <c>/trending</c> endpoints.</summary>
public interface ITrendingEndpoints
{
    /// <summary>Movies, series, and people in one list.</summary>
    Task<PagedResult<MultiSearchResult>> AllAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="AllAsync"/>
    Task<PagedResult<T>> AllAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);

    Task<PagedResult<MovieSummary>> MoviesAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="MoviesAsync"/>
    Task<PagedResult<T>> MoviesAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);

    Task<PagedResult<TvSeriesSummary>> TvAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="TvAsync"/>
    Task<PagedResult<T>> TvAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);

    Task<PagedResult<PersonSummary>> PeopleAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="PeopleAsync"/>
    Task<PagedResult<T>> PeopleAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default);
}

internal sealed class SearchEndpoints(TmdbClient client) : ISearchEndpoints
{
    public Task<PagedResult<MovieSummary>> MoviesAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, int? year = null, int? primaryReleaseYear = null,
        CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>("3/search/movie", client.Page(language, page, region)
            .Add("query", query)
            .Add("include_adult", includeAdult)
            .Add("year", year)
            .Add("primary_release_year", primaryReleaseYear), cancellationToken);

    public Task<PagedResult<T>> MoviesAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, int? year = null, int? primaryReleaseYear = null,
        CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/search/movie", client.Page(language, page, region)
            .Add("query", query)
            .Add("include_adult", includeAdult)
            .Add("year", year)
            .Add("primary_release_year", primaryReleaseYear), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> TvAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, int? year = null, int? firstAirDateYear = null,
        CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>("3/search/tv", client.Page(language, page)
            .Add("query", query)
            .Add("include_adult", includeAdult)
            .Add("year", year)
            .Add("first_air_date_year", firstAirDateYear), cancellationToken);

    public Task<PagedResult<T>> TvAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, int? year = null, int? firstAirDateYear = null,
        CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/search/tv", client.Page(language, page)
            .Add("query", query)
            .Add("include_adult", includeAdult)
            .Add("year", year)
            .Add("first_air_date_year", firstAirDateYear), cancellationToken);

    public Task<PagedResult<PersonSummary>> PeopleAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<PersonSummary>>("3/search/person", client.Page(language, page)
            .Add("query", query).Add("include_adult", includeAdult), cancellationToken);

    public Task<PagedResult<T>> PeopleAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/search/person", client.Page(language, page)
            .Add("query", query).Add("include_adult", includeAdult), cancellationToken);

    public Task<PagedResult<MultiSearchResult>> MultiAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MultiSearchResult>>("3/search/multi", client.Page(language, page)
            .Add("query", query).Add("include_adult", includeAdult), cancellationToken);

    public Task<PagedResult<T>> MultiAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/search/multi", client.Page(language, page)
            .Add("query", query).Add("include_adult", includeAdult), cancellationToken);

    public Task<PagedResult<CollectionSummary>> CollectionsAsync(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<CollectionSummary>>("3/search/collection", client.Page(language, page, region)
            .Add("query", query).Add("include_adult", includeAdult), cancellationToken);

    public Task<PagedResult<T>> CollectionsAsync<T>(string query, string? language = null, int? page = null,
        bool? includeAdult = null, string? region = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/search/collection", client.Page(language, page, region)
            .Add("query", query).Add("include_adult", includeAdult), cancellationToken);

    public Task<PagedResult<CompanySummary>> CompaniesAsync(string query, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<CompanySummary>>("3/search/company", new QueryString()
            .Add("query", query).Add("page", page), cancellationToken);

    public Task<PagedResult<T>> CompaniesAsync<T>(string query, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/search/company", new QueryString()
            .Add("query", query).Add("page", page), cancellationToken);

    public Task<PagedResult<Keyword>> KeywordsAsync(string query, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<Keyword>>("3/search/keyword", new QueryString()
            .Add("query", query).Add("page", page), cancellationToken);

    public Task<PagedResult<T>> KeywordsAsync<T>(string query, int? page = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/search/keyword", new QueryString()
            .Add("query", query).Add("page", page), cancellationToken);
}

internal sealed class DiscoverEndpoints(TmdbClient client) : IDiscoverEndpoints
{
    public Task<PagedResult<MovieSummary>> MoviesAsync(DiscoverMovieOptions? options = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>("3/discover/movie",
            (options ?? new DiscoverMovieOptions()).ToQuery(client.DefaultLanguage, client.DefaultRegion), cancellationToken);

    public Task<PagedResult<T>> MoviesAsync<T>(DiscoverMovieOptions? options = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/discover/movie",
            (options ?? new DiscoverMovieOptions()).ToQuery(client.DefaultLanguage, client.DefaultRegion), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> TvAsync(DiscoverTvOptions? options = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>("3/discover/tv",
            (options ?? new DiscoverTvOptions()).ToQuery(client.DefaultLanguage), cancellationToken);

    public Task<PagedResult<T>> TvAsync<T>(DiscoverTvOptions? options = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>("3/discover/tv",
            (options ?? new DiscoverTvOptions()).ToQuery(client.DefaultLanguage), cancellationToken);
}

internal sealed class TrendingEndpoints(TmdbClient client) : ITrendingEndpoints
{
    public Task<PagedResult<MultiSearchResult>> AllAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MultiSearchResult>>($"3/trending/all/{window.ToWire()}", client.Language(language), cancellationToken);

    public Task<PagedResult<T>> AllAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/trending/all/{window.ToWire()}", client.Language(language), cancellationToken);

    public Task<PagedResult<MovieSummary>> MoviesAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<MovieSummary>>($"3/trending/movie/{window.ToWire()}", client.Language(language), cancellationToken);

    public Task<PagedResult<T>> MoviesAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/trending/movie/{window.ToWire()}", client.Language(language), cancellationToken);

    public Task<PagedResult<TvSeriesSummary>> TvAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<TvSeriesSummary>>($"3/trending/tv/{window.ToWire()}", client.Language(language), cancellationToken);

    public Task<PagedResult<T>> TvAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/trending/tv/{window.ToWire()}", client.Language(language), cancellationToken);

    public Task<PagedResult<PersonSummary>> PeopleAsync(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<PersonSummary>>($"3/trending/person/{window.ToWire()}", client.Language(language), cancellationToken);

    public Task<PagedResult<T>> PeopleAsync<T>(TimeWindow window = TimeWindow.Day, string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<PagedResult<T>>($"3/trending/person/{window.ToWire()}", client.Language(language), cancellationToken);
}

/// <summary>The <c>/find</c> endpoint: look a title up by an id from another database.</summary>
public interface IFindEndpoints
{
    Task<FindResults> ByExternalIdAsync(string externalId, ExternalSource source = ExternalSource.Imdb,
        string? language = null, CancellationToken cancellationToken = default);
    /// <inheritdoc cref="ByExternalIdAsync"/>
    Task<T> ByExternalIdAsync<T>(string externalId, ExternalSource source = ExternalSource.Imdb,
        string? language = null, CancellationToken cancellationToken = default);
}

internal sealed class FindEndpoints(TmdbClient client) : IFindEndpoints
{
    public Task<FindResults> ByExternalIdAsync(string externalId, ExternalSource source = ExternalSource.Imdb,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<FindResults>($"3/find/{externalId}", client.Language(language)
            .Add("external_source", source.ToWire()), cancellationToken);

    public Task<T> ByExternalIdAsync<T>(string externalId, ExternalSource source = ExternalSource.Imdb,
        string? language = null, CancellationToken cancellationToken = default)
        => client.GetAsync<T>($"3/find/{externalId}", client.Language(language)
            .Add("external_source", source.ToWire()), cancellationToken);
}
