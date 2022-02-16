using TombIDE.Core.Entities;
using TombIDE.Core.Models.Sessions;

namespace TombIDE.Services;

/// <summary>
/// Defines a service for retrieving, processing and writing trproj session records.
/// </summary>
public interface ITrprojSessionService
{
	/// <summary>
	/// Retrieves trproj sessions from a XML database file.
	/// </summary>
	IEnumerable<TrprojSessionRecordEntity> GetSessionsFromXml(string xmlFilePath);

	/// <summary>
	/// Groups session records by timestamps (e.g. "Today", "Yesterday", "Last week", "Last month").
	/// </summary>
	IEnumerable<SessionGroup> GroupSessionsByTimestamps(IEnumerable<TrprojSessionRecordEntity> sessions);

	/// <summary>
	/// Saves session records into a XML database file.
	/// </summary>
	void SaveRecordsToXml(IEnumerable<TrprojSessionRecordEntity> sessions, string xmlFilePath);
}
