using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TombIDE.Avalonia.ViewModels;
using TombIDE.Avalonia.Views;
using TombIDE.Avalonia.Windows;

namespace TombIDE.Avalonia
{
	public class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new StartWindowView
				{
					DataContext = new StartWindowViewModel(),
				};
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}
