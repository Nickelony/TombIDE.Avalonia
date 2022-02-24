namespace TombIDE.Core.Utils;

public class PakFileUtils
{
	/// <returns>
	/// Raw image bytes (pixel array) from a .pak file.
	/// </returns>
	public byte[] ReadFile(string filePath)
	{
		using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

		byte[] bytes = new byte[stream.Length];
		stream.Read(bytes, 0, (int)stream.Length);
		bytes = bytes.Skip(4).ToArray();

		using var inStream = new MemoryStream(bytes, false);
		using var outStream = new MemoryStream();

		NetMiniZ.NetMiniZ.Decompress(inStream, outStream);
		return outStream.ToArray();
	}

	/// <summary>
	/// Writes bitmap data into a .pak file.
	/// </summary>
	public void WriteToFile(string filePath, byte[] bitmapData)
	{
		byte[] pakFileData = CompressData(bitmapData);
		File.WriteAllBytes(filePath, pakFileData);
	}

	private byte[] CompressData(byte[] data)
	{
		byte[] prefix = { 0x00, 0x00, 0x06, 0x00 }; // These bytes are important, otherwise the game won't launch

		using var inStream = new MemoryStream(data, false);
		using var outStream = new MemoryStream();

		NetMiniZ.NetMiniZ.Compress(inStream, outStream, 10);
		byte[] compressedData = outStream.ToArray();

		return prefix.Concat(compressedData).ToArray();
	}
}
