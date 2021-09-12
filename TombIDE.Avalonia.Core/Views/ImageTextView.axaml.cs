using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace TombIDE.Avalonia.Core.Views
{
	public class ImageTextView : TemplatedControl
	{
		public static readonly StyledProperty<IImage> ImageSourceProperty = AvaloniaProperty.Register<Image, IImage>(nameof(ImageSource));
		public IImage ImageSource
		{
			get => GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly StyledProperty<Geometry> PathDataProperty = AvaloniaProperty.Register<PathIcon, Geometry>(nameof(PathData));
		public Geometry PathData
		{
			get => GetValue(PathDataProperty);
			set => SetValue(PathDataProperty, value);
		}

		public static readonly StyledProperty<string> HeaderProperty = AvaloniaProperty.Register<TextBlock, string>(nameof(Header));
		public string Header
		{
			get => GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		public static readonly StyledProperty<string> NoteProperty = AvaloniaProperty.Register<TextBlock, string>(nameof(Note));
		public string Note
		{
			get => GetValue(NoteProperty);
			set => SetValue(NoteProperty, value);
		}

		public static readonly StyledProperty<double> HeaderFontSizeProperty = AvaloniaProperty.Register<TextBlock, double>(nameof(HeaderFontSize), 16);
		public double HeaderFontSize
		{
			get => GetValue(HeaderFontSizeProperty);
			set => SetValue(HeaderFontSizeProperty, value);
		}

		public static readonly StyledProperty<double> NoteFontSizeProperty = AvaloniaProperty.Register<TextBlock, double>(nameof(NoteFontSize), 12);
		public double NoteFontSize
		{
			get => GetValue(NoteFontSizeProperty);
			set => SetValue(NoteFontSizeProperty, value);
		}

		public static readonly StyledProperty<Thickness> IconMarginProperty = AvaloniaProperty.Register<TextBlock, Thickness>(nameof(IconMargin), new Thickness(0, 0, 10, 0));
		public Thickness IconMargin
		{
			get => GetValue(IconMarginProperty);
			set => SetValue(IconMarginProperty, value);
		}

		public static readonly StyledProperty<double> IconWidthProperty = AvaloniaProperty.Register<Control, double>(nameof(IconWidth), 24);
		public double IconWidth
		{
			get => GetValue(IconWidthProperty);
			set => SetValue(IconWidthProperty, value);
		}

		public static readonly StyledProperty<double> IconHeightProperty = AvaloniaProperty.Register<Control, double>(nameof(IconHeight), 24);
		public double IconHeight
		{
			get => GetValue(IconHeightProperty);
			set => SetValue(IconHeightProperty, value);
		}

		public ImageTextView()
			=> AvaloniaXamlLoader.Load(this);
	}
}
