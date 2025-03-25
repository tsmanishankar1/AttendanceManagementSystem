namespace AttendanceManagement.Input_Models
{
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
    public class CompOffAvailResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateOnly WorkedDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string FromDuration { get; set; }
        public string ToDuration { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string? ApproverStatus { get; set; }
        public string? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
    }

    public class CompOffCreditResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateOnly WorkedDate { get; set; }
        public int? Credit { get; set; }
        public string? Reason { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string? ApproverStatus { get; set; }
        public string? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
    }
    public class ManualPunchResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string PunchType { get; set; } = string.Empty;
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public DateTime? AppliedOn { get; set; }
        public string? ApprovalStatus { get; set; }
        public bool IsCancelled { get; set; } 
        public int AppliedBy { get; set; } 
        public int ApprovedBy { get; set; } 
        public DateTime? ApprovedOn { get; set; }
        public DateTime? CancelledOn { get; set; }
    }
    public class BusinessTravelResponse
    {
        public int? StaffId { get; set; }
        public string? StaffCreationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string? FromDuration { get; set; }
        public string? ToDuration { get; set; }
        public string? Duration { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Reason { get; set; }
        public string? TotalHoursDays { get; set; }
        public string? AppliedOn { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
    }
    public class VaccinationReportResponse
    {
        public int? StaffId { get; set; }
        public string? StaffCreationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string? VaccinationDate { get; set; }
        public string? SecondVaccinationDate { get; set; }
        public string? VaccinationNumber { get; set; }
        public bool? IsExempted { get; set; }
        public string? Comments { get; set; }
        public string? ApprovedOn { get; set; }
        public int? AppliedBy { get; set; }
        public int? ApprovedBy { get; set; }
    }
    public class WeeklyOffHolidayWorkingResponse
    {
        public int? StaffId { get; set; }  
        public string? StaffCreationId { get; set; } 
        public int? DepartmentId { get; set; }  
        public int? DesignationId { get; set; }  
        public DateTime? AttendanceDate { get; set; } 
        public DateTime? ShiftIn { get; set; }  
        public DateTime? ShiftOut { get; set; }  
        public string? ApprovalStatus { get; set; }  
        public int? AppliedBy { get; set; }  
        public int? ApprovedBy { get; set; }  
        public string? AppliedOn { get; set; }  
        public string? ApprovedOn { get; set; }  
        public string? IsCancelled { get; set; }  
        public DateTime? CancelledOn { get; set; }  
    }
    public class OnDutyRequisitionResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string From { get; set; }
        public string? To { get; set; }
        public string FromDuration { get; set; }  
        public string? ToDuration { get; set; }
        public string Duration { get; set; }
        public string? TotalHoursDays { get; set; }
        public string Reason { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string? ApproverStatus { get; set; }
        public string? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
    }
    public class LeaveRequisitionResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public int LeaveTypeId { get; set; }
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public string AppliedOn { get; set; } = null!;
        public string? ApproverStatus { get; set; }
        public string? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
    }
    public class LeaveTakenResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
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
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateOnly PermissionDate { get; set; }
        public string PermissionType { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string TotalHours { get; set; }
        public string? Reason { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string? ApproverStatus { get; set; }
        public string? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
    }

    public class WorkFromHomeResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string Duration { get; set; } = null!;
        public string From { get; set; } = null!;
        public string FromDuration { get; set; } = null!;
        public string To { get; set; } = null!;
        public string? ToDuration { get; set; }
        public string TotalHoursOrDays { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string AppliedOn { get; set; } = null!;
        public string ApprovalStatus { get; set; } = null!;
        public string? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
    }

    public class LeaveBalanceResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public decimal CLBalance { get; set; }
        public decimal PLBalance { get; set; }
        public decimal SLBalance { get; set; }
    }
}
