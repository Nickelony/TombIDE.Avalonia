using TombIDE.Core.Models;
using TombIDE.Core.Models.Bases;

namespace TombIDE.Formats.Trproj.V1;

[Serializable, XmlRoot("ProjectLevel")]
public sealed class MapProjectV1 : AccommodatedEntryBase, IMapProject
{
	[XmlElement] public override string Name { get; set; } = string.Empty;
	[XmlElement("FolderPath")] public override string RootDirectoryPath { get; set; } = string.Empty;
	[XmlElement("SpecificFile")] public string StartupFileName { get; set; } = Constants.V1_LastModifiedFileKey;
	[XmlElement("DataFileName")] public string OutputFileName { get; set; } = string.Empty;

	public override void MakePathsAbsolute(string baseDirectory)
		=> RootDirectoryPath = RootDirectoryPath.Replace(Constants.V1_ProjectDirectoryKey, baseDirectory);

	public override void MakePathsRelative(string baseDirectory)
		=> RootDirectoryPath = RootDirectoryPath.Replace(baseDirectory, Constants.V1_ProjectDirectoryKey);
}
