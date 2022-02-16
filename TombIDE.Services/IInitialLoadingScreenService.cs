using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services;

public interface IInitialLoadingScreenService
{
	Bitmap GetInitialLoadingScreen(IGameProject game);
	void ChangeInitialLoadingScreen(IGameProject game, Bitmap bitmap);
}
