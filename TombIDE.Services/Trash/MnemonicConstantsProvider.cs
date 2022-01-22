using TombIDE.Core.Models;
using TombIDE.Core.Utils;
using TombIDE.Services.Abstract;
using TombIDE.Services.Records;

namespace TombIDE.Services;

public sealed class MnemonicConstantsProvider : IMnemonicConstantsService
{
	public string MnemonicConstantsXmlFilePath { get; set; }

	public MnemonicConstantsProvider(string mnemonicConstantsXmlFilePath)
		=> MnemonicConstantsXmlFilePath = mnemonicConstantsXmlFilePath;

	public IEnumerable<MnemonicConstant> GetLocalMnemonicConstants()
	{
		IEnumerable<MnemonicConstantDbRecord> records =
			XmlUtils.ReadXmlFile<IEnumerable<MnemonicConstantDbRecord>>(MnemonicConstantsXmlFilePath);

		foreach (MnemonicConstantDbRecord record in records)
			yield return new MnemonicConstant(record.Name, record.DecimalValue, record.Description);
	}

	public IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugins(TRNGPlugin[] plugins)
	{
		var result = new List<MnemonicConstant>();

		foreach (TRNGPlugin plugin in plugins)
			result.AddRange(plugin.GetMnemonicConstants());

		return result;
	}
}
