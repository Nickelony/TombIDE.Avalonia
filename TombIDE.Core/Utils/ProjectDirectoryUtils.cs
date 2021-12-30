using System.Diagnostics;
using TombIDE.Core.Extensions;
using TombIDE.Core.Models;

namespace TombIDE.Core.Utils;

public static class ProjectDirectoryUtils
{
	public const string TR1Executable = "tombati.exe";
	public const string TR2Executable = "Tomb2.exe";
	public const string TR3Executable = "tomb3.exe";
	public const string TR4Executable = "tomb4.exe";
	public const string TR5Executable = "PCTomb5.exe";
	public const string TENExecutable = "tombengine.exe";

	public const string TRNGDllFile = "Tomb_NextGeneration.dll";

	public static readonly string[] ValidGameExecutableNames = new string[]
	{
		TR1Executable, TR2Executable, TR3Executable, TR4Executable, TR5Executable
	};

	/// <summary>
	/// Finds a valid game launcher file (launch.exe file).
	/// </summary>
	/// <returns>Full launcher file path if one has been found, otherwise <see langword="null"/>.</returns>
	public static string? FindValidLauncher(string projectRootDirectoryPath)
	{
		string[] exeFiles = Directory.GetFiles(projectRootDirectoryPath, "*.exe", SearchOption.TopDirectoryOnly);
		return Array.Find(exeFiles, file => FileVersionInfo.GetVersionInfo(file).OriginalFilename == "launch.exe");
	}

	/// <summary>
	/// Finds a valid game executable file (e.g. tomb4.exe).
	/// </summary>
	/// <returns>Full launcher file path if one has been found, otherwise <see langword="null"/>.</returns>
	public static string? FindValidGameExecutable(string engineDirectoryPath, GameVersion gameVersion)
	{
		string[] exeFiles = Directory.GetFiles(engineDirectoryPath, "*.exe", SearchOption.TopDirectoryOnly);

		string targetExecutableName = GetGameExecutableFileName(gameVersion);

		return Array.Find(exeFiles, file => Path.GetFileName(file)
			.Equals(targetExecutableName, StringComparison.OrdinalIgnoreCase));
	}

	public static string[] FindAllValidGameExecutables(string engineDirectoryPath)
	{
		string[] exeFiles = Directory.GetFiles(engineDirectoryPath, "*.exe", SearchOption.TopDirectoryOnly);

		return Array.FindAll(exeFiles, file => Path.GetFileName(file)
			.BulkStringComparision(StringComparison.OrdinalIgnoreCase, ValidGameExecutableNames));
	}

	public static bool HasTRNGDllFile(string engineDirectoryPath)
	{
		string[] dllFiles = Directory.GetFiles(engineDirectoryPath, "*.dll", SearchOption.TopDirectoryOnly);

		return Array.Exists(dllFiles, file => Path.GetFileName(file)
			.Equals(TRNGDllFile, StringComparison.OrdinalIgnoreCase));
	}

	public static string GetGameExecutableFileName(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.TR1 => TR1Executable,
		GameVersion.TR2 => TR2Executable,
		GameVersion.TR3 => TR3Executable,
		GameVersion.TR4 => TR4Executable,
		GameVersion.TR5 => TR5Executable,
		GameVersion.TRNG => TR4Executable,
		GameVersion.TEN => TENExecutable,
		_ => throw new ArgumentException("Invalid game version.")
	};

	public static GameVersion GetGameVersionFromExecutableFile(string exeFilePath)
	{
		string fileName = Path.GetFileName(exeFilePath);

		if (fileName.Equals(TR1Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR1;
		else if (fileName.Equals(TR2Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR2;
		else if (fileName.Equals(TR3Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR3;
		else if (fileName.Equals(TR4Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR4;
		else if (fileName.Equals(TR5Executable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TR5;
		else if (fileName.Equals(TENExecutable, StringComparison.OrdinalIgnoreCase))
			return GameVersion.TEN;

		return GameVersion.Unknown;
	};
}
