using System.Globalization;
using System.Text;

namespace TmdbPlus;

/// <summary>
/// Builds a query string, skipping absent values so an endpoint never has to branch on them.
/// Mutable and short-lived: built and stringified inside a single call.
/// </summary>
internal struct QueryString()
{
    StringBuilder? _sb = null;

    public QueryString Add(string name, string? value)
    {
        if (string.IsNullOrEmpty(value)) return this;
        _sb ??= new StringBuilder();
        _sb.Append(_sb.Length == 0 ? '?' : '&')
           .Append(Uri.EscapeDataString(name))
           .Append('=')
           .Append(Uri.EscapeDataString(value));
        return this;
    }

    public QueryString Add(string name, int? value)
        => value is null ? this : Add(name, value.Value.ToString(CultureInfo.InvariantCulture));

    public QueryString Add(string name, bool? value)
        => value is null ? this : Add(name, value.Value ? "true" : "false");

    public QueryString Add(string name, double? value)
        => value is null ? this : Add(name, value.Value.ToString(CultureInfo.InvariantCulture));

    public QueryString Add(string name, DateOnly? value)
        => value is null ? this : Add(name, value.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

    public override readonly string ToString() => _sb?.ToString() ?? string.Empty;

    public static implicit operator string(QueryString q) => q.ToString();
}
