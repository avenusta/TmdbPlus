using Microsoft.Extensions.Options;

namespace TmdbPlus.Auth;

/// <summary>
/// Adds <c>Authorization: Bearer &lt;token&gt;</c> to every request. The token is resolved
/// per request, so a rotating token needs no new client. Issue #5.
/// </summary>
public sealed class TmdbAuthHandler(IOptionsMonitor<TmdbOptions> options) : DelegatingHandler
{
    /// <summary>
    /// Lets a single request carry a different bearer token. v4's user-scoped endpoints need the
    /// user's access token rather than the application one, and the token has to reach the
    /// handler without becoming client state.
    /// </summary>
    internal static readonly HttpRequestOptionsKey<string> TokenOverride = new("TmdbPlus.Token");

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var opts = options.CurrentValue;

        var token = request.Options.TryGetValue(TokenOverride, out var perRequest)
            ? perRequest
            : opts.TokenResolver?.Invoke() ?? opts.ReadAccessToken;

        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException(
                $"No TMDB read access token. Set {nameof(TmdbOptions)}.{nameof(TmdbOptions.ReadAccessToken)} " +
                $"or {nameof(TmdbOptions.TokenResolver)}.");

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return base.SendAsync(request, cancellationToken);
    }
}
