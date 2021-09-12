namespace ScriptLib.Core.Utils
{
	public interface IErrorDetector
	{
		object? FindErrors(object content);
	}
}
