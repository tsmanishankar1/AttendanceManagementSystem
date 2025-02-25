using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftPlan
{
    public string? StaffId { get; set; }

    public string? ShiftId { get; set; }

    public string? ShiftShortName { get; set; }

    public DateTime? ShiftInDate { get; set; }

    public DateTime? ShiftInTime { get; set; }

    public DateTime? ShiftOutDate { get; set; }

    public DateTime? ShiftOutTime { get; set; }

    public DateTime? ExpectedWorkingHours { get; set; }

    public DateTime? ShiftIn { get; set; }

    public DateTime? ShiftOut { get; set; }
}
