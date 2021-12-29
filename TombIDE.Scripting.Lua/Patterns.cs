namespace TombIDE.Scripting.Lua;

public static class Patterns
{
	public const string Comments = @"--.*$";

	public static string Operators => @"(" + string.Join("|", Keywords.Operators) + @")";
	public static string SpecialOperators => @"\b(" + string.Join("|", Keywords.SpecialOperators) + @")\b";
	public static string Statements => @"\b(" + string.Join("|", Keywords.Statements) + @")\b";
	public static string Values => @"\b(" + string.Join("|", Keywords.Values) + @")\b";
}
