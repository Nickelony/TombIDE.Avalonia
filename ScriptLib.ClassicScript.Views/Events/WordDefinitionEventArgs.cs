using ScriptLib.ClassicScript.Data.Enums;
using System;

namespace ScriptLib.ClassicScript.Views.Events
{
	public class WordDefinitionEventArgs : EventArgs
	{
		public string Word { get; }
		public WordType Type { get; }

		public WordDefinitionEventArgs(string word, WordType type)
		{
			Word = word;
			Type = type;
		}
	}
}
