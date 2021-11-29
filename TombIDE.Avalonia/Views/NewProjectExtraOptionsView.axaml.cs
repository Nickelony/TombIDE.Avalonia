using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TombIDE.Avalonia.Views
{
	public partial class NewProjectExtraOptionsView : UserControl
	{
		public NewProjectExtraOptionsView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
