namespace TombIDE.Core.Models.Interfaces;

/// <summary>
/// Defines a map project.
/// </summary>
public interface IMapProject : IProject
{
	/// <summary>
	/// The exact map file (.prj2 file) which is opened in Tomb Editor when selecting the map project on the list.
	/// <para>Set this property to <see langword="null" /> to use the most recently modified .prj2 file.</para>
	/// </summary>
	FileInfo? StartupFile { get; set; }
}
