using TombIDE.Core.Models;
using TombIDE.Services;

namespace TombIDE.Core.Utils;

public static class GameExecutableUtils
{
	public static string GetGameExecutableFileName(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.TR1 => Constants.TR1Executable,
		GameVersion.TR2 => Constants.TR2Executable,
		GameVersion.TR3 => Constants.TR3Executable,
		GameVersion.TR4 or GameVersion.TRNG => Constants.TR4Executable,
		GameVersion.TR5 => Constants.TR5Executable,
		GameVersion.TEN => Constants.TENExecutable,
		_ => throw new ArgumentException("Invalid game version.")
	};

	public static GameVersion GetGameVersionFromExecutableFile(FileInfo executableFile)
	{
		string fileName = executableFile.Name;

		if (fileName.Equals(Constants.TR1Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR1;
		else if (fileName.Equals(Constants.TR2Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR2;
		else if (fileName.Equals(Constants.TR3Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR3;
		else if (fileName.Equals(Constants.TR4Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR4;
		else if (fileName.Equals(Constants.TR5Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR5;
		else if (fileName.Equals(Constants.TENExecutable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TEN;

		return GameVersion.Unknown;
	}
}
