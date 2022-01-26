namespace TombIDE.Formats.Trproj.V1;

[XmlRoot("Project")]
public sealed record Trproj : ITrproj
{
	[XmlIgnore] public FileInfo ProjectFile { get; init; } = new(string.Empty);
	[XmlIgnore] public int Version => 1;

	[XmlElement] public string Name { get; init; } = string.Empty;
	[XmlElement] public string ScriptPath { get; init; } = string.Empty;
	[XmlElement] public string LevelsPath { get; init; } = string.Empty;
	[XmlArray] public List<MapRecord> Levels { get; init; } = new();
}
