using Avalonia.Media.Imaging;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using System;

namespace ScriptLib.Core.Data
{
	/// <summary>
	/// Implements AvalonEdit ICompletionData interface to provide the entries in the completion drop down.
	/// </summary>
	public sealed class CompletionData : ICompletionData
	{
		public CompletionData(string text)
			=> Text = text;

		public string Text { get; }
		public object Content => Text;
		public object Description => "";
		public double Priority => 0;

		public IBitmap? Image => null;

		public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
			=> textArea.Document.Replace(completionSegment, Text);
	}
}
