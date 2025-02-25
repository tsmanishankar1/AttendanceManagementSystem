using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LateComingForAttendance
{
    public string StaffId { get; set; } = null!;

    public DateTime TxnDate { get; set; }

    public DateTime ShiftIn { get; set; }

    public DateTime ShiftOut { get; set; }

    public DateTime? SwipeIn { get; set; }

    public DateTime? SwipeOut { get; set; }

    public bool IsLate { get; set; }

    public bool IsEarly { get; set; }

    public DateTime? LateHours { get; set; }

    public DateTime? EarlyHours { get; set; }

    public bool IsAbsentMarked { get; set; }

    public bool IsLeaveDeducted { get; set; }

    public decimal? WeekCount { get; set; }

    public decimal? MonthCount { get; set; }

    public long Id { get; set; }
}
