using TombIDE.Services.Generic;

namespace TombIDE.Services.Records;

public record DailySessionGroup(DateOnly SessionDay, IEnumerable<ISessionRecord> Sessions);
