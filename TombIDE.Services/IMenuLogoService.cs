using System.Drawing;
using TombIDE.Core.Models;

namespace TombIDE.Services;

public interface IMenuLogoService
{
	Image GetMenuLogo(IGameProject game);
	void ChangeMenuLogo(IGameProject game, Image image);
}
