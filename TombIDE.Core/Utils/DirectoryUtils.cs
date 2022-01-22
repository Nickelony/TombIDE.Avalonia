using TombIDE.Core.Extensions;

namespace TombIDE.Core.Utils;

public static class DirectoryUtils
{
	public static string? RenameDirectoryEx(DirectoryInfo directory, string newName)
	{
		DirectoryInfo? parentDirectory = directory.Parent;

		if (parentDirectory == null)
			return null;

		string oldDirectoryPath = directory.FullName;
		string newDirectoryPath = Path.Combine(parentDirectory.FullName, newName);

		if (newName.IsEqualButCaseChanged(directory.Name))
		{
			// Fix for Windows not being able to update just the letter case in folder names
			string tempDirectory = $"{oldDirectoryPath}_{Guid.NewGuid}";

			Directory.Move(oldDirectoryPath, tempDirectory);
			Directory.Move(tempDirectory, newDirectoryPath);
		}
		else
		{
			Directory.Move(oldDirectoryPath, newDirectoryPath);
		}

		return newDirectoryPath;
	}
}
