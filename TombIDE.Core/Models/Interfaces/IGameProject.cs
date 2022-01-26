namespace TombIDE.Core.Models.Interfaces;

/// <summary>
/// Defines a game project.
/// </summary>
public interface IGameProject : IProject
{
	/// <summary>
	/// The project file. (e.g .trproj file)
	/// </summary>
	FileInfo ProjectFile { get; }

	/// <summary>
	/// The directory where the project's script files are stored.
	/// </summary>
	DirectoryInfo ScriptDirectory { get; }

	/// <summary>
	/// The directory where the project's newly created maps will be stored.
	/// </summary>
	DirectoryInfo MapsDirectory { get; }

	/// <summary>
	/// The directory from which the project should be reading TRNG plugins.
	/// </summary>
	DirectoryInfo? TRNGPluginsDirectory { get; }

	/// <summary>
	/// A list of the project's maps.
	/// </summary>
	List<IMapProject> Maps { get; }
}
