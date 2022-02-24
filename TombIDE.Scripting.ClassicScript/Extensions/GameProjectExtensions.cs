using TombIDE.Scripting.ClassicScript.Utils;

namespace TombIDE.Scripting.ClassicScript.Extensions;

public static class GameProjectExtensions
{
	public static IEnumerable<string> GetAvailableLanguageFiles(this GameProject gameProject)
	{
		var scriptDirectory = new DirectoryInfo(gameProject.ScriptDirectoryPath);
		FileInfo[] scriptFiles = scriptDirectory.GetFiles("*.txt", SearchOption.AllDirectories);

		foreach (FileInfo file in scriptFiles)
		{
			if (ScriptFileUtils.IsLanguageFile(file.FullName))
				yield return file.FullName;
		}
	}
}
