namespace TombIDE.Services.Implementations;

public sealed class TRNGPluginService : ITRNGPluginService
{
	public FileInfo GetDllFile(DirectoryInfo pluginDirectory)
	{
		FileInfo[] pluginDllFiles = pluginDirectory.GetFiles("plugin_*.dll", SearchOption.TopDirectoryOnly);

		if (pluginDllFiles.Length == 0)
			throw new FileNotFoundException("Specified plugin directory is invalid.");

		return pluginDllFiles.First();
	}

	public string GetPluginName(DirectoryInfo pluginDirectory)
		=> GetValueFromBtnFile(pluginDirectory, "NAME")
		?? Path.GetFileNameWithoutExtension(GetDllFile(pluginDirectory).Name);

	public FileInfo? FindBtnFile(DirectoryInfo pluginDirectory) => GetPluginFile(pluginDirectory, ".btn");
	public FileInfo? FindDescriptionFile(DirectoryInfo pluginDirectory) => GetPluginFile(pluginDirectory, ".txt");
	public FileInfo? FindOcbFile(DirectoryInfo pluginDirectory) => GetPluginFile(pluginDirectory, ".ocb");
	public FileInfo? FindScriptFile(DirectoryInfo pluginDirectory) => GetPluginFile(pluginDirectory, ".script");
	public FileInfo? FindTrgFile(DirectoryInfo pluginDirectory) => GetPluginFile(pluginDirectory, ".trg");

	public FileInfo? FindLogoFile(DirectoryInfo pluginDirectory)
	{
		string? logoFileName = GetValueFromBtnFile(pluginDirectory, "LOGO");

		if (logoFileName is null)
			return null;

		string logoFilePath = Path.Combine(pluginDirectory.FullName, logoFileName);
		var logoFile = new FileInfo(logoFilePath);

		return logoFile.Exists ? logoFile : null;
	}

	public void Install(DirectoryInfo pluginDirectory, string engineDirectoryPath, string ngcDirectoryPath)
	{
		InstallIntoGame(pluginDirectory, engineDirectoryPath);
		InstallIntoNGC(pluginDirectory, ngcDirectoryPath);
	}

	public bool IsValid(DirectoryInfo pluginDirectory)
		=> pluginDirectory.GetFiles("plugin_*.dll", SearchOption.TopDirectoryOnly).Length > 0;

	private FileInfo? GetPluginFile(DirectoryInfo pluginDirectory, string fileExtension)
	{
		FileInfo dllFile = GetDllFile(pluginDirectory);

		string targetFileName = Path.GetFileNameWithoutExtension(dllFile.Name) + fileExtension;
		string targetFilePath = Path.Combine(pluginDirectory.FullName, targetFileName);

		return File.Exists(targetFilePath) ? new(targetFilePath) : null;
	}

	private string? GetValueFromBtnFile(DirectoryInfo pluginDirectory, string key)
	{
		FileInfo? btnFile = FindBtnFile(pluginDirectory);

		if (btnFile is null)
			return null;

		string btnFileContent = File.ReadAllText(btnFile.FullName);
		string[] lines = btnFileContent.SplitLines();

		string? targetLine = Array.Find(lines, line =>
			line.StartsWith(key, StringComparison.OrdinalIgnoreCase));

		if (targetLine is null || !targetLine.Contains('#'))
			return null;

		return targetLine.Split('#')[1];
	}

	private void InstallIntoGame(DirectoryInfo pluginDirectory, string engineDirectoryPath)
	{
		FileInfo dllFile = GetDllFile(pluginDirectory);
		string destFilePath = Path.Combine(engineDirectoryPath, dllFile.Name);
		dllFile.CopyTo(destFilePath, true);
	}

	private void InstallIntoNGC(DirectoryInfo pluginDirectory, string ngcDirectoryPath)
	{
		FileInfo? scriptFile = FindScriptFile(pluginDirectory);

		if (scriptFile is null)
			return;

		string destFilePath = Path.Combine(ngcDirectoryPath, scriptFile.Name);
		scriptFile.CopyTo(destFilePath, true);
	}
}
