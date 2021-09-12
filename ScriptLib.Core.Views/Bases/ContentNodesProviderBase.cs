using Avalonia.Controls;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScriptLib.Core.Views.Bases
{
	public abstract class ContentNodesProviderBase : BackgroundWorker
	{
		public volatile string Filter = string.Empty;

		public void RunWorkerAsync(string editorContent)
			=> base.RunWorkerAsync(editorContent);

		protected override void OnDoWork(DoWorkEventArgs e)
		{
			if (e.Argument is string editorContent)
				e.Result = GetNodes(editorContent);

			base.OnDoWork(e);
		}

		protected abstract IEnumerable<TreeViewItem> GetNodes(string editorContent);
	}
}
