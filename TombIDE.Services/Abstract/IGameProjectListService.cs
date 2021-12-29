using TombIDE.Core.Models;

namespace TombIDE.Services.Abstract;

public interface IGameProjectListService
{
	IEnumerable<GameProject> GetGameProjectList();
	List<GameProjectRecord> GetGameProjectRecords();
	GameProject? GetGameProject(Predicate<GameProject> predicate);
	void AddProject(GameProject project);
}
