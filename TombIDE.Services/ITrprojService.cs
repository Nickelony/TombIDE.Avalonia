using TombIDE.Core.Models.Interfaces;
using TombIDE.Formats.Trproj;
using TrprojV1 = TombIDE.Formats.Trproj.V1.Trproj;
using TrprojV2 = TombIDE.Formats.Trproj.V2.Trproj;

namespace TombIDE.Services;

public interface ITrprojService
{
	/// <summary>
	/// Creates a trproj data structure from file data.
	/// </summary>
	ITrproj CreateFromFile(string trprojFilePath);

	/// <summary>
	/// Creates a trproj data structure from a game project.
	/// </summary>
	ITrproj CreateFromGameProject(IGameProject game);

	/// <summary>
	/// Writes trproj data into a file.
	/// </summary>
	/// <returns><see langword="true" /> if saving was successful, otherwise <see langword="false" />.</returns>
	bool SaveTrprojToFile(ITrproj trproj, string filePath);

	/// <summary>
	/// Converts a trproj V1 data structure to a V2 data structure.
	/// </summary>
	TrprojV2 ConvertV1ToV2(TrprojV1 trprojV1);

	ITrproj MakePathsRelative(ITrproj trproj, string baseDirectory);
	ITrproj MakePathsAbsolute(ITrproj trproj, string baseDirectory);
}
