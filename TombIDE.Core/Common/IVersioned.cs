namespace TombIDE.Core.Common;

/// <summary>
/// Defines an object which has a version number assigned to it.
/// </summary>
public interface IVersioned
{
	/// <summary>
	/// Version number of the object.
	/// </summary>
	int Version { get; }
}
