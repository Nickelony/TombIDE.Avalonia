using Splat;

namespace TombIDE;

public class AppBootstrapper
{
	public AppBootstrapper()
		=> RegisterServices();

	private void RegisterServices()
	{
		IMutableDependencyResolver mutable = Locator.CurrentMutable;

		//mutable.RegisterConstant(() => new TrprojDbService(""), typeof(ITrprojService));
		//mutable.RegisterConstant(() => new MnemonicConstantsProvider(""), typeof(ITrprojService));
	}
}
