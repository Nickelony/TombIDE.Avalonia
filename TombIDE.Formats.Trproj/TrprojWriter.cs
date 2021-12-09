using TombIDE.Core.Models;

namespace TombIDE.Formats.Trproj;

public static class TrprojWriter
{
	public static void WriteToFile(string filePath, IGameProject project)
	{
		project.MakePathsRelative();

		try
		{
			var settings = new XmlWriterSettings
			{
				Indent = true,
				IndentChars = "\t",
				NewLineOnAttributes = true
			};

			using var writer = XmlWriter.Create(filePath, settings);
			var serializer = new XmlSerializer(typeof(IGameProject));
			serializer.Serialize(writer, project);
		}
		finally
		{
			project.MakePathsAbsolute();
		}
	}
}
