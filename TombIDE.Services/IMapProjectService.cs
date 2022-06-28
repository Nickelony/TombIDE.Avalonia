using TombIDE.Core.Models;

namespace TombIDE.Services;

/// <summary>
/// Defines a service for map project CRUD handling, data fetching and data processing.
/// </summary>
public interface IMapProjectService
{
	/// <summary>
	/// Creates a new, empty .prj2 map file.
	/// </summary>
	// void CreatePrj2File(IMapProject map, LevelSettings? settings = null);

	/// <summary>
	/// Creates a brand new map project instance.
	/// <para><b>Note:</b> The .trmap file won't be created until <c>SaveProject()</c> is used.</para>
	/// </summary>
	/// <returns>A new map project instance.</returns>
	IMapProject CreateNewMap(string name);

	/// <summary>
	/// Creates a map project instance from an existing .trmap file.
	/// </summary>
	/// <param name="mapFilePath">Path of the target .trmap file.</param>
	/// <returns>A map project instance.</returns>
	IMapProject CreateFromFile(string mapFilePath);

	/// <summary>
	/// Creates a map project instance from a map subdirectory.
	/// <para><b>NOTE:</b> Use this only when the map subdirectory doesn't have a .trmap file.</para>
	/// </summary>
	/// <param name="mapSubdirectoryPath">Path of the target map subdirectory.</param>
	/// <returns>A map project instance.</returns>
	IMapProject CreateFromDirectory(string mapSubdirectoryPath);

	/// <returns>
	/// A collection of valid (non backup) .prj2 files from the map's root directory.
	/// </returns>
	IEnumerable<FileInfo> GetPrj2Files(IMapProject map);

	/// <returns>
	/// A collection of only backup .prj2 files from the map's root directory.
	/// </returns>
	IEnumerable<FileInfo> GetBackupPrj2Files(IMapProject map);

	/// <returns>
	/// A collection of all files of the .prj2 format from the map's root directory.<br />
	/// This includes both <b>normal</b> .prj2 files and <b>backup</b> files.
	/// </returns>
	IEnumerable<FileInfo> GetAllPrj2Files(IMapProject map);

	/// <returns>
	/// The most recently modified .prj2 file.
	/// </returns>
	FileInfo GetMostRecentlyModifiedPrj2File(IMapProject map);

	/// <summary>
	/// Determines whether the given map project has any valid .prj2 files inside of its root directory.
	/// <para><b>NOTE:</b> Backup files are not considered valid .prj2 files.</para>
	/// </summary>
	bool IsValidProject(IMapProject map);

	/// <summary>
	/// Determines whether the given map subdirectory has any valid .prj2 files inside.
	/// <para><b>NOTE:</b> Backup files are not considered valid .prj2 files.</para>
	/// </summary>
	bool IsValidMapSubdirectory(DirectoryInfo subdirectory);

	/// <summary>
	/// Saves the given project into a .trmap file, which will be created inside the map's root directory.
	/// <para><b>NOTE:</b> The name of the map file will be <b>map.trmap</b></para>
	/// </summary>
	void SaveProject(IMapProject map);
}
