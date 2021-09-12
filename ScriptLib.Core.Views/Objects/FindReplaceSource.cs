using System.Collections.Generic;

namespace ScriptLib.Core.Views.Objects
{
	public class FindReplaceSource : List<FindReplaceItem>
	{
		public string Name { get; set; }

		public FindReplaceSource()
		{ }

		public FindReplaceSource(string name)
			=> Name = name;
	}
}
