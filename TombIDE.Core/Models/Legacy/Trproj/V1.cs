using TombIDE.Core.Models.Enums;
using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models.Legacy.Trproj;

[XmlRoot("Project")]
public sealed class V1 : ITrprojFile
{
	public const string ProjectDirectoryKey = "$(ProjectDirectory)";

	[XmlIgnore] public string FilePath { get; set; } = string.Empty;
	[XmlIgnore] public int Version => 1;

	[XmlElement] public string Name { get; set; } = string.Empty;
	[XmlElement] public GameVersion GameVersion { get; set; } = GameVersion.Unknown;
	[XmlElement] public string LaunchFilePath { get; set; } = string.Empty;
	[XmlElement] public string ScriptPath { get; set; } = string.Empty;
	[XmlElement] public string LevelsPath { get; set; } = string.Empty;
	[XmlArray] public List<ProjectLevel> Levels { get; set; } = new();

	public void MakePathsRelative(string baseDirectory)
	{
		LaunchFilePath = LaunchFilePath.Replace(baseDirectory, ProjectDirectoryKey);
		ScriptPath = ScriptPath.Replace(baseDirectory, ProjectDirectoryKey);
		LevelsPath = LevelsPath.Replace(baseDirectory, ProjectDirectoryKey);

		Levels.ForEach(level => level.MakePathsRelative(baseDirectory));
	}

	public void MakePathsAbsolute(string baseDirectory)
	{
		LaunchFilePath = LaunchFilePath.Replace(ProjectDirectoryKey, baseDirectory);
		ScriptPath = ScriptPath.Replace(ProjectDirectoryKey, baseDirectory);
		LevelsPath = LevelsPath.Replace(ProjectDirectoryKey, baseDirectory);

		Levels.ForEach(level => level.MakePathsAbsolute(baseDirectory));
	}

	public sealed class ProjectLevel : ISupportsRelativePaths
	{
		public const string LastModifiedFileKey = "$(LatestFile)";

		[XmlElement] public string Name { get; set; } = string.Empty;
		[XmlElement] public string FolderPath { get; set; } = string.Empty;
		[XmlElement] public string SpecificFile { get; set; } = LastModifiedFileKey;
		[XmlElement] public string DataFileName { get; set; } = string.Empty;

		public void MakePathsAbsolute(string baseDirectory)
			=> FolderPath = FolderPath.Replace(ProjectDirectoryKey, baseDirectory);

		public void MakePathsRelative(string baseDirectory)
			=> FolderPath = FolderPath.Replace(baseDirectory, ProjectDirectoryKey);
	}
}
