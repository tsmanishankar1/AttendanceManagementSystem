using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class BackDateProcessingAttendanceDatum
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public string? ShiftId { get; set; }

    public string? ShiftShortName { get; set; }

    public DateTime? ShiftInDate { get; set; }

    public DateTime? ShiftInTime { get; set; }

    public DateTime? ShiftOutDate { get; set; }

    public DateTime? ShiftOutTime { get; set; }

    public DateTime? ExpectedWorkingHours { get; set; }

    public DateTime? ActualInDate { get; set; }

    public DateTime? ActualInTime { get; set; }

    public DateTime? ActualOutDate { get; set; }

    public DateTime? ActualOutTime { get; set; }

    public DateTime? ActualWorkedHours { get; set; }

    public string? AttendanceStatus { get; set; }

    public string Fhstatus { get; set; } = null!;

    public string Shstatus { get; set; } = null!;

    public double AbsentCount { get; set; }

    public double DayAccount { get; set; }

    public bool IsWeeklyOff { get; set; }

    public bool IsPaidHoliday { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public string? ApplicationId { get; set; }

    public string? ApplicationType { get; set; }
}
