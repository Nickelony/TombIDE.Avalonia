namespace TombIDE.Core.Models.Interfaces;

/// <summary>
/// Defines an object which has a root directory assigned to it.
/// </summary>
public interface IRooted
{
	/// <summary>
	/// Path of the object's main directory.
	/// <para>This property can be used as a unique identifier for the object.</para>
	/// </summary>
	string RootDirectoryPath { get; }
}
