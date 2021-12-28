namespace TombIDE.Core.Models.Interfaces;

/// <summary>
/// Defines an object which can make all its path properties both relative and absolute.
/// </summary>
public interface ISupportsRelativePaths
{
	void MakePathsAbsolute(string baseDirectory);
	void MakePathsRelative(string baseDirectory);
}
