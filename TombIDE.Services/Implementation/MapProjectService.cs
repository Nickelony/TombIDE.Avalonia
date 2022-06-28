using TombIDE.Core.Models;
using TombIDE.Core.Utils;

namespace TombIDE.Services.Implementations;

public sealed class MapProjectService : IMapProjectService
{
	public const string DefaultMapFileName = "map.trmap";

	private readonly ITrmapService _trmapService;

	public MapProjectService(ITrmapService trmapService)
		=> _trmapService = trmapService;

	public bool IsValidProject(IMapProject project)
		=> project.RootDirectory.Exists
		&& GetPrj2Files(project).Length > 0;

	public FileInfo[] GetAllPrj2Files(IMapProject map)
		=> map.RootDirectory.GetFiles("*.prj2", SearchOption.TopDirectoryOnly);

	public FileInfo[] GetPrj2Files(IMapProject map)
		=> Array.FindAll(GetAllPrj2Files(map), file => !Prj2FileUtils.IsBackupFile(file.FullName));

	public FileInfo[] GetBackupFiles(IMapProject map)
		=> Array.FindAll(GetAllPrj2Files(map), file => Prj2FileUtils.IsBackupFile(file.FullName));

	public FileInfo GetMostRecentlyModifiedPrj2File(IMapProject map)
	{
		IOrderedEnumerable<FileInfo> prj2Files = GetPrj2Files(map)
			.OrderByDescending(file => file.LastWriteTime);

		return prj2Files.FirstOrDefault() ??
			throw new ArgumentException(
				"Given map does not contain .prj2 files in its root directory.",
				nameof(map));
	}

	public bool IsValidMapSubdirectory(DirectoryInfo subdirectory) => throw new NotImplementedException();

	public IMapProject CreateNewMap(string name) => throw new NotImplementedException();
	public IMapProject CreateFromFile(string mapFilePath) => throw new NotImplementedException();
	public IGameProject CreateFromDirectory(string mapSubdirectoryPath) => throw new NotImplementedException();
	IEnumerable<FileInfo> IMapProjectService.GetPrj2Files(IMapProject map) => throw new NotImplementedException();
	public IEnumerable<FileInfo> GetBackupPrj2Files(IMapProject map) => throw new NotImplementedException();
	IEnumerable<FileInfo> IMapProjectService.GetAllPrj2Files(IMapProject map) => throw new NotImplementedException();

	public void SaveProject(IMapProject map) => throw new NotImplementedException();

	IMapProject IMapProjectService.CreateFromDirectory(string mapSubdirectoryPath)
	{
		throw new NotImplementedException();
	}
}
