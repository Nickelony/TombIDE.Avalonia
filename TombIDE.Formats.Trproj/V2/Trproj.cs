namespace TombIDE.Formats.Trproj.V2;

[XmlRoot("GameProject")]
public sealed record Trproj : ITrproj
{
	[XmlIgnore] public FileInfo ProjectFile { get; init; } = new(string.Empty);
	[XmlAttribute] public int Version => 2;

	[XmlAttribute] public string ProjectName { get; init; } = string.Empty;
	[XmlAttribute] public string ScriptDirectoryPath { get; init; } = string.Empty;
	[XmlAttribute] public string MapsDirectoryPath { get; init; } = string.Empty;
	[XmlAttribute] public string? TRNGPluginsDirectoryPath { get; init; }
	[XmlArray] public List<MapRecord> MapRecords { get; init; } = new();
}
