namespace ScriptLib.ClassicScript.Data
{
	/// <summary>
	/// The holy bible of ClassicScript syntax and more.
	/// </summary>
	public struct Patterns
	{
		#region Constant patterns

		public const string LineStart = @"^\s*";
		public const string LineEnd = @"\s*$";

		/// <summary>
		/// Basically LineEnd but it tolerates comments as well.<br /><br />
		/// Available groups:<br />
		/// $1 is the comment (if available)
		/// </summary>
		public const string CodeLineEnd = @"\s*(;.*)?$";

		/// <summary>
		/// Available groups:<br />
		/// $1 is the actual word
		/// </summary>
		public const string AnyWord = @"\b(\w+)\b";

		/// <summary>
		/// Available groups:<br />
		/// $1 is <c>AnyWord</c>
		/// </summary>
		public const string AnyDirectiveStart = LineStart + "#" + AnyWord;

		/// <summary>
		/// Available groups:<br />
		/// $1 is <c>AnyWord</c><br />
		/// $2 is the comment (if available)
		/// </summary>
		public const string AnySectionStart = LineStart + @"\[\s*" + AnyWord + @"\s*\]" + CodeLineEnd;

		/// <summary>
		/// Available groups:<br />
		/// $1 is <c>AnyWord</c>
		/// </summary>
		public const string AnyCommandStart = LineStart + AnyWord + @"\s*=";

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀SomeCommand =⠀SOME_VALUE, ANOTHER_VALUE, 2137</c><br /><br />
		/// Available groups:<br />
		/// $1 is the command name (in the example's case it would be <c>"SomeCommand"</c>)<br />
		/// $2 are the arguments (in the example's case it would be <c>"SOME_VALUE, ANOTHER_VALUE, 2137"</c>)<br /><br />
		///
		/// <b>Note:</b> If you want to use this on a multi-line command (handled with ">"), you have to Escape / Remove<br />
		/// the comments first and then make sure to Escape / Remove all ">", "\r" and "\n" symbols as well. Otherwise<br />
		/// this pattern won't work correctly!
		/// </summary>
		public const string AnyCommandWithArguments = AnyCommandStart + @"\s*([^;\r\n]+)";

		public const string Comment = ";.*$";
		public const string HexValue = @"\$[a-fA-F0-9]+";

		/// <summary>
		/// Available groups:<br />
		/// $1 is the comment (if available)
		/// </summary>
		public const string NextLineKey = @"[,=]\s*>" + CodeLineEnd;

		/// <summary>
		/// Available groups:<br />
		/// $1 is the comment (if available)
		/// </summary>
		public const string PartialLine = LineStart + ".*" + NextLineKey;

		/// <summary>
		/// Available groups:<br />
		/// $1 is the command name
		/// $2 is the comment (if available)
		/// </summary>
		public const string PartialLineCommandStart = AnyCommandStart + ".*" + NextLineKey;

		/// <summary>
		/// Example:<br />
		/// <c>(SARG_...)</c><br /><br />
		/// Available groups:<br />
		/// $1 is the <c>SARG</c> value (without the underscore)
		/// </summary>
		public const string CommandPrefixInParenthesis = @"\(([^_]+).*\)";

		/// <summary>
		/// Example:<br />
		/// <c>"..."</c><br /><br />
		/// Available groups:<br />
		/// $1 is the content inside the quotation marks
		/// </summary>
		public const string FilePath = "\"([^\"]*)\"";

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀CONST_FLAG⠀⠀: $D2137⠀⠀⠀⠀; Plugin description>⠀⠀⠀⠀which uses multiple lines</c><br /><br />
		/// Available groups:<br />
		/// $1 is the <c>CONST_FLAG</c> value (in the example's case it would be <c>"CONST_FLAG⠀⠀"</c>)<br />
		/// $2 is the hex / decimal value (in the example's case it would be <c>" $D2137⠀⠀⠀⠀"</c>)<br />
		/// $3 is the description after the ";" symbol (excluding the ";" of course)
		/// </summary>
		public const string PluginConstantDescription = LineStart + @"([^:]+):([^;\r\n]+);?(.*)$";

		/// <summary>
		/// Example:<br />
		/// <c>⠀2137:</c><br /><br />
		/// Available groups:<br />
		/// $1 is the index
		/// </summary>
		public const string NGStringIndex = LineStart + @"(\d+):";

