using TombIDE.Core.Models;

namespace TombIDE.Services.Abstract;

public interface IMnemonicConstantsProvider
{
	string MnemonicConstantsXmlFilePath { get; set; }

	IEnumerable<MnemonicConstant> GetInternalMnemonicConstants();
	IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugins(TRNGPlugin[] plugins);
}
