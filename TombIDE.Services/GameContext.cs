using System.Diagnostics;
using TombIDE.Core.Extensions;
using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils;
using TombIDE.Formats.Trproj;
using TombIDE.Formats.Trproj.V2;
using TombIDE.Services.Generic;

namespace TombIDE.Services;

public sealed class GameContext : IGameProjectService
{
	public IGameProject Project { get; }
	public bool IsValid => Project.RootDirectory.Exists;

	public FileInfo XmlDatabaseFile { get; }

	private readonly List<IMapProjectService> _maps = new();

	public GameContext(IGameProject project, IEnumerable<IMapProject> maps)
	{
		Project = project;
		maps.ToList().ForEach(map => _maps.Add(new MapContext(map)));
	}

	/// <summary>
	/// Creates and adds a new MapContext into the internal map database of the game.
	/// </summary>
	/// <returns><see langword="true" /> if the map is valid and has been successfully added, otherwise <see langword="false" />.</returns>
	public bool AddMap(IMapProject map)
	{
		var mapContext = new MapContext(map);

		if (!mapContext.IsValid)
		{
			mapContext.Dispose();
			return false;
		}

		_maps.Add(mapContext);
		return true;
	}

	public IEnumerable<IMapProjectService> GetAllMaps()
	{
		IEnumerable<IMapProjectService> validMaps = _maps.Where(map => map.IsValid);
		IEnumerable<IMapProjectService> invalidMaps = _maps.Where(map => !map.IsValid);

		_maps.Clear();
		_maps.AddRange(validMaps);

		foreach (IMapProjectService map in invalidMaps)
			map.Dispose();

		IEnumerable<IMapProjectService> newMaps = ScanForNewMapDirectories();
		_maps.AddRange(newMaps);

		return _maps;
	}

	private IEnumerable<IMapProjectService> ScanForNewMapDirectories()
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

	public IMapProjectService? FindMap(Predicate<IMapProjectService> predicate)
		=> GetAllMaps().ToList().Find(predicate);

	public GameVersion DetectGameVersion()
	{
		DirectoryInfo engineDirectory = GetEngineDirectory();
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

	public FileInfo? FindGameLauncher()
	{
		FileInfo? launcherFilePath = FindValidLauncher(Project.RootDirectory);

		if (launcherFilePath == null)
			return FindValidGameExecutable(GetEngineDirectory());

		return launcherFilePath;
	}

	public DirectoryInfo GetEngineDirectory()
	{
		string engineDirectoryPath = Path.Combine(Project.RootDirectory.FullName, "Engine");

		if (!Directory.Exists(engineDirectoryPath))
			return Project.RootDirectory;

		var engineDirectory = new DirectoryInfo(engineDirectoryPath);
		return FindValidGameExecutable(engineDirectory) != null ? engineDirectory : Project.RootDirectory;
	}

	public IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins()
	{
		if (Project.TRNGPluginsDirectory == null)
			yield break;

		DirectoryInfo[] pluginSubdirectories = Project.TRNGPluginsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);

		foreach (DirectoryInfo directory in pluginSubdirectories)
		{
			var plugin = new TRNGPlugin(directory.FullName);

			if (plugin.IsValid)
				yield return plugin;
		}
	}

	public void SaveProject()
	{
		Trproj trproj = TrprojFactory.FromGameProject(CurrentProject);
		TrprojWriter.WriteToFile(ProjectFilePath, trproj);
	}

	/// <summary>
	/// Finds a valid game launcher file (launch.exe file).
	/// </summary>
	/// <returns>Full launcher file path if one has been found, otherwise <see langword="null"/>.</returns>
	private FileInfo? FindValidLauncher(DirectoryInfo searchDirectory)
	{
		FileInfo[] exeFiles = searchDirectory.GetFiles("*.exe", SearchOption.TopDirectoryOnly);
		return Array.Find(exeFiles, file => FileVersionInfo.GetVersionInfo(file.FullName).OriginalFilename == "launch.exe");
	}

	/// <summary>
	/// Finds a valid game executable file (e.g. tomb4.exe).
	/// </summary>
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

	public void Rename(string newName, bool renameDirectory = true)
	{
		if (renameDirectory)
		{
			string oldRootPath = Project.RootDirectory.FullName;
			string? newRootPath = DirectoryUtils.RenameDirectoryEx(Project.RootDirectory, newName);

			if (newRootPath != null)
			{
				Project.RootDirectory = new DirectoryInfo(newRootPath);

				Project.ProjectFile = new FileInfo(Project.ProjectFile.FullName.Replace(oldRootPath, newRootPath));

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

		Project.Name = newName;
	}

	public IGameProject CreateNewProject(string projectFilePath, string name,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null)
			=> new GameProject(projectFilePath, name, scriptDirectoryPath, mapsDirectoryPath, trngPluginsDirectoryPath);
	public IGameProject CreateFromTrproj(ITrproj trproj) => throw new NotImplementedException();
	public IEnumerable<IGameProject> GetProjects() => throw new NotImplementedException();
	public IGameProject? FindProject(Predicate<IGameProject> predicate) => throw new NotImplementedException();
	public bool AddProject(IGameProject game) => throw new NotImplementedException();
	public DirectoryInfo GetEngineDirectory(IGameProject game) => throw new NotImplementedException();
	public FileInfo? FindGameLauncher(IGameProject game) => throw new NotImplementedException();
	public GameVersion GetGameVersion(IGameProject game) => throw new NotImplementedException();
	public IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins(IGameProject game) => throw new NotImplementedException();
	public void SaveProject(IGameProject game) => throw new NotImplementedException();
	public bool IsValidProject(IGameProject project) => throw new NotImplementedException();
	public void MoveRootDirectory(IGameProject project, string newRootPath) => throw new NotImplementedException();
}
