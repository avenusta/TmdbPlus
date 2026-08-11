using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using TmdbPlus.Endpoints;
using TmdbPlus.Json;
using TmdbPlus.Models;

namespace TmdbPlus;

/// <summary>
/// Entry point. Endpoints are grouped by TMDB area and reached through the properties below.
/// Immutable and safe to register as a singleton -- sessions are passed per call, never held.
/// </summary>
public sealed class TmdbClient : ITmdbClient
{
    internal const string HttpClientName = "TmdbPlus";

    readonly HttpClient _http;
    readonly TmdbOptions _options;

    public TmdbClient(HttpClient http, IOptions<TmdbOptions> options)
    {
        _http = http;
        _options = options.Value;
        if (_http.BaseAddress is null) _http.BaseAddress = _options.BaseAddress;

        Movies = new MovieEndpoints(this);
        Tv = new TvEndpoints(this);
        People = new PeopleEndpoints(this);
        Search = new SearchEndpoints(this);
        Discover = new DiscoverEndpoints(this);
        Trending = new TrendingEndpoints(this);
        Find = new FindEndpoints(this);
        Account = new AccountEndpoints(this);
        GuestSessions = new GuestSessionEndpoints(this);
        Authentication = new AuthenticationEndpoints(this);
        Lists = new ListEndpoints(this);
        Configuration = new ConfigurationEndpoints(this);
        Certifications = new CertificationEndpoints(this);
        Genres = new GenreEndpoints(this);
        Collections = new CollectionEndpoints(this);
        Companies = new CompanyEndpoints(this);
        Networks = new NetworkEndpoints(this);
        Keywords = new KeywordEndpoints(this);
        Credits = new CreditEndpoints(this);
        Reviews = new ReviewEndpoints(this);
        WatchProviders = new WatchProviderEndpoints(this);
        Changes = new ChangesEndpoints(this);
    }

    public IMovieEndpoints Movies { get; }
    public ITvEndpoints Tv { get; }
    public IPeopleEndpoints People { get; }
    public ISearchEndpoints Search { get; }
    public IDiscoverEndpoints Discover { get; }
    public ITrendingEndpoints Trending { get; }
    public IFindEndpoints Find { get; }
    public IAccountEndpoints Account { get; }
    public IGuestSessionEndpoints GuestSessions { get; }
    public IAuthenticationEndpoints Authentication { get; }
    public IListEndpoints Lists { get; }
    public IConfigurationEndpoints Configuration { get; }
    public ICertificationEndpoints Certifications { get; }
    public IGenreEndpoints Genres { get; }
    public ICollectionEndpoints Collections { get; }
    public ICompanyEndpoints Companies { get; }
    public INetworkEndpoints Networks { get; }
    public IKeywordEndpoints Keywords { get; }
    public ICreditEndpoints Credits { get; }
    public IReviewEndpoints Reviews { get; }
    public IWatchProviderEndpoints WatchProviders { get; }
    public IChangesEndpoints Changes { get; }

    /// <summary>
    /// Shared serializer options. Endpoints bind JSON straight into the public types, so the
    /// converters that have to apply everywhere live here rather than on every property.
    /// </summary>
    internal static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = null,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
            new TmdbDateOnlyConverter(),
            new TmdbDateTimeOffsetConverter(),
        },
    };

    internal string? DefaultLanguage => _options.DefaultLanguage;
    internal string? DefaultRegion => _options.DefaultRegion;

    // The three query shapes that recur across nearly every area, hoisted so an endpoint that
    // uses one stays a single expression.

    /// <summary>Just <c>language</c>, falling back to the configured default.</summary>
    internal QueryString Language(string? language)
        => new QueryString().Add("language", language ?? DefaultLanguage);

    /// <summary><c>language</c> + <c>page</c>.</summary>
    internal QueryString Page(string? language, int? page)
        => Language(language).Add("page", page);

    /// <summary><c>language</c> + <c>page</c> + <c>region</c>.</summary>
    internal QueryString Page(string? language, int? page, string? region)
        => Page(language, page).Add("region", region ?? DefaultRegion);

    // ---------------------------------------------------------------------
    // The one path every endpoint goes through.
    // ---------------------------------------------------------------------

    internal Task<T> GetAsync<T>(string path, QueryString query, CancellationToken ct)
        => SendAsync<T>(new HttpRequestMessage(HttpMethod.Get, path + query), ct);

    internal Task<T> PostAsync<T>(string path, QueryString query, object? body, CancellationToken ct)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path + query);
        if (body is not null) request.Content = JsonContent.Create(body, options: Json);
        return SendAsync<T>(request, ct);
    }

    /// <summary>DELETE carries a body on <c>/authentication/session</c>, so one is allowed here.</summary>
    internal Task<T> DeleteAsync<T>(string path, QueryString query, CancellationToken ct, object? body = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, path + query);
        if (body is not null) request.Content = JsonContent.Create(body, options: Json);
        return SendAsync<T>(request, ct);
    }

    async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken ct)
    {
        using var response = await _http.SendAsync(request, ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            TmdbStatusResponse? status = null;
            try { status = JsonSerializer.Deserialize<TmdbStatusResponse>(body, Json); }
            catch (JsonException) { /* not the usual envelope; Body carries it instead */ }
            throw new TmdbApiException(response.StatusCode, status, body);
        }

        var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        var result = await JsonSerializer.DeserializeAsync<T>(stream, Json, ct).ConfigureAwait(false);

        return result ?? throw new TmdbApiException(response.StatusCode, null,
            "TMDB returned a success status with a null body.");
    }
}

/// <inheritdoc cref="TmdbClient"/>
public interface ITmdbClient
{
    IMovieEndpoints Movies { get; }
    ITvEndpoints Tv { get; }
    IPeopleEndpoints People { get; }
    ISearchEndpoints Search { get; }
    IDiscoverEndpoints Discover { get; }
    ITrendingEndpoints Trending { get; }
    IFindEndpoints Find { get; }
    IAccountEndpoints Account { get; }
    IGuestSessionEndpoints GuestSessions { get; }
    IAuthenticationEndpoints Authentication { get; }
    IListEndpoints Lists { get; }
    IConfigurationEndpoints Configuration { get; }
    ICertificationEndpoints Certifications { get; }
    IGenreEndpoints Genres { get; }
    ICollectionEndpoints Collections { get; }
    ICompanyEndpoints Companies { get; }
    INetworkEndpoints Networks { get; }
    IKeywordEndpoints Keywords { get; }
    ICreditEndpoints Credits { get; }
    IReviewEndpoints Reviews { get; }
    IWatchProviderEndpoints WatchProviders { get; }
    IChangesEndpoints Changes { get; }
}
