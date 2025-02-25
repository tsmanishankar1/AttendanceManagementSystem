using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class MarchManualStatus
{
    public string StaffId { get; set; } = null!;

    public DateTime FromDate { get; set; }
}
