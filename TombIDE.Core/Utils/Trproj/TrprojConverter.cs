using TombIDE.Core.Models;
using TrprojV1 = TombIDE.Core.Models.Legacy.Trproj.V1;

namespace TombIDE.Core.Utils.Trproj;

public static class TrprojConverter
{
	public static TrprojFile V1ToLatest(TrprojV1 trprojV1)
	{
		string trprojDirectory = Path.GetDirectoryName(trprojV1.FilePath)!;
		string defaultTRNGPluginsDirectoryPath = Path.Combine(trprojDirectory, "Plugins");

		var result = new TrprojFile
		{
			FilePath = trprojV1.FilePath,
			Name = trprojV1.Name,
			ScriptDirectoryPath = trprojV1.ScriptPath,
			MapsDirectoryPath = trprojV1.LevelsPath,
			TRNGPluginsDirectoryPath = defaultTRNGPluginsDirectoryPath
		};

		foreach (TrprojV1.ProjectLevel level in trprojV1.Levels)
		{
			result.MapRecords.Add(new TrprojFile.MapRecord
			{
				Name = level.Name,
				RootDirectoryPath = level.FolderPath,

				StartupFileName = level.SpecificFile == TrprojV1.ProjectLevel.LastModifiedFileKey ?
					string.Empty : level.SpecificFile
			});
		}

		return result;
	}
}
