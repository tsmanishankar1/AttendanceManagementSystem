using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftRelay
{
    public int Id { get; set; }

    public DateTime FromTime { get; set; }

    public DateTime ToTime { get; set; }
}
