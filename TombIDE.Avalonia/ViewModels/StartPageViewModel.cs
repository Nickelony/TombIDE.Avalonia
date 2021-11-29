using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using TombIDE.Avalonia.Core;
using TombIDE.Avalonia.Core.ViewModels;

namespace TombIDE.Avalonia.ViewModels
{
	internal class StartPageViewModel : ViewModelBase
	{
		[Reactive] public ObservableCollection<ProjectModel> Projects { get; set; }

		public ICommand CreateNewProjectCmd { get; }

		public ICommand OpenTrprojCmd { get; }
		public Interaction<Unit, string?> OpenTrprojInter { get; }

		public ICommand ImportProjectFromExecutableCmd { get; }
		public Interaction<Unit, string?> ImportProjectFromExecutableInter { get; }

		public ICommand OpenSelectedProjectCmd { get; }
		public Interaction<Unit, Unit> OpenSelectedProjectInter { get; }

		public StartWindowViewModel Parent { get; }

		public StartPageViewModel(StartWindowViewModel parent)
		{
			Parent = parent;

			Projects = new();

			OpenTrprojInter = new();
			ImportProjectFromExecutableInter = new();
			OpenSelectedProjectInter = new();

			CreateNewProjectCmd = ReactiveCommand.CreateFromTask(CreateNewProject);
			OpenTrprojCmd = ReactiveCommand.CreateFromTask(OpenTrproj);
			ImportProjectFromExecutableCmd = ReactiveCommand.CreateFromTask(ImportProjectFromExecutable);
			OpenSelectedProjectCmd = ReactiveCommand.CreateFromTask(OpenSelectedProject);
		}

		private async Task CreateNewProject()
		{
			Parent.ShowCreateNewProjectView();
		}

		private async Task OpenTrproj()
		{
		}

		private async Task ImportProjectFromExecutable()
		{
		}

		private async Task OpenSelectedProject()
		{
		}
	}
}
