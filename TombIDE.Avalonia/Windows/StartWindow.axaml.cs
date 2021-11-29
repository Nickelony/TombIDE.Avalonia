using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TombIDE.Avalonia.Windows
{
	public partial class StartWindowView : Window
	{
		public StartWindowView()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
