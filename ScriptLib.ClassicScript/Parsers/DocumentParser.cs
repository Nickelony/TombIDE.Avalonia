using AvaloniaEdit.Document;
using ScriptLib.ClassicScript.Data;
using ScriptLib.ClassicScript.Data.Enums;
using System;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Parsers
{
	public static class DocumentParser
	{
		public static bool DocumentContainsSections(TextDocument document)
			=> GetSectionsCount(document) > 0;

		public static string? GetSectionNameFromOffset(TextDocument document, int offset)
		{
			DocumentLine? sectionStartLine = GetSectionStartLineFromOffset(document, offset);

			if (sectionStartLine == null)
				return null;

			string lineText = document.GetText(sectionStartLine);
			return lineText.Split('[')[1].Split(']')[0]; // "[PSXStrings]" >> "PSXStrings"
		}

		public static int GetSectionsCount(TextDocument document)
		{
			int sectionsCount = 0;

			foreach (DocumentLine line in document.Lines)
			{
				string lineText = document.GetText(line);

				if (LineParser.IsSectionHeaderLine(lineText))
					sectionsCount++;
			}

			return sectionsCount;
		}

		public static DocumentLine? GetSectionStartLineFromOffset(TextDocument document, int offset)
		{
			DocumentLine offsetLine = document.GetLineByOffset(offset);

			for (int i = offsetLine.LineNumber; i >= 1; i--)
			{
				DocumentLine currentLine = document.GetLineByNumber(i);
				string currentLineText = document.GetText(currentLine);

				if (LineParser.IsSectionHeaderLine(currentLineText))
					return currentLine;
			}

			return null;
		}

		// TODO: Refactor!

		public static DocumentLine? GetLastLineOfCurrentSection(TextDocument document, int offset)
		{
			DocumentLine offsetLine = document.GetLineByOffset(offset);
			DocumentLine? sectionStartLine = GetSectionStartLineFromOffset(document, offset);

			for (int i = offsetLine.LineNumber; i <= document.LineCount; i++)
			{
				DocumentLine iline = document.GetLineByNumber(i);
				string ilineText = document.GetText(iline);

				if (iline != sectionStartLine && (ilineText.StartsWith("[") || i == document.LineCount))
				{
					for (int j = i == document.LineCount ? i : i - 1; j >= 1; j--)
					{
						DocumentLine jline = document.GetLineByNumber(j);
						string jlineText = document.GetText(jline);

						if (!string.IsNullOrWhiteSpace(LineParser.RemoveComments(jlineText)))
							return jline;
					}

					break;
				}
			}

			return null;
		}

		public static DocumentLine? FindDocumentLineOfSection(TextDocument document, string sectionName)
		{
			sectionName = sectionName.Trim('[').Trim(']').Trim();

			foreach (DocumentLine line in document.Lines)
			{
				string lineText = document.GetText(line.Offset, line.Length);

				if (lineText.StartsWith("["))
				{
					string headerText = LineParser.GetSectionHeaderText(lineText);

					if (headerText.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
						return line;
				}
			}

			return null;
		}

		public static DocumentLine? FindDocumentLineOfObject(TextDocument document, string objectName, ObjectType type)
		{
			foreach (DocumentLine line in document.Lines)
			{
				string lineText = document.GetText(line.Offset, line.Length);

				switch (type)
				{
					case ObjectType.Section:
						if (lineText.StartsWith(objectName))
							return line;
						break;

					case ObjectType.Level:
						if (Regex.Replace(lineText, Patterns.SpecificCommandStart("Name"), string.Empty, RegexOptions.IgnoreCase).StartsWith(objectName))
							return line;
						break;

					case ObjectType.Include:
						if (Regex.Replace(lineText, Patterns.IncludeWithValue, string.Empty, RegexOptions.IgnoreCase).TrimStart('"').StartsWith(objectName))
							return line;
						break;

					case ObjectType.Define:
						if (Regex.Replace(lineText, Patterns.DefineWithValue, string.Empty, RegexOptions.IgnoreCase).StartsWith(objectName))
							return line;
						break;
				}
			}

			return null;
		}

		public static bool IsLevelScriptDefined(TextDocument document, string levelName)
		{
			foreach (DocumentLine line in document.Lines)
			{
				string lineText = document.GetText(line.Offset, line.Length);
				var regex = new Regex(Patterns.SpecificCommandStart("Name"), RegexOptions.IgnoreCase);

				if (regex.IsMatch(lineText))
				{
					string scriptLevelName = regex.Replace(LineParser.RemoveComments(lineText), string.Empty).Trim();

					if (scriptLevelName == levelName)
						return true;
				}
			}

			return false;
		}

		public static bool IsPluginDefined(TextDocument document, string pluginName)
		{
			DocumentLine? optionsSectionLine = FindDocumentLineOfSection(document, "Options");

			if (optionsSectionLine == null)
				return false;

			for (int i = optionsSectionLine.LineNumber; i <= document.LineCount; i++)
			{
				DocumentLine line = document.GetLineByNumber(i);
				string? commandKey = CommandParser.GetCommandKeyFromOffset(document, line.Offset);

				if (commandKey != null && commandKey.Equals("Plugin", StringComparison.OrdinalIgnoreCase))
				{
					string? wholeCommandLineText = CommandParser.GetWholeCommandLineTextFromOffset(document, line.Offset, CommentHandling.Escape);

					if (wholeCommandLineText == null)
						continue;

					wholeCommandLineText = LineParser.RemoveComments(wholeCommandLineText);

					if (wholeCommandLineText.Contains(","))
					{
						string definedName = wholeCommandLineText.Split(',')[1].Replace("\n", "").Replace("\r", "").Replace(">", "").Trim();

						if (definedName.Equals(pluginName, StringComparison.OrdinalIgnoreCase))
							return true;
					}
				}
			}

			return false;
		}

		public static bool IsLevelLanguageStringDefined(TextDocument document, string levelName)
		{
			foreach (DocumentLine line in document.Lines)
			{
				string lineText = document.GetText(line.Offset, line.Length);
				string cleanString = LineParser.RemoveComments(LineParser.RemoveNGStringIndex(lineText)).Trim();

				if (cleanString == levelName)
					return true;
			}

			return false;
		}
	}
}
