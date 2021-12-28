using TombIDE.Core.Models;
using TombIDE.Core.Models.Records;

namespace TombIDE.Services.Abstract;

public interface IGameProjectListService
{
	IEnumerable<GameProject> GetGameProjectList();
	List<GameProjectRecord> GetGameProjectRecords();
	GameProject? GetGameProject(Predicate<GameProject> predicate);
	void AddProject(GameProject project);
}
