using System.Text.Json.Serialization;

namespace TmdbPlus.Models;

/// <summary>
/// The envelope 12 of the 17 write operations return. Exposed rather than collapsed to a
/// <c>bool</c> as TMDbLib does -- the error path carries the reason. Issue #8.
/// </summary>
public class TmdbStatusResponse : ITmdbStatusResponse
{
    [JsonPropertyName("status_code")] public int StatusCode { get; set; }
    [JsonPropertyName("status_message")] public string? StatusMessage { get; set; }

    /// <summary>TMDB sends this; the OpenAPI spec omits it.</summary>
    [JsonPropertyName("success")] public bool? Success { get; set; }
}

/// <inheritdoc cref="TmdbStatusResponse"/>
public interface ITmdbStatusResponse
{
    int StatusCode { get; set; }
    string? StatusMessage { get; set; }
    bool? Success { get; set; }
}
