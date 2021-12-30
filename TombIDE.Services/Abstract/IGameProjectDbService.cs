﻿using TombIDE.Core.Models;
using TombIDE.Services.Records;

namespace TombIDE.Services.Abstract;

public interface IGameProjectDbService
{
	string GameProjectsXmlFilePath { get; set; }

	IEnumerable<GameProject> GetGameProjects();
	IEnumerable<GameProjectRecord> GetGameProjectRecords();
	GameProject? GetGameProject(Predicate<GameProject> predicate);
	void AddProject(GameProject project);
}