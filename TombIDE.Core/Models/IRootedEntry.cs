namespace TombIDE.Core.Models;

/// <summary>
/// Defines an entry which has a root directory assigned to it.
/// </summary>
public interface IRootedEntry
{
	/// <summary>
	/// Path of the entry's main directory.
	/// <para>This property can be used as a unique identifier for the entry.</para>
	/// </summary>
	string RootDirectoryPath { get; set; }
}
