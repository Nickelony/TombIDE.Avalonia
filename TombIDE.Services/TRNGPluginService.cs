using TombIDE.Core;
using TombIDE.Core.Models;
using TombIDE.Core.Models.TRNG;
using TombIDE.Services.Abstract;

namespace TombIDE.Services;

public class TRNGPluginService : ITRNGPluginService
{
	public const string PluginArchiveFileName = "plugins.parc";

	public IEnumerable<TRNGPlugin> GetAvailableTRNGPlugins()
	{
		string[] directories = Directory.GetDirectories(DefaultPaths.TRNGPluginsDirectory, "*", SearchOption.TopDirectoryOnly);

		foreach (string directory in directories)
		{
			if (!IsValidPluginDirectory(directory))
				continue;

			//var plugin = TRNGPlugin.InstallPluginFolder(directory);
			//yield return plugin;
		}

		return null;
	}

	public IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins(IGameProject gameProject)
	{
		var availablePlugins = GetAvailableTRNGPlugins().ToList();

		string[] pluginFiles = Directory.GetFiles(
			gameProject.EngineDirectoryPath, "plugin_*.dll", SearchOption.TopDirectoryOnly);

		foreach (string pluginFile in pluginFiles)
		{
			string pluginFileName = Path.GetFileName(pluginFile);

			TRNGPlugin? targetPlugin = availablePlugins.Find(plugin =>
				Path.GetFileName(plugin.InternalDLLFilePath).Equals(pluginFileName, StringComparison.OrdinalIgnoreCase));

			if (targetPlugin != null)
			{
				yield return targetPlugin;
				continue;
			}
			else if (PluginExistsInPARCFile(gameProject.EngineDirectoryPath, pluginFile))
			{
				availablePlugins = GetAvailableTRNGPlugins().ToList();

				targetPlugin = availablePlugins.Find(plugin =>
					Path.GetFileName(plugin.InternalDLLFilePath).Equals(pluginFileName, StringComparison.OrdinalIgnoreCase));

				if (targetPlugin != null)
					yield return targetPlugin;
			}
			else
			{
				var unknownPlugin = new TRNGPlugin { Name = Path.GetFileName(pluginFile) };
				yield return unknownPlugin;
			}
		}
	}

	private bool IsValidPluginDirectory(string directoryPath)
	{
		string directoryName = Path.GetFileName(directoryPath);
		string[] files = Directory.GetFiles(directoryPath, "*.dll");

		return Array.Exists(files, file => Path.GetFileNameWithoutExtension(file) == directoryName);
	}
}
