namespace TombIDE.Core.Entities;

/// <summary>
/// XML database record of a trproj session.
/// </summary>
public sealed record TrprojSessionRecordEntity(string ProjectFilePath, DateTime LastClosed) : ISessionRecordEntity
{
	/// <summary>
	/// Path of the .trproj file.
	/// </summary>
	[XmlAttribute] public string ProjectFilePath { get; init; } = ProjectFilePath;

	[XmlAttribute] public DateTime LastClosed { get; init; } = LastClosed;
}
