namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class ReportTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class WorkingTypeAmountRequest
    {
        public int WorkingTypeId { get; set; }
        public decimal Amount { get; set; }
        public int CreatedBy { get; set; }
    }

    public class WorkingTypeAmountResponse
    {
        public int Id { get; set; }
        public int WorkingTypeId { get; set; }
        public decimal Amount { get; set; }
    }

    public class UpdateWorkingTypeAmount
    {
        public int Id { get; set; }
        public int WorkingTypeId { get; set; }
        public decimal Amount { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class DailyReportRequest
    {
        public int DailyReportsId { get; set; }
        public List<int> StaffIds { get; set; } = new List<int>();
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public bool? CurrentMonth { get; set; }
        public bool? PreviousMonth { get; set; }
        public DateOnly? FromMonth { get; set; }
        public DateOnly? ToMonth { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public bool? IncludeTerminated { get; set; }
        public DateOnly? TerminatedFromDate { get; set; }
        public DateOnly? TerminatedToDate { get; set; }
        public int CreatedBy { get; set; }
    }

    public class AbsentListResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string? TransactionDate { get; set; }
        public string AttendanceStatus { get; set; } = null!;
    }

    public class ContinuousAbsentListResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int? TotalDays { get; set; }
    }

    public class CompOffAvailResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public DateOnly WorkedDate { get; set; }
        public DateOnly FromDate { get; set; }
        public string FromDuration { get; set; } = null!;
        public DateOnly ToDate { get; set; }
        public string? ToDuration { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class CompOffCreditResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public DateOnly WorkedDate { get; set; }
        public int? Credit { get; set; }
        public string? Reason { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class AttendanceResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ShiftName { get; set; } = null!;
        public DateOnly ShiftInDate { get; set; }
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public TimeOnly? InTime { get; set; }
        public TimeOnly? OutTime { get; set; }
        public TimeOnly? TotalHoursWorked { get; set; }
        public string AttendanceStatus { get; set; } = null!;
    }

    public class DailyPerformanceResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string ShiftName { get; set; } = null!;
        public DateOnly? Date { get; set; }
        public TimeOnly? InTime { get; set; }
        public TimeOnly? OutTime { get; set; }
        public TimeOnly? TotalHoursWorked { get; set; }
        public string? BreakHours { get; set; }
        public bool? IsBreakHoursExceeded { get; set; }
        public decimal? ProductiveHours { get; set; }
        public string AttendanceStatus { get; set; } = null!;
        public string EarlyEntry { get; set; } = null!;
        public string LateEntry { get; set; } = null!;
        public string EarlyExit { get; set; } = null!;
        public string ExtraHoursWorked { get; set; } = null!;
    }

    public class ManualPunchResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string PunchType { get; set; } = null!;
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string? AppliedOn { get; set; }
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class FirstInLastOutResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public DateOnly? SwipeDate { get; set; }
        public string Shift { get; set; } = null!;
        public TimeOnly? SwipeIn { get; set; }
        public TimeOnly? SwipeOut { get; set; }
        public TimeOnly? TotalHoursWorked { get; set; }
    }

    public class CurrentDaySwipeInResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string Shift { get; set; } = null!;
        public TimeOnly? InTime { get; set; }
    }

    public class RawPunchResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateOnly? SwipeDate { get; set; }
        public TimeOnly? SwipeTime { get; set; }
        public string ReaderName { get; set; } = null!;
        public string PunchType { get; set; } = null!;
        public string SwipeLocation { get; set; } = null!;
    }

    public class NightShiftCountResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Plant { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DateOnly? Date { get; set; }
        public int NightShiftCount { get; set; }
    }

    public class MonthlyReportResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public decimal OpeningCl { get; set; }
        public decimal OpeningPl { get; set; }
        public decimal OpeningSl { get; set; }
        public decimal CLCredits { get; set; }
        public decimal PLCredits { get; set; }
        public decimal SLCredits { get; set; }
        public decimal CLDebit { get; set; }
        public decimal PLDebit { get; set; }
        public decimal SLDebit { get; set; }
        public decimal CLAvailed { get; set; }
        public decimal PLAvailed { get; set; }
        public decimal SLAvailed { get; set; }
        public decimal CLClosingBalance { get; set; }
        public decimal PLClosingBalance { get; set; }
        public decimal SLClosingBalance { get; set; }
    }

    public class BusinessTravelResponse
    {
        public int? StaffId { get; set; }
        public string? StaffCreationId { get; set; }
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string? FromDuration { get; set; }
        public string? ToDuration { get; set; }
        public string? Duration { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Reason { get; set; }
        public string? TotalHoursDays { get; set; }
        public string? AppliedOn { get; set; }
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class VaccinationReportResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string? VaccinationDate { get; set; }
        public string? SecondVaccinatedDate { get; set; }
        public int? VaccinationNumber { get; set; }
        public bool? IsExempted { get; set; }
        public string? Comments { get; set; }
        public string? ApprovedOn { get; set; }
        public string? AppliedBy { get; set; }
        public string? ApprovedBy { get; set; }
    }

    public class WeeklyOffHolidayWorkingResponse
    {
        public int StaffId { get; set; }  
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;  
        public string Designation { get; set; } = null!;  
        public DateTime? AttendanceDate { get; set; } 
        public DateTime? ShiftIn { get; set; }  
        public DateTime? ShiftOut { get; set; }
        public string? AppliedOn { get; set; }
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }  
        public string? CancelledOn { get; set; }  
        public string? CancelledBy { get; set; }
    }

    public class PresentListResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public DateOnly? Date { get; set; }
        public string AttendanceStatus { get; set; } = null!;
    }

    public class OnDutyRequisitionResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string? Duration { get; set; }
        public string From { get; set; } = null!;
        public string FromDuration { get; set; } = null!;
        public string? To { get; set; }
        public string? ToDuration { get; set; }
        public string? TotalHoursDays { get; set; }
        public string Reason { get; set; } = null!;
        public string AppliedOn { get; set; } = null!;
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class LeaveRequisitionResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public string AppliedOn { get; set; } = null!;
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class LeaveTakenResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public decimal CLAvailed { get; set; }
        public decimal SLAvailed { get; set; }
        public decimal NCLTaken { get; set; }
        public decimal PTLTaken { get; set; }
        public decimal MGLTaken { get; set; }
    }

    public class PermissionRequisitionResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public DateOnly PermissionDate { get; set; }
        public string PermissionType { get; set; } = null!;
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string TotalHours { get; set; } = null!;
        public string? Reason { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class WorkFromHomeResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string From { get; set; } = null!;
        public string FromDuration { get; set; } = null!;
        public string To { get; set; } = null!;
        public string? ToDuration { get; set; }
        public string TotalHoursOrDays { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string AppliedOn { get; set; } = null!;
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }

    public class LeaveBalanceResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public decimal CLBalance { get; set; }
        public decimal PLBalance { get; set; }
        public decimal SLBalance { get; set; }
    }

    public class ShiftExtensionResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string ShiftName { get; set; } = null!;
        public string TxnDate { get; set; } = null!;
        public string DurationOfHoursExtension { get; set; } = null!;
        public string? HoursBeforeShift { get; set; }
        public string? HoursAfterShift { get; set; }
        public string Remarks { get; set; } = null!;
        public string AppliedOn { get; set; } = null!;
        public string? Approval1Status { get; set; }
        public string? Approved1On { get; set; }
        public string? Approved1By { get; set; }
        public string? Approval2Status { get; set; }
        public string? Approved2On { get; set; }
        public string? Approved2By { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public string? CancelledBy { get; set; }
    }
}