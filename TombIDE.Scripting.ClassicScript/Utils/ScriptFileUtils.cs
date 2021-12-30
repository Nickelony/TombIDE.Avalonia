namespace TombIDE.Scripting.ClassicScript.Utils;

public static class ScriptFileUtils
{
	public static bool IsLanguageFile(string filePath)
	{
		string[] lines = File.ReadAllLines(filePath);

		return Array.Exists(lines, line =>
			line.TrimStart().StartsWith("[Strings]", StringComparison.OrdinalIgnoreCase));
	}

	public static bool IsClassicScriptFile(string filePath)
	{
		string[] lines = File.ReadAllLines(filePath);

		return Array.Exists(lines, line =>
			line.TrimStart().StartsWith("[PSXExtensions]", StringComparison.OrdinalIgnoreCase)
			|| line.TrimStart().StartsWith("[PCExtensions]", StringComparison.OrdinalIgnoreCase)
			|| line.TrimStart().StartsWith("[Language]", StringComparison.OrdinalIgnoreCase)
			|| line.TrimStart().StartsWith("[Options]", StringComparison.OrdinalIgnoreCase)
			|| line.TrimStart().StartsWith("[Title]", StringComparison.OrdinalIgnoreCase)
			|| line.TrimStart().StartsWith("[Level]", StringComparison.OrdinalIgnoreCase));
	}
}
