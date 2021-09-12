namespace ScriptLib.ClassicScript.Utils
{
	public static class DataConverter
	{
		public static string GetShortHex(short decimalValue, int size = 4)
		{
			string hexValue = decimalValue.ToString("X");

			int zerosToAdd = size - hexValue.Length;
			hexValue = $"${new string('0', zerosToAdd)}{hexValue}";

			return hexValue;
		}
	}
}
