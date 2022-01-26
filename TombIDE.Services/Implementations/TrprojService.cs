using TombIDE.Core.Models.Interfaces;
using TombIDE.Formats.Trproj;
using TombIDE.Services.Generic;
using MapRecordV1 = TombIDE.Formats.Trproj.V1.MapRecord;
using MapRecordV2 = TombIDE.Formats.Trproj.V2.MapRecord;
using TrprojV1 = TombIDE.Formats.Trproj.V1.Trproj;
using TrprojV2 = TombIDE.Formats.Trproj.V2.Trproj;

namespace TombIDE.Services;

public sealed class TrprojService : ITrprojService
{
	public TrprojV2 ConvertV1ToV2(TrprojV1 trprojV1)
	{
		string trprojDirectory = trprojV1.ProjectFile.DirectoryName!;
		string defaultTRNGPluginsDirectoryPath = Path.Combine(trprojDirectory, "Plugins");

		var result = new TrprojV2
		{
			ProjectFile = trprojV1.ProjectFile,
			ProjectName = trprojV1.Name,
			ScriptDirectoryPath = trprojV1.ScriptPath,
			MapsDirectoryPath = trprojV1.LevelsPath,
			TRNGPluginsDirectoryPath = defaultTRNGPluginsDirectoryPath
		};

		foreach (MapRecordV1 level in trprojV1.Levels)
		{
			result.MapRecords.Add(new MapRecordV2
			{
				Name = level.Name,
				RootDirectoryPath = level.FolderPath,

				StartupFileName = level.SpecificFile == Formats.Trproj.V1.Constants.LastModifiedFileKey ?
					string.Empty : level.SpecificFile
			});
		}

		return result;
	}

	public ITrproj CreateFromFile(string trprojFilePath) => throw new NotImplementedException();

	public ITrproj CreateFromGameProject(IGameProject game) => throw new NotImplementedException();

	public bool SaveTrprojToFile(ITrproj trproj, string filePath) => throw new NotImplementedException();

	#region Path translation

	public ITrproj MakePathsAbsolute(ITrproj trproj, string baseDirectory)
	{
		if (trproj is TrprojV1 v1)
			return MakePathsAbsolute(v1, baseDirectory);
		else if (trproj is TrprojV2 v2)
			return MakePathsAbsolute(v2, baseDirectory);
		else
			throw new InvalidDataException("Invalid trproj data structure.");
	}

	public ITrproj MakePathsRelative(ITrproj trproj, string baseDirectory)
	{
		if (trproj is TrprojV1 v1)
			return MakePathsRelative(v1, baseDirectory);
		else if (trproj is TrprojV2 v2)
			return MakePathsRelative(v2, baseDirectory);
		else
			throw new InvalidDataException("Invalid trproj data structure.");
	}

	private ITrproj MakePathsRelative(TrprojV1 trproj, string baseDirectory)
	{
		string directoryKey = Formats.Trproj.V1.Constants.ProjectDirectoryKey;

		var result = new TrprojV1
		{
			ProjectFile = trproj.ProjectFile,
			Name = trproj.Name,
			ScriptPath = trproj.ScriptPath.Replace(baseDirectory, directoryKey),
			LevelsPath = trproj.LevelsPath.Replace(baseDirectory, directoryKey)
		};

		IEnumerable<MapRecordV1> relativeLevels = trproj.Levels.Select(level =>
			new MapRecordV1
			{
				Name = level.Name,
				FolderPath = level.FolderPath.Replace(baseDirectory, directoryKey),
				SpecificFile = level.SpecificFile
			}
		);

		result.Levels.AddRange(relativeLevels);
		return result;
	}

	private ITrproj MakePathsRelative(TrprojV2 trproj, string baseDirectory)
	{
		var result = new TrprojV2
		{
			ProjectFile = trproj.ProjectFile,
			ProjectName = trproj.ProjectName,
			ScriptDirectoryPath = Path.GetRelativePath(baseDirectory, trproj.ScriptDirectoryPath),
			MapsDirectoryPath = Path.GetRelativePath(baseDirectory, trproj.MapsDirectoryPath),
			TRNGPluginsDirectoryPath = trproj.TRNGPluginsDirectoryPath != null ?
				Path.GetRelativePath(baseDirectory, trproj.TRNGPluginsDirectoryPath) : null
		};

		IEnumerable<MapRecordV2> relativeMaps = trproj.MapRecords.Select(map =>
			new MapRecordV2
			{
				Name = map.Name,
				RootDirectoryPath = Path.GetRelativePath(baseDirectory, map.RootDirectoryPath),
				StartupFileName = map.StartupFileName
			}
		);

		result.MapRecords.AddRange(relativeMaps);
		return result;
	}

	private ITrproj MakePathsAbsolute(TrprojV1 trproj, string baseDirectory)
	{
		string directoryKey = Formats.Trproj.V1.Constants.ProjectDirectoryKey;

		var result = new TrprojV1
		{
			ProjectFile = trproj.ProjectFile,
			Name = trproj.Name,
			ScriptPath = trproj.ScriptPath.Replace(directoryKey, baseDirectory),
			LevelsPath = trproj.LevelsPath.Replace(directoryKey, baseDirectory)
		};

		IEnumerable<MapRecordV1> absoluteLevels = trproj.Levels.Select(level =>
			new MapRecordV1
			{
				Name = level.Name,
				FolderPath = level.FolderPath.Replace(directoryKey, baseDirectory),
				SpecificFile = level.SpecificFile
			}
		);

		result.Levels.AddRange(absoluteLevels);
		return result;
	}

	private ITrproj MakePathsAbsolute(TrprojV2 trproj, string baseDirectory)
	{
		var result = new TrprojV2
		{
			ProjectFile = trproj.ProjectFile,
			ProjectName = trproj.ProjectName,
			ScriptDirectoryPath = Path.GetFullPath(trproj.ScriptDirectoryPath, baseDirectory),
			MapsDirectoryPath = Path.GetFullPath(trproj.MapsDirectoryPath, baseDirectory),
			TRNGPluginsDirectoryPath = trproj.TRNGPluginsDirectoryPath != null ?
				Path.GetFullPath(trproj.TRNGPluginsDirectoryPath, baseDirectory) : null
		};

		IEnumerable<MapRecordV2> absoluteMaps = trproj.MapRecords.Select(map =>
			new MapRecordV2
			{
				Name = map.Name,
				RootDirectoryPath = Path.GetFullPath(map.RootDirectoryPath, baseDirectory),
				StartupFileName = map.StartupFileName
			}
		);

		result.MapRecords.AddRange(absoluteMaps);
		return result;
	}

	#endregion Path translation
}
