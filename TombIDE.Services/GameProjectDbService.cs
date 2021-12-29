using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils;
using TombIDE.Core.Utils.Trproj;
using TombIDE.Services.Abstract;
using TombIDE.Services.Records;

namespace TombIDE.Services;

public sealed class GameProjectDbService : IGameProjectDbService
{
	public string GameProjectsXmlFilePath { get; set; }

	public GameProjectDbService(string gameProjectsXmlFilePath)
		=> GameProjectsXmlFilePath = gameProjectsXmlFilePath;

	public void AddProject(GameProject project)
	{
		var projectRecords = GetGameProjectRecords().ToList();

		bool projectAlreadyExists = projectRecords.Exists(record =>
			record.ProjectFilePath == project.ProjectFilePath);

		if (projectAlreadyExists)
			return;

		var newProjectRecord = new GameProjectRecord(project.ProjectFilePath, DateTime.Now);

		projectRecords.Add(newProjectRecord);
		XmlUtils.SaveXmlFile(GameProjectsXmlFilePath, projectRecords);
	}

	public IEnumerable<GameProject> GetGameProjects()
	{
		IEnumerable<GameProjectRecord> projectRecords = GetGameProjectRecords();

		foreach (GameProjectRecord record in projectRecords)
		{
			ITrprojFile? trproj = TrprojReader.ReadFile(record.ProjectFilePath);

			if (trproj == null)
				continue;

			GameProject? gameProject = GameProjectFactory.FromTrproj(trproj);

			if (gameProject != null && gameProject.IsValid)
				yield return gameProject;
		}
	}

	public IEnumerable<GameProjectRecord> GetGameProjectRecords()
		=> XmlUtils.ReadXmlFile<IEnumerable<GameProjectRecord>>(GameProjectsXmlFilePath);

	public GameProject? GetGameProject(Predicate<GameProject> match)
		=> GetGameProjects().ToList().Find(match);
}
