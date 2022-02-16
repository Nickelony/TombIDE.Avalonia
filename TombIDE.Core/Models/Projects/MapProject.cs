namespace TombIDE.Core.Models.Projects;

public sealed class MapProject : ProjectBase, IMapProject
{
	public FileInfo? StartupFile { get; private set; }

	public MapProject(string name, string rootDirectoryPath, string? startupFileName = null)
		: base(name, rootDirectoryPath)
	{
		if (startupFileName is not null)
			SetStartupFile(startupFileName);
	}

	public event EventHandler? StartupFileChanged;

	public override void MoveRootDirectory(string newRootPath)
	{
		string oldRootPath = RootDirectory.FullName;

		base.MoveRootDirectory(newRootPath);

		if (StartupFile is not null && StartupFile.FullName.Contains(oldRootPath))
			StartupFile = new FileInfo(StartupFile.FullName.Replace(oldRootPath, newRootPath));
	}

	public void SetStartupFile(string fileName)
	{
		string filePath = Path.Combine(RootDirectory.FullName, fileName);
		StartupFile = new FileInfo(filePath);

		StartupFileChanged?.Invoke(this, EventArgs.Empty);
	}
}
