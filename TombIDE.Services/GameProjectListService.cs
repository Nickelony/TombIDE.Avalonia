using TombIDE.Core;
using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Models.Records;
using TombIDE.Core.Utils;
using TombIDE.Core.Utils.Trproj;
using TombIDE.Services.Abstract;

namespace TombIDE.Services;

public class GameProjectListService : IGameProjectListService
{
	public void AddProject(GameProject project)
	{
		List<GameProjectRecord> projectRecords = GetGameProjectRecords();

		bool projectAlreadyExists = projectRecords.Exists(record =>
			record.ProjectFilePath == project.ProjectFilePath);

		if (projectAlreadyExists)
			return;

		var newProjectRecord = new GameProjectRecord(project.ProjectFilePath, DateTime.Now);

		projectRecords.Add(newProjectRecord);
		XmlUtils.SaveXmlFile(DefaultPaths.ProjectRecordsFilePath, projectRecords);
	}

	public IEnumerable<GameProject> GetGameProjectList()
	{
		List<GameProjectRecord> projectRecords = XmlUtils.ReadXmlFile<List<GameProjectRecord>>("");

		foreach (GameProjectRecord record in projectRecords)
		{
			ITrprojFile? trproj = TrprojReader.FromFile(record.ProjectFilePath);

			if (trproj == null)
				continue;

			GameProject? gameProject = GameProjectFactory.FromTrproj(trproj);

			if (gameProject != null)
				yield return gameProject;
		}
	}

	public List<GameProjectRecord> GetGameProjectRecords()
		=> XmlUtils.ReadXmlFile<List<GameProjectRecord>>(DefaultPaths.ProjectRecordsFilePath);

	public GameProject? GetGameProject(Predicate<GameProject> match)
		=> GetGameProjectList().ToList().Find(match);
}
