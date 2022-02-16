using TombIDE.Core;
using TombIDE.Core.Models;

namespace TombIDE.Services;

/// <summary>
/// Defines a service for game project CRUD handling, data fetching and data processing.
/// </summary>
public interface IGameProjectService
{
	/// <summary>
	/// Creates a brand new game project instance.
	/// <para><b>Note:</b> The .trproj file won't be created until <c>SaveProject()</c> is used.</para>
	/// </summary>
	/// <returns>A new game project instance.</returns>
	IGameProject CreateNewProject(string name, string rootDirectoryPath,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null,
		IEnumerable<DirectoryInfo>? externalMapSubdirectories = null);

	/// <summary>
	/// Creates a game project instance from an existing .trproj file.
	/// </summary>
	/// <param name="projectFilePath">Path of the target .trproj file.</param>
	/// <returns>A game project instance.</returns>
	IGameProject CreateFromFile(string projectFilePath);

	/// <returns>
	/// A collection of all the project's maps, which is a combination of <b>local</b> and <b>external</b> maps.
	/// </returns>
	IEnumerable<IMapProject> GetAllMaps(IGameProject game);

	/// <returns>
	/// The directory where the <b>engine executable</b> is being stored (e.g. tomb4.exe).
	/// </returns>
	DirectoryInfo GetEngineDirectory(IGameProject game);

	/// <returns>
	/// The <b>launch.exe</b> file if it exists, otherwise the engine executable (e.g. tomb4.exe).
	/// </returns>
	FileInfo GetGameLauncher(IGameProject game);

	/// <returns>
	/// The engine executable (e.g. tomb4.exe).
	/// </returns>
	FileInfo GetEngineExecutable(IGameProject game);

	/// <summary>
	/// Detects the project's game version based on its engine folder contents.
	/// </summary>
	/// <returns>The game version (e.g. TR2, TR3, TR4, ...)</returns>
	GameVersion DetectGameVersion(IGameProject game);

	/// <summary>
	/// Retrieves plugin subdirectories from the project's specified <c>TRNGPluginsDirectory</c>.
	/// <para><b>NOTE:</b> The resulting collection will be empty if <c>TRNGPluginsDirectory</c> is <see langword="null" /></para>
	/// </summary>
	IEnumerable<DirectoryInfo> GetInstalledTRNGPluginDirectories(IGameProject game);

	/// <summary>
	/// Determines whether the core components of the given game project are available and valid.
	/// </summary>
	bool IsValidProject(IGameProject game);

	/// <summary>
	/// Saves the given project into a .trproj file, which will be created inside the project's root directory.
	/// <para><b>NOTE:</b> The name of the project file will be <b>project.trproj</b></para>
	/// </summary>
	void SaveProject(IGameProject game);
}
