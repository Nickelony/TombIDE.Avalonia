using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TombIDE.Avalonia.Core.ViewModels;

namespace TombIDE.Avalonia.ViewModels
{
	internal class NewProjectHostViewModel : ViewModelBase
	{
		public StartWindowViewModel Parent { get; }

		public NewProjectBasicInfoViewModel BasicInfoPage { get; }
		public NewProjectExtraOptionsViewModel ExtraOptionsPage { get; }

		[Reactive] public ViewModelBase Content { get; set; }

		public NewProjectHostViewModel(StartWindowViewModel parent)
		{
			Parent = parent;

			BasicInfoPage = new(this);
			ExtraOptionsPage = new(this);

			Content = BasicInfoPage;
		}

		public void Next() => Content = ExtraOptionsPage;
		public void Back() => Content = BasicInfoPage;
		public void Cancel() => Parent.ShowProjectSelectionView();

		public async Task Install()
		{

		}
	}
}
