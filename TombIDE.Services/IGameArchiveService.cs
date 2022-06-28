using TombIDE.Core.Models;

namespace TombIDE.Services;

public interface IGameArchiveService
{
	void GenerateGameArchive(IGameProject game, string archiveFilePath, string? readmeText);
}
