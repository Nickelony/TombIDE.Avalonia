namespace TombIDE.Formats.Trproj.V1;

[XmlRoot("ProjectLevel")]
public sealed record MapRecord
{
	[XmlElement] public string Name { get; set; } = string.Empty;
	[XmlElement] public string FolderPath { get; set; } = string.Empty;
	[XmlElement] public string SpecificFile { get; set; } = Constants.LastModifiedFileKey;
}
