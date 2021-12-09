using System.Diagnostics.CodeAnalysis;
using TombIDE.Core.Extensions;

namespace TombIDE.Core.Models.Bases;

/// <summary>
/// Implements base functions of an accommodated entry.
/// </summary>
[Serializable]
public abstract class AccommodatedEntryBase : IAccommodatedEntry
{
	[XmlIgnore] public abstract string Name { get; set; }
	[XmlIgnore] public abstract string RootDirectoryPath { get; set; }

	public abstract void MakePathsRelative(string baseDirectory);
	public abstract void MakePathsAbsolute(string baseDirectory);

	public bool Equals(IAccommodatedEntry? other) => other is not null && other.RootDirectoryPath == RootDirectoryPath;
	public override bool Equals(object? obj) => Equals(obj as IAccommodatedEntry);

	public static int GetHashCode([DisallowNull] IAccommodatedEntry obj) => obj.GetHashCode();
	public override int GetHashCode() => RootDirectoryPath.GetHashCode();

	public static bool operator ==(AccommodatedEntryBase? a, AccommodatedEntryBase? b)
		=> a is null ? b is null : a.Equals(b);

	public static bool operator !=(AccommodatedEntryBase? a, AccommodatedEntryBase? b)
		=> !(a == b);

	public void Rename(string newName, bool renameDirectory = true)
	{
		if (renameDirectory)
			RenameRootDirectory(newName);

		Name = newName;
	}

	private void RenameRootDirectory(string newName)
	{
		string parentDirectory = Path.GetDirectoryName(RootDirectoryPath)!;
		string newDirectory = Path.Combine(parentDirectory, newName);

		if (newName.IsEqualButCaseChanged(Name))
		{
			// Fix for Windows not being able to update just the letter case in folder names
			string tempDirectory = $"{RootDirectoryPath}_{Guid.NewGuid}";

			Directory.Move(RootDirectoryPath, tempDirectory);
			Directory.Move(tempDirectory, newDirectory);
		}
		else
		{
			Directory.Move(RootDirectoryPath, newDirectory);
		}

		RootDirectoryPath = newDirectory;
	}
}
