namespace TombIDE.Core.Models.References;

public sealed record MnemonicConstant(string Name, short DecimalValue, string Description)
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
