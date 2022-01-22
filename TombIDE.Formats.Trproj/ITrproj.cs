using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Formats.Trproj;

/// <summary>
/// Defines the base of a .trproj file.
/// </summary>
public interface ITrproj : IDisposable, IVersioned, ISupportsRelativePaths
{
	/// <summary>
	/// The .trproj file stream.
	/// </summary>
	public FileStream ProjectFile { get; }
}
