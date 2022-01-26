using System.Diagnostics;
using TombIDE.Core.Extensions;
using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Formats.Trproj;
using TombIDE.Formats.Trproj.V2;
using TrprojV1 = TombIDE.Formats.Trproj.V1.Trproj;
using TrprojV2 = TombIDE.Formats.Trproj.V2.Trproj;

namespace TombIDE.Services;

public class GameProjectService : IGameProjectService
{
	private readonly ITrprojService _trprojService;
	private readonly IMapProjectService _mapProjectService;

	public GameProjectService(ITrprojService trprojService, IMapProjectService mapProjectService)
	{
		_trprojService = trprojService;
		_mapProjectService = mapProjectService;
	}

	#region Factory

	public IGameProject CreateNewProject(string name, string projectFilePath,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null,
		List<IMapProject>? maps = null)
	{
		string rootDirectoryPath = Path.GetDirectoryName(projectFilePath)!;
		DirectoryInfo? trngPluginsDirectory = null;

		if (trngPluginsDirectoryPath != null)
			trngPluginsDirectory = new DirectoryInfo(trngPluginsDirectoryPath);

		return new GameProject(name,
			new FileInfo(projectFilePath),
			new DirectoryInfo(rootDirectoryPath),
			new DirectoryInfo(scriptDirectoryPath),
			new DirectoryInfo(mapsDirectoryPath),
			maps ?? new List<IMapProject>(),
			trngPluginsDirectory
		);
	}

	public IGameProject CreateFromFile(string projectFilePath)
	{
		ITrproj trproj = _trprojService.CreateFromFile(projectFilePath);
		return CreateFromTrproj(trproj);
	}

	private IGameProject CreateFromTrproj(ITrproj trproj)
	{
		if (trproj is TrprojV1 v1)
			return CreateFromTrproj(v1);
		else if (trproj is TrprojV2 latest)
			return CreateFromTrproj(latest);
		else
			throw new ArgumentException("Trproj data structure is invalid.", nameof(trproj));
	}

	private IGameProject CreateFromTrproj(TrprojV1 trproj)
	{
		TrprojV2 trprojV2 = _trprojService.ConvertV1ToV2(trproj);
		return CreateFromTrproj(trprojV2);
	}

	private IGameProject CreateFromTrproj(TrprojV2 trproj)
	{
		var mapProjects = new List<IMapProject>();

		foreach (MapRecord mapRecord in trproj.MapRecords)
		{
			var mapProject = new MapProject(
				mapRecord.Name,
				mapRecord.RootDirectoryPath,
				mapRecord.StartupFileName
			);

			if (_mapProjectService.IsValidProject(mapProject))
				mapProjects.Add(mapProject);
		}

		return CreateNewProject(
			trproj.ProjectName,
			trproj.ProjectFile.FullName,
			trproj.ScriptDirectoryPath,
			trproj.MapsDirectoryPath,
			trproj.TRNGPluginsDirectoryPath,
			mapProjects
		);
	}

	#endregion Factory

	public DirectoryInfo GetEngineDirectory(IGameProject game)
	{
		string engineDirectoryPath = Path.Combine(game.RootDirectory.FullName, "Engine");

		if (!Directory.Exists(engineDirectoryPath))
			return game.RootDirectory;

		var engineDirectory = new DirectoryInfo(engineDirectoryPath);
		return FindValidGameExecutable(engineDirectory) != null ? engineDirectory : game.RootDirectory;
	}

	public FileInfo GetGameLauncher(IGameProject game)
	{
		FileInfo? launcherFilePath = FindValidLauncher(game.RootDirectory);
		return launcherFilePath ?? FindGameExecutable(game);
	}

	public GameVersion DetectGameVersion(IGameProject game)
	{
		DirectoryInfo engineDirectory = GetEngineDirectory(game);
		FileInfo? validGameExecutable = FindValidGameExecutable(engineDirectory);

		if (validGameExecutable == null)
			return GameVersion.Unknown;

		GameVersion gameVersion = GameExecutableUtils.GetGameVersionFromExecutableFile(validGameExecutable);

		if (gameVersion == GameVersion.TR4)
		{
			bool isTRNG = HasTRNGDllFile(engineDirectory);
			return isTRNG ? GameVersion.TRNG : GameVersion.TR4;
		}

		return gameVersion;
	}

