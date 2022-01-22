using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Formats.Trproj.V2;

public sealed record MapRecord : ISupportsRelativePaths
{
	[XmlAttribute] public string Name { get; set; } = string.Empty;
	[XmlAttribute] public string RootDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string StartupFileName { get; set; } = string.Empty;

	public void MakePathsAbsolute(string baseDirectory)
		=> RootDirectoryPath = Path.GetFullPath(RootDirectoryPath, baseDirectory);

	public void MakePathsRelative(string baseDirectory)
		=> RootDirectoryPath = Path.GetRelativePath(baseDirectory, RootDirectoryPath);
}
