using AvaloniaEdit.Document;
using ScriptLib.ClassicScript.Data;
using ScriptLib.Core.Utils;
using System;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Parsers
{
	public static class LineParser
	{
		public static MatchCollection GetComments(string lineText)
			=> Regex.Matches(lineText, Patterns.Comment, RegexOptions.Multiline);

		public static string RemoveComments(string lineText)
			=> Regex.Replace(lineText, Patterns.Comment, string.Empty, RegexOptions.Multiline);

		/// <summary>
		/// Replaces comments with whitespace to maintain the original string length.
		/// </summary>
		public static string EscapeComments(string lineText)
		{
			MatchCollection comments = GetComments(lineText);

			foreach (Match match in comments)
				lineText = Regex.Replace(lineText, Regex.Escape(match.Value), new string(' ', match.Length));

			return lineText;
		}

		public static string RemoveNGStringIndex(string lineText)
			=> Regex.Replace(lineText, Patterns.NGStringIndex, string.Empty, RegexOptions.Multiline).TrimStart();

		public static bool IsEmptyOrComments(string lineText)
			=> string.IsNullOrWhiteSpace(lineText) || lineText.TrimStart().StartsWith(";");

		public static bool IsSectionHeaderLine(string lineText)
			=> Regex.IsMatch(lineText, Patterns.AnySectionStart);

		/// <summary>
		/// Input: "[Options] ; Options section"<br />
		/// Output: "Options"
		/// </summary>
		public static string GetSectionHeaderText(string sectionHeaderLine)
			=> Regex.Match(sectionHeaderLine, Patterns.AnySectionStart).Groups[1].Value;

		/// <summary>
		/// Checks if the line is in the [Strings], [PCStrings] or [PSXStrings] section.
		/// </summary>
		public static bool IsLineInStandardStringSection(TextDocument document, DocumentLine line)
		{
			string? lineSectionName = DocumentParser.GetSectionNameFromOffset(document, line.Offset);

			if (string.IsNullOrEmpty(lineSectionName))
				return false;

			return StringExtensions.BulkStringComparision(lineSectionName, StringComparison.OrdinalIgnoreCase,
				"strings", "pcstrings", "psxstrings");
		}

		public static bool IsLineInExtraNGSection(TextDocument document, DocumentLine line)
		{
			string? lineSectionName = DocumentParser.GetSectionNameFromOffset(document, line.Offset);

			if (string.IsNullOrEmpty(lineSectionName))
				return false;

			return lineSectionName.Equals("extrang", StringComparison.OrdinalIgnoreCase);
		}
	}
}
