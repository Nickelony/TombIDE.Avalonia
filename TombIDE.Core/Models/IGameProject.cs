using TombIDE.Core.Models.Enums;
using TombIDE.Core.Models.TRNG;

namespace TombIDE.Core.Models;

/// <summary>
/// Defines a game project.
/// </summary>
public interface IGameProject : IAccommodatedEntry
{
	/// <summary>
	/// Path of the project file. (e.g .trproj file path)
	/// </summary>
	string ProjectFilePath { get; set; }

	/// <summary>
	/// The version number of the project file. It's used to identify which reader version should be used when reading the file.
	/// </summary>
	int ProjectFileVersion { get; }

	/// <summary>
	/// Game version the project is based on. (e.g. TR2, TR4, TRNG, TEN etc.)
	/// </summary>
	GameVersion GameVersion { get; set; }

	/// <summary>
	/// The path where the project's engine executable (e.g tomb4.exe) file is stored.
	/// </summary>
	string EngineDirectoryPath { get; }

	/// <summary>
	/// Path of the file, which launches the game.
	/// </summary>
	string LauncherFilePath { get; set; }

	/// <summary>
	/// Path of the directory where the project's script files are stored.
	/// </summary>
	string ScriptDirectoryPath { get; set; }

	/// <summary>
	/// Path of the directory where all the project's newly created / imported maps are stored.
	/// </summary>
	string MapsDirectoryPath { get; set; }

	/// <summary>
	/// Index of the default language of the project.
	/// </summary>
	int DefaultLanguageIndex { get; set; }

	/// <summary>
	/// A list of all languages the project supports.
	/// </summary>
	List<IGameLanguage> SupportedLanguages { get; set; }

	/// <summary>
	/// A list of the project's installed TRNG plugins.
	/// </summary>
	List<TRNGPlugin> InstalledTRNGPlugins { get; set; }

	/// <summary>
	/// A list of the project's maps.
	/// </summary>
	List<IMapProject> MapProjects { get; set; }

	void MakePathsRelative();
	void MakePathsAbsolute();

	void Save();
}
