using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public sealed record GameLanguage(string Name, string StringsFileName, string OutputFileName) : INamed
{
	public string Name { get; set; } = Name;

	/// <summary>
	/// Name of the language file (e.g. English.txt), which stores all in-game strings.
	/// </summary>
	public string StringsFileName { get; set; } = StringsFileName;

	/// <summary>
	/// Name of the output data file (e.g. English.dat).
	/// </summary>
	public string OutputFileName { get; set; } = OutputFileName;
}
