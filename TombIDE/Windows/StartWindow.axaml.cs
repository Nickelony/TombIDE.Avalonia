namespace TombIDE.Windows;

public partial class StartWindow : Window
{
	public StartWindow()
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
	}

	private void InitializeComponent()
		=> AvaloniaXamlLoader.Load(this);
}
