using ScriptLib.ClassicScript.Data.Enums;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Utils
{
	public class RddaReader
	{
		#region Properties

		public string SourceDirectory { get; }

		#endregion Properties

		#region Construction

		public RddaReader(string sourceDirectory)
		{
			if (!Directory.Exists(sourceDirectory))
				throw new DirectoryNotFoundException("Rdda source directory doesn't exist.");

			string[] sourceDirFiles = Directory.GetFiles(sourceDirectory);

			if (!Array.Exists(sourceDirFiles, x => x.EndsWith(".rdda", StringComparison.OrdinalIgnoreCase)))
				throw new ArgumentException("Source directory doesn't contain any .rdda files.");

			SourceDirectory = sourceDirectory;
		}

		#endregion Construction

		#region Public methods

		public string? GetKeywordDescription(string keyword, ReferenceType type)
		{
			string? result = GetDescriptionFromRdda(keyword, type);

			if (result != null)
				result = Regex.Replace(result, @"\r\n?|\n", "\n"); // Fixes broken newlines

			return result;
		}

		public ReferenceType GetCommandType(string command)
		{
			string? oldCommand = GetDescriptionFromRdda(command, ReferenceType.OldCommand);
			bool validOldCommandFound = oldCommand != null;

			if (validOldCommandFound)
				return ReferenceType.OldCommand;

			string? newCommand = GetDescriptionFromRdda(command, ReferenceType.NewCommand);
			bool validNewCommandFound = newCommand != null;

			if (validNewCommandFound)
				return ReferenceType.NewCommand;

			return ReferenceType.Unknown;
		}

		public string? GetRddaFileName(ReferenceType type)
		{
			if (type == ReferenceType.Unknown)
				return null;

			string? typeName = Enum.GetName(typeof(ReferenceType), type);

			if (typeName != null)
			{
				string rddaFileName = typeName + "s" + ".rdda"; // Example: "NewCommand" + "s" + ".rdda" = "NewCommands.rdda"
				return rddaFileName;
			}

			return null;
		}

		#endregion Public methods

		#region Private methods

		private string? GetDescriptionFromRdda(string keyword, ReferenceType type)
		{
			string? rddaFileName = GetRddaFileName(type);

			if (rddaFileName == null)
				return null;

			string archivePath = Path.Combine(SourceDirectory, rddaFileName);
			string? keywordDescriptionFileName = GetKeywordDescriptionFileName(keyword);

			if (!File.Exists(archivePath) || keywordDescriptionFileName == null)
				return null;

			try
			{
				using FileStream file = File.OpenRead(archivePath);
				using var archive = new ZipArchive(file);

				foreach (ZipArchiveEntry entry in archive.Entries)
					if (entry.Name.Equals(keywordDescriptionFileName, StringComparison.OrdinalIgnoreCase))
					{
						using Stream stream = entry.Open();
						using var reader = new StreamReader(stream);

						return reader.ReadToEnd();
					}
			}
			catch (Exception) { }

			return null;
		}

		private string? GetKeywordDescriptionFileName(string keyword)
		{
			if (string.IsNullOrWhiteSpace(keyword))
				return null;

			keyword = keyword.TrimStart('_'); // "_NEW Switch 1/2/3" >> "NEW Switch 1/2/3"
			keyword = keyword.Replace(" ", "_"); // "NEW Switch 1/2/3" >> "NEW_Switch_1/2/3"
			keyword = keyword.Replace("/", string.Empty); // "NEW_Switch_1/2/3" >> "NEW_Switch_123"

			return "info_" + keyword + ".txt";
		}

		#endregion Private methods
	}
}
