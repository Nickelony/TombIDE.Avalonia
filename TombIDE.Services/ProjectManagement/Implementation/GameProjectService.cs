using System.Diagnostics;
using TombIDE.Core;
using TombIDE.Core.Extensions;
using TombIDE.Core.Formats;
using TombIDE.Core.Models;
using TombIDE.Core.Models.Projects;
using TombIDE.Core.Utils;
using TrprojV1 = TombIDE.Core.Formats.Trproj.V1.Trproj;
using TrprojV2 = TombIDE.Core.Formats.Trproj.V2.Trproj;

namespace TombIDE.Services;

public class GameProjectService : IGameProjectService
{
	public const string DefaultProjectFileName = "project.trproj";

	#region Construction

	private readonly ITrprojService _trprojService;
	private readonly IMapProjectService _mapProjectService;
	private readonly ITRNGPluginService _trngPluginService;

	public GameProjectService(ITrprojService trprojService, IMapProjectService mapProjectService, ITRNGPluginService trngPluginService)
	{
		_trprojService = trprojService;
		_mapProjectService = mapProjectService;
		_trngPluginService = trngPluginService;
	}

	#endregion Construction

	#region Factory

	public IGameProject CreateNewProject(string name, string rootDirectoryPath,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null,
		IEnumerable<DirectoryInfo>? externalMapSubdirectories = null)
	{
		return new GameProject(name, rootDirectoryPath,
			scriptDirectoryPath, mapsDirectoryPath, trngPluginsDirectoryPath,
			externalMapSubdirectories);
	}

	public IGameProject CreateFromFile(string projectFilePath)
	{
		ITrproj trproj = _trprojService.CreateFromFile(projectFilePath);
		string rootDirectoryPath = Path.GetDirectoryName(projectFilePath)!;
		return CreateFromTrproj(trproj, rootDirectoryPath);
	}

	private IGameProject CreateFromTrproj(ITrproj trproj, string rootDirectoryPath)
	{
		if (trproj is TrprojV1 v1)
			return CreateFromTrproj(v1, rootDirectoryPath);
		else if (trproj is TrprojV2 v2)
			return CreateFromTrproj(v2, rootDirectoryPath);

		throw new ArgumentException(
			"Given trproj data structure is invalid.",
			nameof(trproj));
	}

	private IGameProject CreateFromTrproj(TrprojV1 trproj, string rootDirectoryPath)
	{
		TrprojV2 trprojV2 = _trprojService.ConvertV1ToV2(trproj);
		return CreateFromTrproj(trprojV2, rootDirectoryPath);
	}

	private IGameProject CreateFromTrproj(TrprojV2 trproj, string rootDirectoryPath)
	{
		IEnumerable<DirectoryInfo> externalMapSubdirectories = trproj.ExternalMapSubdirectoryPaths
			.Select(path =>
			{
				var directory = new DirectoryInfo(path);
				return directory.Exists && _mapProjectService.IsValidMapSubdirectory(directory) ?
					directory : null;
			})
			.Where(directory => directory != null)!;

		return CreateNewProject(
			trproj.ProjectName,
			rootDirectoryPath,
			trproj.ScriptDirectoryPath,
			trproj.MapsDirectoryPath,
			trproj.TRNGPluginsDirectoryPath,
			externalMapSubdirectories
		);
	}

	#endregion Factory

	#region Public methods

	public IEnumerable<IMapProject> GetAllMaps(IGameProject game)
	{
		foreach (DirectoryInfo directory in game.MapsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly))
		{
			IMapProject map = _mapProjectService.CreateFromDirectory(directory.FullName);

			if (_mapProjectService.IsValidProject(map))
				yield return map;
		}

		foreach (DirectoryInfo directory in game.ExternalMapSubdirectories)
		{
			IMapProject map = _mapProjectService.CreateFromDirectory(directory.FullName);

			if (_mapProjectService.IsValidProject(map))
				yield return map;
		}
	}

	public DirectoryInfo GetEngineDirectory(IGameProject game)
	{
		string engineDirectoryPath = Path.Combine(game.RootDirectory.FullName, "Engine");

		if (!Directory.Exists(engineDirectoryPath))
			return game.RootDirectory;

		var engineDirectory = new DirectoryInfo(engineDirectoryPath);

		return FindValidEngineExecutable(engineDirectory) != null ?
			engineDirectory : game.RootDirectory;
	}

	public FileInfo GetGameLauncher(IGameProject game)
	{
		FileInfo? launcherFilePath = FindValidGameLauncher(game.RootDirectory);

		if (launcherFilePath != null)
			return launcherFilePath;

		DirectoryInfo engineDirectory = GetEngineDirectory(game);
		FileInfo? validEngineExecutable = FindValidEngineExecutable(engineDirectory);

		if (validEngineExecutable != null)
			return validEngineExecutable;

		throw new ArgumentException(
			"Given game project does not have a valid game launcher file.",
			nameof(game));
	}

	public FileInfo GetEngineExecutable(IGameProject game)
	{
		DirectoryInfo engineDirectory = GetEngineDirectory(game);
		FileInfo? validEngineExecutable = FindValidEngineExecutable(engineDirectory);

		if (validEngineExecutable != null)
			return validEngineExecutable;

		throw new ArgumentException(
			"Given game project does not have a valid engine executable file.",
			nameof(game));
	}

	public GameVersion DetectGameVersion(IGameProject game)
	{
		DirectoryInfo engineDirectory = GetEngineDirectory(game);
		FileInfo? validGameExecutable = FindValidEngineExecutable(engineDirectory);

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

	public IEnumerable<DirectoryInfo> GetInstalledTRNGPluginDirectories(IGameProject game)
	{
		if (game.TRNGPluginsDirectory == null)
			return new List<DirectoryInfo>();

		DirectoryInfo[] pluginSubdirectories = game.TRNGPluginsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);
		return pluginSubdirectories.Where(directory => _trngPluginService.IsValidPluginDirectory(directory));
	}

	public bool IsValidProject(IGameProject project)
		=> project.RootDirectory.Exists;

	public void SaveProject(IGameProject project)
	{
		string filePath = Path.Combine(project.RootDirectory.FullName, DefaultProjectFileName);
		ITrproj trproj = _trprojService.CreateFromGameProject(project);

		_trprojService.SaveToFile(filePath, trproj);
	}

	#endregion Public methods

	#region Private methods

	private static FileInfo? FindValidGameLauncher(DirectoryInfo searchDirectory)
	{
		FileInfo[] exeFiles = searchDirectory.GetFiles("*.exe", SearchOption.TopDirectoryOnly);

		return Array.Find(exeFiles, file =>
			FileVersionInfo.GetVersionInfo(file.FullName).OriginalFilename == "launch.exe");
	}

	private static FileInfo? FindValidEngineExecutable(DirectoryInfo searchDirectory)
	{
		FileInfo[] exeFiles = searchDirectory.GetFiles("*.exe", SearchOption.TopDirectoryOnly);

		return Array.Find(exeFiles, file =>
			file.Name.BulkStringComparision(StringComparison.OrdinalIgnoreCase, Constants.ValidGameExeNames));
	}

	private static bool HasTRNGDllFile(DirectoryInfo searchDirectory)
	{
		FileInfo[] dllFiles = searchDirectory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

		return Array.Exists(dllFiles, file => file.Name
			.Equals(Constants.TRNGDllName, StringComparison.OrdinalIgnoreCase));
	}

	#endregion Private methods
}
