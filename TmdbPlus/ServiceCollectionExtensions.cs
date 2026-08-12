using Microsoft.Extensions.DependencyInjection;
using TmdbPlus.Auth;

namespace TmdbPlus;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="ITmdbClient"/> on a named <see cref="HttpClient"/> whose pipeline
    /// carries the bearer-auth handler. The client holds no session state, so it is safe as a
    /// singleton; the token is resolved per request (issue #5).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Returns <see cref="IHttpClientBuilder"/> so retry, throttling, and circuit-breaking stay
    /// yours to compose — the library ships none of them (issue #16):
    /// </para>
    /// <code>
    /// services.AddTmdb(token)
    ///         .AddStandardResilienceHandler();
    /// </code>
    /// <para>
    /// TMDB's documented ceiling is roughly 40 requests per second, and it "could change at any
    /// time", so no threshold is baked in here. Exceeding it answers <c>429</c>, which surfaces
    /// as <see cref="TmdbApiException"/> — catch it with
    /// <c>when (ex.HttpStatus == HttpStatusCode.TooManyRequests)</c>. TMDB sends no
    /// <c>Retry-After</c> header, so a backoff delay is the caller's choice.
    /// </para>
    /// </remarks>
    public static IHttpClientBuilder AddTmdb(this IServiceCollection services, Action<TmdbOptions> configure)
    {
        services.Configure(configure);
        services.AddTransient<TmdbAuthHandler>();

        return services.AddHttpClient<ITmdbClient, TmdbClient>(TmdbClient.HttpClientName, (sp, http) =>
            {
                var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<TmdbOptions>>().Value;
                http.BaseAddress = options.BaseAddress;
            })
            .AddHttpMessageHandler<TmdbAuthHandler>();
    }

    /// <summary>Registers with the token read from configuration or an environment variable.</summary>
    public static IHttpClientBuilder AddTmdb(this IServiceCollection services, string readAccessToken)
        => services.AddTmdb(o => o.ReadAccessToken = readAccessToken);
}
