using Avalonia.Controls;
using System.Collections.Generic;
using TombIDE.Avalonia.Core.Enums;

namespace TombIDE.Avalonia.Core.Data
{
	public static class EngineTypes
	{
		public static List<ComboBoxItem> TR2 => new()
		{
			new ComboBoxItem { Content = "Vanilla", Tag = EngineType.Vanilla }
		};

		public static List<ComboBoxItem> TR3 => new()
		{
			new ComboBoxItem { Content = "Vanilla", Tag = EngineType.Vanilla }
		};

		public static List<ComboBoxItem> TR4 => new()
		{
			new ComboBoxItem { Content = "Vanilla", Tag = EngineType.Vanilla },
			new ComboBoxItem { Content = "TRNG", Tag = EngineType.Custom1 },
			new ComboBoxItem { Content = "TRNG + FLEP", Tag = EngineType.Custom2 }
		};

		public static List<ComboBoxItem> TEN => new()
		{
			new ComboBoxItem { Content = "Vanilla", Tag = EngineType.Vanilla }
		};
	}
}
