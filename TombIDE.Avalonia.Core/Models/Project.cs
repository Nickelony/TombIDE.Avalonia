﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TombIDE.Avalonia.Core.Enums;

namespace TombIDE.Avalonia.Core
{
	public class Project
	{
		public const string ProjectDirectoryKey = "$(ProjectDirectory)";

		#region Public properties

		/// <summary>
		/// Displayed project name.
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// Game version. (TR2, TR3, TR4, ...)
		/// </summary>
		public GameVersion GameVersion { get; set; }

		/// <summary>
		/// The path of the file, which launches the game.
		/// </summary>
		public string? LaunchFilePath { get; set; }

		/// <summary>
		/// The .trproj folder path.
		/// </summary>
		[XmlIgnore]
		public string? ProjectPath { get; set; }

		/// <summary>
		/// The path where the project's tomb4.exe / PCTomb5.exe file is stored.
		/// <para>For the new project format, the game's .exe file is stored in the /Engine/ directory.</para>
		/// </summary>
		[XmlIgnore]
		public string? EnginePath { get; set; }

		/// <summary>
		/// The path where the project's SCRIPT.TXT and {LANGUAGE}.TXT files are stored.
		/// </summary>
		public string? ScriptPath { get; set; }

		/// <summary>
		/// The path where all the project's newly created / imported levels are stored.
		/// </summary>
		public string? LevelsPath { get; set; }

		/// <summary>
		/// A list of the project's levels.
		/// </summary>
		public List<ProjectLevel> Levels { get; set; } = new();

		/// <summary>
		/// A list of the project's installed plugins.
		/// </summary>
		[XmlIgnore]
		public List<Plugin> InstalledPlugins { get; set; } = new();

		/// <summary>
		/// .trproj file name = game's .exe file name. (tomb4, PCTomb5, ...)
		/// <para>Returns null if the paths haven't been decoded yet.</para>
		/// <para>To decode paths use DecodeProjectPaths()</para>
		/// </summary>
		public string? TrprojFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(ProjectPath) || string.IsNullOrEmpty(EnginePath))
					return null;

				foreach (string file in Directory.GetFiles(EnginePath, "*.exe", SearchOption.TopDirectoryOnly))
					if (GameVersion == GameVersion.TR4 && Path.GetFileName(file).Equals("tomb4.exe", StringComparison.OrdinalIgnoreCase))
						return Path.Combine(ProjectPath, Path.GetFileNameWithoutExtension(file) + ".trproj");
					else if (GameVersion == GameVersion.TEN && Path.GetFileName(file).Equals("pctomb5.exe", StringComparison.OrdinalIgnoreCase))
						return Path.Combine(ProjectPath, Path.GetFileNameWithoutExtension(file) + ".trproj");

