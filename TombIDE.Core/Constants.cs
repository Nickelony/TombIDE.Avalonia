namespace TombIDE.Core;

public static class Constants
{
	public const string TRNGDllName = "Tomb_NextGeneration.dll";

	public const string ValidTR1ExeName = "tombati.exe";
	public const string ValidTR2ExeName = "Tomb2.exe";
	public const string ValidTR3ExeName = "tomb3.exe";
	public const string ValidTR4ExeName = "tomb4.exe";
	public const string ValidTR5ExeName = "PCTomb5.exe";
	public const string ValidTENExeName = "tombengine.exe";

	public static readonly string[] ValidGameExeNames = new[]
	{
		ValidTR1ExeName,
		ValidTR2ExeName,
		ValidTR3ExeName,
		ValidTR4ExeName,
		ValidTR5ExeName,
		ValidTENExeName
	};
}
