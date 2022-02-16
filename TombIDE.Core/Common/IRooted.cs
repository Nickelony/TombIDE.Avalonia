namespace TombIDE.Core.Common;

/// <summary>
/// Defines an object which has a root directory assigned to it.
/// </summary>
public interface IRooted
{
	/// <summary>
	/// The object's main directory.
	/// </summary>
	DirectoryInfo RootDirectory { get; }
}
