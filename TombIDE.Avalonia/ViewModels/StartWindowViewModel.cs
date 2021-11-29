using ReactiveUI.Fody.Helpers;
using TombIDE.Avalonia.Core.ViewModels;

namespace TombIDE.Avalonia.ViewModels
{
	internal class StartWindowViewModel : ViewModelBase
	{
		[Reactive] public ViewModelBase Content { get; set; }

		public StartPageViewModel StartPage { get; }

		public StartWindowViewModel()
		{
			Content = StartPage = new StartPageViewModel(this);
		}

		public void ShowCreateNewProjectView() => Content = new NewProjectHostViewModel(this);

		public void ShowProjectSelectionView() => Content = StartPage;
	}
}
