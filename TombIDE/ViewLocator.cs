using Avalonia.Controls.Templates;

namespace TombIDE;

public class ViewLocator : IDataTemplate
{
	public IControl Build(object data)
	{
		string? name = data.GetType().FullName!.Replace("ViewModel", "View");
		var type = Type.GetType(name);

		if (type != null)
			return (Control)Activator.CreateInstance(type)!;
		else
			return new TextBlock { Text = "Not Found: " + name };
	}

	public bool Match(object data)
		=> data is ReactiveObject;
}