namespace TombIDE.Core.Models;

public readonly record struct MnemonicConstant(string Name, short DecimalValue, string Description) : INamed
{
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
