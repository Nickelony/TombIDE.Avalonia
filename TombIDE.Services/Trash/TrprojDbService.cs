using TombIDE.Core.Models;
using TombIDE.Core.Models.Interfaces;
using TombIDE.Core.Utils;
using TombIDE.Core.Utils.Trproj;
using TombIDE.Services.Abstract;
using TombIDE.Services.Records;

namespace TombIDE.Services;

public sealed class TrprojDbService : ITrprojService
{
	public string DbFilePath { get; set; }

	public TrprojDbService(string dbFilePath)
		=> DbFilePath = dbFilePath;

	public void AddProject(GameProjectRecord project)
	{
		var projectRecords = GetGameProjectRecords().ToList();

		bool projectAlreadyExists = projectRecords.Exists(record =>
			record.ProjectFilePath == project.ProjectFilePath);

		if (projectAlreadyExists)
			return;

		var newProjectRecord = new GameProjectRecord(project.ProjectFilePath, DateTime.Now);

		projectRecords.Add(newProjectRecord);
		XmlUtils.SaveXmlFile(DbFilePath, projectRecords);
	}

	public IEnumerable<GameProjectRecord> GetGameProjects()
	{
		IEnumerable<TrprojSessionRecord> projectRecords = GetGameProjectRecords();

		foreach (TrprojSessionRecord record in projectRecords)
		{
			ITrprojFile? trproj = TrprojReader.ReadFile(record.ProjectFilePath);

			if (trproj == null)
				continue;

			GameProjectRecord? gameProject = GameProjectFactory.FromTrproj(trproj);

			if (gameProject != null && gameProject.IsValid)
				yield return gameProject;
		}
	}

	public IEnumerable<TrprojSessionRecord> GetGameProjectRecords()
		=> XmlUtils.ReadXmlFile<IEnumerable<TrprojSessionRecord>>(DbFilePath);

	public GameProjectRecord? GetGameProject(Predicate<GameProjectRecord> match)
		=> GetGameProjects().ToList().Find(match);
}
