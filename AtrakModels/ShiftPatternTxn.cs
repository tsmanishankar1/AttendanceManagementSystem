using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftPatternTxn
{
    public long Id { get; set; }

    public int PatternId { get; set; }

    public string ParentId { get; set; } = null!;

    public string ParentType { get; set; } = null!;

    public virtual ShiftPattern Pattern { get; set; } = null!;
}
