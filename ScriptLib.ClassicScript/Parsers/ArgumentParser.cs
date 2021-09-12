using AvaloniaEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Parsers
{
	public static class ArgumentParser
	{
		public static int? GetArgumentIndexAtOffset(TextDocument document, int offset)
		{
			string? wholeLineText = CommandParser.GetWholeCommandLineTextFromOffset(document, offset, CommentHandling.Escape);

			if (wholeLineText == null)
				return null;

			wholeLineText = MergeMultipleFlags(wholeLineText);

			int totalArgumentCount = wholeLineText.Split(',').Length;

			DocumentLine? commandStartLine = CommandParser.GetCommandStartLineFromOffset(document, offset);

			if (commandStartLine == null)
				return null;

			int wholeLineSubstringOffset = offset - commandStartLine.Offset;

			if (wholeLineSubstringOffset > wholeLineText.Length) // Useless?
				return totalArgumentCount - 1;

			string textAfterOffset = wholeLineText.Remove(0, wholeLineSubstringOffset);
			int argumentCountAfterOffset = textAfterOffset.Split(',').Length;

			return totalArgumentCount - argumentCountAfterOffset;
		}

		// TODO: Refactor !!!

		public static string? GetArgumentFromIndex(TextDocument document, int offset, int index)
		{
			string? wholeLineText = CommandParser.GetWholeCommandLineTextFromOffset(document, offset, CommentHandling.Escape);

			if (wholeLineText == null)
				return null;

			wholeLineText = MergeMultipleFlags(wholeLineText);

			return wholeLineText.Split(',')[index];
		}

		private static string MergeMultipleFlags(string wholeLineText)
		{
			string cachedArgument = string.Empty;

			string command = wholeLineText.Split('=')[0].Trim();
			string? commandSyntax = CommandParser.GetCommandSyntax(command);

			string[] arguments = LineParser.EscapeComments(wholeLineText).Split('=')[1]
				.Replace('>', ' ').Replace('\t', ' ').Replace('\n', ' ').Replace('\r', ' ').Split(',');

			var newArgumentList = new List<string>();

			for (int i = 0; i < arguments.Length; i++)
			{
				string argument = arguments[i];
				string argumentSyntax = string.Empty;

				if (!string.IsNullOrEmpty(commandSyntax) && i < commandSyntax.Split(',').Length)
					argumentSyntax = commandSyntax.Split(',')[i];

				if (!argument.Contains("_"))
				{
					newArgumentList.Add(argument);
					cachedArgument = argument;
					continue;
				}

				string flagPrefix = argument.Split('_')[0].Trim();

				if (flagPrefix.Equals(cachedArgument.Split('_')[0].Trim(), StringComparison.OrdinalIgnoreCase))
				{
					if (argumentSyntax.Contains(flagPrefix + "_"))
					{
						newArgumentList.Add(argument);
						cachedArgument = argument;
						continue;
					}

					if (newArgumentList.Count > 0)
						newArgumentList.RemoveAt(newArgumentList.Count - 1);

					cachedArgument = cachedArgument + "." + argument;

					newArgumentList.Add(cachedArgument);
				}
				else
				{
					newArgumentList.Add(argument);
					cachedArgument = argument;
				}
			}

			return command + "=" + string.Join(",", newArgumentList.ToArray());
		}

		public static char? GetFirstLetterOfArgumentFromOffset(TextDocument document, int offset)
		{
			int? currentArgumentIndex = GetArgumentIndexAtOffset(document, offset);

			if (currentArgumentIndex == null)
				return null;

			string? syntax = CommandParser.GetCommandSyntaxFromOffset(document, offset);

			if (syntax == null)
				return null;

			string[] syntaxArguments = syntax.Split(',');

			if (currentArgumentIndex > syntaxArguments.Length)
				return null;

			string currentSyntaxArgument = syntaxArguments[currentArgumentIndex.Value];

			var regex = new Regex(@"\(([^_]*).*\)");

			if (!regex.IsMatch(currentSyntaxArgument))
				return null;

			string flagPrefix = regex.Match(currentSyntaxArgument).Groups[1].Value;

			return flagPrefix[0];
		}

		public static char? GetFirstLetterOfPreviousFlag(TextDocument document, int offset)
		{
			int? currentArgumentIndex = GetArgumentIndexAtOffset(document, offset);

			if (currentArgumentIndex == null || currentArgumentIndex == 0)
				return null;

			string? prevArgument = GetArgumentFromIndex(document, offset, currentArgumentIndex.Value - 1)?.Trim();

			if (prevArgument == null || !prevArgument.Contains("_"))
				return null;

			if (prevArgument.Contains("="))
				prevArgument.Split('=').Last().Trim();

			return prevArgument[0];
		}
	}
}
