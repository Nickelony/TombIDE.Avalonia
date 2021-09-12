using Avalonia.Controls;
using System.Collections.Generic;
using TombIDE.Avalonia.Core.Enums;

namespace TombIDE.Avalonia.Core.Data
{
	public static class GameVersions
	{
		public static List<ComboBoxItem> Versions => new()
		{
			new ComboBoxItem { Content = "TR2", Tag = GameVersion.TR2 },
			new ComboBoxItem { Content = "TR3", Tag = GameVersion.TR3 },
			new ComboBoxItem { Content = "TR4", Tag = GameVersion.TR4 },
			new ComboBoxItem { Content = "Tomb Engine", Tag = GameVersion.TEN }
		};
	}
}
