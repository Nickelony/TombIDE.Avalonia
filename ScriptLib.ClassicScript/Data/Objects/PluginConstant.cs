using ScriptLib.ClassicScript.Utils;

namespace ScriptLib.ClassicScript.Data.Objects
{
	public class PluginConstant
	{
		public PluginConstant(string flagName, string description, short decimalValue)
		{
			FlagName = flagName;
			Description = description;
			DecimalValue = decimalValue;
			HexValue = DataConverter.GetShortHex(decimalValue);
		}

		public string FlagName { get; }
		public string Description { get; }
		public short DecimalValue { get; }
		public string HexValue { get; }
	}
}
