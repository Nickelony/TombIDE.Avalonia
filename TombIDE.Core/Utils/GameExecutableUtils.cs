namespace TombIDE.Core.Utils;

public static class GameExecutableUtils
{
	public static string GetGameExecutableFileName(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.TR1 => Constants.ValidTR1ExeName,
		GameVersion.TR2 => Constants.ValidTR2ExeName,
		GameVersion.TR3 => Constants.ValidTR3ExeName,
		GameVersion.TR4 or GameVersion.TRNG => Constants.ValidTR4ExeName,
		GameVersion.TR5 => Constants.ValidTR5ExeName,
		GameVersion.TEN => Constants.ValidTENExeName,
		_ => throw new ArgumentException("Invalid game version.")
	};

	public static GameVersion GetGameVersionFromExecutableFile(FileInfo executableFile)
	{
		string fileName = executableFile.Name;

		return fileName switch
		{
			Constants.ValidTR1ExeName when fileName.Equals(Constants.ValidTR1ExeName, StringComparison.OrdinalIgnoreCase) => GameVersion.TR1,
			Constants.ValidTR2ExeName when fileName.Equals(Constants.ValidTR2ExeName, StringComparison.OrdinalIgnoreCase) => GameVersion.TR2,
			Constants.ValidTR3ExeName when fileName.Equals(Constants.ValidTR3ExeName, StringComparison.OrdinalIgnoreCase) => GameVersion.TR3,
			Constants.ValidTR4ExeName when fileName.Equals(Constants.ValidTR4ExeName, StringComparison.OrdinalIgnoreCase) => GameVersion.TR4,
			Constants.ValidTR5ExeName when fileName.Equals(Constants.ValidTR5ExeName, StringComparison.OrdinalIgnoreCase) => GameVersion.TR5,
			Constants.ValidTENExeName when fileName.Equals(Constants.ValidTENExeName, StringComparison.OrdinalIgnoreCase) => GameVersion.TEN,
			_ => GameVersion.Unknown
		};
	}
}
