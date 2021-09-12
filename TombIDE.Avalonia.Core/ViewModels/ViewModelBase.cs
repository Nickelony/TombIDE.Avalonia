using ReactiveUI.Validation.Helpers;

namespace TombIDE.Avalonia.Core.ViewModels
{
	public class ViewModelBase : ReactiveValidationObject
	{
		private static Localization.Localization _strings = Localization.Localization.GetLocalization(Localization.Language.English);
		private Localization.Localization Strings { get; set; } = _strings;
	}
}
