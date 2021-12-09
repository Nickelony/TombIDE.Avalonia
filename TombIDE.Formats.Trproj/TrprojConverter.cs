namespace TombIDE.Formats.Trproj;

public static class TrprojConverter
{
	public static V2.GameProjectV2 V1ToV2(V1.GameProjectV1 gameV1)
	{
		var result = new V2.GameProjectV2
		{
			ProjectFilePath = gameV1.ProjectFilePath,
			GameVersion = gameV1.GameVersion,
			Name = gameV1.Name,
			LauncherFilePath = gameV1.LauncherFilePath,
			ScriptDirectoryPath = gameV1.ScriptDirectoryPath,
			MapsDirectoryPath = gameV1.MapsDirectoryPath,
			DefaultLanguageIndex = gameV1.DefaultLanguageIndex,
			SupportedLanguages = gameV1.SupportedLanguages,
			InstalledTRNGPlugins = gameV1.InstalledTRNGPlugins
		};

		foreach (V1.MapProjectV1 mapV1 in gameV1.MapProjects)
		{
			result.MapProjects.Add(new V2.MapProjectV2
			{
				Name = mapV1.Name,
				RootDirectoryPath = mapV1.RootDirectoryPath,

				StartupFileName = mapV1.StartupFileName == Constants.V1_LastModifiedFileKey ?
					string.Empty : mapV1.StartupFileName,

				OutputFileName = mapV1.OutputFileName
			});
		}

		return result;
	}
}
