namespace TombIDE.Core.Models;

/// <summary>
/// Defines an entry which has a name assigned to it.
/// </summary>
public interface INamedEntry
{
	/// <summary>
	/// Displayed name of the entry.
	/// </summary>
	string Name { get; set; }
}
