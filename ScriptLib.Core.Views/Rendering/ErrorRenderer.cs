using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using ScriptLib.Core.Views.Bases;

namespace ScriptLib.Core.Views.Rendering
{
	public sealed class ErrorRenderer : IBackgroundRenderer
	{
		private TextEditorBase _editor;

		#region Construction

		public ErrorRenderer(TextEditorBase e)
			=> _editor = e;

		public KnownLayer Layer => KnownLayer.Caret;

		#endregion Construction

		#region Drawing

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			foreach (DocumentLine line in _editor.Document.Lines)
			{
				if (!line.HasError)
					continue;

				string lineText = _editor.Document.GetText(line.Offset, line.Length);

				int matchIndex = lineText.IndexOf(line.Error.ErrorSegmentText);

				if (matchIndex == -1)
					continue;

				var segment = new TextSegment
				{
					StartOffset = line.Offset + matchIndex,
					Length = line.Error.ErrorSegmentText.Length
				};

				foreach (Rect rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment))
				{
					//IBitmap underlining = TextRendering.CreateZigZagUnderlining((int)rect.Width, System.Drawing.Color.FromArgb(192, 255, 0, 0));

					//if (underlining == null)
					//	continue;

					//drawingContext.DrawImage(underlining,
					//	new Rect(new Point(rect.Position.X, rect.Position.Y + rect.Height - 2), new Size(rect.Width, 4)));
				}
			}
		}

		#endregion Drawing
	}
}
