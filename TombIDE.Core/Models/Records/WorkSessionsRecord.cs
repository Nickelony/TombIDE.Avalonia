namespace TombIDE.Services.Records;

public sealed record WorkSessionsRecord(DateOnly Date, List<TrprojSessionRecord> TrprojRecords)
{
	public DateOnly Date { get; } = Date;
	public List<TrprojSessionRecord> GameProjectRecords { get; } = TrprojRecords;
}
