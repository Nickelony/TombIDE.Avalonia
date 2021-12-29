using TombIDE.Core.Models.Interfaces;

namespace TombIDE.Core.Models;

public sealed record MnemonicConstant(string Name, short DecimalValue, string Description) : INamed
{
	public string Name { get; } = Name;
	public short DecimalValue { get; } = DecimalValue;
	public string Description { get; } = Description;

	public string HexValue
	{
		get
		{
			string hexValue = DecimalValue.ToString("X");

			int zerosToAdd = 4 - hexValue.Length;
			return $"${new string('0', zerosToAdd)}{hexValue}";
		}
	}
}
