namespace TombIDE.Controls.Settings;

public class SettingsPage : ItemsControl
{
	public static readonly StyledProperty<string> HeaderProperty =
		AvaloniaProperty.Register<TextBlock, string>(nameof(Header));

	public string Header
	{
		get => GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}
}
