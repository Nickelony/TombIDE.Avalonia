using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services.GameManagement;

public sealed class TR3GameManagementService : IGameManagementService, ILauncherIconService, ISplashScreenService,
	IInitialLoadingScreenService, IMenuBackgroundService, IGameArchiveService
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

	public Bitmap GetMenuBackground(IGameProject game)
	{
		throw new NotImplementedException();
	}

	public void SetMenuBackground(IGameProject game, Bitmap bitmap)
	{
		throw new NotImplementedException();
	}

	public void GenerateGameArchive(IGameProject game, string archiveFilePath, string? readmeText)
	{
		throw new NotImplementedException();
	}
}
