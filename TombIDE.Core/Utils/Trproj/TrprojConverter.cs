using TombIDE.Core.Models;

namespace TombIDE.Core.Utils.Trproj;

public static class TrprojConverter
{
	public static TrprojFile V1ToLatest(Models.Legacy.Trproj.V1 trprojV1)
	{
		var result = new TrprojFile
		{
			FilePath = trprojV1.FilePath,
			Name = trprojV1.Name,
			GameVersion = trprojV1.GameVersion,
			LauncherFilePath = trprojV1.LaunchFilePath,
			ScriptDirectoryPath = trprojV1.ScriptPath,
			MapsDirectoryPath = trprojV1.LevelsPath
		};

		foreach (Models.Legacy.Trproj.V1.ProjectLevel level in trprojV1.Levels)
		{
			result.MapRecords.Add(new TrprojFile.MapRecord
			{
				Name = level.Name,
				RootDirectoryPath = level.FolderPath,

				StartupFileName = level.SpecificFile == Models.Legacy.Trproj.V1.ProjectLevel.LastModifiedFileKey ?
					string.Empty : level.SpecificFile,

				OutputFileName = level.DataFileName
			});
		}

		return result;
	}
}
