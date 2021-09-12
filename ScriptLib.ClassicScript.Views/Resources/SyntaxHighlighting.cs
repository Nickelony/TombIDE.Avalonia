using Avalonia.Media;
using AvaloniaEdit.Highlighting;
using ScriptLib.ClassicScript.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScriptLib.ClassicScript.Views.Resources
{
	public sealed class SyntaxHighlighting : IHighlightingDefinition
	{
		private readonly ColorScheme _scheme;

		#region Construction

		public SyntaxHighlighting(ColorScheme scheme)
			=> _scheme = scheme;

		#endregion Construction

		#region Rules

		public HighlightingRuleSet MainRuleSet
		{
			get
			{
				var ruleSet = new HighlightingRuleSet();

				/* Comments */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.Comment),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.Comments.HtmlColor)),
						FontWeight = _scheme.Comments.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.Comments.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				/* Sections */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.ValidSections, RegexOptions.IgnoreCase),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.Sections.HtmlColor)),
						FontWeight = _scheme.Sections.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.Sections.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				/* Old commands */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.ValidOldCommands, RegexOptions.IgnoreCase),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.StandardCommands.HtmlColor)),
						FontWeight = _scheme.StandardCommands.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.StandardCommands.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				/* New commands */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.ValidNewCommands, RegexOptions.IgnoreCase),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.NewCommands.HtmlColor)),
						FontWeight = _scheme.NewCommands.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.NewCommands.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				/* Next line keys */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.NextLineKey),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.NewCommands.HtmlColor)),
						FontWeight = FontWeight.Bold // Always bold
					}
				});

				/* Mnemonic Constants */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.ValidConstants, RegexOptions.IgnoreCase),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.References.HtmlColor)),
						FontWeight = _scheme.References.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.References.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				/* Hex values */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.HexValue, RegexOptions.IgnoreCase),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.References.HtmlColor)),
						FontWeight = _scheme.References.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.References.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				/* Directives (#...) */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.ValidDirectives, RegexOptions.IgnoreCase),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.References.HtmlColor)),
						FontWeight = _scheme.References.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.References.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				/* Values */
				ruleSet.Rules.Add(new HighlightingRule
				{
					Regex = new Regex(Patterns.Values),
					Color = new HighlightingColor
					{
						Foreground = new SimpleHighlightingBrush(Color.Parse(_scheme.Values.HtmlColor)),
						FontWeight = _scheme.Values.IsBold ? FontWeight.Bold : FontWeight.Normal,
						FontStyle = _scheme.Values.IsItalic ? FontStyle.Italic : FontStyle.Normal
					}
				});

				ruleSet.Name = "ClassicScript Rules";
				return ruleSet;
			}
		}

		#endregion Rules

		#region Other

		public string Name => "ClassicScript Rules";

		public IEnumerable<HighlightingColor> NamedHighlightingColors => throw new NotImplementedException();
		public IDictionary<string, string> Properties => throw new NotImplementedException();

		public HighlightingColor GetNamedColor(string name)
			=> throw new NotImplementedException();

		public HighlightingRuleSet GetNamedRuleSet(string name)
			=> throw new NotImplementedException();

		#endregion Other
	}
}
