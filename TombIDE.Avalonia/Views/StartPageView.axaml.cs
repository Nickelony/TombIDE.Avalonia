using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TombIDE.Avalonia.Views
{
	public partial class StartPageView : UserControl
	{
		public StartPageView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
