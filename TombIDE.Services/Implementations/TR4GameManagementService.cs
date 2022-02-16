using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services.Implementations;

public sealed class TR4GameManagementService : IGameManagementService, ILauncherIconService, ISplashScreenService,
	IInitialLoadingScreenService, IMenuLogoService
{
	public void ChangeInitialLoadingScreen(IGameProject game, Bitmap bitmap) => throw new NotImplementedException();
	public void ChangeLauncherIcon(IGameProject game, Icon icon) => throw new NotImplementedException();
	public void ChangeMenuLogo(IGameProject game, Image image) => throw new NotImplementedException();
	public void ChangeSplashScreen(IGameProject game, Bitmap bitmap) => throw new NotImplementedException();
}
