using ScriptLib.ClassicScript.Data.Objects;
using ScriptLib.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ScriptLib.ClassicScript.Data
{
	public static class MnemonicData
	{
		#region Public objects and methods

		public const string MnemonicConstantsXmlFileName = "MnemonicConstants.xml";

		public static List<string> StandardConstants { get; private set; } = new();
		public static List<PluginConstant> PluginConstants { get; private set; } = new();

		/// <summary>
		/// All flags combined into one list.
		/// </summary>
		public static List<string> AllConstantFlags { get; private set; } = new();

		/// <summary>
		/// Sets up mnemonic constants for runtime use. Scans both built-in constants and installed project plugins' constants.
		/// </summary>
		/// <param name="ngcDir">The directory where the target NG_Center.exe file is being stored.</param>
		/// <param name="referencesDir">The directory where reference definitions (in .xml format) are being stored.</param>
		/// <exception cref="DirectoryNotFoundException" />
		/// <exception cref="ArgumentException" />
		public static void SetupConstants(string ngcDir, string referencesDir)
		{
			ValidateSetupData(ngcDir, referencesDir);

			StandardConstants = GetMnemonicConstants(referencesDir);
			PluginConstants = GetPluginConstants(ngcDir);

			var allMnemonics = new List<string>();
			allMnemonics.AddRange(StandardConstants);

			foreach (PluginConstant pluginMnemonic in PluginConstants)
				allMnemonics.Add(pluginMnemonic.FlagName);

			AllConstantFlags = allMnemonics;
		}

		/// <summary>
		/// Finds and gets the description of the specified plugin constant.
		/// </summary>
		/// <exception cref="InvalidOperationException" />
		public static string? GetPluginConstantDescription(string flagName)
		{
			if (PluginConstants.Count == 0)
				throw new InvalidOperationException("Constants haven't been setup yet.");

			foreach (PluginConstant constant in PluginConstants)
				if (constant.FlagName.Equals(flagName, StringComparison.OrdinalIgnoreCase))
				{
					string description = constant.Description;
					description = Regex.Replace(description, @"\r\n?|\n", "\n"); // Fixes broken newlines

					return description;
				}

			return null;
		}

		#endregion Public objects and methods

		#region Validation

		private static void ValidateSetupData(string ngcDir, string referencesDir)
		{
			if (!Directory.Exists(ngcDir))
				throw new DirectoryNotFoundException("NGC directory doesn't exist.");

			if (!Directory.Exists(referencesDir))
				throw new DirectoryNotFoundException("References directory doesn't exist.");

			string[] ngcDirFiles = Directory.GetFiles(ngcDir, "*.*", SearchOption.TopDirectoryOnly);

			bool ngcExecutableExists = Array.Exists(ngcDirFiles,
				x => Path.GetFileName(x).Equals("NG_Center.exe", StringComparison.OrdinalIgnoreCase));

			if (!ngcExecutableExists)
				throw new ArgumentException("Invalid NGC directory.");

			string[] referencesDirFiles = Directory.GetFiles(referencesDir, "*.*", SearchOption.TopDirectoryOnly);

			bool requiredXmlFileExists = Array.Exists(referencesDirFiles,
				x => Path.GetFileName(x).Equals(MnemonicConstantsXmlFileName, StringComparison.OrdinalIgnoreCase));

			if (!requiredXmlFileExists)
				throw new ArgumentException("References directory doesn't contain the required files.");
		}

		#endregion Validation

		#region Data fetching

		private static List<string> GetMnemonicConstants(string referencesPath)
		{
			try
			{
				var mnemonicConstants = new List<string>();

				string xmlPath = Path.Combine(referencesPath, MnemonicConstantsXmlFileName);

				using var reader = XmlReader.Create(xmlPath);
				using var dataSet = new DataSet();

				dataSet.ReadXml(reader);

				DataTable dataTable = dataSet.Tables[0];

				foreach (DataRow row in dataTable.Rows)
				{
					string? constantFlag = row[2].ToString();

					if (constantFlag != null)
						mnemonicConstants.Add(constantFlag);
				}

				return mnemonicConstants;
			}
			catch (Exception)
			{
				return new(); // Empty list
			}
		}

		private static List<PluginConstant> GetPluginConstants(string ngcPath)
		{
			try
			{
				var pluginMnemonics = new List<PluginConstant>();

				string[] pluginScriptFiles = Directory.GetFiles(ngcPath, "plugin_*.script", SearchOption.TopDirectoryOnly);

				foreach (string file in pluginScriptFiles)
				{
					var constants = GetPluginConstantsFromFile(file);

					if (constants != null)
						pluginMnemonics.AddRange(constants);
				}

				return pluginMnemonics;
			}
			catch (Exception)
			{
				return new(); // Empty list
			}
		}

		private static IEnumerable<PluginConstant> GetPluginConstantsFromFile(string file)
		{
			string[] lines = File.ReadAllLines(file, Encoding.GetEncoding(1252));

			bool startSectionFound = false;

			foreach (string line in lines)
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;

				string lineContent = line.TrimStart();

				if (lineContent.StartsWith("<start_constants>", StringComparison.OrdinalIgnoreCase))
				{
					startSectionFound = true;
					continue;
				}

				if (!startSectionFound)
					continue;

				if (lineContent.StartsWith("<end>", StringComparison.OrdinalIgnoreCase))
					break;

				var regex = new Regex(Patterns.PluginConstantDescription);
				var match = regex.Match(line);

				if (match.Success)
				{
					var groups = match.Groups;

					string flagName = groups[1].Value.Trim();

					string decimalString = groups[2].Value.Trim();
					short? decimalValue = GetDecimalValueFromString(decimalString);

					if (decimalValue == null)
						continue; // https://youtu.be/T9NjXekZ8kA

					string rawDescription = groups[3].Value.Trim();
					string cleanDescription = GetParsedPluginConstantDescription(rawDescription);

					yield return new(flagName, cleanDescription, decimalValue.Value);
				}
			}
		}

		private static short? GetDecimalValueFromString(string @string)
		{
			try
			{
				if (!short.TryParse(@string, out short decimalValue))
					decimalValue = Convert.ToInt16(@string.Replace("$", string.Empty), 16);

				return decimalValue;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static string GetParsedPluginConstantDescription(string rawDescription)
		{
			string[] descriptionLines = rawDescription.Split('>');
			descriptionLines = BasicCleaner.TrimEndingWhitespaceOnLines(descriptionLines);

			return string.Join(Environment.NewLine, descriptionLines);
		}

		#endregion Data fetching
	}
}
