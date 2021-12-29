using TombIDE.Core.Models;
using TrprojV1 = TombIDE.Core.Models.Legacy.Trproj.V1;

namespace TombIDE.Core.Utils.Trproj;

public static class TrprojWriter
{
	public static void WriteToFile(string filePath, TrprojV1 trproj, bool makePathsRelative = true)
	{
		if (makePathsRelative)
			trproj.MakePathsRelative(Path.GetDirectoryName(filePath)!);

		XmlUtils.SaveXmlFile(filePath, trproj);
	}

	public static void WriteToFile(string filePath, TrprojFile trproj, bool makePathsRelative = true)
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
		var serializer = new XmlSerializer(typeof(TrprojFile));
		serializer.Serialize(writer, trproj);
	}
}
