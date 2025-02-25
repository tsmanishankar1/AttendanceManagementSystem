using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwShiftRelay
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime GraceLateBy { get; set; }

    public DateTime GraceEarlyBy { get; set; }

    public DateTime BreakStartTime { get; set; }

    public DateTime BreakEndTime { get; set; }

    public int MinDayHours { get; set; }

    public int MinWeekHours { get; set; }

    public bool IsActive { get; set; }

    public int? RelayNo { get; set; }
}
