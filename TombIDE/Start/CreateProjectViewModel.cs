using TombIDE.Start.CreateProject;
using TombIDE.Windows;

namespace TombIDE.Start;

public class CreateProjectViewModel : ReactiveObject
{
	public StartWindowViewModel Parent { get; }

	public BasicInfoViewModel BasicInfoView { get; }
	public ExtraOptionsViewModel ExtraOptionsView { get; }

	[Reactive] public ReactiveObject Content { get; set; }

	public CreateProjectViewModel(StartWindowViewModel parent)
	{
		Parent = parent;

		BasicInfoView = new(this);
		ExtraOptionsView = new(this);

		Content = BasicInfoView;
	}

	public void Next() => Content = ExtraOptionsView;
	public void Back() => Content = BasicInfoView;
	public void Cancel() => Parent.ShowSelectProjectView();

	public async Task Install()
	{
	}
}
