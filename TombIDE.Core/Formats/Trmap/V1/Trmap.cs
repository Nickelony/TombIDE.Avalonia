namespace TombIDE.Core.Formats.Trmap.V1;

public sealed record Trmap : ITrmap
{
	[XmlAttribute] public int Version => 1;

	[XmlAttribute] public string MapName { get; set; } = string.Empty;

	/// <summary>
	/// Name of the startup .prj2 file from the root directory (e.g "My Map.prj2").
	/// </summary>
	[XmlAttribute] public string? StartupFileName { get; set; }
}
