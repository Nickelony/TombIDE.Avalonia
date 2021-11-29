using Avalonia.Controls;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TombIDE.Avalonia.Core.Enums;
using TombIDE.Avalonia.Core.ViewModels;

namespace TombIDE.Avalonia.ViewModels
{
	internal class NewProjectBasicInfoViewModel : ViewModelBase
	{
		public NewProjectHostViewModel Parent { get; }

		public List<ComboBoxItem> GameVersions { get; set; } // List is hard-coded

		[Reactive] public string? ProjectName { get; set; }
		[Reactive] public string? ProjectLocation { get; set; }
		[Reactive] public ComboBoxItem? SelectedGameVersion { get; set; }
		[Reactive] public ComboBoxItem? SelectedEngineType { get; set; }
		[Reactive] public ObservableCollection<ComboBoxItem> EngineTypes { get; set; }

		[ObservableAsProperty] public bool CanSelectEngineType { get; }

		public ICommand UpdateEngineTypesCommand { get; }
		public ICommand GameVersionHelpCommand { get; }
		public ICommand EngineTypeHelpCommand { get; }
		public ICommand BrowseProjectLocationCommand { get; }

		public ICommand NextCmd { get; }
		public ICommand CancelCmd { get; }

		public Interaction<Unit, string?> ShowOpenFolderDialog { get; }

		public NewProjectBasicInfoViewModel(NewProjectHostViewModel parent)
		{
			Parent = parent;

			/* Initialize properties */

			GameVersions = Core.Data.GameVersions.Versions;

			EngineTypes = new();
			ShowOpenFolderDialog = new();

			UpdateEngineTypesCommand = ReactiveCommand.Create(UpdateEngineTypes);
			GameVersionHelpCommand = ReactiveCommand.Create(() => { });
			EngineTypeHelpCommand = ReactiveCommand.Create(() => { });

			BrowseProjectLocationCommand = ReactiveCommand.CreateFromTask(async () =>
				ProjectLocation = await BrowseFolderAsync() ?? ProjectLocation);

			NextCmd = ReactiveCommand.Create(Parent.Next);
			CancelCmd = ReactiveCommand.Create(Parent.Cancel);

			/* Initialize observables */

			this.WhenAnyValue(x => x.SelectedGameVersion)
				.Select(x => x?.Tag is GameVersion version && version != GameVersion.None)
				.ToPropertyEx(this, x => x.CanSelectEngineType);

			this.WhenAnyValue(x => x.SelectedGameVersion)
				.IgnoreElements()
				.InvokeCommand(UpdateEngineTypesCommand);

			/* Initial methods */

			UpdateEngineTypes();
		}

		private void UpdateEngineTypes()
		{
			EngineTypes.Clear();

			if (SelectedGameVersion?.Tag is GameVersion version)
			{
				var availableTypes = GetAvailableEngineTypes(version);

				if (availableTypes != null)
					EngineTypes.AddRange(availableTypes);
			}
		}

		private List<ComboBoxItem>? GetAvailableEngineTypes(GameVersion version)
			=> version switch
			{
				GameVersion.TR2 => Core.Data.EngineTypes.TR2,
				GameVersion.TR3 => Core.Data.EngineTypes.TR3,
				GameVersion.TR4 => Core.Data.EngineTypes.TR4,
				GameVersion.TEN => Core.Data.EngineTypes.TEN,
				_ => null
			};

		private async Task<string?> BrowseFolderAsync()
		{
			string result = null; //await ShowOpenFolderDialog.Handle(Unit.Default);
			return string.IsNullOrEmpty(result) ? null : result;
		}
	}
}
