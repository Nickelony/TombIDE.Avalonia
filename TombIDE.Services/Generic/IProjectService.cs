using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Services.Generic;

public interface IProjectService<T> where T : class, IProject
{
	bool IsValidProject(T project);
	void MoveRootDirectory(T project, string newRootPath);
}
