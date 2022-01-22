using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Formats.Trproj;

namespace TombIDE.Services.Generic;

public interface IGameProjectService : IProjectService<IGameProject>
{
	IGameProject CreateNewProject(string projectFilePath, string name,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null);

	IGameProject CreateFromTrproj(ITrproj trproj);

	DirectoryInfo GetEngineDirectory(IGameProject game);
	FileInfo? FindGameLauncher(IGameProject game);
	FileInfo? FindGameExecutable(IGameProject game);
	GameVersion GetGameVersion(IGameProject game);

	IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins(IGameProject game);

	void SaveProject(IGameProject game);
}
