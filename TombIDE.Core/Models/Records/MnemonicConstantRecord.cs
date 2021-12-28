namespace TombIDE.Core.Models.Records;

public record MnemonicConstantRecord(string ConstantName, short DecimalValue, string Description)
{
	public string ConstantName { get; } = ConstantName;
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
