using AvaloniaEdit.Document;
using ScriptLib.ClassicScript.Data;
using ScriptLib.ClassicScript.Parsers;
using ScriptLib.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Utils
{
	public class ErrorDetector : IErrorDetector
	{
		#region Public methods

		/// <param name="content">TextDocument</param>
		/// <returns>IEnumerable&lt;ErrorLine&gt;</returns>
		public object? FindErrors(object content)
		{
			if (content is TextDocument document)
				return DetectErrorLines(document);

			return null;
		}

		#endregion Public methods

		#region Error line finding

		private IEnumerable<ErrorLine> DetectErrorLines(TextDocument document)
		{
			bool commandSectionCheckRequired = DocumentParser.DocumentContainsSections(document);

			foreach (DocumentLine line in document.Lines)
			{
				string lineText = document.GetText(line);

				if (LineParser.IsEmptyOrComments(lineText))
					continue;

				ErrorLine? error = FindErrorsInLine(document, line, commandSectionCheckRequired);

				if (error != null)
					yield return error;
			}
		}

		private ErrorLine? FindErrorsInLine(TextDocument document, DocumentLine line, bool commandSectionCheckRequired)
		{
			string lineText = document.GetText(line);

			if (LineParser.IsSectionHeaderLine(lineText))
				return FindErrorsInSectionHeaderLine(document, line);
			else
			{
				if (commandSectionCheckRequired && LineParser.IsLineInStandardStringSection(document, line))
					return null;
				else if (commandSectionCheckRequired && LineParser.IsLineInExtraNGSection(document, line))
					return FindErrorsInNGStringLine(document, line);
				else
					return FindErrorsInCommandLine(document, line, commandSectionCheckRequired);
			}
		}

		private ErrorLine? FindErrorsInSectionHeaderLine(TextDocument document, DocumentLine line)
		{
			string lineText = document.GetText(line);

			if (!IsValidSectionName(lineText))
				return new(line.LineNumber, LineParser.RemoveComments(lineText), 0,
					"Invalid section name. Please check its spelling.");

			return null;
		}

		private ErrorLine? FindErrorsInNGStringLine(TextDocument document, DocumentLine line)
		{
			string lineText = document.GetText(line);

			if (!IsNGStringLineWellFormatted(lineText))
				return new(line.LineNumber, LineParser.RemoveComments(lineText), 0,
					"NG string must start with an index.\n\nExample:\n0: First String\n1: Second String");

			return null;
		}

		private ErrorLine? FindErrorsInCommandLine(TextDocument document, DocumentLine line, bool commandSectionCheckRequired)
		{
			string lineText = document.GetText(line);
			string? commandKey = CommandParser.GetCommandKeyFromOffset(document, line.Offset);

			if (!IsValidCommandKey(commandKey))
			{
				string errorSegmentText = Regex.Match(LineParser.RemoveComments(lineText), "^.*=").Value.TrimEnd();

				if (errorSegmentText.Length == 0 && commandKey != null)
					return null;

				if (commandKey == null)
					errorSegmentText = lineText.TrimEnd();

				return new(line.LineNumber, errorSegmentText, 0,
					"Invalid command. Please check its spelling.");
			}

			if (commandSectionCheckRequired && !IsCommandLineInCorrectSection(document, line.LineNumber, commandKey!))
				return new(line.LineNumber, LineParser.RemoveComments(lineText), 0,
					"Command is placed in the wrong section. Please check the command syntax.");

			if (!IsArgumentCountValid(document, line.Offset))
			{
				string errorSegmentText = Regex.Match(LineParser.RemoveComments(lineText), @"=\s*(\b.*)").Groups[1].Value;

				if (errorSegmentText.Length == 0)
					errorSegmentText = LineParser.RemoveComments(lineText);

				return new(line.LineNumber, errorSegmentText, 0,
					"Invalid argument count. Please check the command syntax.");
			}

			if (ContainsEmptyArguments(document, line.Offset))
			{
				string errorSegmentText = Regex.Match(LineParser.RemoveComments(lineText), @"=\s*(\b.*)").Groups[1].Value;

				if (errorSegmentText.Length == 0)
					errorSegmentText = LineParser.RemoveComments(lineText);

				return new(line.LineNumber, errorSegmentText, 0,
					"Empty arguments were found.");
			}

			return null;
		}

		#endregion Error line finding

		#region Error detection methods

		private static bool IsValidSectionName(string sectionHeaderLineText)
		{
			string section = sectionHeaderLineText.Split('[')[1].Split(']')[0];

			foreach (string entry in Keywords.Sections)
				if (section.Equals(entry, StringComparison.OrdinalIgnoreCase))
					return true;

			return false;
		}

		private static bool IsNGStringLineWellFormatted(string lineText)
			=> Regex.IsMatch(lineText, @"^\d*:.*");

		private static bool IsValidCommandKey(string? commandKey)
		{
			if (commandKey == null)
				return false;

			foreach (DictionaryEntry entry in CommandParser.GetCommandSyntaxResources())
				if (commandKey.Equals(entry.Key.ToString(), StringComparison.OrdinalIgnoreCase))
					return true;

			return false;
		}

		private static bool IsCommandLineInCorrectSection(TextDocument document, int lineNumber, string command)
		{
			string correctSection = string.Empty;

			foreach (DictionaryEntry entry in CommandParser.GetCommandSyntaxResources())
				if (command.Equals(entry.Key.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					string? value = entry.Value?.ToString();

					if (value != null)
					{
						correctSection = value.Split('[')[1].Split(']')[0].Trim();
						break;
					}
				}

			if (correctSection.Equals("any", StringComparison.OrdinalIgnoreCase))
				return true;

			for (int i = lineNumber - 1; i > 0; i--)
			{
				DocumentLine currentLine = document.GetLineByNumber(i);
				string currentLineText = document.GetText(currentLine.Offset, currentLine.Length);

				if (currentLineText.StartsWith("["))
				{
					if (correctSection.Equals("level", StringComparison.OrdinalIgnoreCase))
					{
						if (Regex.IsMatch(currentLineText, @"\[(level|title)\]", RegexOptions.IgnoreCase))
							return true;
						else
							return false;
					}
					else if (Regex.IsMatch(currentLineText, @"\[" + correctSection + @"\]", RegexOptions.IgnoreCase))
						return true;
					else
						return false;
				}
			}

			return false;
		}

		private static bool IsArgumentCountValid(TextDocument document, int lineOffset)
		{
			string? wholeLineText = CommandParser.GetWholeCommandLineTextFromOffset(document, lineOffset, CommentHandling.Escape);

			if (wholeLineText == null)
				return false;

			if (wholeLineText.StartsWith("#"))
				return true;

			wholeLineText = LineParser.EscapeComments(wholeLineText);

			if (!wholeLineText.Contains("="))
				return false;

			string? command = CommandParser.GetCommandKeyFromOffset(document, lineOffset);

			if (command == null)
				return false;

			int argumentCount = LineParser.EscapeComments(wholeLineText).Split('=')[1].Split(',').Length;

			if (argumentCount == 1 && string.IsNullOrWhiteSpace(wholeLineText.Split('=')[1]))
				argumentCount = 0;

			foreach (DictionaryEntry entry in CommandParser.GetCommandSyntaxResources())
				if (command.Equals(entry.Key.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					string? value = entry.Value?.ToString();

					if (value != null)
					{
						int correctArgumentCount = value.Split(']')[1].Split(',').Length;

						if (value.ToUpper().Contains("ARRAY"))
							return true; // Whatever.
						else
							return argumentCount == correctArgumentCount;
					}
				}

			return false;
		}

		private static bool ContainsEmptyArguments(TextDocument document, int lineOffset)
		{
			string? wholeLineText = CommandParser.GetWholeCommandLineTextFromOffset(document, lineOffset, CommentHandling.Escape);

			if (wholeLineText == null)
				return true;

			string[] arguments = LineParser.EscapeComments(wholeLineText).Split(',');

			foreach (string argument in arguments)
				if (string.IsNullOrWhiteSpace(argument.Replace('>', ' ')))
					return true;

			return false;
		}

		#endregion Error detection methods
	}
}
