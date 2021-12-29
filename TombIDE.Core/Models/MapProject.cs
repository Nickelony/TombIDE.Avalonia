using TombIDE.Core.Models.Bases;
using TombIDE.Core.Utils;

namespace TombIDE.Core.Models;

public sealed class MapProject : ProjectBase
{
	public override string Name { get; set; }
	public override string RootDirectoryPath { get; set; }

	/// <summary>
	/// The exact map file (.prj2 file) which is opened in Tomb Editor when selecting the map project on the list.
	/// <para>Set this property to <c><see cref="string.Empty"/></c> to use the most recently modified .prj2 file.</para>
	/// </summary>
	public string StartupFileName { get; set; }

	/// <summary>
	/// Name of the output level file (.dat file).
	/// </summary>
	public string OutputFileName { get; set; }

	public MapProject(string name, string rootDirectoryPath, string outputFileName, string? startupFileName = null)
	{
		Name = name;
		RootDirectoryPath = rootDirectoryPath;
		OutputFileName = outputFileName;
		StartupFileName = startupFileName ?? string.Empty;
	}

	public string[] GetPrj2Files(bool includeBackupFiles)
	{
		string[] prj2Files = Directory.GetFiles(RootDirectoryPath, "*.prj2", SearchOption.TopDirectoryOnly);

		if (includeBackupFiles)
			return prj2Files;

		return Array.FindAll(prj2Files, file => !Prj2Utils.IsBackupFile(file));
	}

	public string? GetMostRecentlyModifiedPrj2FilePath()
	{
		var directory = new DirectoryInfo(RootDirectoryPath);

		IOrderedEnumerable<FileInfo> prj2Files = directory
			.GetFiles("*.prj2", SearchOption.TopDirectoryOnly)
			.OrderByDescending(file => file.LastWriteTime);

		return prj2Files.ToList().Find(file =>
			!Prj2Utils.IsBackupFile(file.FullName))?.FullName;
	}
}
