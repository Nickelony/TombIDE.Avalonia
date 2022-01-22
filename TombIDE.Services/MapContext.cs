using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils;
using TombIDE.Services.Generic;

namespace TombIDE.Services;

public sealed class MapContext : IMapProjectService, IValidated
{
	public IMapProject Project { get; }

	public bool IsValid
		=> Project.RootDirectory.Exists
		&& GetPrj2Files().Length > 0;

	public MapContext(IMapProject project)
		=> Project = project;

	public FileInfo[] GetPrj2Files(bool includeBackupFiles = false)
	{
		FileInfo[] prj2Files = Project.RootDirectory.GetFiles("*.prj2", SearchOption.TopDirectoryOnly);

		if (includeBackupFiles)
			return prj2Files;

		return Array.FindAll(prj2Files, file => !Prj2Utils.IsBackupFile(file.FullName));
	}

	public FileInfo? GetMostRecentlyModifiedPrj2File()
	{
		IOrderedEnumerable<FileInfo> prj2Files = GetPrj2Files()
			.OrderByDescending(file => file.LastWriteTime);

		return prj2Files.ToList().Find(file =>
			!Prj2Utils.IsBackupFile(file.FullName));
	}

	public void Rename(string newName, bool renameDirectory = true)
	{
		if (renameDirectory)
		{
			string? newDirectoryPath = DirectoryUtils.RenameDirectoryEx(Project.RootDirectory, newName);

			if (newDirectoryPath != null)
				Project.RootDirectory = new DirectoryInfo(newDirectoryPath);
		}

		Project.Name = newName;
	}

	public void Dispose() => GC.SuppressFinalize(this);
}
