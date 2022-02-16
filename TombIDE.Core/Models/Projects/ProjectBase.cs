using TombIDE.Core.Utils;

namespace TombIDE.Core.Models.Projects;

public abstract class ProjectBase : IProject
{
	public string Name { get; private set; }
	public DirectoryInfo RootDirectory { get; private set; }

	public ProjectBase(string name, string rootDirectoryPath)
	{
		Name = name;
		RootDirectory = new DirectoryInfo(rootDirectoryPath);
	}

	public event EventHandler? NameChanged;
	public event EventHandler? RootDirectoryChanged;

	public void Dispose() => GC.SuppressFinalize(this);

	public virtual void Rename(string newName, bool renameRootDirectory = false)
	{
		if (renameRootDirectory)
		{
			string newRootPath = Path.Combine(RootDirectory.Parent!.FullName, newName);
			MoveRootDirectory(newRootPath);
		}

		Name = newName;
		NameChanged?.Invoke(this, EventArgs.Empty);
	}

	public virtual void MoveRootDirectory(string newRootPath)
	{
		DirectoryUtils.MoveDirectoryEx(RootDirectory, newRootPath);
		RootDirectory = new DirectoryInfo(newRootPath);
		RootDirectoryChanged?.Invoke(this, EventArgs.Empty);
	}
}
