using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TombIDE.Services;

internal class TrprojFileService
{
	public static ITrprojFile? ReadFile(string filePath)
	{
		if (!XmlUtils.IsXmlDocument(filePath, out XmlDocument document))
			return null;

		int fileVersion = ReadProjectFileVersion(document);

		return fileVersion switch
		{
			1 => ReadFileExact<V1.TrprojFile>(filePath),
			2 => ReadFileExact<V2.TrprojFile>(filePath),
			_ => null
		};
	}

	public static T? ReadFileExact<T>(string filePath) where T : class, ITrprojFile
	{
		using var reader = new StreamReader(filePath);
		var project = new XmlSerializer(typeof(T)).Deserialize(reader) as T;

		if (project != null)
		{
			string projectDirectory = Path.GetDirectoryName(filePath)!;
			project.MakePathsAbsolute(projectDirectory);

			project.FilePath = filePath;
		}

		return project;
	}

	public static void WriteToFile(string filePath, V1.TrprojFile trproj, bool makePathsRelative = true)
	{
		if (makePathsRelative)
			trproj.MakePathsRelative(Path.GetDirectoryName(filePath)!);

		XmlUtils.SaveXmlFile(filePath, trproj);
	}

	public static void WriteToFile(string filePath, V2.TrprojFile trproj, bool makePathsRelative = true)
	{
		if (makePathsRelative)
			trproj.MakePathsRelative(Path.GetDirectoryName(filePath)!);

		var settings = new XmlWriterSettings
		{
			Indent = true,
			IndentChars = "\t",
			NewLineOnAttributes = true
		};

		using var writer = XmlWriter.Create(filePath, settings);
		var serializer = new XmlSerializer(typeof(V2.TrprojFile));
		serializer.Serialize(writer, trproj);
	}

	public static V2.TrprojFile ConvertV1ToLatest(V1.TrprojFile trprojV1)
	{
		string trprojDirectory = Path.GetDirectoryName(trprojV1.FilePath)!;
		string defaultTRNGPluginsDirectoryPath = Path.Combine(trprojDirectory, "Plugins");

		var result = new V2.TrprojFile
		{
			FilePath = trprojV1.FilePath,
			Name = trprojV1.Name,
			ScriptDirectoryPath = trprojV1.ScriptPath,
			MapsDirectoryPath = trprojV1.LevelsPath,
			TRNGPluginsDirectoryPath = defaultTRNGPluginsDirectoryPath
		};

		foreach (V1.MapRecord level in trprojV1.Levels)
		{
			result.MapRecords.Add(new V2.MapRecord
			{
				Name = level.Name,
				RootDirectoryPath = level.FolderPath,

				StartupFileName = level.SpecificFile == V1.Constants.LastModifiedFileKey ?
					string.Empty : level.SpecificFile
			});
		}

		return result;
	}

	private static int ReadProjectFileVersion(XmlDocument document)
	{
		bool isValidDocument = document.ChildNodes.Count > 1;

		if (!isValidDocument)
			return -1;

		XmlNode? projectNode = document.ChildNodes[1];
		XmlAttribute? fileVersionAttribute = projectNode?.Attributes?["ProjectFileVersion"];
		string? fileVersionString = fileVersionAttribute?.Value;

		if (string.IsNullOrEmpty(fileVersionString))
		{
			bool isV1RootName = projectNode?.Name == "Project";
			return isV1RootName ? 1 : -1;
		}

		if (int.TryParse(fileVersionString, out int fileVersion))
			return fileVersion;

		return -1;
	}
}
