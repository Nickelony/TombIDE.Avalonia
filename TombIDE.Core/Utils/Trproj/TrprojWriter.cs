using TombIDE.Core.Models;

namespace TombIDE.Core.Utils.Trproj;

public static class TrprojWriter
{
	public static void WriteToFile(string filePath, Models.Legacy.Trproj.V1 trproj)
		=> XmlUtils.SaveXmlFile(filePath, trproj);

	public static void WriteToFile(string filePath, TrprojFile trproj)
	{
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
