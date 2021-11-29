using AvaloniaEdit.Document;
using ScriptLib.ClassicScript.Data;
using ScriptLib.ClassicScript.Data.Enums;
using ScriptLib.ClassicScript.Data.Objects;
using ScriptLib.ClassicScript.Data.Syntaxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Parsers
{
	public static class CommandParser
	{
		#region Extension methods

		public static string? GetCommandSyntaxFromOffset(this TextDocument document, int offset)
		{
			string? wholeCommandLineText = GetWholeCommandLineTextFromOffset(document, offset, CommentHandling.Escape);

			if (wholeCommandLineText == null)
				return null;

			if (IsValidCustomizeCommand(wholeCommandLineText, out string firstCustParam))
			{
				return GetSubcommandSyntax(SubcommandType.Cust, firstCustParam);
			}
			else if (IsValidParametersCommand(wholeCommandLineText, out string firstParamParam))
			{
				return GetSubcommandSyntax(SubcommandType.Param, firstParamParam);
			}
			else
			{
				string? commandKey = GetCommandKeyFromOffset(document, offset);

				if (commandKey == null)
					return null;

				return GetCommandSyntax(commandKey);
			}
		}

		/// <summary>
		/// Merges all lines of a single command into one string (Handles multi-line character ">").
		/// </summary>
		public static string? GetWholeCommandLineTextFromOffset(this TextDocument document, int offset, CommentHandling ch)
		{
			DocumentLine? commandStartLine = GetCommandStartLineFromOffset(document, offset);

			if (commandStartLine == null)
				return null;

			string commandStartLineText = document.GetText(commandStartLine);
			string wholeCommandLine;

			if (Regex.IsMatch(commandStartLineText, Patterns.NextLineKey)) // If commandStartLineText is multi-line
			{
				IEnumerable<DocumentLine> linesToMerge = GetLinesToMerge(document, commandStartLine);
				wholeCommandLine = MergeLines(document, linesToMerge);
			}
			else
				wholeCommandLine = commandStartLineText;

			return ch switch
			{
				CommentHandling.Escape => LineParser.EscapeComments(wholeCommandLine),
				CommentHandling.Remove => LineParser.RemoveComments(wholeCommandLine),
				_ => wholeCommandLine
			};
		}

		/// <summary>
		/// Gets the starting line of a command. Mainly used to find the starting point of a multi-line command (Handled via ">").
		/// </summary>
		public static DocumentLine? GetCommandStartLineFromOffset(this TextDocument document, int offset)
		{
			DocumentLine offsetLine = document.GetLineByOffset(offset);
			string offsetLineText = document.GetText(offsetLine);

			if (LineParser.IsSectionHeaderLine(offsetLineText))
				return null;

			bool isCommandStartLine = Regex.IsMatch(offsetLineText, Patterns.AnyCommandStart);

			if (isCommandStartLine)
				return offsetLine;

			bool isDirectiveStartLine = Regex.IsMatch(offsetLineText, Patterns.AnyDirectiveStart);

			if (isDirectiveStartLine)
				return offsetLine;

			return FindCommandStartLine(document, offsetLine);
		}

		public static string? GetCommandKeyFromOffset(this TextDocument document, int offset)
		{
			DocumentLine? commandStartLine = GetCommandStartLineFromOffset(document, offset);

			if (commandStartLine == null)
				return null;

			string commandStartLineText = document.GetText(commandStartLine);

			var commandMatch = Regex.Match(commandStartLineText, Patterns.AnyCommandStart);

			if (commandMatch.Success)
			{
				string commandName = commandMatch.Groups[1].Value;
				string? currentSection = DocumentParser.GetSectionNameFromOffset(document, offset);

				return GetCorrectCommandVariation(commandName, currentSection);
			}

			var directiveMatch = Regex.Match(commandStartLineText, Patterns.AnyDirectiveStart);

			if (directiveMatch.Success)
				return "#" + directiveMatch.Groups[1].Value;

			return null;
		}

		public static string? GetFullIncludePathFromOffset(this TextDocument document, int offset)
		{
			DocumentLine caretLine = document.GetLineByOffset(offset);
			string caretLineText = document.GetText(caretLine);

			var match = Regex.Match(caretLineText, Patterns.IncludeWithValue);

			if (match.Success)
			{
				string? rootPath = Path.GetDirectoryName(document.FileName);

				if (rootPath != null)
				{
					string secondPathPart = match.Groups[2].Value.Trim(); // {#INCLUDE " file.txt "} >> {file.txt}
					return Path.Combine(rootPath, secondPathPart);
				}
			}

			return null;
		}

		#endregion Extension methods

		#region Public methods

		public static string? GetCommandSyntax(string commandKey)
		{
			var resources = GetCommandSyntaxResources();

			int commandEntryIndex = resources.FindIndex(x =>
			{
				string? key = x.Key.ToString();
				return key != null && key.Equals(commandKey, StringComparison.OrdinalIgnoreCase);
			});

			if (commandEntryIndex == -1)
				return null;

			return resources[commandEntryIndex].Value?.ToString();
		}

		public static bool IsValidCustomizeCommand(string lineText, out string firstParam)
		{
			var customizeMatch = Regex.Match(lineText, Patterns.CustomizeCommandWithFirstArg, RegexOptions.IgnoreCase);

			firstParam = customizeMatch.Success ? customizeMatch.Groups[2].Value : string.Empty;
			return firstParam != string.Empty;
		}

		public static bool IsValidParametersCommand(string lineText, out string firstParam)
		{
			var parametersMatch = Regex.Match(lineText, Patterns.ParametersCommandWithFirstArg, RegexOptions.IgnoreCase);

			firstParam = parametersMatch.Success ? parametersMatch.Groups[2].Value : string.Empty;
			return firstParam != string.Empty;
		}

		public static List<DictionaryEntry> GetCommandSyntaxResources()
		{
			var entries = new List<DictionaryEntry>();

			// Get resources from OldCommandSyntaxes.resx
			var oldCommandSyntaxResource = new ResourceManager(typeof(OldCommandSyntaxes));
			ResourceSet? oldCommandResourceSet = oldCommandSyntaxResource.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

			if (oldCommandResourceSet != null)
				entries.AddRange(oldCommandResourceSet.Cast<DictionaryEntry>().ToList());

			// Get resources from NewCommandSyntaxes.resx
			var newCommandSyntaxResource = new ResourceManager(typeof(NewCommandSyntaxes));
			ResourceSet? newCommandResourceSet = newCommandSyntaxResource.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

			if (newCommandResourceSet != null)
				entries.AddRange(newCommandResourceSet.Cast<DictionaryEntry>().ToList());

			return entries;
		}

		#endregion Public methods

		#region Subcommands

		private static string? GetSubcommandSyntax(SubcommandType subcommandType, string subcommandKey)
		{
			var syntaxResource = subcommandType switch
			{
				SubcommandType.Cust => new ResourceManager(typeof(CustSyntaxes)),
				SubcommandType.Param => new ResourceManager(typeof(ParamSyntaxes)),
				_ => null
			};

			ResourceSet? resourceSet = syntaxResource?.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

			if (resourceSet == null)
				return null;

			string? custParamSyntax = FindCustParamSyntaxByKey(resourceSet, subcommandKey);

			if (custParamSyntax == null)
				return subcommandType switch
				{
					SubcommandType.Cust => GetCommandSyntax("Customize"),
					SubcommandType.Param => GetCommandSyntax("Parameters"),
					_ => null
				};

			return custParamSyntax;
		}

		private static string? FindCustParamSyntaxByKey(ResourceSet resourceSet, string key)
		{
			// Search in the given ResourceSet
			foreach (DictionaryEntry entry in resourceSet)
			{
				string? entryKey = entry.Key.ToString();

				if (entryKey != null && entryKey.Equals(key, StringComparison.OrdinalIgnoreCase))
					return entry.Value?.ToString();
			}

			// Search in PluginConstants
			foreach (PluginConstant constant in MnemonicData.PluginConstants)
				if (constant.FlagName.Equals(key, StringComparison.OrdinalIgnoreCase))
				{
					var regex = new Regex("syntax:", RegexOptions.IgnoreCase);

					if (regex.IsMatch(constant.Description))
					{
						string syntax = regex.Split(constant.Description)[1];
						syntax = syntax.Replace("\r", string.Empty).Split('\n')[0];

						return syntax.Trim();
					}
				}

			return null;
		}

		#endregion Subcommands

		#region Command variations

		private static string? GetCorrectCommandVariation(string command, string? sectionName)
		{
			return command.ToUpper() switch
			{
				"LEVEL" => GetCorrectLevelCommandForSection(sectionName),
				"CUT" => GetCorrectCutCommandForSection(sectionName),
				"FMV" => GetCorrectFMVCommandForSection(sectionName),
				_ => command
			};
		}

		private static string GetCorrectLevelCommandForSection(string? sectionName)
		{
			if (string.IsNullOrWhiteSpace(sectionName))
				return "LevelLevel";

			return sectionName.ToUpper() switch
			{
				"PCEXTENSIONS" => "LevelPC",
				"PSXEXTENSIONS" => "LevelPSX",
				_ => "LevelLevel",
			};
		}

		private static string? GetCorrectCutCommandForSection(string? sectionName)
		{
			if (string.IsNullOrWhiteSpace(sectionName))
				return null;

			return sectionName.ToUpper() switch
			{
				"PCEXTENSIONS" => "CutPC",
				"PSXEXTENSIONS" => "CutPSX",
				_ => null,
			};
		}

		private static string GetCorrectFMVCommandForSection(string? sectionName)
		{
			if (string.IsNullOrWhiteSpace(sectionName))
				return "FMVLevel";

			return sectionName.ToUpper() switch
			{
				"PCEXTENSIONS" => "FMVPC",
				"PSXEXTENSIONS" => "FMVPSX",
				_ => "FMVLevel",
			};
		}

		#endregion Command variations

		#region Other methods

		private static DocumentLine? FindCommandStartLine(TextDocument document, DocumentLine searchStartingLine)
		{
			DocumentLine previousLine;
			string previousLineText;

			int i = searchStartingLine.LineNumber - 1;

			do
			{
				if (i < 1)
					return null;

				previousLine = document.GetLineByNumber(i);

				previousLineText = document.GetText(previousLine);
				previousLineText = LineParser.EscapeComments(previousLineText);

				if (Regex.IsMatch(previousLineText, Patterns.PartialLineCommandStart))
					return previousLine;

				i--;
			}
			while (Regex.IsMatch(previousLineText, Patterns.PartialLine));

			return null;
		}

		private static IEnumerable<DocumentLine> GetLinesToMerge(TextDocument document, DocumentLine startingLine)
		{
			yield return startingLine;

			DocumentLine nextLine;
			string nextLineText;

			int i = startingLine.LineNumber + 1;

			do
			{
				if (i > document.LineCount)
					yield break;

				nextLine = document.GetLineByNumber(i);
				nextLineText = document.GetText(nextLine);

				yield return nextLine;

				i++;
			}
			while (Regex.IsMatch(nextLineText, Patterns.NextLineKey));
		}

		private static string MergeLines(TextDocument document, IEnumerable<DocumentLine> lines)
		{
			var builder = new StringBuilder();

			foreach (DocumentLine line in lines)
				builder.Append(document.GetText(line) + Environment.NewLine);

			return builder.ToString();
		}

		#endregion Other methods
	}
}
