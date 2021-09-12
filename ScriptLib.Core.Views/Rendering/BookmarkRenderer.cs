﻿using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using ScriptLib.Core.Views.Bases;

namespace ScriptLib.Core.Views.Rendering
{
	public sealed class BookmarkRenderer : IBackgroundRenderer
	{
		private TextEditorBase _editor;

		#region Construction

		public BookmarkRenderer(TextEditorBase e)
			=> _editor = e;

		public KnownLayer Layer => KnownLayer.Background;

		#endregion Construction

		#region Drawing

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			foreach (DocumentLine line in _editor.Document.Lines)
				if (line.IsBookmarked)
				{
					var segment = new TextSegment { StartOffset = line.Offset, EndOffset = line.EndOffset };
					var background = new SolidColorBrush(Color.FromArgb(40, 128, 128, 255)); // Light blue
					var border = new Pen(Brushes.Transparent, 0);

					foreach (Rect rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, segment, true))
						drawingContext.DrawRectangle(background, border, new Rect(rect.Position, new Size(textView.Width, rect.Height)));
				}
		}

		#endregion Drawing
	}
}
