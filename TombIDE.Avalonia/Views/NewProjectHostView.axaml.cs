using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TombIDE.Avalonia.Views
{
	public partial class NewProjectHostView : UserControl
	{
		public NewProjectHostView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
