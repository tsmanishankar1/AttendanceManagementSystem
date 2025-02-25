using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxHoliday
{
    public int HdId { get; set; }

    public DateOnly HdDate { get; set; }

    public string HdDesc { get; set; } = null!;

    public bool HdIsreaderdownload { get; set; }

    public bool? HdUpdateStatus { get; set; }

    public DateTime? HdCreated { get; set; }

    public DateTime? HdModified { get; set; }

    public string? HdModifiedby { get; set; }
}
