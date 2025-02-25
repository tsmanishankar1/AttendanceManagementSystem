using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AugBreakHoursExceed
{
    public string? StaffId { get; set; }

    public DateTime? ShiftInDate { get; set; }

    public string Fhstatus { get; set; } = null!;

    public string Shstatus { get; set; } = null!;

    public string AttendanceStatus { get; set; } = null!;

    public decimal Fhaccount { get; set; }

    public decimal Shaccount { get; set; }

    public double DayAccount { get; set; }

    public double AbsentCount { get; set; }

    public bool IsManualStatus { get; set; }

    public string? PermissionId { get; set; }
}
