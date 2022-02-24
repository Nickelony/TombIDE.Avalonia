using TombIDE.Core.Formats;
using TombIDE.Core.Models;
using TrprojV1 = TombIDE.Core.Formats.Trproj.V1.Trproj;
using TrprojV2 = TombIDE.Core.Formats.Trproj.V2.Trproj;

namespace TombIDE.Services;

/// <summary>
/// Defines a service for .trproj file and data I/O.
/// </summary>
public interface ITrprojService
{
	/// <summary>
	/// Creates a trproj data structure from a file.
	/// </summary>
	/// <returns>A new trproj data structure.</returns>
	ITrproj CreateFromFile(string filePath);

	/// <summary>
	/// Creates a trproj data structure from a game project.
	/// </summary>
	/// <returns>A new trproj data structure.</returns>
	ITrproj CreateFromGameProject(IGameProject game);

	/// <summary>
	/// Writes trproj data into a file.
	/// </summary>
	void SaveToFile(string filePath, ITrproj trproj, bool makePathsRelative = true);

	/// <summary>
	/// Converts a trproj V1 data structure into a V2 data structure.
	/// </summary>
	/// <returns>An updated trproj V2 data structure.</returns>
	TrprojV2 ConvertV1ToV2(TrprojV1 trprojV1);

	/// <summary>
	/// Creates a new trproj data structure where all paths are relative to a base directory.
	/// </summary>
	/// <returns>An updated trproj data structure.</returns>
	ITrproj MakePathsRelative(ITrproj trproj, string baseDirectory);

	/// <summary>
	/// Creates a new trproj data structure where all paths are absolute from a base directory.
	/// </summary>
	/// <returns>An updated trproj data structure.</returns>
	ITrproj MakePathsAbsolute(ITrproj trproj, string baseDirectory);
}
