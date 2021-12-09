using TombIDE.Start;

namespace TombIDE.Windows;

public class StartWindowViewModel : ReactiveObject
{
	[Reactive] public ReactiveObject Content { get; set; }

	public SelectProjectViewModel SelectProjectView { get; }

	public StartWindowViewModel()
	{
		SelectProjectView = new(this);

		Content = SelectProjectView;
	}

	public void ShowSelectProjectView() => Content = SelectProjectView;
	public void ShowSettingsView()
	{ }
	public void ShowCreateProjectView() => Content = new CreateProjectViewModel(this);
	public void ShowImportProjectView() => Content = new ImportProjectViewModel(this);
}
