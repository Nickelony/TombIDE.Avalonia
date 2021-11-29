using ScriptLib.ClassicScript.Utils;

namespace ScriptLib.ClassicScript.Data.Objects
{
	public record PluginConstant(string FlagName, string Description, short DecimalValue)
	{
		public string FlagName { get; init; } = FlagName;
		public string Description { get; init; } = Description;
		public short DecimalValue { get; init; } = DecimalValue;
		public string HexValue => DataConverter.GetShortHex(DecimalValue);
	}
}
