using System.IO.Compression;

namespace TombIDE.Formats.Parc;

internal class PluginArchive : ZipArchive
{
	public PluginArchive(Stream stream) : base(stream)
	{ }

	public bool ContainsPlugin(string pluginName)
		=> Entries.ToList().Exists(entry =>
		{
			string? entryRoot = Path.GetPathRoot(entry.FullName);
			string? entryRootName = entryRoot?.TrimEnd('\\');

			return entryRootName != null && entryRootName == pluginName;
		});

	public void ExtractPlugin(string pluginName, string destPath)
	{
		string? dirName = Path.GetDirectoryName(destPath);

		if (dirName != null && !Directory.Exists(dirName))
			Directory.CreateDirectory(dirName);

		ZipArchiveEntry? pluginEntry = Entries.ToList().Find(entry => entry.Name == pluginName);
		pluginEntry?.ExtractToFile(destPath, true);
	}
}
