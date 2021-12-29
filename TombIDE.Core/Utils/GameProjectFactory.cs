﻿using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils.Trproj;

namespace TombIDE.Core.Utils;

public static class GameProjectFactory
{
	private static GameLanguage DefaultInitialLanguage => new("English", "English.txt", "English.dat");

	public static GameProject CreateNew(string projectFilePath, string name,
		GameVersion gameVersion, string launcherFilePath, string scriptDirectoryPath,
		string mapsDirectoryPath, string? trngPluginsDirectoryPath = null)
	{
		var supportedLanguages = new List<GameLanguage> { DefaultInitialLanguage };

		return new GameProject(
			projectFilePath, name, gameVersion, launcherFilePath,
			scriptDirectoryPath, mapsDirectoryPath, null,
			supportedLanguages, 0, trngPluginsDirectoryPath
		);
	}

	public static GameProject? FromTrproj(ITrprojFile trproj)
	{
		if (trproj is Models.Legacy.Trproj.V1 v1)
			return FromTrproj(v1);
		else if (trproj is TrprojFile latest)
			return FromTrproj(latest);
		else
			return null;
	}

	public static GameProject FromTrproj(Models.Legacy.Trproj.V1 trproj)
	{
		TrprojFile trprojV2 = TrprojConverter.V1ToLatest(trproj);
		return FromTrproj(trprojV2);
	}

	public static GameProject FromTrproj(TrprojFile trproj)
	{
		var mapProjects = new List<MapProject>();

		foreach (TrprojFile.MapRecord mapRecord in trproj.MapRecords)
		{
			var mapProject = new MapProject(
				mapRecord.Name,
				mapRecord.RootDirectoryPath,
				mapRecord.OutputFileName,
				mapRecord.StartupFileName
			);

			if (mapProject.IsValid)
				mapProjects.Add(mapProject);
		}

		var supportedLanguages = new List<GameLanguage>();

		foreach (TrprojFile.LanguageRecord languageRecord in trproj.SupportedLanguages)
		{
			var gameLanguage = new GameLanguage(
				languageRecord.Name,
				languageRecord.StringsFileName,
				languageRecord.OutputFileName
			);

			if (gameLanguage.IsValid)
				supportedLanguages.Add(gameLanguage);
		}

		if (supportedLanguages.Count == 0)
			supportedLanguages.Add(DefaultInitialLanguage);

		return new GameProject(
			trproj.FilePath,
			trproj.Name,
			trproj.GameVersion,
			trproj.LauncherFilePath,
			trproj.ScriptDirectoryPath,
			trproj.MapsDirectoryPath,
			mapProjects,
			supportedLanguages,
			trproj.DefaultLanguageIndex,
			trproj.TRNGPluginsDirectoryPath
		);
	}
}
