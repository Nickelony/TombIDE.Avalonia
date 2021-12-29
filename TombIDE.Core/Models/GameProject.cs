using TombIDE.Core.Models.Bases;
using TombIDE.Core.Models.Enums;
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
	public GameVersion GameVersion { get; set; }

	/// <summary>
	/// Path of the file, which launches the game.
	/// </summary>
	public string LauncherFilePath { get; set; }

	/// <summary>
	/// Path of the directory where the project's script files are stored.
	/// </summary>
	public string ScriptDirectoryPath { get; set; }

	/// <summary>
	/// Path of the directory where all the project's newly created / imported maps are stored.
	/// </summary>
	public string MapsDirectoryPath { get; set; }

	/// <summary>
	/// A list of the project's maps.
	/// </summary>
	public List<MapProject> MapProjects { get; set; }

	/// <summary>
	/// A list of all languages the project supports.
	/// </summary>
	public List<GameLanguage> SupportedLanguages { get; set; }

	/// <summary>
	/// Index of the default language of the project.
	/// </summary>
	public int DefaultLanguageIndex { get; set; }

	/// <summary>
	/// Path of the directory from which the project should be reading TRNG plugins.
	/// </summary>
	public string? TRNGPluginsDirectoryPath { get; set; }

	/// <summary>
	/// A list of the project's installed TRNG plugins.
	/// </summary>
	public List<TRNGPlugin> InstalledTRNGPlugins
	{
		get
		{
			var result = new List<TRNGPlugin>();

			if (string.IsNullOrEmpty(TRNGPluginsDirectoryPath))
				return result;

			var rootPluginsDirectory = new DirectoryInfo(TRNGPluginsDirectoryPath);
			DirectoryInfo[] pluginSubdirectories = rootPluginsDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);

			foreach (DirectoryInfo directory in pluginSubdirectories)
			{
				var plugin = new TRNGPlugin(directory.FullName);

				if (plugin.DllFilePath == null)
					continue;

				result.Add(plugin);
			}

			return result;
		}
	}

	public override string Name { get; set; }

	public override string RootDirectoryPath
	{
		get => Path.GetDirectoryName(ProjectFilePath)
			?? throw new InvalidOperationException("The game project is corrupted.");
		set
		{
			string newDirectory = value.TrimEnd('\\');

			ProjectFilePath = ProjectFilePath.Replace(RootDirectoryPath, newDirectory);
			LauncherFilePath = LauncherFilePath.Replace(RootDirectoryPath, newDirectory);
			ScriptDirectoryPath = ScriptDirectoryPath.Replace(RootDirectoryPath, newDirectory);
			MapsDirectoryPath = MapsDirectoryPath.Replace(RootDirectoryPath, newDirectory);

			TRNGPluginsDirectoryPath = TRNGPluginsDirectoryPath?.Replace(RootDirectoryPath, newDirectory);
		}
	}

	/// <summary>
	/// The path where the project's engine executable (e.g tomb4.exe) file is stored.
	/// </summary>
	public string EngineDirectoryPath
	{
		get
		{
			string engineDirectoryPath = Path.Combine(RootDirectoryPath, "Engine");
			return Directory.Exists(engineDirectoryPath) ? engineDirectoryPath : RootDirectoryPath;
		}
	}

	public GameProject(string projectFilePath, string name, GameVersion gameVersion, string launcherFilePath,
		string scriptDirectoryPath, string mapsDirectoryPath, List<MapProject>? mapProjects = null,
		List<GameLanguage>? supportedLanguages = null, int defaultLanguageIndex = 0,
		string? trngPluginsDirectoryPath = null)
	{
		ProjectFilePath = projectFilePath;
		Name = name;
		GameVersion = gameVersion;
		LauncherFilePath = launcherFilePath;
		ScriptDirectoryPath = scriptDirectoryPath;
		MapsDirectoryPath = mapsDirectoryPath;
		MapProjects = mapProjects ?? new();
		SupportedLanguages = supportedLanguages ?? new();
		DefaultLanguageIndex = defaultLanguageIndex;
		TRNGPluginsDirectoryPath = trngPluginsDirectoryPath;
	}

	public void Save()
	{
		var trproj = new TrprojFile
		{
			FilePath = ProjectFilePath,
			Name = Name,
			GameVersion = GameVersion,
			LauncherFilePath = LauncherFilePath,
			ScriptDirectoryPath = ScriptDirectoryPath,
			MapsDirectoryPath = MapsDirectoryPath,
			DefaultLanguageIndex = DefaultLanguageIndex,
			TRNGPluginsDirectoryPath = TRNGPluginsDirectoryPath
		};

		foreach (MapProject map in MapProjects)
		{
			trproj.MapRecords.Add(new TrprojFile.MapRecord
			{
				Name = map.Name,
				RootDirectoryPath = map.RootDirectoryPath,
				StartupFileName = map.StartupFileName,
				OutputFileName = map.OutputFileName
			});
		}

		foreach (GameLanguage language in SupportedLanguages)
		{
			trproj.SupportedLanguages.Add(new TrprojFile.LanguageRecord
			{
				Name = language.Name,
				StringsFileName = language.StringsFileName,
				OutputFileName = language.OutputFileName
			});
		}

		TrprojWriter.WriteToFile(ProjectFilePath, trproj);
	}
}
