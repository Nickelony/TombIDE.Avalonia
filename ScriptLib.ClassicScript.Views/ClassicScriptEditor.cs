using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using ScriptLib.ClassicScript.Data;
using ScriptLib.ClassicScript.Data.Enums;
using ScriptLib.ClassicScript.Parsers;
using ScriptLib.ClassicScript.Utils;
using ScriptLib.ClassicScript.Views.Events;
using ScriptLib.ClassicScript.Views.Rendering;
using ScriptLib.ClassicScript.Views.Resources;
using ScriptLib.Core.Data;
using ScriptLib.Core.Utils;
using ScriptLib.Core.Views.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScriptLib.ClassicScript.Views
{
	public sealed class ClassicScriptEditor : TextEditorBase
	{
		#region Properties

		public CodeCleaner Cleaner { get; set; } = new();

		private bool _showSectionSeparators;

		public bool ShowSectionSeparators
		{
			get => _showSectionSeparators;
			set
			{
				_showSectionSeparators = value;

				if (_showSectionSeparators)
				{
					if (!TextArea.TextView.BackgroundRenderers.Contains(_sectionRenderer))
						TextArea.TextView.BackgroundRenderers.Add(_sectionRenderer);
				}
				else
				{
					if (TextArea.TextView.BackgroundRenderers.Contains(_sectionRenderer))
						TextArea.TextView.BackgroundRenderers.Remove(_sectionRenderer);
				}

				TextArea.TextView.InvalidateLayer(KnownLayer.Caret);
			}
		}

		#endregion Properties

		#region Fields

		private BackgroundWorker _autocompleteWorker;
		private ErrorDetectionWorker _errorDetectionWorker;

		private IBackgroundRenderer _sectionRenderer;

		#endregion Fields

		#region Construction

		public ClassicScriptEditor()
		{
			InitializeBackgroundWorkers();
			InitializeRenderers();

			BindEventMethods();

			CommentPrefix = ";";
		}

		private void InitializeBackgroundWorkers()
		{
			_autocompleteWorker = new BackgroundWorker();
			_autocompleteWorker.DoWork += AutocompleteWorker_DoWork;
			_autocompleteWorker.RunWorkerCompleted += AutocompleteWorker_RunWorkerCompleted;

			_errorDetectionWorker = new ErrorDetectionWorker(new ErrorDetector(), new TimeSpan(500));
			_errorDetectionWorker.RunWorkerCompleted += ErrorWorker_RunWorkerCompleted;
		}

		private void InitializeRenderers()
		{
			_sectionRenderer = new SectionRenderer(this);

			if (ShowSectionSeparators)
				TextArea.TextView.BackgroundRenderers.Add(_sectionRenderer);
		}

		private void BindEventMethods()
		{
			TextArea.TextEntered += TextEditor_TextEntered;
			TextChanged += TextEditor_TextChanged;

			KeyDown += TextEditor_KeyDown;
			PointerHover += TextEditor_MouseHover;
		}

		#endregion Construction

		#region Events

		private void TextEditor_TextEntered(object? sender, TextInputEventArgs e)
		{
			if (AutocompleteEnabled)
				HandleAutocomplete(e);
		}

		private void TextEditor_TextChanged(object? sender, EventArgs e)
		{
			if (LiveErrorUnderlining)
				_errorDetectionWorker.RunErrorCheckOnIdle(Text);
		}

		private void TextEditor_KeyDown(object? sender, KeyEventArgs e)
		{
			if (e.Key == Key.F1)
				InputFreeIndex();
			else if (e.Key == Key.F12)
			{
				if (_specialToolTip.IsVisible && HoveredWordArgs != null)
				{
					OnWordDefinitionRequested(HoveredWordArgs);
					HoveredWordArgs = null;
				}
				else
				{
					string word = WordParser.GetWordFromOffset(Document, CaretOffset);
					WordType type = WordParser.GetWordTypeFromOffset(Document, CaretOffset);

					if (!string.IsNullOrEmpty(word) && type != WordType.Unknown)
						OnWordDefinitionRequested(new WordDefinitionEventArgs(word, type));
				}
			}
		}

		private void TextEditor_MouseHover(object? sender, PointerEventArgs e)
			=> HandleDefinitionToolTips(e);

		#endregion Events

		#region Autocomplete

		// TODO: Recheck

		private void HandleAutocomplete(TextInputEventArgs e)
		{
			if (_completionWindow == null) // Prevents window duplicates
			{
				//if (e.Text == " " && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
				//	HandleAutocompleteAfterSpaceCtrl();
				if (Document.GetLineByOffset(CaretOffset).Length == 1)
					HandleAutocompleteOnEmptyLine();
				else if (e.Text != "_" && CaretOffset > 1)
				{
					char? firstLetterOfLastFlag = ArgumentParser.GetFirstLetterOfPreviousFlag(Document, CaretOffset);
					char? firstLetterOfCurrentFlag = ArgumentParser.GetFirstLetterOfArgumentFromOffset(Document, CaretOffset);

					if (firstLetterOfLastFlag != null
						&& char.ToUpper(e.Text[0]).Equals(char.ToUpper(firstLetterOfLastFlag.Value))
						&& CaretOffset > 1)
					{
						if (firstLetterOfCurrentFlag != null
						&& char.ToUpper(e.Text[0]).Equals(char.ToUpper(firstLetterOfCurrentFlag.Value)))
							HandleAutocompleteAfterSpace();
						else
							HandleAutocompleteForNextFlag();
					}
					else
						HandleAutocompleteAfterSpace();
				}
				else if (e.Text == "_" && CaretOffset > 1)
					HandleAutocompleteAfterUnderscore();
			}
		}

		private void HandleAutocompleteAfterSpaceCtrl()
		{
			Select(CaretOffset - 1, 1);
			SelectedText = string.Empty;

			string? wholeLineText = CommandParser.GetWholeCommandLineTextFromOffset(Document, CaretOffset, CommentHandling.Escape);

			if (string.IsNullOrEmpty(wholeLineText))
				HandleAutocompleteOnEmptyLine();
			else if (!_autocompleteWorker.IsBusy)
			{
				var data = new List<object>
				{
					Text,
					CaretOffset,
					-1
				};

				_autocompleteWorker.RunWorkerAsync(data);
			}
		}

		private void HandleAutocompleteAfterSpace()
		{
			if ((Document.GetCharAt(CaretOffset - 2) == '='
				|| Document.GetCharAt(CaretOffset - 2) == ','
				|| Document.GetCharAt(CaretOffset - 2) == '_')
				&& !_autocompleteWorker.IsBusy)
			{
				var data = new List<object>
				{
					Text,
					CaretOffset,
					-1
				};

				_autocompleteWorker.RunWorkerAsync(data);
			}
		}

		private void HandleAutocompleteAfterUnderscore()
		{
			if (Document.GetCharAt(CaretOffset - 1) == '_')
			{
				int wordStartOffset =
					TextUtilities.GetNextCaretPosition(Document, CaretOffset - 1, LogicalDirection.Backward, CaretPositioningMode.WordStart);

				string word = Document.GetText(wordStartOffset, CaretOffset - wordStartOffset);

				InitializeCompletionWindow();
				_completionWindow.StartOffset = wordStartOffset;

				foreach (string mnemonicConstant in MnemonicData.AllConstantFlags)
					if (mnemonicConstant.StartsWith(word, StringComparison.OrdinalIgnoreCase))
						_completionWindow.CompletionList.CompletionData.Add(new CompletionData(mnemonicConstant));

				ShowCompletionWindow();
			}
		}

		private void HandleAutocompleteOnEmptyLine()
		{
			string currentSection = DocumentParser.GetSectionNameFromOffset(Document, CaretOffset);

			if (currentSection != null && StringExtensions.BulkStringComparision(currentSection, StringComparison.OrdinalIgnoreCase,
				"Strings", "PSXStrings", "PCStrings", "ExtraNG"))
				return;

			InitializeCompletionWindow();
			_completionWindow.StartOffset = Document.GetLineByOffset(CaretOffset).Offset;

			foreach (ICompletionData item in Autocomplete.GetNewLineAutocompleteList())
				_completionWindow.CompletionList.CompletionData.Add(item);

			ShowCompletionWindow();
		}

		private void HandleAutocompleteForNextFlag()
		{
			if (!_autocompleteWorker.IsBusy)
			{
				var data = new List<object>
				{
					Text,
					CaretOffset,
					ArgumentParser.GetArgumentIndexAtOffset(Document, CaretOffset) - 1
				};

				_autocompleteWorker.RunWorkerAsync(data);
			}
		}

		private void AutocompleteWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			var data = e.Argument as List<object>;

			var document = new TextDocument(data[0].ToString());
			int caretOffset = (int)data[1];
			int argumentIndex = (int)data[2];

			e.Result = Autocomplete.GetCompletionData(document, caretOffset, argumentIndex);
		}

		private void AutocompleteWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			var completionData = e.Result as List<ICompletionData>;

			if (completionData == null)
				return;

			if (completionData.Count == 0)
				return;

			InitializeCompletionWindow();

			int wordStartOffset =
				TextUtilities.GetNextCaretPosition(Document, CaretOffset, LogicalDirection.Backward, CaretPositioningMode.WordStart);

			string word = Document.GetText(wordStartOffset, CaretOffset - wordStartOffset);

			if (!word.StartsWith("=") && !word.StartsWith(","))
				_completionWindow.StartOffset = wordStartOffset;

			foreach (ICompletionData item in completionData)
				_completionWindow.CompletionList.CompletionData.Add(item);

			ShowCompletionWindow();
		}

		#endregion Autocomplete

		#region Error handling

		public void CheckForErrors()
		{
			if (!IsSilentSession && !_errorDetectionWorker.IsBusy)
				_errorDetectionWorker.CheckForErrorsAsync(Text);
		}

		private void ErrorWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result == null)
				return;

			ResetAllErrors();
			ApplyErrorsToLines(e.Result as List<ErrorLine>);

			TextArea.TextView.InvalidateLayer(KnownLayer.Caret);
		}

		#endregion Error handling

		#region Other public methods

		public override void TidyCode(bool trimOnly = false)
		{
			Vector scrollOffset = TextArea.TextView.ScrollOffset;

			SelectAll();
			SelectedText = trimOnly ? BasicCleaner.TrimEndingWhitespace(Text) : Cleaner.ReindentScript(Text);
			ResetSelection();

			ScrollToHorizontalOffset(scrollOffset.X);
			ScrollToVerticalOffset(scrollOffset.Y);
		}

		#endregion Other public methods

		// TODO: Refactor

		private void InputFreeIndex()
		{
			int nextFreeIndex = GlobalParser.GetNextFreeIndex(Document, CaretOffset);

			if (nextFreeIndex == -1)
				return;

			TextArea.PerformTextInput(nextFreeIndex.ToString());
		}

		public override void UpdateSettings(ConfigurationBase configuration)
		{
			var config = configuration as CS_EditorConfiguration;

			SyntaxHighlighting = new SyntaxHighlighting(config.ColorScheme);

			Background = new SolidColorBrush(Color.Parse(config.ColorScheme.Background));
			Foreground = new SolidColorBrush(Color.Parse(config.ColorScheme.Foreground));

			ShowSectionSeparators = config.ShowSectionSeparators;

			Cleaner.PreEqualSpace = config.Tidy_PreEqualSpace;
			Cleaner.PostEqualSpace = config.Tidy_PostEqualSpace;
			Cleaner.PreCommaSpace = config.Tidy_PreCommaSpace;
			Cleaner.PostCommaSpace = config.Tidy_PostCommaSpace;
			Cleaner.ReduceSpaces = config.Tidy_ReduceSpaces;

			base.UpdateSettings(configuration);
		}

		private void HandleDefinitionToolTips(PointerEventArgs e)
		{
			int hoveredOffset = GetOffsetFromPoint(e.GetPosition(this));

			if (hoveredOffset == -1)
				return;

			DocumentLine hoveredLine = Document.GetLineByOffset(hoveredOffset);

			if (hoveredLine.HasError)
				return;

			string hoveredWord = WordParser.GetWordFromOffset(Document, hoveredOffset);
			WordType type = WordParser.GetWordTypeFromOffset(Document, hoveredOffset);

			if (type != WordType.Unknown)
			{
				ShowToolTip($"For more information about the {hoveredWord} {type}, Press F12");
				HoveredWordArgs = new WordDefinitionEventArgs(hoveredWord, type);
			}
		}

		private WordDefinitionEventArgs HoveredWordArgs = null;

		public delegate void WordDefinitionRequestedEventHandler(object sender, WordDefinitionEventArgs e);

		public event WordDefinitionRequestedEventHandler WordDefinitionRequested;

		public void OnWordDefinitionRequested(WordDefinitionEventArgs e) => WordDefinitionRequested?.Invoke(this, e);

		[Obsolete("This method shouldn't be used for ClassicScript.\nUse WordParser.GetWordFromOffset() instead.")]
		public new void GetWordFromOffset(int offset)
			=> base.GetWordFromOffset(offset);

		public override void GoToObject(string objectName, object identifyingObject = null)
		{
			if (identifyingObject is ObjectType type)
			{
				DocumentLine objectLine = DocumentParser.FindDocumentLineOfObject(Document, objectName, type);

				if (objectLine != null)
				{
					Focus();
					ScrollToLine(objectLine.LineNumber);
					SelectLine(objectLine);
				}
			}
		}

		public bool TryAddNewPluginEntry(string pluginString)
		{
			DocumentLine optionsSectionLine = DocumentParser.FindDocumentLineOfSection(Document, "Options");

			if (optionsSectionLine == null)
				return false;

			if (DocumentParser.IsPluginDefined(Document, pluginString))
				return false;

			int nextFreePluginIndex = GlobalParser.GetNextFreeIndex(Document, optionsSectionLine.Offset, "Plugin");
			DocumentLine lastSectionLine = DocumentParser.GetLastLineOfCurrentSection(Document, optionsSectionLine.Offset);

			CaretOffset = lastSectionLine.Offset + lastSectionLine.Length;

			TextArea.PerformTextInput($"{Environment.NewLine}Plugin= {nextFreePluginIndex}, {pluginString}, IGNORE");

			return true;
		}
	}
}
