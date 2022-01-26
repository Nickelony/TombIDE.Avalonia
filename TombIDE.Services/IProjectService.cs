using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Services;

public interface IProjectService<T> where T : class, IProject
{
	bool IsValidProject(T project);
	void MoveRootDirectory(T project, string newRootPath);
}