	public IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins(IGameProject game)
	{
		if (game.TRNGPluginsDirectory == null)
			yield break;

		DirectoryInfo[] pluginSubdirectories = game.TRNGPluginsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);

		foreach (DirectoryInfo directory in pluginSubdirectories)
		{
			var plugin = new TRNGPlugin(directory.FullName);

			if (plugin.IsValid)
				yield return plugin;
		}
	}

	public bool IsValidProject(IGameProject project) => throw new NotImplementedException();

	public void MoveRootDirectory(IGameProject project, string newRootPath)
	{
		string oldRootPath = project.RootDirectory.FullName;
		string? newRootPath = DirectoryUtils.RenameDirectoryEx(project.RootDirectory, newName);

		if (newRootPath != null)
		{
			project.RootDirectory = new DirectoryInfo(newRootPath);

			project.ProjectFile = new FileInfo(project.ProjectFile.FullName.Replace(oldRootPath, newRootPath));

			if (Project.ScriptDirectory.FullName.Contains(oldRootPath))
				Project.ScriptDirectory = new DirectoryInfo(Project.ScriptDirectory.FullName.Replace(oldRootPath, newRootPath));

			if (Project.MapsDirectory.FullName.Contains(oldRootPath))
				Project.MapsDirectory = new DirectoryInfo(Project.MapsDirectory.FullName.Replace(oldRootPath, newRootPath));

			if (Project.TRNGPluginsDirectory != null && Project.TRNGPluginsDirectory.FullName.Contains(oldRootPath))
				Project.TRNGPluginsDirectory = new DirectoryInfo(Project.TRNGPluginsDirectory.FullName.Replace(oldRootPath, newRootPath));

			foreach (IMapProject map in _maps.Select(map => map.Project))
			{
				if (map.RootDirectory.FullName.Contains(oldRootPath))
					map.RootDirectory = new DirectoryInfo(map.RootDirectory.FullName.Replace(oldRootPath, newRootPath));
			}
		}
	}

	public void SaveProject(IGameProject game)
	{
		ITrproj trproj = _trprojService.CreateFromGameProject(game);
		_trprojService.SaveTrprojToFile(trproj, trproj.ProjectFile.FullName);
	}

	private FileInfo? FindValidGameLauncher(IGameProject game)
	{
		DirectoryInfo searchDirectory = game.RootDirectory;
		FileInfo[] exeFiles = searchDirectory.GetFiles("*.exe", SearchOption.TopDirectoryOnly);
		return Array.Find(exeFiles, file =>
			FileVersionInfo.GetVersionInfo(file.FullName).OriginalFilename == "launch.exe");
	}

	private FileInfo? FindValidGameExecutable(DirectoryInfo searchDirectory)
	{
		FileInfo[] exeFiles = searchDirectory.GetFiles("*.exe", SearchOption.TopDirectoryOnly);

		return Array.Find(exeFiles, file => file.Name
			.BulkStringComparision(StringComparison.OrdinalIgnoreCase, Constants.ValidGameExecutableNames));
	}

	private bool HasTRNGDllFile(DirectoryInfo searchDirectory)
	{
		FileInfo[] dllFiles = searchDirectory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

		return Array.Exists(dllFiles, file => file.Name
			.Equals(Constants.TRNGDll, StringComparison.OrdinalIgnoreCase));
	}

	private IEnumerable<IMapProject> ScanForNewMapDirectories()
	{
		DirectoryInfo[] mapSubdirectories = Project.MapsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);

		foreach (DirectoryInfo directory in mapSubdirectories)
		{
			bool mapAlreadyOnList = _maps
				.Exists(map => map.Project.RootDirectory.FullName
					.Equals(directory.FullName, StringComparison.OrdinalIgnoreCase));

			if (mapAlreadyOnList)
				continue;

			var newMapProject = new MapProject(directory.Name, directory.FullName);
			var mapService = new MapContext(newMapProject);

			if (mapService.IsValid)
				yield return mapService;
		}
	}
}
