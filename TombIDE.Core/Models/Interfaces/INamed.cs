namespace TombIDE.Core.Models.Interfaces;

/// <summary>
/// Defines an object which has a name assigned to it.
/// </summary>
public interface INamed
{
	/// <summary>
	/// Displayed name of the object.
	/// </summary>
	string Name { get; set; }
}
