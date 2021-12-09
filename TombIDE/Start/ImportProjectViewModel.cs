using TombIDE.Windows;

namespace TombIDE.Start;

public class ImportProjectViewModel : ReactiveObject
{
	public StartWindowViewModel Parent { get; }

	public ImportProjectViewModel(StartWindowViewModel parent)
		=> Parent = parent;
}
