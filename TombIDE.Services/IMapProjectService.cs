using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Services;

public interface IMapProjectService : IProjectService<IMapProject>
{
	// bool CreatePrj2File(IMapProject map, LevelSettings settings);
	FileInfo[] GetPrj2Files(IMapProject map);
	FileInfo? GetMostRecentlyModifiedPrj2File(IMapProject map);
}
