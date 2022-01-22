using System.Xml.Serialization;

namespace TombIDE.Services.Records;

public sealed record MnemonicConstantDbRecord(string Name, short DecimalValue, string Description)
{
	[XmlAttribute] public string Name { get; } = Name;
	[XmlAttribute] public short DecimalValue { get; } = DecimalValue;
	[XmlText] public string Description { get; } = Description;
}
