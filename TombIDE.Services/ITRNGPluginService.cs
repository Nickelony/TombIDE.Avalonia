namespace TombIDE.Services;

/// <summary>
/// Defines a service for TRNG plugin processing.
/// </summary>
public interface ITRNGPluginService
{
	FileInfo GetDllFile(DirectoryInfo pluginDirectory);
	string GetPluginName(DirectoryInfo pluginDirectory);

	FileInfo? FindBtnFile(DirectoryInfo pluginDirectory);
	FileInfo? FindDescriptionFile(DirectoryInfo pluginDirectory);
	FileInfo? FindOcbFile(DirectoryInfo pluginDirectory);
	FileInfo? FindScriptFile(DirectoryInfo pluginDirectory);
	FileInfo? FindTrgFile(DirectoryInfo pluginDirectory);
	FileInfo? FindLogoFile(DirectoryInfo pluginDirectory);

	bool IsValidPluginDirectory(DirectoryInfo pluginDirectory);

	void Install(DirectoryInfo pluginDirectory, string engineDirectoryPath, string ngcDirectoryPath);
}
