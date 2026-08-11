using System.Text.Json;
using System.Text.Json.Serialization;

namespace TmdbPlus.Json;

/// <summary>
/// <c>rated</c> is polymorphic on one key: <c>false</c> when unrated, <c>{"value": 7.5}</c> when
/// rated. Issue #7. Maps both onto <c>double?</c>.
/// </summary>
public sealed class TmdbRatingConverter : JsonConverter<double?>
{
    public override double? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.False:
            case JsonTokenType.True:
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.Number:
                return reader.GetDouble();

            case JsonTokenType.StartObject:
                double? value = null;
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType != JsonTokenType.PropertyName) continue;
                    var name = reader.GetString();
                    reader.Read();
                    if (name == "value" && reader.TokenType == JsonTokenType.Number)
                        value = reader.GetDouble();
                    else
                        reader.Skip();
                }
                return value;

            default:
                reader.Skip();
                return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
    {
        if (value is null) { writer.WriteBooleanValue(false); return; }
        writer.WriteStartObject();
        writer.WriteNumber("value", value.Value);
        writer.WriteEndObject();
    }
}
