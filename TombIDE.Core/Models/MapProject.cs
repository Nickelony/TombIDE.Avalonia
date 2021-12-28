using TombIDE.Core.Models.Bases;

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
}
