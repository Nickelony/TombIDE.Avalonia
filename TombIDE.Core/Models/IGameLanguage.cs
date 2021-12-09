namespace TombIDE.Core.Models;

/// <summary>
/// Defines an in-game language.
/// </summary>
public interface IGameLanguage : INamedEntry
{
	/// <summary>
	/// Name of the language file (e.g. English.txt), which stores all in-game strings.
	/// </summary>
	string StringsFileName { get; set; }

	/// <summary>
	/// Name of the output data file (e.g. English.dat).
	/// </summary>
	string OutputFileName { get; set; }
}
