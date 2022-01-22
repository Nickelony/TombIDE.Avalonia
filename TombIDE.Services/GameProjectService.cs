using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Formats.Trproj;
using TombIDE.Services.Generic;
using TrprojV1 = TombIDE.Formats.Trproj.V1.Trproj;
using TrprojV2 = TombIDE.Formats.Trproj.V2.Trproj;

namespace TombIDE.Services;

public class GameProjectService : IGameProjectService
{
	public IGameProject CreateFromTrproj(ITrproj trproj)
	{
		if (trproj is TrprojV1 v1)
			return CreateFromTrproj(v1);
		else if (trproj is TrprojV2 latest)
			return CreateFromTrproj(latest);
		else
			throw new ArgumentException("Trproj data structure is invalid.", nameof(trproj));
	}

	public GameProject CreateFromTrproj(TrprojV1 trproj)
	{
		TrprojV2 trprojV2 = TrprojConverter.V1ToLatest(trproj);
		return CreateFromTrproj(trprojV2);
	}

	public GameProject CreateFromTrproj(TrprojV2 trproj)
	{
		var mapProjects = new List<IMapProject>();

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

	public IGameProject CreateNewProject(string projectFilePath, string name, string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null) => throw new NotImplementedException();
	public FileInfo? FindGameExecutable(IGameProject game) => throw new NotImplementedException();
	public FileInfo? FindGameLauncher(IGameProject game) => throw new NotImplementedException();
	public DirectoryInfo GetEngineDirectory(IGameProject game) => throw new NotImplementedException();
	public GameVersion GetGameVersion(IGameProject game) => throw new NotImplementedException();
	public IEnumerable<TRNGPlugin> GetInstalledTRNGPlugins(IGameProject game) => throw new NotImplementedException();
	public bool IsValidProject(IGameProject project) => throw new NotImplementedException();
	public void MoveRootDirectory(IGameProject project, string newRootPath) => throw new NotImplementedException();
	public void SaveProject(IGameProject game) => throw new NotImplementedException();

	private V2.TrprojFile ConvertV1ToLatest(V1.TrprojFile trprojV1)
	{
		string trprojDirectory = Path.GetDirectoryName(trprojV1.FilePath)!;
		string defaultTRNGPluginsDirectoryPath = Path.Combine(trprojDirectory, "Plugins");

		var result = new V2.TrprojFile
		{
			FilePath = trprojV1.FilePath,
			Name = trprojV1.Name,
			ScriptDirectoryPath = trprojV1.ScriptPath,
			MapsDirectoryPath = trprojV1.LevelsPath,
			TRNGPluginsDirectoryPath = defaultTRNGPluginsDirectoryPath
		};

		foreach (V1.MapRecord level in trprojV1.Levels)
		{
			result.MapRecords.Add(new V2.MapRecord
			{
				Name = level.Name,
				RootDirectoryPath = level.FolderPath,

				StartupFileName = level.SpecificFile == V1.Constants.LastModifiedFileKey ?
					string.Empty : level.SpecificFile
			});
		}

		return result;
	}
}
