using Microsoft.Extensions.Options;

namespace TmdbPlus.Auth;

/// <summary>
/// Adds <c>Authorization: Bearer &lt;token&gt;</c> to every request. The token is resolved
/// per request, so a rotating token needs no new client. Issue #5.
/// </summary>
public sealed class TmdbAuthHandler(IOptionsMonitor<TmdbOptions> options) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var opts = options.CurrentValue;
        var token = opts.TokenResolver?.Invoke() ?? opts.ReadAccessToken;

        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException(
                $"No TMDB read access token. Set {nameof(TmdbOptions)}.{nameof(TmdbOptions.ReadAccessToken)} " +
                $"or {nameof(TmdbOptions.TokenResolver)}.");

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return base.SendAsync(request, cancellationToken);
    }
}
