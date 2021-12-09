namespace TombIDE.Core.Models;

/// <summary>
/// Defines an entry which has a name and a root directory assigned to it.
/// </summary>
public interface IAccommodatedEntry : INamedEntry, IRootedEntry, IEquatable<IAccommodatedEntry>
{
	void MakePathsRelative(string baseDirectory);
	void MakePathsAbsolute(string baseDirectory);

	void Rename(string newName, bool renameDirectory = true);
}
