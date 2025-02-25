using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class IcfValidHoliday
{
    public long IvhId { get; set; }

    public DateOnly? IvhDate { get; set; }

    public string? IvhHoliday { get; set; }

    public DateTime? IvhCreatedon { get; set; }
}
