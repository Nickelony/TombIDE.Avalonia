using Avalonia.Controls.ApplicationLifetimes;
using TombIDE.Assets;
using TombIDE.Windows;

namespace TombIDE;

public class App : Application
{
	public override void Initialize()
		=> AvaloniaXamlLoader.Load(this);

	public override void OnFrameworkInitializationCompleted()
	{
		new AppBootstrapper();

		Localizer.Instance.LoadLanguage("en");

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			desktop.MainWindow = new StartWindow { DataContext = new StartWindowViewModel() };

		base.OnFrameworkInitializationCompleted();
	}

	internal static IClassicDesktopStyleApplicationLifetime? ClassicLifetime
		=> Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
}
