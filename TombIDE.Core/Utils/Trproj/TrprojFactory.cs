using TombIDE.Core.Models;

namespace TombIDE.Core.Utils.Trproj;

public static class TrprojFactory
{
	public static TrprojFile FromGameProject(GameProject gameProject)
	{
		var trproj = new TrprojFile
		{
			FilePath = gameProject.ProjectFilePath,
			Name = gameProject.Name,
			GameVersion = gameProject.GameVersion,
			LauncherFilePath = gameProject.LauncherFilePath,
			ScriptDirectoryPath = gameProject.ScriptDirectoryPath,
			MapsDirectoryPath = gameProject.MapsDirectoryPath,
			DefaultLanguageIndex = gameProject.DefaultLanguageIndex,
			TRNGPluginsDirectoryPath = gameProject.TRNGPluginsDirectoryPath
		};

		foreach (MapProject map in gameProject.MapProjects)
		{
			trproj.MapRecords.Add(new TrprojFile.MapRecord
			{
				Name = map.Name,
				RootDirectoryPath = map.RootDirectoryPath,
				StartupFileName = map.StartupFileName,
				OutputFileName = map.OutputFileName
			});
		}

		foreach (GameLanguage language in gameProject.SupportedLanguages)
		{
			trproj.SupportedLanguages.Add(new TrprojFile.LanguageRecord
			{
				Name = language.Name,
				StringsFileName = language.StringsFileName,
				OutputFileName = language.OutputFileName
			});
		}

		return trproj;
	}
}
