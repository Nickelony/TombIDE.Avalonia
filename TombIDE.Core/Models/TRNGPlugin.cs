using TombIDE.Core.Extensions;
using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public sealed class TRNGPlugin : INamed, IRooted
{
	public string RootDirectoryPath { get; set; }

	public string? DllFilePath
	{
		get
		{
			var directory = new DirectoryInfo(RootDirectoryPath);
			FileInfo[] pluginDllFiles = directory.GetFiles("plugin_*.dll", SearchOption.TopDirectoryOnly);

			FileInfo? firstDllFile = pluginDllFiles.FirstOrDefault();

			if (firstDllFile == null)
				return null;

			return firstDllFile.FullName;
		}
	}

	public string Name => GetValueFromBtnFile("NAME")
		?? Path.GetFileNameWithoutExtension(DllFilePath)
		?? "ERROR";

	public string? BtnFilePath => GetPluginFile(".btn");
	public string? DescriptionFilePath => GetPluginFile(".txt");
	public string? OcbFilePath => GetPluginFile(".ocb");
	public string? ScriptFilePath => GetPluginFile(".script");
	public string? TrgFilePath => GetPluginFile(".trg");

	public string? LogoFilePath
	{
		get
		{
			string? logoFileName = GetValueFromBtnFile("LOGO");

			if (logoFileName == null)
				return null;

			return Path.Combine(RootDirectoryPath, logoFileName);
		}
	}

	public TRNGPlugin(string pluginDirectoryPath)
		=> RootDirectoryPath = pluginDirectoryPath;

	private string? GetPluginFile(string fileExtension)
	{
		string targetFileName = Path.GetFileNameWithoutExtension(DllFilePath) + fileExtension;
		string targetFilePath = Path.Combine(RootDirectoryPath, targetFileName);

		if (!File.Exists(targetFilePath))
			return null;

		return targetFilePath;
	}

	private string? GetValueFromBtnFile(string key)
	{
		if (BtnFilePath == null)
			return null;

		string btnFileContent = File.ReadAllText(BtnFilePath);
		string[] lines = btnFileContent.SplitLines();

		string? targetLine = Array.Find(lines, line =>
			line.StartsWith(key, StringComparison.OrdinalIgnoreCase));

		if (targetLine == null || !targetLine.Contains('#'))
			return null;

		return targetLine.Split('#')[1];
	}
}
