using TombIDE.Core.Formats;
using TombIDE.Core.Models;

namespace TombIDE.Services;

/// <summary>
/// Defines a service for .trmap file and data I/O.
/// </summary>
public interface ITrmapService
{
	/// <summary>
	/// Creates a trmap data structure from a file.
	/// </summary>
	/// <returns>A new trmap data structure.</returns>
	ITrmap CreateFromFile(string filePath);

	/// <summary>
	/// Creates a trmap data structure from a directory.
	/// </summary>
	/// <returns>A new trmap data structure.</returns>
	ITrmap CreateFromDirectory(string directoryPath);

	/// <summary>
	/// Creates a trmap data structure from a map project.
	/// </summary>
	/// <returns>A new trmap data structure.</returns>
	ITrmap CreateFromGameProject(IMapProject map);

	/// <summary>
	/// Writes trmap data into a file.
	/// </summary>
	void SaveToFile(string filePath, ITrmap trmap);
}
