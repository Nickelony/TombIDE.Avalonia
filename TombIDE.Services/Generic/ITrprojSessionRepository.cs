using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TombIDE.Services.Records;

namespace TombIDE.Services.Generic;

public interface ITrprojSessionRepository
{
	/// <summary>
	/// Fetches all available session records from the database.
	/// </summary>
	IEnumerable<TrprojSessionRecord> GetAllRecords();

	/// <summary>
	/// Fetches an exact session record from the database.
	/// </summary>
	/// <returns><see langword="null" /> if no matching record has been found, otherwise returns a single matching record.</returns>
	TrprojSessionRecord? Find(Predicate<TrprojSessionRecord> predicate);

	/// <summary>
	/// Fetches matching session records from the database.
	/// </summary>
	IEnumerable<TrprojSessionRecord> FindAll(Predicate<TrprojSessionRecord> predicate);

	void Add(TrprojSessionRecord session);
	void AddRange(IEnumerable<TrprojSessionRecord> entities);

	void Remove(TrprojSessionRecord session);
	void RemoveRange(IEnumerable<TrprojSessionRecord> sessions);

	void Save();

	/// <summary>
	/// Adds a session record into the database.
	/// </summary>
	void AddRecord(TrprojSessionRecord record);

	/// <summary>
	/// Fetches a list of all session records which are grouped by days.
	/// </summary>
	IEnumerable<DailySessionGroup> GetDailySessionGroups();
}
