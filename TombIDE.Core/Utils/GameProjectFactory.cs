using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils.Trproj;

namespace TombIDE.Core.Utils;

public static class GameProjectFactory
{
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

		foreach (TrprojFile.MapRecord map in trproj.MapRecords)
		{
			mapProjects.Add(new MapProject(
				map.Name,
				map.RootDirectoryPath,
				map.OutputFileName,
				map.StartupFileName
			));
		}

		var supportedLanguages = new List<GameLanguage>();

		foreach (TrprojFile.LanguageRecord language in trproj.SupportedLanguages)
		{
			supportedLanguages.Add(new GameLanguage(
				language.Name,
				language.StringsFileName,
				language.OutputFileName
			));
		}

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
