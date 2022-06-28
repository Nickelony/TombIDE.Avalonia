using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services;

public interface IInitialLoadingScreenService
{
	Bitmap GetInitialLoadingScreen(IGameProject game);
	void SetInitialLoadingScreen(IGameProject game, Bitmap bitmap);
}
