using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

[XmlRoot("GameProject")]
public sealed class TrprojFile : ITrprojFile
{
	[XmlIgnore] public string FilePath { get; set; } = string.Empty;
	[XmlAttribute] public int Version => 2;

	[XmlAttribute] public string Name { get; set; } = string.Empty;
	[XmlAttribute] public GameVersion GameVersion { get; set; } = GameVersion.Unknown;
	[XmlAttribute] public string LauncherFilePath { get; set; } = string.Empty;
	[XmlAttribute] public string ScriptDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string MapsDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string? TRNGPluginsDirectoryPath { get; set; }
	[XmlAttribute] public int DefaultLanguageIndex { get; set; } = 0;
	[XmlArray] public List<LanguageRecord> SupportedLanguages { get; set; } = new();
	[XmlArray] public List<MapRecord> MapRecords { get; set; } = new();

	public void MakePathsRelative(string baseDirectory)
	{
		LauncherFilePath = Path.GetRelativePath(baseDirectory, LauncherFilePath);
		ScriptDirectoryPath = Path.GetRelativePath(baseDirectory, ScriptDirectoryPath);
		MapsDirectoryPath = Path.GetRelativePath(baseDirectory, MapsDirectoryPath);

		if (TRNGPluginsDirectoryPath != null)
			TRNGPluginsDirectoryPath = Path.GetRelativePath(baseDirectory, TRNGPluginsDirectoryPath);

		MapRecords.ForEach(map => map.MakePathsRelative(baseDirectory));
	}

	public void MakePathsAbsolute(string baseDirectory)
	{
		LauncherFilePath = Path.GetFullPath(LauncherFilePath, baseDirectory);
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
		[XmlAttribute] public string OutputFileName { get; set; } = string.Empty;

		public void MakePathsAbsolute(string baseDirectory)
			=> RootDirectoryPath = Path.GetFullPath(RootDirectoryPath, baseDirectory);

		public void MakePathsRelative(string baseDirectory)
			=> RootDirectoryPath = Path.GetRelativePath(baseDirectory, RootDirectoryPath);
	}

	public sealed class LanguageRecord
	{
		[XmlAttribute] public string Name { get; set; } = string.Empty;
		[XmlAttribute] public string StringsFileName { get; set; } = string.Empty;
		[XmlAttribute] public string OutputFileName { get; set; } = string.Empty;
	}
}
