namespace TombIDE.Core.Extensions;

public static class StringExtensions
{
	public static bool BulkStringComparision(this string value, StringComparison comparisonType, params string[] strings)
		=> Array.Exists(strings, x => x.Equals(value, comparisonType));

	public static bool IsEqualButCaseChanged(this string value, string? toCompare)
		=> value.Equals(toCompare, StringComparison.OrdinalIgnoreCase) && value != toCompare;

	public static string[] SplitLines(this string value)
		=> value.Replace("\r\n", "\n").Split("\n");

	public static string[] TrimStartAll(this string[] array)
		=> array.Select(entry => entry.TrimStart()).ToArray();

	public static string[] TrimEndAll(this string[] array)
		=> array.Select(entry => entry.TrimEnd()).ToArray();

	public static string[] TrimAll(this string[] array)
		=> array.Select(entry => entry.Trim()).ToArray();
}
