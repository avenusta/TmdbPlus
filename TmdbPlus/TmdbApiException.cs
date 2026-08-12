using System.Net;
using TmdbPlus.Models;

namespace TmdbPlus;

/// <summary>
/// A non-success response from TMDB. Carries the <c>status_code</c>/<c>status_message</c>
/// envelope, which TMDB returns on failures too (issue #8 observed HTTP 400 with
/// <c>status_code: 18</c> and a usable message) and which
/// <see cref="HttpRequestException"/> would discard.
/// </summary>
/// <remarks>
/// There is no rate-limit subclass: a <c>429</c> carries nothing this type does not already hold
/// (TMDB sends no <c>Retry-After</c>), so filter on the status instead (issue #16):
/// <code>
/// catch (TmdbApiException ex) when (ex.HttpStatus == HttpStatusCode.TooManyRequests)
/// </code>
/// </remarks>
public sealed class TmdbApiException(
    HttpStatusCode httpStatus,
    TmdbStatusResponse? status,
    string? body)
    : Exception(BuildMessage(httpStatus, status))
{
    public HttpStatusCode HttpStatus { get; } = httpStatus;

    /// <summary>TMDB's own status code, or <c>null</c> when the body was not the usual envelope.</summary>
    public int? StatusCode { get; } = status?.StatusCode;

    public string? StatusMessage { get; } = status?.StatusMessage;

    /// <summary>The raw response body, for failures that carry a shape we do not model.</summary>
    public string? Body { get; } = body;

    static string BuildMessage(HttpStatusCode http, TmdbStatusResponse? s)
        => s?.StatusMessage is { Length: > 0 } m
            ? $"TMDB returned {(int)http} {http} (status_code {s.StatusCode}): {m}"
            : $"TMDB returned {(int)http} {http}.";
}
