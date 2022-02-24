using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services.GameManagement;

public interface ILauncherIconService
{
	Icon GetLauncherIcon(IGameProject game);
	void ChangeLauncherIcon(IGameProject game, Icon icon);
}
