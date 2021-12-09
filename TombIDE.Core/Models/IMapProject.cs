namespace TombIDE.Core.Models;

/// <summary>
/// Defines a map project.
/// </summary>
public interface IMapProject : IAccommodatedEntry
{
	/// <summary>
	/// The exact map file (.prj2 file) which is opened in Tomb Editor when selecting the map project on the list.
	/// <para>Set this property to <c><see cref="string.Empty"/></c> to use the most recently modified .prj2 file.</para>
	/// </summary>
	string StartupFileName { get; set; }

	/// <summary>
	/// Name of the output level file (.dat file).
	/// </summary>
	string OutputFileName { get; set; }
}
