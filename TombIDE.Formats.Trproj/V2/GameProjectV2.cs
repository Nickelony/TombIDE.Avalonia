using TombIDE.Core.Models;
using TombIDE.Core.Models.Enums;
using TombIDE.Core.Models.TRNG;
using TombIDE.Formats.Trproj.Bases;

namespace TombIDE.Formats.Trproj.V2;

[Serializable, XmlRoot("GameProject")]
public sealed class GameProjectV2 : GameProjectBase
{
	[XmlAttribute]
	public override int ProjectFileVersion => 2;

	[XmlAttribute] public override GameVersion GameVersion { get; set; } = GameVersion.Unknown;
	[XmlAttribute] public override string Name { get; set; } = string.Empty;
	[XmlAttribute] public override string LauncherFilePath { get; set; } = string.Empty;
	[XmlAttribute] public override string ScriptDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public override string MapsDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public override int DefaultLanguageIndex { get; set; } = 0;
	[XmlArray] public override List<IGameLanguage> SupportedLanguages { get; set; } = new();
	[XmlIgnore] public override List<TRNGPlugin> InstalledTRNGPlugins { get; set; } = new();
	[XmlArray] public override List<IMapProject> MapProjects { get; set; } = new();

	public override void MakePathsRelative(string baseDirectory)
	{
		LauncherFilePath = Path.GetRelativePath(baseDirectory, LauncherFilePath);
		ScriptDirectoryPath = Path.GetRelativePath(baseDirectory, ScriptDirectoryPath);
		MapsDirectoryPath = Path.GetRelativePath(baseDirectory, MapsDirectoryPath);

		MapProjects.ForEach(map => (map as MapProjectV2)?.MakePathsRelative(baseDirectory));
	}

	public override void MakePathsAbsolute(string baseDirectory)
	{
		LauncherFilePath = Path.GetFullPath(LauncherFilePath, baseDirectory);
		ScriptDirectoryPath = Path.GetFullPath(ScriptDirectoryPath, baseDirectory);
		MapsDirectoryPath = Path.GetFullPath(MapsDirectoryPath, baseDirectory);

		MapProjects.ForEach(map => (map as MapProjectV2)?.MakePathsAbsolute(baseDirectory));
	}
}
