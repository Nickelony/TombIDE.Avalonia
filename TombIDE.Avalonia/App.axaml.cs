using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FluentAvalonia.Styling;
using TombIDE.Avalonia.ViewModels;
using TombIDE.Avalonia.Views.Windows;

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
				desktop.MainWindow = new StartWindow
				{
					DataContext = new StartWindowViewModel(),
				};
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}
