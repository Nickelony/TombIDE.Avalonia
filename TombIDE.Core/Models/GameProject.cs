using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public record GameProject
(
	string Name,
	FileInfo ProjectFile,
	DirectoryInfo RootDirectory,
	DirectoryInfo ScriptDirectory,
	DirectoryInfo MapsDirectory,
	List<IMapProject> Maps,
	DirectoryInfo? TRNGPluginsDirectory = null
) : IGameProject;
