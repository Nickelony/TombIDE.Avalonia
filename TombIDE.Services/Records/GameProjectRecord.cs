using System.Xml.Serialization;

namespace TombIDE.Services.Records;

public sealed record GameProjectRecord(string ProjectFilePath, DateTime LastOpened)
{
	[XmlAttribute] public string ProjectFilePath { get; } = ProjectFilePath;
	[XmlAttribute] public DateTime LastOpened { get; } = LastOpened;
}
