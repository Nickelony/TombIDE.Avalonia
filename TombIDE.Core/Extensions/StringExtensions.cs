namespace TombIDE.Core.Extensions;

public static class StringExtensions
{
	public static bool BulkStringComparision(this string value, StringComparison comparisonType, params string[] strings)
		=> Array.Exists(strings, x => x.Equals(value, comparisonType));

	public static bool IsEqualButCaseChanged(this string value, string? toCompare)
		=> value.Equals(toCompare, StringComparison.OrdinalIgnoreCase) && value != toCompare;
}
