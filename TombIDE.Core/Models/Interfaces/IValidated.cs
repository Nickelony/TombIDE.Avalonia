namespace TombIDE.Core.Models.Interfaces;

/// <summary>
/// Defines an object which can be easily validated.
/// </summary>
public interface IValidated
{
	/// <summary>
	/// Determines whether the object's required fields and properties are valid.
	/// </summary>
	bool IsValid { get; }
}
