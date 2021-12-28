namespace TombIDE.Core.Models.Interfaces;

/// <summary>
/// Defines the base of a .trproj file.
/// </summary>
public interface ITrprojFile : IVersioned, ISupportsRelativePaths
{
	/// <summary>
	/// File path of the .trproj file.
	/// </summary>
	string FilePath { get; }
}
