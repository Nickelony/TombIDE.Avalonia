using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using ScriptLib.ClassicScript.Parsers;
using ScriptLib.Core.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Data
{
	public static class Autocomplete
	{
		public static IEnumerable<ICompletionData> GetNewLineAutocompleteList()
		{
			foreach (string keyword in Keywords.Directives)
				yield return new CompletionData("#" + keyword + " ");

			foreach (string keyword in Keywords.Sections)
				yield return new CompletionData("[" + keyword + "]");

			foreach (string keyword in Keywords.OldCommands)
				yield return new CompletionData(keyword + "=");

			foreach (string keyword in Keywords.NewCommands)
				yield return new CompletionData(keyword + "=");
		}

		public static List<ICompletionData>? GetCompletionData(TextDocument document, int caretOffset, int argumentIndex = -1)
		{
			var completionData = new List<ICompletionData>();

			string? syntax = CommandParser.GetCommandSyntaxFromOffset(document, caretOffset);

			if (syntax == null)
				return null;

			var regex = new Regex(Patterns.CommandPrefixInParenthesis);

			bool hasAnyValidDataToAutocomplete = regex.IsMatch(syntax)
				|| syntax.Contains("ENABLED", StringComparison.OrdinalIgnoreCase)
				|| syntax.Contains("DISABLED", StringComparison.OrdinalIgnoreCase);

			if (!hasAnyValidDataToAutocomplete)
				return null;

			string[] arguments = syntax.Split(',');

			if (argumentIndex == -1) // Auto-detect argument index from caret position
			{
				int? caretArgumentIndex = ArgumentParser.GetArgumentIndexAtOffset(document, caretOffset);

				if (caretArgumentIndex == null)
					return null;

				argumentIndex = caretArgumentIndex.Value;
			}

			if (argumentIndex >= arguments.Length || argumentIndex == -1)
				return null;

			string currentArgument = arguments[argumentIndex];

			var match = regex.Match(currentArgument);

			if (match.Success)
			{
				string constantPrefix = match.Groups[1].Value.Trim();

				foreach (string constant in MnemonicData.AllConstantFlags)
					if (constant.StartsWith(constantPrefix, StringComparison.OrdinalIgnoreCase))
						completionData.Add(new CompletionData(constant));
			}
			else if (currentArgument.Contains("ENABLED", StringComparison.OrdinalIgnoreCase)
				|| currentArgument.Contains("DISABLED", StringComparison.OrdinalIgnoreCase))
			{
				completionData.Add(new CompletionData("ENABLED"));
				completionData.Add(new CompletionData("DISABLED"));
			}

			return completionData;
		}
	}
}
