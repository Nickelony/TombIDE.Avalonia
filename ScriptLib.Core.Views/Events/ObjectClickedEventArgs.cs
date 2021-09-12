using System;

namespace ScriptLib.Core.Views.Events
{
	public class ObjectClickedEventArgs : EventArgs
	{
		public string ObjectName { get; }
		public object IdentifyingObject { get; }

		public ObjectClickedEventArgs(string objectName, object identifyingObject = null)
		{
			ObjectName = objectName;
			IdentifyingObject = identifyingObject;
		}
	}
}
