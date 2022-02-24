using TombIDE.Core.Models.References;

namespace TombIDE.Services;

/// <summary>
/// Defines a service for retrieving mnemonic constants from different sources.
/// </summary>
public interface IMnemonicConstantsService
{
	/// <summary>
	/// Retrieves constants from a XML database file.
	/// </summary>
	IEnumerable<MnemonicConstant> GetMnemonicConstantsFromXml(FileInfo xmlFile);

	/// <summary>
	/// Retrieves constants from a plugin's .script file.
	/// </summary>
	IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugin(DirectoryInfo pluginDirectory);

	/// <summary>
	/// Retrieves constants from multiple plugins.
	/// </summary>
	IEnumerable<MnemonicConstant> GetMnemonicConstantsFromPlugins(DirectoryInfo[] pluginDirectories);
}
