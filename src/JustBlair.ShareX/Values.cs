using System.Text.Json;

namespace JustBlair.ShareX;

/// <summary>Parameter values arrive as whatever the wire produced (a CLR string or a JsonElement), so
/// every read goes through here.</summary>
internal static class Values
{
	public static string? ReadString(object? value) => value switch
	{
		null => null,
		string s => string.IsNullOrWhiteSpace(s) ? null : s.Trim(),
		JsonElement { ValueKind: JsonValueKind.String } e => ReadString(e.GetString()),
		_ => null
	};
}
