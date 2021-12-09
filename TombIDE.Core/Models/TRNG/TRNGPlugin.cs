using TombIDE.Core.Models.Enums;

namespace TombIDE.Core.Models.TRNG;

public sealed class TRNGPlugin : IPlugin
{
	public string Name { get; set; } = string.Empty;
	public GameVersion TargetGameVersion => GameVersion.TRNG;

	/// <summary>
	/// Path of the .dll file which is stored inside the TIDE program directory.
	/// </summary>
	public string InternalDLLFilePath { get; set; } = string.Empty;
}
