namespace TombIDE.Core.Models;

/// <summary>
/// Defines a map project.
/// </summary>
public interface IMapProject : IProject
{
	/// <summary>
	/// The exact map file (.prj2 file) which is opened in Tomb Editor when selecting the map on the list.
	/// <para>Set this property to <see langword="null" /> to use the most recently modified .prj2 file.</para>
	/// </summary>
	FileInfo? StartupFile { get; }

	event EventHandler? StartupFileChanged;

	/// <param name="fileName">Just the file name (not the path), e.g "My Map.prj2".</param>
	void SetStartupFile(string fileName);
}
