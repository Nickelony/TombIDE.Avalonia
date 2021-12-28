using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public sealed class GameLanguage : INamed
{
	public string Name { get; set; }

	/// <summary>
	/// Name of the language file (e.g. English.txt), which stores all in-game strings.
	/// </summary>
	public string StringsFileName { get; set; }

	/// <summary>
	/// Name of the output data file (e.g. English.dat).
	/// </summary>
	public string OutputFileName { get; set; }

	public GameLanguage(string name, string stringsFileName, string outputFileName)
	{
		Name = name;
		StringsFileName = stringsFileName;
		OutputFileName = outputFileName;
	}
}
