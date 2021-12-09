using TombIDE.Core.Models;
using TombIDE.Core.Models.Bases;

namespace TombIDE.Formats.Trproj.V2;

[Serializable, XmlRoot("MapProject")]
public sealed class MapProjectV2 : AccommodatedEntryBase, IMapProject
{
	[XmlAttribute] public override string Name { get; set; } = string.Empty;
	[XmlAttribute] public override string RootDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string StartupFileName { get; set; } = string.Empty;
	[XmlAttribute] public string OutputFileName { get; set; } = string.Empty;

	public override void MakePathsAbsolute(string baseDirectory)
		=> RootDirectoryPath = Path.GetFullPath(RootDirectoryPath, baseDirectory);

	public override void MakePathsRelative(string baseDirectory)
		=> RootDirectoryPath = Path.GetRelativePath(baseDirectory, RootDirectoryPath);
}
