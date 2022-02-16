using TombIDE.Core.Entities;

namespace TombIDE.Core.Models.Sessions;

/// <summary>
/// Defines a labeled group of session records.
/// </summary>
public record SessionGroup(string Header, IEnumerable<ISessionRecordEntity> Sessions);
