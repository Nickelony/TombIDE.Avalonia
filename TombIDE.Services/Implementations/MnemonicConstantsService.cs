namespace TombIDE.Services.Implementations;

public sealed class MnemonicConstantsService : IMnemonicConstantsService
{
	private readonly ITRNGPluginService _trngPluginService;

	public MnemonicConstantsService(ITRNGPluginService trngPluginService)
		=> _trngPluginService = trngPluginService;

	public IEnumerable<MnemonicConstant> GetMnemonicConstantsFromXml(FileInfo xmlFile)
	{
		IEnumerable<MnemonicConstantXmlRecord> records =
			XmlUtils.ReadXmlFile<IEnumerable<MnemonicConstantXmlRecord>>(xmlFile.FullName);

		foreach (MnemonicConstantXmlRecord record in records)
			yield return new MnemonicConstant(record.Name, record.DecimalValue, record.Description);
	}

	public IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugin(DirectoryInfo pluginDirectory)
	{
		FileInfo? scriptFile = _trngPluginService.FindScriptFile(pluginDirectory);

		if (scriptFile is null)
			yield break;

		string[] lines = File.ReadAllLines(scriptFile.FullName);

		int startConstantsLineIndex = Array.FindIndex(lines, line =>
			line.TrimStart().StartsWith("<start_constants>", StringComparison.OrdinalIgnoreCase));

		int endLineIndex = Array.FindIndex(lines, line =>
			line.TrimStart().StartsWith("<end>", StringComparison.OrdinalIgnoreCase));

		if (startConstantsLineIndex == -1 || endLineIndex == -1)
			yield break;

		for (int i = startConstantsLineIndex + 1; i < endLineIndex; i++)
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
				description = string.Join(Environment.NewLine, descriptionLines.TrimEndAll());
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

	public IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugins(DirectoryInfo[] pluginDirectories)
	{
		var result = new List<MnemonicConstant>();

		foreach (DirectoryInfo pluginDirectory in pluginDirectories)
			result.AddRange(GetMnemonicConstantsFromPlugin(pluginDirectory));

		return result;
	}
}
