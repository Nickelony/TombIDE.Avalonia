namespace TombIDE.Formats.Trproj.V2;

[XmlRoot("GameProject")]
public sealed class Trproj : ITrproj
{
	[XmlIgnore] public FileStream ProjectFile { get; init; }
	[XmlAttribute] public int Version => 2;

	[XmlAttribute] public string Name { get; set; } = string.Empty;
	[XmlAttribute] public string ScriptDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string MapsDirectoryPath { get; set; } = string.Empty;
	[XmlAttribute] public string? TRNGPluginsDirectoryPath { get; set; }
	[XmlArray] public List<MapRecord> MapRecords { get; set; } = new();

	public Trproj(string filePath) : this(File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
	{ }

	public Trproj(FileStream file)
		=> ProjectFile = file;

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

	public void Dispose() => ProjectFile.Dispose();
}
