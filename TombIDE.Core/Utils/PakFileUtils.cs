namespace TombIDE.Core.Utils;

public static class PakFileUtils
{
	public static byte[] GetDecompressedData(string pakFilePath)
	{
		using var stream = new FileStream(pakFilePath, FileMode.Open, FileAccess.Read);

		byte[] bytes = new byte[stream.Length];
		stream.Read(bytes, 0, (int)stream.Length);
		bytes = bytes.Skip(4).ToArray();

		using var inStream = new MemoryStream(bytes, false);
		using var outStream = new MemoryStream();

		NetMiniZ.NetMiniZ.Decompress(inStream, outStream);
		return outStream.ToArray();
	}

	public static void WriteToFile(string pakFilePath, byte[] rawImageData)
	{
		byte[] pakFileData = CompressData(rawImageData);
		File.WriteAllBytes(pakFilePath, pakFileData);
	}

	private static byte[] CompressData(byte[] data)
	{
		byte[] prefix = { 0x00, 0x00, 0x06, 0x00 }; // These bytes are important, otherwise the game won't launch

		using var inStream = new MemoryStream(data, false);
		using var outStream = new MemoryStream();

		NetMiniZ.NetMiniZ.Compress(inStream, outStream, 10);
		byte[] compressedData = outStream.ToArray();

		return prefix.Concat(compressedData).ToArray();
	}
}
