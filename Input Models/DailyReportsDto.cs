using DocumentFormat.OpenXml.Bibliography;

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
        public DateOnly? TerminatedFromDate {  get; set; }
        public DateOnly? TerminatedToDate {  get; set; }
        public int CreatedBy { get; set; }
    }

    public class DailyReportResponse
    {
        public int Id { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public int LeaveTypeId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public bool? Status1 { get; set; }
        public int CreatedBy {  get; set; }
        public int? StaffId { get; set; }
        public DateTime CreatedUtc {get; set;}
        public int? UpdatedBy { get; set; }   
        public DateTime? UpdatedUtc { get; set; }
     
        public bool? IsCancelled { get; set; }
        public DateTime? CancelledOn { get; set; }
    }

    public class DailyReportLeaveRequisitionResponse
    {
        public int Id { get; set; }
        public string ReportName { get; set; } = null!;
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserCreationId { get; set; } = null!;
        public string FromDate { get; set; } = null!;
        public string ToDate { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public string? ApproverStatus { get; set; }
        public string AppliedOn { get; set; } = null!;
        public string ApprovedBy { get; set; } = null!;
        public string? ApprovedOn { get; set; }     
        public string? IsCancelled { get; set; }
        public string? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
        public string ReportDate { get; set; } = null!;
    }

}
