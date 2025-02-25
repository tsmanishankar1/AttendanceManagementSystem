using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AttendanceDetail
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public string? ShiftId { get; set; }

    public DateTime? ShiftInDate { get; set; }

    public DateTime? ShiftInTime { get; set; }

    public DateTime? ShiftOutDate { get; set; }

    public DateTime? ShiftOutTime { get; set; }

    public DateTime? ActualInDate { get; set; }

    public DateTime? ActualInTime { get; set; }

    public DateTime? ActualOutDate { get; set; }

    public DateTime? ActualOutTime { get; set; }

    public DateTime? ExpectedWorkingHours { get; set; }

    public DateTime? ActualWorkedHours { get; set; }

    public DateTime? NetWorkedHours { get; set; }

    public DateTime? BreakHours { get; set; }

    public DateTime? EarlyComing { get; set; }

    public DateTime? LateComing { get; set; }

    public DateTime? EarlyGoing { get; set; }

    public DateTime? LateGoing { get; set; }

    public DateTime? Othours { get; set; }

    public string AttendanceStatus { get; set; } = null!;

    public string Fhstatus { get; set; } = null!;

    public string Shstatus { get; set; } = null!;

    public bool IsManualPunch { get; set; }

    public bool IsDisputed { get; set; }

    public bool IsProcessed { get; set; }

    public bool IsSinglePunch { get; set; }

    public bool IsEarlyComing { get; set; }

    public bool IsLateComing { get; set; }

    public bool IsEarlyGoing { get; set; }

    public bool IsLateGoing { get; set; }

    public bool IsIncorrectPunches { get; set; }

    public double AbsentCount { get; set; }

    public double DayAccount { get; set; }

    public int StaffStatusId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? BranchName { get; set; }

    public string? DeptName { get; set; }

    public string? DivisionName { get; set; }

    public string? DesignationName { get; set; }

    public string? GradeName { get; set; }

    public string? DateOfJoining { get; set; }

    public string? DateOfResignation { get; set; }

    public string? ParentId { get; set; }

    public string? ParentType { get; set; }

    public string? ShiftName { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public DateTime? GraceLateBy { get; set; }

    public DateTime? GraceEarlyBy { get; set; }

    public int? MinDayHours { get; set; }

    public int? MinWeekHours { get; set; }

    public string? LeaveName { get; set; }
}
