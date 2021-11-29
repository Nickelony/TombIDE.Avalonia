using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TombIDE.Avalonia.Views
{
	public partial class NewProjectBasicInfoView : UserControl
	{
		public NewProjectBasicInfoView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
