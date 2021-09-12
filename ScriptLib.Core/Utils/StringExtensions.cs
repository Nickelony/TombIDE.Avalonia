using System;

namespace ScriptLib.Core.Utils
{
	public static class StringExtensions
	{
		public static bool BulkStringComparision(string value, StringComparison comparisonType, params string[] strings)
		{
			foreach (string @string in strings)
				if (value.Equals(@string, comparisonType))
					return true;

			return false;
		}
	}
}
