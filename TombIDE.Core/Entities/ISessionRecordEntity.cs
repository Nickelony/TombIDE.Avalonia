namespace TombIDE.Core.Entities;

/// <summary>
/// Defines the base of a database record of a session.
/// </summary>
public interface ISessionRecordEntity
{
	/// <summary>
	/// The exact time the last session was closed.
	/// </summary>
	DateTime LastClosed { get; }
}
