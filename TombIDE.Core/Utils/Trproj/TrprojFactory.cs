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
			ScriptDirectoryPath = gameProject.ScriptDirectoryPath,
			MapsDirectoryPath = gameProject.MapsDirectoryPath,
			TRNGPluginsDirectoryPath = gameProject.TRNGPluginsDirectoryPath
		};

		foreach (MapProject map in gameProject.MapProjects)
		{
			trproj.MapRecords.Add(new TrprojFile.MapRecord
			{
				Name = map.Name,
				RootDirectoryPath = map.RootDirectoryPath,
				StartupFileName = map.StartupFileName
			});
		}

		return trproj;
	}
}
