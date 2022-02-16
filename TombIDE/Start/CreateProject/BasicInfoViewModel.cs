using System.Reactive.Linq;
using TombIDE.Data.Models;

namespace TombIDE.Start.CreateProject;

public class BasicInfoViewModel : ReactiveObject
{
	public CreateProjectViewModel Parent { get; }

	public string[] GameVersions { get; set; }

	[Reactive] public string? ProjectName { get; set; }
	[Reactive] public string? ProjectLocation { get; set; }
	[Reactive] public ComboBoxItem? SelectedGameVersion { get; set; }
	[Reactive] public ComboBoxItem? SelectedEngineType { get; set; }

	public ICommand GameVersionHelpCommand { get; }
	public ICommand BrowseProjectLocationCommand { get; }

	public ICommand NextCmd { get; }
	public ICommand CancelCmd { get; }

	public Interaction<Unit, string?> ShowOpenFolderDialog { get; }

	public BasicInfoViewModel(CreateProjectViewModel parent)
	{
		Parent = parent;

		GameVersions = Enum.GetNames<GameVersion>();

		ShowOpenFolderDialog = new();

		GameVersionHelpCommand = ReactiveCommand.Create(() => { });

		BrowseProjectLocationCommand = ReactiveCommand.CreateFromTask(async () =>
			ProjectLocation = await BrowseFolderAsync() ?? ProjectLocation);

		NextCmd = ReactiveCommand.Create(Parent.Next);
		CancelCmd = ReactiveCommand.Create(Parent.Cancel);
	}

	private async Task<string?> BrowseFolderAsync()
	{
		string result = await ShowOpenFolderDialog.Handle(Unit.Default);
		return string.IsNullOrEmpty(result) ? null : result;
	}
}
