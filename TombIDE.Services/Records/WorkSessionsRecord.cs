namespace TombIDE.Services.Records;

public sealed record WorkSessionsRecord(DateOnly Date, List<GameProjectRecord> GameProjectRecords);
