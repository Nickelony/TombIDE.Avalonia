using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services;

public interface IMenuBackgroundService
{
	Bitmap GetMenuBackground(IGameProject game);
	void SetMenuBackground(IGameProject game, Bitmap bitmap);
}
