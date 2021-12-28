using System.Diagnostics.CodeAnalysis;
using TombIDE.Core.Extensions;
using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models.Bases;

public abstract class ProjectBase : INamed, IRooted, IEquatable<ProjectBase>
{
	public abstract string Name { get; set; }
	public abstract string RootDirectoryPath { get; set; }

	public bool Equals(ProjectBase? other) => other is not null && other.RootDirectoryPath == RootDirectoryPath;
	public override bool Equals(object? obj) => Equals(obj as ProjectBase);

	public override int GetHashCode() => RootDirectoryPath.GetHashCode();
	public static int GetHashCode([DisallowNull] ProjectBase obj) => obj.GetHashCode();

	public static bool operator ==(ProjectBase? a, ProjectBase? b) => a is null ? b is null : a.Equals(b);
	public static bool operator !=(ProjectBase? a, ProjectBase? b) => !(a == b);

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
