using System.Xml.Serialization;
using TombIDE.Services.Generic;

namespace TombIDE.Services.Records;

public sealed record TrprojSessionRecord(string ProjectFilePath, DateTime LastSession) : ISessionRecord
{
	[XmlAttribute] public string ProjectFilePath { get; } = ProjectFilePath;
	[XmlAttribute] public DateTime LastSession { get; } = LastSession;
}
