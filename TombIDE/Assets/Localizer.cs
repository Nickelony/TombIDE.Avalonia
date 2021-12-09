using Avalonia.Platform;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace TombIDE.Assets;

public class Localizer : INotifyPropertyChanged
{
	private const string IndexerName = "Item";
	private const string IndexerArrayName = "Item[]";

	private Dictionary<string, string>? Strings;

	public bool LoadLanguage(string language)
	{
		Language = language;
		IAssetLoader? assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

		var uri = new Uri($"avares://TombIDE/Assets/i18n/{language}.json");

		if (assets != null && assets.Exists(uri))
		{
			using var reader = new StreamReader(assets.Open(uri), Encoding.UTF8);
			Strings = JsonSerializer.Deserialize<Dictionary<string, string>>(reader.ReadToEnd());

			Invalidate();

			return true;
		}

		return false;
	}

	public string? Language { get; private set; }

	public string this[string key]
	{
		get
		{
			if (Strings != null && Strings.TryGetValue(key, out string? value))
				return value.Replace("\\n", "\n");

			return $"{Language}:{key}";
		}
	}

	public static Localizer Instance { get; set; } = new Localizer();

	public event PropertyChangedEventHandler? PropertyChanged;

	public void Invalidate()
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
	}
}
