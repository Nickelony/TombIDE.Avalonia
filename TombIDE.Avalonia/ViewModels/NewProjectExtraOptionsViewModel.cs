using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Windows.Input;
using TombIDE.Avalonia.Core.ViewModels;

namespace TombIDE.Avalonia.ViewModels
{
	internal class NewProjectExtraOptionsViewModel : ViewModelBase
	{
		public NewProjectHostViewModel Parent { get; }

		[Reactive] public bool UseCustomScriptLocation { get; set; }
		[Reactive] public string? CustomScriptLocation { get; set; }
		public ICommand SelectScriptFolderCmd { get; }
		public Interaction<Unit, string?> SelectScriptFolderInter { get; }

		[Reactive] public bool UseCustomMapsLocation { get; set; }
		[Reactive] public string? CustomMapsLocation { get; set; }
		public ICommand SelectMapsFolderCmd { get; }
		public Interaction<Unit, string?> SelectMapsFolderInter { get; }

		[Reactive] public bool IncludeAudioFolder { get; set; }
		[Reactive] public bool IncludeStockWads { get; set; }
		[Reactive] public bool UseLegacyProjectStructure { get; set; }

		public ICommand BackCmd { get; }
		public ICommand InstallCmd { get; }

		public NewProjectExtraOptionsViewModel(NewProjectHostViewModel parent)
		{
			Parent = parent;

			SelectScriptFolderInter = new();
			SelectMapsFolderInter = new();

			BackCmd = ReactiveCommand.Create(Parent.Back);
			InstallCmd = ReactiveCommand.CreateFromTask(Parent.Install);

			SelectScriptFolderCmd = ReactiveCommand.CreateFromTask(async () => { });
			SelectMapsFolderCmd = ReactiveCommand.CreateFromTask(async () => { });
		}
	}
}
