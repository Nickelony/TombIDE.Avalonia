namespace TombIDE.Services;

/// <summary>
/// Defines a service for processing .pak files.
/// </summary>
public interface IPakFileService
{
	/// <returns>
	/// Raw image bytes (pixel array) from a .pak file.
	/// </returns>
	byte[] ReadFile(string filePath);

	/// <summary>
	/// Writes bitmap data into a .pak file.
	/// </summary>
	void WriteToFile(string filePath, byte[] bitmapData);
}
