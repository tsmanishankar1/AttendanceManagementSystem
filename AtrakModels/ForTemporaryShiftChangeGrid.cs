using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ForTemporaryShiftChangeGrid
{
    public string Id { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}
