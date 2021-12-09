using TombIDE.Core.Models;
using TombIDE.Windows;

namespace TombIDE.Start;

public class SelectProjectViewModel : ReactiveObject
{
	[Reactive] public ObservableCollection<IGameProject> RecentProjects { get; set; }
	[Reactive] public bool RememberProject { get; set; }

	public ICommand ShowSettings { get; }
	public ICommand CreateNewProjectCmd { get; }

	public ICommand OpenTrprojCmd { get; }
	public Interaction<Unit, string?> OpenTrprojInter { get; }

	public ICommand ImportUsingExeCmd { get; }
	public Interaction<Unit, string?> ImportUsingExeInter { get; }

	public ICommand OpenSelectedProjectCmd { get; }
	public Interaction<Unit, Unit> OpenSelectedProjectInter { get; }

	public StartWindowViewModel Parent { get; }

	public SelectProjectViewModel(StartWindowViewModel parent)
	{
		Parent = parent;

		RecentProjects = new();

		OpenTrprojInter = new();
		ImportUsingExeInter = new();
		OpenSelectedProjectInter = new();

		ShowSettings = ReactiveCommand.CreateFromTask(async () => Parent.ShowSettingsView());
		CreateNewProjectCmd = ReactiveCommand.CreateFromTask(CreateNewProject);
		OpenTrprojCmd = ReactiveCommand.CreateFromTask(OpenTrproj);
		ImportUsingExeCmd = ReactiveCommand.CreateFromTask(ImportProjectFromExecutable);
		OpenSelectedProjectCmd = ReactiveCommand.CreateFromTask(OpenSelectedProject);
	}

	private async Task CreateNewProject() => Parent.ShowCreateProjectView();

	private async Task OpenTrproj()
	{
	}

	private async Task ImportProjectFromExecutable() => Parent.ShowImportProjectView();

	private async Task OpenSelectedProject()
	{
	}
}
