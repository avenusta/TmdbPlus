using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TmdbPlus.Json;

/// <summary>
/// An enum value plus the wire text TMDB sent. Two properties cannot share a JSON key and
/// <c>[JsonExtensionData]</c> only captures unmatched keys, so raw preservation has to live in
/// the value itself. Issue #10.
/// </summary>
public readonly struct TmdbEnum<T>(T value, string? raw) where T : struct, Enum
{
    public T Value { get; } = value;

    /// <summary>The wire value as TMDB sent it. Set even when <see cref="Value"/> is Unknown.</summary>
    public string? Raw { get; } = raw;

    public bool IsKnown => !Value.Equals(default(T));

    public override string ToString() => IsKnown ? Value.ToString() : $"Unknown({Raw})";

    public static implicit operator T(TmdbEnum<T> e) => e.Value;
}

/// <summary>
/// Maps TMDB's wire vocabulary onto an enum, degrading an unrecognised value to
/// <c>Unknown = 0</c> rather than throwing. The built-in <c>JsonStringEnumConverter</c> throws;
/// a plain numeric enum silently casts anything, so numbers are checked with
/// <see cref="Enum.IsDefined(Type, object)"/>. Issue #10.
/// </summary>
public sealed class TmdbEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    static readonly Dictionary<string, T> Wire = Build();
    static readonly Dictionary<T, string> Names = Build().ToDictionary(kv => kv.Value, kv => kv.Key);

    static Dictionary<string, T> Build()
    {
        var map = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        foreach (var f in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var value = (T)f.GetValue(null)!;
            var attr = f.GetCustomAttribute<JsonStringEnumMemberNameAttribute>();
            map[attr?.Name ?? f.Name] = value;
        }
        return map;
    }

    internal static T Lookup(string s) => Wire.TryGetValue(s, out var v) ? v : default;

    internal static string? NameOf(T value) => Names.TryGetValue(value, out var s) ? s : null;

    public override T Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var boxed = Enum.ToObject(typeof(T), reader.GetInt32());
            return Enum.IsDefined(typeof(T), boxed) ? (T)boxed : default;
        }

        var s = reader.GetString();
        return string.IsNullOrEmpty(s) ? default : Lookup(s);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var wire = NameOf(value);
        if (wire is null) writer.WriteNullValue(); else writer.WriteStringValue(wire);
    }
}

/// <summary>Reads into a <see cref="TmdbEnum{T}"/>, keeping the raw wire text.</summary>
public sealed class TmdbEnumValueConverter<T> : JsonConverter<TmdbEnum<T>> where T : struct, Enum
{
    public override TmdbEnum<T> Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var n = reader.GetInt32();
            var boxed = Enum.ToObject(typeof(T), n);
            var known = Enum.IsDefined(typeof(T), boxed);
            return new TmdbEnum<T>(known ? (T)boxed : default, n.ToString());
        }

        var s = reader.GetString();
        if (string.IsNullOrEmpty(s)) return new TmdbEnum<T>(default, s);
        return new TmdbEnum<T>(TmdbEnumConverter<T>.Lookup(s), s);
    }

    public override void Write(Utf8JsonWriter writer, TmdbEnum<T> value, JsonSerializerOptions options)
    {
        // Round-trip exactly what came in, known or not.
        if (value.Raw is not null) writer.WriteStringValue(value.Raw);
        else writer.WriteNullValue();
    }
}
