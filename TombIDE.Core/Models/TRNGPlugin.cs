using TombIDE.Core.Extensions;
using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public sealed class TRNGPlugin : INamed, IRooted, IValidated
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

	public bool IsValid
		=> File.Exists(DllFilePath);

	public TRNGPlugin(string pluginDirectoryPath)
		=> RootDirectoryPath = pluginDirectoryPath;

	public IEnumerable<MnemonicConstant> GetMnemonicConstants()
	{
		if (!File.Exists(ScriptFilePath))
			yield break;

		string[] lines = File.ReadAllLines(ScriptFilePath);

		int startConstantsLineIndex = Array.FindIndex(lines, line =>
			line.TrimStart().StartsWith("<start_constants>", StringComparison.OrdinalIgnoreCase));

		if (startConstantsLineIndex == -1)
			yield break;

		int endConstantsLineIndex = Array.FindIndex(lines, line =>
			line.TrimStart().StartsWith("<end>", StringComparison.OrdinalIgnoreCase));

		if (endConstantsLineIndex == -1)
			yield break;

		for (int i = startConstantsLineIndex + 1; i < endConstantsLineIndex; i++)
		{
			string line = lines[i];

			bool hasValue = line.Contains(':');

			if (!hasValue)
				continue;

			string constantName = line.Split(':')[0].Trim();
			string valueString = line.Split(':')[1].Trim();
			string description = string.Empty;

			int descriptionStartIndex = line.IndexOf(';') + 1;
			bool hasDescription = descriptionStartIndex != 0;

			if (hasDescription)
			{
				valueString = valueString.Split(';')[0].Trim();

				string[] descriptionLines = line[descriptionStartIndex..].Split('>');
				description = string.Join(Environment.NewLine, descriptionLines);
			}

			bool isValidShort = short.TryParse(valueString, out short shortValue);

			if (!isValidShort)
			{
				try { shortValue = Convert.ToInt16(valueString.Replace("$", string.Empty), 16); }
				catch { continue; }
			}

			yield return new MnemonicConstant(constantName, shortValue, description);
		}
	}

	public void Install(string engineDirectoryPath, string ngcDirectoryPath)
	{
		InstallIntoGame(engineDirectoryPath);
		InstallIntoNGC(ngcDirectoryPath);
	}

	private void InstallIntoGame(string engineDirectoryPath)
	{
		if (!File.Exists(DllFilePath))
			return;

		string destFilePath = Path.Combine(engineDirectoryPath, Path.GetFileName(DllFilePath));
		File.Copy(DllFilePath, destFilePath);
	}

	private void InstallIntoNGC(string ngcDirectoryPath)
	{
		if (!File.Exists(ScriptFilePath))
			return;

		string destFilePath = Path.Combine(ngcDirectoryPath, Path.GetFileName(ScriptFilePath));
		File.Copy(ScriptFilePath, destFilePath);
	}

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
