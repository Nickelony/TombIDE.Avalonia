using ScriptLib.Core.Utils;

namespace ScriptLib.ClassicScript.Utils
{
	public sealed class CodeCleaner : BasicCleaner
	{
		public bool PreEqualSpace { get; set; } = true; //ConfigurationDefaults.Tidy_PreEqualSpace;
		public bool PostEqualSpace { get; set; } = true; //ConfigurationDefaults.Tidy_PostEqualSpace;

		public bool PreCommaSpace { get; set; } = false; //ConfigurationDefaults.Tidy_PreCommaSpace;
		public bool PostCommaSpace { get; set; } = true; //ConfigurationDefaults.Tidy_PostCommaSpace;

		public bool ReduceSpaces { get; set; } = true; //ConfigurationDefaults.Tidy_ReduceSpaces;

		public string ReindentScript(string editorContent)
		{
			editorContent = HandleSpacesBeforeEquals(editorContent);
			editorContent = HandleSpacesAfterEquals(editorContent);

			editorContent = HandleSpacesBeforeCommas(editorContent);
			editorContent = HandleSpacesAfterCommas(editorContent);

			editorContent = HandleSpaceReduction(editorContent);

			return TrimEndingWhitespace(editorContent);
		}

		private string HandleSpacesBeforeEquals(string editorContent)
		{
			if (PreEqualSpace)
			{
				editorContent = editorContent.Replace("=", " =");

				while (editorContent.Contains("  ="))
					editorContent = editorContent.Replace("  =", " =");
			}
			else
				while (editorContent.Contains(" ="))
					editorContent = editorContent.Replace(" =", "=");

			return editorContent;
		}

		private string HandleSpacesAfterEquals(string editorContent)
		{
			if (PostEqualSpace)
			{
				editorContent = editorContent.Replace("=", "= ");

				while (editorContent.Contains("=  "))
					editorContent = editorContent.Replace("=  ", "= ");
			}
			else
				while (editorContent.Contains("= "))
					editorContent = editorContent.Replace("= ", "=");

			return editorContent;
		}

		private string HandleSpacesBeforeCommas(string editorContent)
		{
			if (PreCommaSpace)
			{
				editorContent = editorContent.Replace(",", " ,");

				while (editorContent.Contains("  ,"))
					editorContent = editorContent.Replace("  ,", " ,");
			}
			else
				while (editorContent.Contains(" ,"))
					editorContent = editorContent.Replace(" ,", ",");

			return editorContent;
		}

		private string HandleSpacesAfterCommas(string editorContent)
		{
			if (PostCommaSpace)
			{
				editorContent = editorContent.Replace(",", ", ");

				while (editorContent.Contains(",  "))
					editorContent = editorContent.Replace(",  ", ", ");
			}
			else
				while (editorContent.Contains(", "))
					editorContent = editorContent.Replace(", ", ",");

			return editorContent;
		}

		private string HandleSpaceReduction(string editorContent)
		{
			if (ReduceSpaces)
				while (editorContent.Contains("  "))
					editorContent = editorContent.Replace("  ", " ");

			return editorContent;
		}
	}
}