				return null;
			}
		}

		/// <summary>
		/// Gets the .exe file name depending on the project's GameVersion. (tomb4.exe, PCTomb5.exe, ...)
		/// </summary>
		public string? ExeFileName
			=> GameVersion switch
			{
				GameVersion.TR4 => "tomb4.exe",
				GameVersion.TEN => "PCTomb5.exe",
				_ => null
			};

		/// <summary>
		/// Gets the level data file extension depending on the project's GameVersion. (.tr4, .trc, ...)
		/// </summary>
		public string? LevelFileExtension
			=> GameVersion switch
			{
				GameVersion.TR4 => ".tr4",
				GameVersion.TEN => ".trc",
				_ => null
			};

		#endregion Public properties

		#region Public methods

		/// <summary>
		/// Saves the .trproj file.
		/// </summary>
		public void Save()
		{
			if (string.IsNullOrEmpty(TrprojFilePath))
				return;

			using var writer = new StreamWriter(TrprojFilePath);

			Project projectCopy = Clone();
			projectCopy.EncodeProjectPaths();

			var serializer = new XmlSerializer(typeof(Project));
			serializer.Serialize(writer, projectCopy);
		}

		/// <summary>
		/// Creates and returns an exact copy of the current project without overriding the original data while in use.
		/// </summary>
		public Project Clone()
		{
			var projectCopy = new Project()
			{
				Name = Name,
				GameVersion = GameVersion,
				LaunchFilePath = LaunchFilePath,
				ProjectPath = ProjectPath,
				EnginePath = EnginePath,
				ScriptPath = ScriptPath,
				LevelsPath = LevelsPath
			};

			foreach (var level in Levels)
				projectCopy.Levels.Add(level.Clone()); // Clone the levels too, because their data can be overridden as well

			projectCopy.InstalledPlugins.AddRange(InstalledPlugins);

			return projectCopy;
		}

		/// <summary>
		/// Renames the project (and its directory if renameDirectory is true).
		/// </summary>
		public void Rename(string newName, bool renameDirectory)
		{
			if (renameDirectory)
			{
				if (string.IsNullOrEmpty(ProjectPath) || string.IsNullOrEmpty(EnginePath)
					|| string.IsNullOrEmpty(ScriptPath) || string.IsNullOrEmpty(LevelsPath))
					return;

				var projectDirectory = Path.GetDirectoryName(ProjectPath);

				if (string.IsNullOrEmpty(projectDirectory))
					return;

				// Rename the project directory
				string newProjectPath = Path.Combine(projectDirectory, newName);

				if (Directory.Exists(ProjectPath + "_TEMP")) // The "_TEMP" suffix exists only when the directory name just changed letter cases
					Directory.Move(ProjectPath + "_TEMP", newProjectPath);
				else
					Directory.Move(ProjectPath, newProjectPath);

				EnginePath = Path.Combine(newProjectPath, EnginePath.Remove(0, ProjectPath.Length + 1));

				// Change ScriptPath / LevelsPath values of the project if they were inside the ProjectPath folder
				if (ScriptPath.StartsWith(ProjectPath))
					ScriptPath = Path.Combine(newProjectPath, ScriptPath.Remove(0, ProjectPath.Length + 1));

				if (LevelsPath.StartsWith(ProjectPath))
					LevelsPath = Path.Combine(newProjectPath, LevelsPath.Remove(0, ProjectPath.Length + 1));

				var cachedLevelList = new List<ProjectLevel>();
				cachedLevelList.AddRange(Levels);

				// Remove all internal levels from the project's list to update all .prj2 files with new paths
				Levels.Clear();

				// Restore external levels, because we don't update them
				foreach (var projectLevel in cachedLevelList)
				{
					if (string.IsNullOrEmpty(projectLevel.FolderPath))
						continue;

					var levelRootDirectory = Path.GetDirectoryName(projectLevel.FolderPath);

					if (string.IsNullOrEmpty(levelRootDirectory))
						continue;

					if (levelRootDirectory.Equals(LevelsPath, StringComparison.OrdinalIgnoreCase))
						Levels.Add(projectLevel);
				}

				ProjectPath = newProjectPath;
			}

			Name = newName;
		}

		/// <summary>
		/// Replaces "$(ProjectDirectory)" with the project's .trproj folder path.
		/// <para>This is used to make projects inside TombIDE easier to read for the software.</para>
		/// </summary>
		public void DecodeProjectPaths(string trprojFilePath)
		{
			ProjectPath = Path.GetDirectoryName(trprojFilePath);

			if (!string.IsNullOrEmpty(LaunchFilePath))
				LaunchFilePath = Path.Combine(ProjectPath, LaunchFilePath.Replace(@"$(ProjectDirectory)\", string.Empty));

			string engineDirectory = Path.Combine(ProjectPath, "Engine");

			if (Directory.Exists(engineDirectory))
				foreach (string file in Directory.GetFiles(engineDirectory, "*.exe", SearchOption.TopDirectoryOnly))
					if ((GameVersion == GameVersion.TR4 && Path.GetFileName(file).Equals("tomb4.exe", StringComparison.OrdinalIgnoreCase))
						|| (GameVersion == GameVersion.TEN && Path.GetFileName(file).Equals("pctomb5.exe", StringComparison.OrdinalIgnoreCase)))
					{
						EnginePath = engineDirectory;
						break;
					}

			// If the /Engine/ directory doesn't exist or no valid .exe file was found in that directory
			if (string.IsNullOrEmpty(EnginePath))
				EnginePath = ProjectPath;

			if (ScriptPath.StartsWith(ProjectDirectoryKey))
				ScriptPath = ScriptPath.Replace(ProjectDirectoryKey, ProjectPath);

			if (LevelsPath.StartsWith(ProjectDirectoryKey))
				LevelsPath = LevelsPath.Replace(ProjectDirectoryKey, ProjectPath);

			foreach (ProjectLevel level in Levels)
				if (level.FolderPath.StartsWith(ProjectDirectoryKey))
					level.FolderPath = level.FolderPath.Replace(ProjectDirectoryKey, ProjectPath);
		}

		/// <summary>
		/// Replaces the project's .trproj folder path with "$(ProjectDirectory)".
		/// <para>This is used before saving .trproj files to avoid having "hardcoded" paths.</para>
		/// </summary>
		public void EncodeProjectPaths()
		{
			LaunchFilePath = Path.Combine(ProjectDirectoryKey, Path.GetFileName(LaunchFilePath));

			if (ScriptPath.StartsWith(ProjectPath))
				ScriptPath = ScriptPath.Replace(ProjectPath, ProjectDirectoryKey);

			if (LevelsPath.StartsWith(ProjectPath))
				LevelsPath = LevelsPath.Replace(ProjectPath, ProjectDirectoryKey);

			foreach (ProjectLevel level in Levels)
				if (level.FolderPath.StartsWith(ProjectPath))
					level.FolderPath = level.FolderPath.Replace(ProjectPath, ProjectDirectoryKey);
		}

		#endregion Public methods

		#region Public static methods

		public static Project FromFile(string trprojPath)
		{
			using (StreamReader reader = new StreamReader(trprojPath))
			{
				var serializer = new XmlSerializer(typeof(Project));
				var project = serializer.Deserialize(reader) as Project;

				project.DecodeProjectPaths(trprojPath);

				return project;
			}
		}

		#endregion Public static methods
	}
}
