using TombIDE.Core.Models.Enums;

namespace TombIDE.Core.Models;

/// <summary>
/// Defines a game plugin.
/// </summary>
public interface IPlugin : INamedEntry
{
	/// <summary>
	/// Target game version the plugin is made for.
	/// </summary>
	GameVersion TargetGameVersion { get; }
}
