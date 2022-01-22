using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public record struct GameProject(string ProjectFilePath, string Name, string ScriptDirectoryPath,
	string MapsDirectoryPath, string? TRNGPluginsDirectoryPath = null, List<IMapProject>? Maps = null) : IGameProject
{
	public FileInfo ProjectFile { get; set; } = new FileInfo(ProjectFilePath);
	public string Name { get; set; } = Name;

	public DirectoryInfo RootDirectory => ProjectFile.Directory!;
	public DirectoryInfo ScriptDirectory { get; set; } = new DirectoryInfo(ScriptDirectoryPath);
	public DirectoryInfo MapsDirectory { get; set; } = new DirectoryInfo(MapsDirectoryPath);

	public DirectoryInfo? TRNGPluginsDirectory { get; set; }
		= TRNGPluginsDirectoryPath == null ? null : new DirectoryInfo(TRNGPluginsDirectoryPath);

	public List<IMapProject> Maps { get; set; } = Maps ?? new();
}
