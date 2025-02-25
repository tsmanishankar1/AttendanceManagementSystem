using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AttendanceDatum
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

    public string? ActualShiftId { get; set; }

    public string? ActualShiftShortName { get; set; }

    public DateTime? ActualInDate { get; set; }

    public DateTime? ActualInTime { get; set; }

    public DateTime? BreakOutTime { get; set; }

    public DateTime? BreakInTime { get; set; }

    public DateTime? ActualOutDate { get; set; }

    public DateTime? ActualOutTime { get; set; }

    public DateTime? ActualWorkedHours { get; set; }

    public DateTime? NetWorkedHours { get; set; }

    public DateTime? BreakHours { get; set; }

    public DateTime? ExtraBreakTime { get; set; }

    public DateTime? Othours { get; set; }

    public DateTime? EarlyComing { get; set; }

    public DateTime? LateComing { get; set; }

    public DateTime? EarlyGoing { get; set; }

    public DateTime? LateGoing { get; set; }

    public DateTime? ActualEarlyComingTime { get; set; }

    public DateTime? ActualLateComingTime { get; set; }

    public DateTime? ActualEarlyGoingTime { get; set; }

    public DateTime? ActualLateGoingTime { get; set; }

    public DateTime? ActualOttime { get; set; }

    public DateTime? AccountedEarlyComingTime { get; set; }

    public DateTime? AccountedLateComingTime { get; set; }

    public DateTime? AccountedEarlyGoingTime { get; set; }

    public DateTime? AccountedLateGoingTime { get; set; }

    public DateTime? AccountedOttime { get; set; }

    public bool IsEarlyComing { get; set; }

    public bool IsEarlyComingValid { get; set; }

    public bool IsLateComing { get; set; }

    public bool IsLateComingValid { get; set; }

    public bool IsEarlyGoing { get; set; }

    public bool IsEarlyGoingValid { get; set; }

    public bool IsLateGoing { get; set; }

    public bool IsLateGoingValid { get; set; }

    public bool IsOt { get; set; }

    public bool IsOtvalid { get; set; }

    public bool IsManualPunch { get; set; }

    public bool IsSinglePunch { get; set; }

    public bool IsIncorrectPunches { get; set; }

    public bool IsDisputed { get; set; }

    public bool OverRideEarlyComing { get; set; }

    public bool OverRideLateComing { get; set; }

    public bool OverRideEarlyGoing { get; set; }

    public bool OverRideLateGoing { get; set; }

    public bool OverRideOt { get; set; }

    public string AttendanceStatus { get; set; } = null!;

    public string Fhstatus { get; set; } = null!;

    public string Shstatus { get; set; } = null!;

    public double AbsentCount { get; set; }

    public double DayAccount { get; set; }

    public bool IsLeave { get; set; }

    public bool IsLeaveValid { get; set; }

    public bool IsLeaveWithWages { get; set; }

    public bool IsAutoShift { get; set; }

    public bool IsWeeklyOff { get; set; }

    public bool IsPaidHoliday { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? InReaderName { get; set; }

    public string? OutReaderName { get; set; }

    public bool? IsShiftPlanMissing { get; set; }

    public bool IsBreakExceeded { get; set; }

    public bool IsBreakExceedValid { get; set; }

    public bool IsBreakDisputed { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? PreShiftOthours { get; set; }

    public DateTime? PostShiftOthours { get; set; }

    public DateTime? ActualShiftIn { get; set; }

    public DateTime? ActualShiftOut { get; set; }

    public DateTime? ActualWorkingHours { get; set; }

    public decimal Fhaccount { get; set; }

    public decimal Shaccount { get; set; }

    public int ProcessId { get; set; }

    public bool IsToBeLeaveDeducted { get; set; }

    public bool IsAutoLeaveDeducted { get; set; }

    public bool IsManualStatus { get; set; }

    public string? ApprovedExtraHours { get; set; }

    public string? ConsiderExtraHoursFor { get; set; }

    public bool IsExtraHoursProcessed { get; set; }

    public DateTime? ExtraHoursApprovedOn { get; set; }

    public string? ExtraHoursApprovedBy { get; set; }

    public int IflexiShiftTime { get; set; }

    public bool IsFlexiShift { get; set; }

    public bool IsSpecialLate { get; set; }

    public string? DepartmentId { get; set; }

    public string? Comments { get; set; }

    public DateTime? AutoShiftBasedSwipeInTime { get; set; }

    public DateTime? AutoShiftBasedSwipeOutTime { get; set; }

    public DateTime? AutoShiftBasedWorkedHours { get; set; }

    public virtual Staff? Staff { get; set; }
}
