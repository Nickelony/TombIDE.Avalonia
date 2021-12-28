using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Utils.Trproj;

public static class TrprojReader
{
	public static ITrprojFile? FromFile(string filePath)
	{
		if (!XmlUtils.IsXmlDocument(filePath, out XmlDocument document))
			return null;

		int fileVersion = ReadProjectFileVersion(document);

		return fileVersion switch
		{
			1 => FromFileExact<Models.Legacy.Trproj.V1>(filePath),
			2 => FromFileExact<TrprojFile>(filePath),
			_ => null
		};
	}

	public static T? FromFileExact<T>(string filePath) where T : class, ITrprojFile
	{
		using var reader = new StreamReader(filePath);
		var project = new XmlSerializer(typeof(T)).Deserialize(reader) as T;

		if (project != null)
		{
			string projectDirectory = Path.GetDirectoryName(filePath)!;
			project.MakePathsAbsolute(projectDirectory);
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
