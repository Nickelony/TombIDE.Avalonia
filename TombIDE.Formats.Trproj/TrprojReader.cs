using TombIDE.Core.Models;
using TombIDE.Core.Utils;

namespace TombIDE.Formats.Trproj;

public static class TrprojReader
{
	public static IGameProject? FromFile(string filePath)
	{
		if (!XmlUtils.IsXmlDocument(filePath, out XmlDocument document))
			return null;

		int fileVersion = ReadProjectFileVersion(document);

		return fileVersion switch
		{
			1 => FromFileExact<V1.GameProjectV1>(filePath),
			2 => FromFileExact<V2.GameProjectV2>(filePath),
			_ => null
		};
	}

	public static T? FromFileExact<T>(string filePath) where T : class, IGameProject
	{
		using var reader = new StreamReader(filePath);
		var project = new XmlSerializer(typeof(T)).Deserialize(reader) as T;

		if (project != null)
		{
			project.ProjectFilePath = filePath;
			project.MakePathsAbsolute(project.RootDirectoryPath);
		}

		return project;
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
			return projectNode?.Name == "Project" ? 1 : -1;

		if (int.TryParse(fileVersionString, out int fileVersion))
			return fileVersion;

		return -1;
	}
}
