using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Formats.Trproj;

/// <summary>
/// Defines the base of a .trproj file.
/// </summary>
public interface ITrproj : IVersioned
{
	/// <summary>
	/// The .trproj file.
	/// </summary>
	public FileInfo ProjectFile { get; }
}
