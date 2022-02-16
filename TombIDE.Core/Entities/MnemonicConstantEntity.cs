namespace TombIDE.Core.Entities;

/// <summary>
/// <c>MnemonicConstant</c> XML database entity.
/// </summary>
public sealed record MnemonicConstantEntity(string Name, short DecimalValue, string Description)
{
	[XmlAttribute] public string Name { get; } = Name;
	[XmlAttribute] public short DecimalValue { get; } = DecimalValue;
	[XmlText] public string Description { get; } = Description;
}
