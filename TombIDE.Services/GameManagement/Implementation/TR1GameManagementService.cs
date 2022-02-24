﻿using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services.GameManagement;

public sealed class TR1GameManagementService : IGameManagementService, ILauncherIconService, ISplashScreenService,
	IInitialLoadingScreenService, IMenuBackgroundService
{
	public void ChangeInitialLoadingScreen(IGameProject game, Bitmap bitmap) => throw new NotImplementedException();
	public void ChangeLauncherIcon(IGameProject game, Icon icon) => throw new NotImplementedException();
	public void ChangeMenuBackground(IGameProject game, Bitmap bitmap) => throw new NotImplementedException();
	public void ChangeSplashScreen(IGameProject game, Bitmap bitmap) => throw new NotImplementedException();
}