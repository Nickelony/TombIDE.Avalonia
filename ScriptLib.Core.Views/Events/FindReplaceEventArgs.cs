using ScriptLib.Core.Views.Objects;
using System;
using System.Collections.Generic;

namespace ScriptLib.Core.Views.Events
{
	public class FindReplaceEventArgs : EventArgs
	{
		public List<FindReplaceSource> SourceCollection { get; }

		public FindReplaceEventArgs(List<FindReplaceSource> collection)
			=> SourceCollection = collection;
	}
}
