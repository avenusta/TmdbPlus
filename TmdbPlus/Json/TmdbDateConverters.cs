using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TmdbPlus.Json;

/// <summary>
/// TMDB sends <c>""</c> for an absent date rather than <c>null</c>, which throws the built-in
/// converter. Load-bearing: see issue #3.
/// </summary>
public sealed class TmdbDateOnlyConverter : JsonConverter<DateOnly?>
{
    public override DateOnly? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        var s = reader.GetString();
        return DateOnly.TryParse(s, CultureInfo.InvariantCulture, out var d) ? d : null;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value is null) writer.WriteNullValue();
        else writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}

/// <inheritdoc cref="TmdbDateOnlyConverter"/>
public sealed class TmdbDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        var s = reader.GetString();
        return DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal, out var d) ? d : null;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value is null) writer.WriteNullValue();
        else writer.WriteStringValue(value.Value);
    }
}
