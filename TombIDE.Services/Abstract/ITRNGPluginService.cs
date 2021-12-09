using TombIDE.Core.Models;
using TombIDE.Core.Models.TRNG;

namespace TombIDE.Services.Abstract;

public interface ITRNGPluginService
{
	IEnumerable<TRNGPlugin> GetAvailableTRNGPlugins();
	IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins(IGameProject gameProject);
}
