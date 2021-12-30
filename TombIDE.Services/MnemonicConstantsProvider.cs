using TombIDE.Core.Models;
using TombIDE.Core.Utils;
using TombIDE.Services.Abstract;
using TombIDE.Services.Records;

namespace TombIDE.Services;

public sealed class MnemonicConstantsProvider : IMnemonicConstantsProvider
{
	public string MnemonicConstantsXmlFilePath { get; set; }

	public MnemonicConstantsProvider(string mnemonicConstantsXmlFilePath)
		=> MnemonicConstantsXmlFilePath = mnemonicConstantsXmlFilePath;

	public IEnumerable<MnemonicConstant> GetInternalMnemonicConstants()
	{
		IEnumerable<MnemonicConstantRecord> records =
			XmlUtils.ReadXmlFile<IEnumerable<MnemonicConstantRecord>>(MnemonicConstantsXmlFilePath);

		foreach (MnemonicConstantRecord record in records)
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
