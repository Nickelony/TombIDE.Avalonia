using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Formats.Trproj.V1;

[XmlRoot("Project")]
public sealed record Trproj : ITrproj
{
	[XmlIgnore] public string FilePath { get; set; } = string.Empty;
	[XmlIgnore] public int Version => 1;

	[XmlElement] public string Name { get; set; } = string.Empty;
	[XmlElement] public string ScriptPath { get; set; } = string.Empty;
	[XmlElement] public string LevelsPath { get; set; } = string.Empty;
	[XmlArray] public List<MapRecord> Levels { get; set; } = new();

	public void MakePathsRelative(string baseDirectory)
	{
		ScriptPath = ScriptPath.Replace(baseDirectory, Constants.ProjectDirectoryKey);
		LevelsPath = LevelsPath.Replace(baseDirectory, Constants.ProjectDirectoryKey);

		Levels.ForEach(level => level.MakePathsRelative(baseDirectory));
	}

	public void MakePathsAbsolute(string baseDirectory)
	{
		ScriptPath = ScriptPath.Replace(Constants.ProjectDirectoryKey, baseDirectory);
		LevelsPath = LevelsPath.Replace(Constants.ProjectDirectoryKey, baseDirectory);

		Levels.ForEach(level => level.MakePathsAbsolute(baseDirectory));
	}
}
