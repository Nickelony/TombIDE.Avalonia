using System.Collections.ObjectModel;

namespace TombIDE.Core.Models;

/// <summary>
/// Defines a game project.
/// </summary>
public interface IGameProject : IProject
{
	/// <summary>
	/// The directory where the project's script files are stored.
	/// </summary>
	DirectoryInfo ScriptDirectory { get; }

	/// <summary>
	/// The directory where the project's local maps are stored.
	/// </summary>
	DirectoryInfo MapsDirectory { get; }

	/// <summary>
	/// The directory from which the project should be reading TRNG plugins.
	/// </summary>
	DirectoryInfo? TRNGPluginsDirectory { get; }

	/// <summary>
	/// A list of the project's map sub-directories, which are stored outside of the project's main maps directory.
	/// </summary>
	ObservableCollection<DirectoryInfo> ExternalMapSubdirectories { get; }

	event EventHandler? ScriptDirectoryChanged;
	event EventHandler? MapsDirectoryChanged;
	event EventHandler? TRNGPluginsDirectoryChanged;
	event EventHandler? MapsDirectoryContentChanged;
	event EventHandler? TRNGPluginsDirectoryContentChanged;

	void ChangeScriptDirectory(string newDirectoryPath);
	void ChangeMapsDirectory(string newDirectoryPath);
	void ChangeTRNGPluginsDirectory(string newDirectoryPath);
}
