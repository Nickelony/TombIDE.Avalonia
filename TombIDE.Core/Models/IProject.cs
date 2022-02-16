using TombIDE.Core.Common;

namespace TombIDE.Core.Models;

/// <summary>
/// Defines the base of a project.
/// </summary>
public interface IProject : INamed, IRooted, IDisposable
{
	event EventHandler? NameChanged;
	event EventHandler? RootDirectoryChanged;

	void Rename(string newName, bool renameRootDirectory = false);
	void MoveRootDirectory(string newRootPath);
}
