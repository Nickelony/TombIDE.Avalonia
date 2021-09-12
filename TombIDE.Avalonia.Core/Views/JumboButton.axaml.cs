using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace TombIDE.Avalonia.Core.Views
{
	public class JumboButton : Button
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

		public JumboButton()
			=> AvaloniaXamlLoader.Load(this);
	}
}
