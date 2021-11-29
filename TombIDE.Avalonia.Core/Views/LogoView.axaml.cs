using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace TombIDE.Avalonia.Core.Views
{
	public partial class LogoView : TemplatedControl
	{
		public LogoView()
			=> AvaloniaXamlLoader.Load(this);
	}
}
