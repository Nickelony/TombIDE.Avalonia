using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ScriptLib.Core.Views
{
	public partial class FindReplaceWindow : Window
	{
		public FindReplaceWindow()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
