using System.Collections.ObjectModel;

namespace TombIDE.Core.Models.Projects;

public sealed class GameProject : ProjectBase, IGameProject
{
	public DirectoryInfo ScriptDirectory { get; private set; }
	public DirectoryInfo MapsDirectory { get; private set; }
	public DirectoryInfo? TRNGPluginsDirectory { get; private set; }
	public ObservableCollection<DirectoryInfo> ExternalMapSubdirectories { get; private set; }

	private FileSystemWatcher? _mapsDirectoryWatcher;
	private FileSystemWatcher? _trngPluginsDirectoryWatcher;

	public GameProject(string name, string rootDirectoryPath,
		string scriptDirectoryPath, string mapsDirectoryPath, string? trngPluginsDirectoryPath = null,
		IEnumerable<DirectoryInfo>? externalMapSubdirectories = null) : base(name, rootDirectoryPath)
	{
		ScriptDirectory = new DirectoryInfo(scriptDirectoryPath);
		MapsDirectory = new DirectoryInfo(mapsDirectoryPath);

		TRNGPluginsDirectory = trngPluginsDirectoryPath is not null ?
			new DirectoryInfo(trngPluginsDirectoryPath) : null;

		ExternalMapSubdirectories = externalMapSubdirectories is not null ?
			new(externalMapSubdirectories) : new();

		SetupMapsDirectoryWatcher();
		SetupTRNGPluginsDirectoryWatcher();
	}

	public event EventHandler? ScriptDirectoryChanged;
	public event EventHandler? MapsDirectoryChanged;
	public event EventHandler? TRNGPluginsDirectoryChanged;
	public event EventHandler? MapsDirectoryContentChanged;
	public event EventHandler? TRNGPluginsDirectoryContentChanged;

	public override void MoveRootDirectory(string newRootPath)
	{
		string oldRootPath = RootDirectory.FullName;

		base.MoveRootDirectory(newRootPath);

		if (ScriptDirectory.FullName.Contains(oldRootPath))
			ChangeScriptDirectory(ScriptDirectory.FullName.Replace(oldRootPath, newRootPath));

		if (MapsDirectory.FullName.Contains(oldRootPath))
			ChangeMapsDirectory(MapsDirectory.FullName.Replace(oldRootPath, newRootPath));

		if (TRNGPluginsDirectory is not null && TRNGPluginsDirectory.FullName.Contains(oldRootPath))
			ChangeTRNGPluginsDirectory(TRNGPluginsDirectory.FullName.Replace(oldRootPath, newRootPath));

		// Update external map subdirectories if some of those directories were somehow inside the project's root directory
		for (int i = 0; i < ExternalMapSubdirectories.Count; i++)
		{
			if (!ExternalMapSubdirectories[i].FullName.Contains(oldRootPath))
				continue;

			string newMapDirectoryPath = ExternalMapSubdirectories[i].FullName.Replace(oldRootPath, newRootPath);
			ExternalMapSubdirectories[i] = new DirectoryInfo(newMapDirectoryPath);
		}
	}

	public void ChangeScriptDirectory(string newDirectoryPath)
	{
		ScriptDirectory = new DirectoryInfo(newDirectoryPath);
		ScriptDirectoryChanged?.Invoke(this, EventArgs.Empty);
	}

	public void ChangeMapsDirectory(string newDirectoryPath)
	{
		MapsDirectory = new DirectoryInfo(newDirectoryPath);
		SetupMapsDirectoryWatcher();

		MapsDirectoryChanged?.Invoke(this, EventArgs.Empty);
	}

	public void ChangeTRNGPluginsDirectory(string newTRNGPluginsDirectoryPath)
	{
		TRNGPluginsDirectory = new DirectoryInfo(newTRNGPluginsDirectoryPath);
		SetupTRNGPluginsDirectoryWatcher();

		TRNGPluginsDirectoryChanged?.Invoke(this, EventArgs.Empty);
	}

	private void SetupMapsDirectoryWatcher()
	{
		_mapsDirectoryWatcher?.Dispose();

		_mapsDirectoryWatcher = new FileSystemWatcher
		{
			Path = MapsDirectory.FullName,
			EnableRaisingEvents = true
		};

		_mapsDirectoryWatcher.Changed += MapsDirectoryWatcher_Changed;
		_mapsDirectoryWatcher.Created += MapsDirectoryWatcher_Changed;
		_mapsDirectoryWatcher.Deleted += MapsDirectoryWatcher_Changed;
		_mapsDirectoryWatcher.Renamed += MapsDirectoryWatcher_Changed;
	}

	private void SetupTRNGPluginsDirectoryWatcher()
	{
		_trngPluginsDirectoryWatcher?.Dispose();

		if (TRNGPluginsDirectory is null)
			return;

		_trngPluginsDirectoryWatcher = new FileSystemWatcher
		{
			Path = TRNGPluginsDirectory.FullName,
			EnableRaisingEvents = true
		};

		_trngPluginsDirectoryWatcher.Changed += TRNGPluginsDirectoryWatcher_Changed;
		_trngPluginsDirectoryWatcher.Created += TRNGPluginsDirectoryWatcher_Changed;
		_trngPluginsDirectoryWatcher.Deleted += TRNGPluginsDirectoryWatcher_Changed;
		_trngPluginsDirectoryWatcher.Renamed += TRNGPluginsDirectoryWatcher_Changed;
	}

	private void MapsDirectoryWatcher_Changed(object sender, FileSystemEventArgs e)
		=> MapsDirectoryContentChanged?.Invoke(sender, e);

	private void TRNGPluginsDirectoryWatcher_Changed(object sender, FileSystemEventArgs e)
		=> TRNGPluginsDirectoryChanged?.Invoke(sender, e);
}
