using System.Xml.Serialization;

namespace TombIDE.Services.Records;

public sealed record WorkSessionsRecord(DateOnly Date, List<GameProjectRecord> GameProjectRecords)
{
	[XmlAttribute] public DateOnly Date { get; } = Date;
	[XmlArray] public List<GameProjectRecord> GameProjectRecords { get; } = GameProjectRecords;
}
