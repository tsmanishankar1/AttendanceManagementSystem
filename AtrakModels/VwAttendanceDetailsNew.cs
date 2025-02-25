using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwAttendanceDetailsNew
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public string? CardCode { get; set; }

    public string FirstName { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? CompanyShortName { get; set; }

    public string? BranchName { get; set; }

    public string? DeptName { get; set; }

    public string? DivisionName { get; set; }

    public string? DesignationName { get; set; }

    public string? VolumeName { get; set; }

    public string? DesignationShortName { get; set; }

    public string? GradeName { get; set; }

    public string? ShiftId { get; set; }

    public string? ShiftShortName { get; set; }

    public string ShiftName { get; set; } = null!;

    public DateTime? ShiftInDate { get; set; }

    public DateTime? ShiftInTime { get; set; }

    public DateTime? ShiftOutDate { get; set; }

    public DateTime? ShiftOutTime { get; set; }

    public DateTime? ActualInDate { get; set; }

    public DateTime? ActualInTime { get; set; }

    public DateTime? ActualOutDate { get; set; }

    public DateTime? ActualOutTime { get; set; }

    public DateTime? ExpectedWorkingHours { get; set; }

    public DateTime? NetWorkedHours { get; set; }

    public DateTime? ExtraBreakTime { get; set; }

    public DateTime? Othours { get; set; }

    public DateTime? AccountedOttime { get; set; }

    public DateTime? EarlyComing { get; set; }

    public DateTime? AccountedEarlyComingTime { get; set; }

    public DateTime? LateComing { get; set; }

    public DateTime? AccountedLateComingTime { get; set; }

    public DateTime? EarlyGoing { get; set; }

    public DateTime? AccountedEarlyGoingTime { get; set; }

    public DateTime? LateGoing { get; set; }

    public DateTime? AccountedLateGoingTime { get; set; }

    public bool IsDisputed { get; set; }

    public bool IsProcessed { get; set; }

    public bool IsSinglePunch { get; set; }

    public bool IsEarlyComing { get; set; }

    public bool IsEarlyComingValid { get; set; }

    public bool IsLateComing { get; set; }

    public bool IsLateComingValid { get; set; }

    public bool IsEarlyGoing { get; set; }

    public bool IsEarlyGoingValid { get; set; }

    public bool IsLateGoing { get; set; }

    public bool IsLateGoingValid { get; set; }

    public double AbsentCount { get; set; }

    public double DayAccount { get; set; }

    public bool IsOt { get; set; }

    public bool IsOtvalid { get; set; }

    public bool IsManualPunch { get; set; }

    public bool IsIncorrectPunches { get; set; }

    public string AttendanceStatus { get; set; } = null!;

    public string Fhstatus { get; set; } = null!;

    public string Shstatus { get; set; } = null!;

    public bool IsLeave { get; set; }

    public bool IsLeaveValid { get; set; }

    public bool IsLeaveWithWages { get; set; }

    public bool IsAutoShift { get; set; }

    public bool IsWeeklyOff { get; set; }

    public bool IsPaidHoliday { get; set; }

    public DateTime? ActualWorkedHours { get; set; }
}
