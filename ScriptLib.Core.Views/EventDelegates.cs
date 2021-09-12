using ScriptLib.Core.Views.Events;

namespace ScriptLib.Core.Views
{
	public delegate void FindReplaceEventHandler(object sender, FindReplaceEventArgs e);

	public delegate void CellValueChangedEventHandler(object sender, CellContentChangedEventArgs e);

	public delegate void ObjectClickedEventHandler(object sender, ObjectClickedEventArgs e);
}
