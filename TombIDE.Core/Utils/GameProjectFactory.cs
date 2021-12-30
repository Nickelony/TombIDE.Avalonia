using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils.Trproj;

namespace TombIDE.Core.Utils;

public static class GameProjectFactory
{
	public static GameProject CreateNew(string projectFilePath, string name, string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null)
		=> new(projectFilePath, name, scriptDirectoryPath, mapsDirectoryPath, trngPluginsDirectoryPath);

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
				mapRecord.StartupFileName
			);

			if (mapProject.IsValid)
				mapProjects.Add(mapProject);
		}

		return new GameProject(
			trproj.FilePath,
			trproj.Name,
			trproj.ScriptDirectoryPath,
			trproj.MapsDirectoryPath,
			trproj.TRNGPluginsDirectoryPath,
			mapProjects
		);
	}
}
