namespace TombIDE.Core.Formats.Trproj.V1;

[XmlRoot("Project")]
public sealed record Trproj : ITrproj
{
	public const string ProjectDirectoryKey = "$(ProjectDirectory)";
	public const string LastModifiedFileKey = "$(LatestFile)";

	[XmlIgnore] public int Version => 1;

	[XmlElement] public string Name { get; init; } = string.Empty;
	[XmlElement] public string ScriptPath { get; init; } = string.Empty;
	[XmlElement] public string LevelsPath { get; init; } = string.Empty;
	[XmlArray] public List<ProjectLevel> Levels { get; init; } = new();

	public sealed record ProjectLevel
	{
		[XmlElement] public string Name { get; init; } = string.Empty;
		[XmlElement] public string FolderPath { get; init; } = string.Empty;
		[XmlElement] public string SpecificFile { get; init; } = LastModifiedFileKey;
	}
}
