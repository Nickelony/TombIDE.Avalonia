using TombIDE.Core.Extensions;

namespace TombIDE.Core.Utils;

public static class DirectoryUtils
{
	/// <summary>
	/// Allows moving a directory, even if <c>destPath</c> is the same, but has different letter cases.
	/// </summary>
	/// <exception cref="ArgumentException" />
	public static void MoveDirectoryEx(DirectoryInfo directory, string destPath)
	{
		DirectoryInfo? parentDirectory = directory.Parent;

		if (parentDirectory is null)
			throw new ArgumentException("Given directory has no parent.");

		string newDirectoryName = Path.GetFileName(destPath);

		if (newDirectoryName.IsEqualButCaseChanged(directory.Name))
		{
			// Fix for Windows not being able to update just the letter case in folder names
			string tempDirectory = $"{directory.FullName}_{Guid.NewGuid}";

			Directory.Move(directory.FullName, tempDirectory);
			Directory.Move(tempDirectory, destPath);
		}
		else
		{
			Directory.Move(directory.FullName, destPath);
		}
	}
}
