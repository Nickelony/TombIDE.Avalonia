using TombIDE.Core.Models;
using TombIDE.Core.Models.Bases;
using TombIDE.Core.Models.Enums;
using TombIDE.Core.Models.TRNG;

namespace TombIDE.Formats.Trproj.Bases;

[Serializable]
public abstract class GameProjectBase : AccommodatedEntryBase, IGameProject
{
	[XmlIgnore]
	public string ProjectFilePath { get; set; } = string.Empty;

	[XmlIgnore]
	public override string RootDirectoryPath
	{
		get => Path.GetDirectoryName(ProjectFilePath) ?? string.Empty;
		set
		{
			if (string.IsNullOrEmpty(ProjectFilePath))
				throw new InvalidOperationException("The game project has not been initialized properly.");

			ProjectFilePath = ProjectFilePath.Replace(RootDirectoryPath, value.TrimEnd('\\'));
		}
	}

	[XmlIgnore]
	public string EngineDirectoryPath
	{
		get
		{
			string engineDirectoryPath = Path.Combine(RootDirectoryPath, "Engine");
			return Directory.Exists(engineDirectoryPath) ? engineDirectoryPath : RootDirectoryPath;
		}
	}

	[XmlIgnore]
	public abstract int ProjectFileVersion { get; }

	[XmlIgnore] public abstract GameVersion GameVersion { get; set; }
	[XmlIgnore] public abstract string LauncherFilePath { get; set; }
	[XmlIgnore] public abstract string ScriptDirectoryPath { get; set; }
	[XmlIgnore] public abstract string MapsDirectoryPath { get; set; }
	[XmlIgnore] public abstract int DefaultLanguageIndex { get; set; }
	[XmlIgnore] public abstract List<IGameLanguage> SupportedLanguages { get; set; }
	[XmlIgnore] public abstract List<TRNGPlugin> InstalledTRNGPlugins { get; set; }
	[XmlIgnore] public abstract List<IMapProject> MapProjects { get; set; }

	public void MakePathsRelative()
		=> MakePathsRelative(RootDirectoryPath);

	public void MakePathsAbsolute()
		=> MakePathsAbsolute(RootDirectoryPath);

	public void Save()
		=> TrprojWriter.WriteToFile(ProjectFilePath, this);
}
