using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Formats.Trproj.V1;

[XmlRoot("ProjectLevel")]
public sealed record MapRecord : ISupportsRelativePaths
{
	[XmlElement] public string Name { get; set; } = string.Empty;
	[XmlElement] public string FolderPath { get; set; } = string.Empty;
	[XmlElement] public string SpecificFile { get; set; } = Constants.LastModifiedFileKey;

	public void MakePathsAbsolute(string baseDirectory)
		=> FolderPath = FolderPath.Replace(Constants.ProjectDirectoryKey, baseDirectory);

	public void MakePathsRelative(string baseDirectory)
		=> FolderPath = FolderPath.Replace(baseDirectory, Constants.ProjectDirectoryKey);
}
