using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services.GameManagement;

public interface ISplashScreenService
{
	Bitmap GetSplashScreen(IGameProject game);
	void ChangeSplashScreen(IGameProject game, Bitmap bitmap);
}
