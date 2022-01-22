using TombIDE.Core.Models;
using TombIDE.Services.Generic;

namespace TombIDE.Services.Abstract;

public interface IMnemonicConstantsService : IXmlDatabaseService
{
	IEnumerable<MnemonicConstant> GetLocalMnemonicConstants();
	IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugin(TRNGPlugin plugin);
	IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugins(TRNGPlugin[] plugins);
}
