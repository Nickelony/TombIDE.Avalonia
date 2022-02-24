﻿namespace TombIDE.Core.Utils;

public static class XmlFileUtils
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

	public static T ReadXmlFile<T>(string filePath, XmlReaderSettings? settings = null)
	{
		using var reader = XmlReader.Create(filePath, settings);
		var serializer = new XmlSerializer(typeof(T));
		return (T)serializer.Deserialize(reader)!;
	}

	public static void SaveXmlFile<T>(string filePath, T content, XmlWriterSettings? settings = null)
	{
		using var writer = XmlWriter.Create(filePath, settings);
		var serializer = new XmlSerializer(typeof(T));
		serializer.Serialize(writer, content);
	}
}