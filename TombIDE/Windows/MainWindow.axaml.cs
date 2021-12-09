namespace TombIDE.Windows;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
	}

	private void InitializeComponent()
		=> AvaloniaXamlLoader.Load(this);
}
