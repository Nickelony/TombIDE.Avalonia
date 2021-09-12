using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using ScriptLib.ClassicScript.Parsers;

namespace ScriptLib.ClassicScript.Views.Rendering
{
	public sealed class SectionRenderer : IBackgroundRenderer
	{
		private ClassicScriptEditor _editor;

		#region Construction

		public SectionRenderer(ClassicScriptEditor e)
			=> _editor = e;

		public KnownLayer Layer => KnownLayer.Caret;

		#endregion Construction

		#region Drawing

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			foreach (DocumentLine line in _editor.Document.Lines)
			{
				string lineText = _editor.Document.GetText(line.Offset, line.Length);

				if (LineParser.IsSectionHeaderLine(lineText))
				{
					var segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
					var border = new Pen(new SolidColorBrush(Color.FromRgb(192, 192, 192)), 0.5);

					foreach (Rect rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment, true))
						drawingContext.DrawLine(border, new Point(rect.Position.X, rect.Position.Y), new Point(textView.Width, rect.Position.Y));
				}
			}
		}

		#endregion Drawing
	}
}
