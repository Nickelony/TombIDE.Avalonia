using TombIDE.Core.Models.Bases;
using TombIDE.Core.Utils;
using TombIDE.Core.Utils.Trproj;

namespace TombIDE.Core.Models;

public sealed class GameProject : ProjectBase
{
	/// <summary>
	/// Path of the project file. (e.g .trproj file path)
	/// </summary>
	public string ProjectFilePath { get; set; }

	/// <summary>
	/// Game version the project is based on. (e.g. TR2, TR4, TRNG, TEN etc.)
	/// </summary>
	public GameVersion GameVersion { get; }

	/// <summary>
	/// Path of the directory where the project's script files are stored.
	/// </summary>
	public string ScriptDirectoryPath { get; set; }

	/// <summary>
	/// Path of the directory where the project's newly created maps will be stored.
	/// </summary>
	public string MapsDirectoryPath { get; set; }

	/// <summary>
	/// Path of the directory from which the project should be reading TRNG plugins.
	/// </summary>
	public string? TRNGPluginsDirectoryPath { get; set; }

	/// <summary>
	/// A list of the project's maps.
	/// </summary>
	public List<MapProject> MapProjects { get; set; }

	public override string RootDirectoryPath
	{
		get => Path.GetDirectoryName(ProjectFilePath)!;
		set
		{
			string oldDirectory = RootDirectoryPath;

			ScriptDirectoryPath = ScriptDirectoryPath.Replace(oldDirectory, value);
			MapsDirectoryPath = MapsDirectoryPath.Replace(oldDirectory, value);
			TRNGPluginsDirectoryPath = TRNGPluginsDirectoryPath?.Replace(oldDirectory, value);

			MapProjects.ForEach(map =>
				map.RootDirectoryPath = map.RootDirectoryPath.Replace(oldDirectory, value));

			ProjectFilePath = ProjectFilePath.Replace(oldDirectory, value);
		}
	}

	/// <summary>
	/// The path where the project's engine executable (e.g. tomb4.exe) file is stored.
	/// </summary>
	public string EngineDirectoryPath
	{
		get
		{
			string engineDirectoryPath = Path.Combine(RootDirectoryPath, "Engine");

			if (!Directory.Exists(engineDirectoryPath))
				return RootDirectoryPath;

			string? validGameExecutable = GameExecutableUtils.FindValidGameExecutable(engineDirectoryPath, GameVersion);
			return validGameExecutable != null ? engineDirectoryPath : RootDirectoryPath;
		}
	}

	public override bool IsValid
		=> GameVersion != GameVersion.Unknown
		&& Directory.Exists(RootDirectoryPath);

	public override string Name { get; set; }

	public GameProject(string projectFilePath, string name, GameVersion gameVersion,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null,
		List<MapProject>? mapProjects = null)
	{
		ProjectFilePath = projectFilePath;
		Name = name;
		GameVersion = gameVersion;
		ScriptDirectoryPath = scriptDirectoryPath;
		MapsDirectoryPath = mapsDirectoryPath;
		TRNGPluginsDirectoryPath = trngPluginsDirectoryPath;
		MapProjects = mapProjects ?? new();
	}

	/// <summary>
	/// Returns the path of the file, which launches the game.
	/// </summary>
	public string? GetLauncherFilePath()
	{
		string? launcherFilePath = GameExecutableUtils.FindValidLauncher(RootDirectoryPath);

		if (string.IsNullOrEmpty(launcherFilePath))
			return GameExecutableUtils.FindValidGameExecutable(EngineDirectoryPath, GameVersion);

		return launcherFilePath;
	}

	public IEnumerable<string> GetLanguageFiles()
	{
		var scriptDirectory = new DirectoryInfo(ScriptDirectoryPath);
		FileInfo[] scriptFiles = scriptDirectory.GetFiles("*.txt", SearchOption.AllDirectories);

		foreach (FileInfo file in scriptFiles)
		{
			if (ScriptFileUtils.IsLanguageFile(file.FullName))
				yield return file.FullName;
		}
	}

	public IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins()
	{
		if (string.IsNullOrEmpty(TRNGPluginsDirectoryPath))
			yield break;

		var pluginsDirectory = new DirectoryInfo(TRNGPluginsDirectoryPath);
		DirectoryInfo[] pluginSubdirectories = pluginsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);

		foreach (DirectoryInfo directory in pluginSubdirectories)
		{
			var plugin = new TRNGPlugin(directory.FullName);

			if (plugin.IsValid)
				yield return plugin;
		}
	}

	public void UpdateMapList()
	{
		MapProjects = MapProjects.Where(map => map.IsValid).ToList();

		var mapsDirectory = new DirectoryInfo(MapsDirectoryPath);
		DirectoryInfo[] mapSubdirectories = mapsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);

		foreach (DirectoryInfo directory in mapSubdirectories)
		{
			bool mapAlreadyOnList = MapProjects.Exists(map => map.RootDirectoryPath == directory.FullName);

			if (mapAlreadyOnList)
				continue;

			var newMapProject = new MapProject(directory.Name, directory.FullName);

			if (newMapProject.IsValid)
				MapProjects.Add(newMapProject);
		}
	}

	public void Save()
	{
		TrprojFile trproj = TrprojFactory.FromGameProject(this);
		TrprojWriter.WriteToFile(ProjectFilePath, trproj);
	}
}
