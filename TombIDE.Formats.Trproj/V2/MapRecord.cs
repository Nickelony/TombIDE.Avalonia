using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Formats.Trproj.V2;

public sealed class MapRecord
{
	[XmlAttribute] public string Name { get; set; } = string.Empty;
	[XmlAttribute] public string RootDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string StartupFileName { get; set; } = string.Empty;
}
