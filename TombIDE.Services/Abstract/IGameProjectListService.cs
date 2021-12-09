using TombIDE.Core.Models;
using TombIDE.Core.Models.Records;

namespace TombIDE.Services.Abstract;

public interface IGameProjectListService
{
	IEnumerable<IGameProject> GetGameProjectList();
	List<GameProjectRecord> GetGameProjectRecords();
	IGameProject? GetGameProject(Predicate<IGameProject> predicate);
	void AddProject(IGameProject project);
}
