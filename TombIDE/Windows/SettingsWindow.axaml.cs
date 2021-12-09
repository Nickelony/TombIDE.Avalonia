namespace TombIDE.Windows;

public partial class SettingsWindow : Window
{
	public SettingsWindow()
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
	}

	private void InitializeComponent()
		=> AvaloniaXamlLoader.Load(this);
}