		#endregion Constant patterns

		#region Static patterns

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀Customize = CUST_CMD ,</c><br /><br />
		/// Available groups:<br />
		/// $1 is just "Customize"<br />
		/// $2 is the <c>CUST_CMD</c> value
		/// </summary>
		public static string CustomizeCommandWithFirstArg => SpecificCommandStart("Customize") + @"\s*" + AnyWord + @"\s*,";

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀Parameters = PARAM_CMD ,</c><br /><br />
		/// Available groups:<br />
		/// $1 is just "Parameters"<br />
		/// $2 is the <c>PARAM_CMD</c> value
		/// </summary>
		public static string ParametersCommandWithFirstArg => SpecificCommandStart("Parameters") + @"\s*" + AnyWord + @"\s*,";

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀#DEFINE⠀⠀CONSTANT VALUE⠀⠀; A test value</c><br /><br />
		/// Available groups:<br />
		/// $1 is just "DEFINE"<br />
		/// $2 is the <c>CONSTANT</c> value<br />
		/// $3 is the <c>VALUE</c> value
		/// </summary>
		public static string DefineWithValue => SpecificDirectiveStart("DEFINE") + @"\s+" + AnyWord + @"\s+" + AnyWord;

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀#INCLUDE⠀⠀"example/file.txt"⠀⠀; A test file</c><br /><br />
		/// Available groups:<br />
		/// $1 is just "INCLUDE"<br />
		/// $2 is the file path inside the quotations
		/// </summary>
		public static string IncludeWithValue => SpecificDirectiveStart("INCLUDE") + @"\s+" + FilePath;

		#endregion Static patterns

		#region Syntax highlighting patterns

		public static string ValidDirectives => SpecificDirectiveStart(string.Join("|", Keywords.Directives));
		public static string ValidSections => SpecificSectionStart(string.Join("|", Keywords.Sections));
		public static string ValidOldCommands => SpecificCommandStart(string.Join("|", Keywords.OldCommands));
		public static string ValidNewCommands => SpecificCommandStart(string.Join("|", Keywords.NewCommands));
		public static string ValidConstants => SpecificWord(string.Join("|", MnemonicData.AllConstantFlags));

		public static string Values
		{
			get
			{
				string anyDecimal = @"\d";
				string anyWordChar = @"\w";
				string backSlash = @"\";
				string forwardSlash = "/";
				string quotationMarks = "\"";
				string apostrophe = "'";
				string dot = @"\.";

				return $"{anyDecimal}|{anyWordChar}|{backSlash}|{forwardSlash}|{quotationMarks}|{apostrophe}|{dot}";
			}
		}

		#endregion Syntax highlighting patterns

		#region Method patterns

		/// <summary>
		/// Exact Regex pattern: <c>\b({word})\b</c><br />
		/// The <c>word</c> argument may also be multiple words merged with the '|' symbol.<br />
		/// <b>Note:</b> The only extra symbol a word can contain to still be considered a single word is an underscore '_'.
		/// </summary>
		public static string SpecificWord(string word)
			=> $@"\b({word})\b";

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀#{directive}</c><br />
		/// The <c>directive</c> argument may also be multiple directives merged with the '|' symbol.
		/// </summary>
		public static string SpecificDirectiveStart(string directive)
			=> LineStart + "#" + SpecificWord(directive);

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀[⠀⠀{sectionName}⠀⠀]⠀⠀; Starts a section</c><br />
		/// The <c>sectionName</c> argument may also be multiple section names merged with the '|' symbol.
		/// </summary>
		public static string SpecificSectionStart(string sectionName)
		{
			string bracketStart = @"\[\s*";
			string bracketEnd = @"\s*\]";

			return LineStart + bracketStart + SpecificWord(sectionName) + bracketEnd + CodeLineEnd;
		}

		/// <summary>
		/// Example:<br />
		/// <c>⠀⠀⠀⠀{command} =</c><br />
		/// The <c>command</c> argument may also be multiple commands merged with the '|' symbol.
		/// </summary>
		public static string SpecificCommandStart(string command)
			=> LineStart + SpecificWord(command) + @"\s*=";

		#endregion Method patterns
	}
}
