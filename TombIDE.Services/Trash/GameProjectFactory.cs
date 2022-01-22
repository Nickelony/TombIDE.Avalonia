using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils.Trproj;

namespace TombIDE.Core.Utils;

public static class GameProjectFactory
{
	public static GameProjectRecord CreateNew(string projectFilePath, string name,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null)
		=> new(projectFilePath, name, scriptDirectoryPath, mapsDirectoryPath, trngPluginsDirectoryPath);
}
