using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Services;

public interface IGameProjectService : IProjectService<IGameProject>
{
	IGameProject CreateNewProject(string name, string projectFilePath,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null,
		List<IMapProject>? maps = null);

	IGameProject CreateFromFile(string projectFilePath);

	/// <returns>The directory where the engine executable is being stored.</returns>
	DirectoryInfo GetEngineDirectory(IGameProject game);

	/// <returns>launch.exe file if it exists, otherwise the engine executable (e.g. tomb4.exe).</returns>
	FileInfo GetGameLauncherFile(IGameProject game);

	/// <returns>The engine executable (e.g. tomb4.exe).</returns>
	FileInfo GetEngineExecutable(IGameProject game);

	GameVersion DetectGameVersion(IGameProject game);

	IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins(IGameProject game);

	void SaveProject(IGameProject game);
}
