using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services.GameManagement;

public sealed class TR4GameManagementService : IGameManagementService, ILauncherIconService, ISplashScreenService,
	IInitialLoadingScreenService, IMenuLogoService, IGameArchiveService
{
	public Icon GetLauncherIcon(IGameProject game)
	{
		throw new NotImplementedException();
	}

	public void SetLauncherIcon(IGameProject game, Icon icon)
	{
		throw new NotImplementedException();
	}

	public Bitmap GetSplashScreen(IGameProject game)
	{
		throw new NotImplementedException();
	}

	public void SetSplashScreen(IGameProject game, Bitmap bitmap)
	{
		throw new NotImplementedException();
	}

	public Bitmap GetInitialLoadingScreen(IGameProject game)
	{
		throw new NotImplementedException();
	}

	public void SetInitialLoadingScreen(IGameProject game, Bitmap bitmap)
	{
		throw new NotImplementedException();
	}

	public Image GetMenuLogo(IGameProject game)
	{
		throw new NotImplementedException();
	}

	public void SetMenuLogo(IGameProject game, Image image)
	{
		throw new NotImplementedException();
	}

	public void GenerateGameArchive(IGameProject game, string archiveFilePath, string? readmeText)
	{
		throw new NotImplementedException();
	}
}
