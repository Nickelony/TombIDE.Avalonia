namespace TombIDE.Core.Formats.Trproj.V2;

[XmlRoot("GameProject")]
public sealed record Trproj : ITrproj
{
	[XmlAttribute] public int Version => 2;

	[XmlAttribute] public string ProjectName { get; init; } = string.Empty;
	[XmlAttribute] public string ScriptDirectoryPath { get; init; } = string.Empty;
	[XmlAttribute] public string MapsDirectoryPath { get; init; } = string.Empty;
	[XmlAttribute] public string? TRNGPluginsDirectoryPath { get; init; }
	[XmlArray] public List<string> ExternalMapSubdirectoryPaths { get; init; } = new();
}
