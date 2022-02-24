namespace TombIDE.Services.Implementations;

public sealed class TrprojSessionService : ITrprojSessionService
{
	public IEnumerable<TrprojSessionRecord> GetSessionsFromXml(string xmlFilePath)
	{
	}

	public IEnumerable<SessionGroup> GroupSessionsByTimestamps(IEnumerable<TrprojSessionRecord> sessions)
	{
	}

	public void SaveRecordsToXml(IEnumerable<TrprojSessionRecord> sessions, string xmlFilePath)
	{
	}
}
