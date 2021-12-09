using TombIDE.Core.Models;
using TombIDE.Core.Models.Enums;
using TombIDE.Core.Models.TRNG;
using TombIDE.Formats.Trproj.Bases;

namespace TombIDE.Formats.Trproj.V1;

[Serializable, XmlRoot("Project")]
public sealed class GameProjectV1 : GameProjectBase
{
	[XmlIgnore]
	public override int ProjectFileVersion => 1;

	[XmlElement] public override GameVersion GameVersion { get; set; } = GameVersion.Unknown;
	[XmlElement] public override string Name { get; set; } = string.Empty;
	[XmlElement("LaunchFilePath")] public override string LauncherFilePath { get; set; } = string.Empty;
	[XmlElement("ScriptPath")] public override string ScriptDirectoryPath { get; set; } = string.Empty;
	[XmlElement("LevelsPath")] public override string MapsDirectoryPath { get; set; } = string.Empty;
	[XmlIgnore] public override int DefaultLanguageIndex { get; set; } = 0;
	[XmlIgnore] public override List<IGameLanguage> SupportedLanguages { get; set; } = new();
	[XmlIgnore] public override List<TRNGPlugin> InstalledTRNGPlugins { get; set; } = new();
	[XmlArray("Levels")] public override List<IMapProject> MapProjects { get; set; } = new();

	public override void MakePathsRelative(string baseDirectory)
	{
		string directoryKey = Constants.V1_ProjectDirectoryKey;

		LauncherFilePath = LauncherFilePath.Replace(baseDirectory, directoryKey);
		ScriptDirectoryPath = ScriptDirectoryPath.Replace(baseDirectory, directoryKey);
		MapsDirectoryPath = MapsDirectoryPath.Replace(baseDirectory, directoryKey);

		MapProjects.ForEach(map => (map as MapProjectV1)?.MakePathsRelative(baseDirectory));
	}

	public override void MakePathsAbsolute(string baseDirectory)
	{
		string directoryKey = Constants.V1_ProjectDirectoryKey;

		LauncherFilePath = LauncherFilePath.Replace(directoryKey, baseDirectory);
		ScriptDirectoryPath = ScriptDirectoryPath.Replace(directoryKey, baseDirectory);
		MapsDirectoryPath = MapsDirectoryPath.Replace(directoryKey, baseDirectory);

		MapProjects.ForEach(map => (map as MapProjectV1)?.MakePathsAbsolute(baseDirectory));
	}
}
