using ScriptLib.ClassicScript.Parsers;
using System;
using System.Collections.Generic;

namespace ScriptLib.ClassicScript.Utils
{
	public class ContentReader
	{
		public static bool NextSectionExists(string[] lines, int lineNumber, out int nextSectionLineNumber)
		{
			for (int i = lineNumber; i < lines.Length; i++)
				if (LineParser.IsSectionHeaderLine(lines[i]))
				{
					nextSectionLineNumber = i;
					return true;
				}

			nextSectionLineNumber = -1;
			return false;
		}

		public static IEnumerable<string> GetStrings(string[] lines, int sectionStartLineNumber)
		{
			for (int i = sectionStartLineNumber + 1; i < lines.Length; i++)
			{
				if (LineParser.IsSectionHeaderLine(lines[i]))
					break;

				string line = GetParsedLine(lines[i]);

				if (!LineParser.IsEmptyOrComments(line))
					yield return line;
			}
		}

		public static string GetParsedLine(string line)
		{
			line = LineParser.RemoveComments(line);
			line = line.Replace("\\x3B", ";");
			line = line.Replace("\\n", Environment.NewLine);
			line = line.Trim();

			return line;
		}
	}
}
