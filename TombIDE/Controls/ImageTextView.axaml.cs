namespace TombIDE.Controls;

public class ImageTextView : TemplatedControl
{
	public static readonly StyledProperty<IImage> ImageSourceProperty =
		AvaloniaProperty.Register<Image, IImage>(nameof(ImageSource));

	public static readonly StyledProperty<Geometry> PathDataProperty =
		AvaloniaProperty.Register<PathIcon, Geometry>(nameof(PathData));

	public static readonly StyledProperty<string> HeaderProperty =
		AvaloniaProperty.Register<TextBlock, string>(nameof(Header));

	public static readonly StyledProperty<string> NoteProperty =
		AvaloniaProperty.Register<TextBlock, string>(nameof(Note));

	public static readonly StyledProperty<double> HeaderFontSizeProperty =
		AvaloniaProperty.Register<TextBlock, double>(nameof(HeaderFontSize), 16);

	public static readonly StyledProperty<double> NoteFontSizeProperty =
		AvaloniaProperty.Register<TextBlock, double>(nameof(NoteFontSize), 12);

	public static readonly StyledProperty<Thickness> IconMarginProperty =
		AvaloniaProperty.Register<TextBlock, Thickness>(nameof(IconMargin), new Thickness(0, 0, 10, 0));

	public static readonly StyledProperty<double> IconWidthProperty =
		AvaloniaProperty.Register<Control, double>(nameof(IconWidth), 24);

	public static readonly StyledProperty<double> IconHeightProperty =
		AvaloniaProperty.Register<Control, double>(nameof(IconHeight), 24);

	public IImage ImageSource
	{
		get => GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}

	public Geometry PathData
	{
		get => GetValue(PathDataProperty);
		set => SetValue(PathDataProperty, value);
	}

	public string Header
	{
		get => GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	public string Note
	{
		get => GetValue(NoteProperty);
		set => SetValue(NoteProperty, value);
	}

	public double HeaderFontSize
	{
		get => GetValue(HeaderFontSizeProperty);
		set => SetValue(HeaderFontSizeProperty, value);
	}

	public double NoteFontSize
	{
		get => GetValue(NoteFontSizeProperty);
		set => SetValue(NoteFontSizeProperty, value);
	}

	public Thickness IconMargin
	{
		get => GetValue(IconMarginProperty);
		set => SetValue(IconMarginProperty, value);
	}

	public double IconWidth
	{
		get => GetValue(IconWidthProperty);
		set => SetValue(IconWidthProperty, value);
	}

	public double IconHeight
	{
		get => GetValue(IconHeightProperty);
		set => SetValue(IconHeightProperty, value);
	}
}
