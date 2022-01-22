using TombIDE.Core.Models.Interfaces;
using TombIDE.Formats.Trproj;
using TombIDE.Services.Generic;
using TombIDE.Services.Records;

namespace TombIDE.Services.Abstract;

public interface ITrprojService : IXmlDatabaseService
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
}
