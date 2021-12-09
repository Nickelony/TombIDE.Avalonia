using TombIDE.Core;
using TombIDE.Core.Models;
using TombIDE.Core.Models.Records;
using TombIDE.Core.Utils;
using TombIDE.Formats.Trproj;
using TombIDE.Services.Abstract;

namespace TombIDE.Services;

public class GameProjectListService : IGameProjectListService
{
	public void AddProject(IGameProject project)
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

	public IEnumerable<IGameProject> GetGameProjectList()
	{
		List<GameProjectRecord> projectRecords = XmlUtils.ReadXmlFile<List<GameProjectRecord>>("");

		foreach (GameProjectRecord record in projectRecords)
		{
			IGameProject? gameProject = TrprojReader.FromFile(record.ProjectFilePath);

			if (gameProject != null)
				yield return gameProject;
		}
	}

	public List<GameProjectRecord> GetGameProjectRecords()
		=> XmlUtils.ReadXmlFile<List<GameProjectRecord>>(DefaultPaths.ProjectRecordsFilePath);

	public IGameProject? GetGameProject(Predicate<IGameProject> match)
		=> GetGameProjectList().ToList().Find(match);
}
