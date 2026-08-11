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
