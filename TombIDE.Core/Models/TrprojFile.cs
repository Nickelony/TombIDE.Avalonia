using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

[XmlRoot("GameProject")]
public sealed class TrprojFile : ITrprojFile
{
	[XmlIgnore] public string FilePath { get; set; } = string.Empty;
	[XmlAttribute] public int Version => 2;

	[XmlAttribute] public string Name { get; set; } = string.Empty;
	[XmlAttribute] public string ScriptDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string MapsDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string? TRNGPluginsDirectoryPath { get; set; }
	[XmlArray] public List<MapRecord> MapRecords { get; set; } = new();

	public void MakePathsRelative(string baseDirectory)
	{
		ScriptDirectoryPath = Path.GetRelativePath(baseDirectory, ScriptDirectoryPath);
		MapsDirectoryPath = Path.GetRelativePath(baseDirectory, MapsDirectoryPath);

		if (TRNGPluginsDirectoryPath != null)
			TRNGPluginsDirectoryPath = Path.GetRelativePath(baseDirectory, TRNGPluginsDirectoryPath);

		MapRecords.ForEach(map => map.MakePathsRelative(baseDirectory));
	}

	public void MakePathsAbsolute(string baseDirectory)
	{
		ScriptDirectoryPath = Path.GetFullPath(ScriptDirectoryPath, baseDirectory);
		MapsDirectoryPath = Path.GetFullPath(MapsDirectoryPath, baseDirectory);

		if (TRNGPluginsDirectoryPath != null)
			TRNGPluginsDirectoryPath = Path.GetFullPath(TRNGPluginsDirectoryPath, baseDirectory);

		MapRecords.ForEach(map => map.MakePathsAbsolute(baseDirectory));
	}

	public sealed class MapRecord : ISupportsRelativePaths
	{
		[XmlAttribute] public string Name { get; set; } = string.Empty;
		[XmlAttribute] public string RootDirectoryPath { get; set; } = string.Empty;
		[XmlAttribute] public string StartupFileName { get; set; } = string.Empty;

		public void MakePathsAbsolute(string baseDirectory)
			=> RootDirectoryPath = Path.GetFullPath(RootDirectoryPath, baseDirectory);

		public void MakePathsRelative(string baseDirectory)
			=> RootDirectoryPath = Path.GetRelativePath(baseDirectory, RootDirectoryPath);
	}
}
