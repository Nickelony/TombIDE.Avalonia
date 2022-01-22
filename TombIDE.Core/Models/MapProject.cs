using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public record struct MapProject(string Name, string RootDirectoryPath, string? StartupFilePath = null) : IMapProject
{
	public string Name { get; set; } = Name;

	private DirectoryInfo _rootDirectory = new(RootDirectoryPath);
	public DirectoryInfo RootDirectory
	{
		get => _rootDirectory;
		set
		{
			if (StartupFile != null)
			{
				string oldRootPath = _rootDirectory.FullName;
				string newRootPath = value.FullName;

				StartupFile = new FileInfo(StartupFile.FullName.Replace(oldRootPath, newRootPath));
			}

			_rootDirectory = value;
		}
	}

	public FileInfo? StartupFile { get; set; }
		= StartupFilePath == null ? null : new FileInfo(StartupFilePath);
}
