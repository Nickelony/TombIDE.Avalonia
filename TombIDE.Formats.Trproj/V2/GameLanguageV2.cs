namespace TombIDE.Core.Models;

[Serializable]
public sealed class GameLanguageV2 : IGameLanguage
{
	[XmlAttribute] public string Name { get; set; } = string.Empty;
	[XmlAttribute] public string StringsFileName { get; set; } = string.Empty;
	[XmlAttribute] public string OutputFileName { get; set; } = string.Empty;
}
