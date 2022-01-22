using System.Xml;
using System.Xml.Serialization;

namespace TombIDE.Core.Utils;

public static class XmlUtils
{
	public static bool IsXmlDocument(string filePath, out XmlDocument document)
	{
		document = new XmlDocument();

		try
		{
			document.Load(filePath);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static T ReadXmlFile<T>(string filePath)
	{
		using var reader = new StreamReader(filePath);
		var serializer = new XmlSerializer(typeof(T));
		return (T)serializer.Deserialize(reader)!;
	}

	public static void SaveXmlFile<T>(string filePath, T content)
	{
		using var writer = new StreamWriter(filePath);
		var serializer = new XmlSerializer(typeof(T));
		serializer.Serialize(writer, content);
	}
}
