namespace TombIDE.Core.Utils;

public static class Prj2FileUtils
{
	public static bool IsBackupFile(string prj2FilePath)
	{
		string fileName = Path.GetFileName(prj2FilePath);

		if (fileName.Length < 9)
			return false;

		// 01-01-0001 || 0001-01-01
		if (DateTime.TryParse(fileName[..9], out _))
			return true;

		if (fileName.Length < 7)
			return false;

		// 01-01-01
		if (DateTime.TryParse(fileName[..7], out _))
			return true;

		return false;
	}
}
