using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace AttendanceManagement.Input_Models
{
    public class LeaveReq
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; } 
        public string? Status2 { get; set; } 
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public DateOnly FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class PermissionDto
    {
        public int Id { get; set; }
        public int? ApplicationTypeId { get; set; }
        public string? ApplicationTypeName { get; set; } 
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string TotalHours { get; set; } = null!;
        public DateOnly PermissionDate { get; set; }
        public string PermissionType { get; set; } = null!;
        public string? Status1 { get; set; } 
        public string? Status2 { get; set; } 
        public string? Remarks { get; set; }
    }

    public class ManualPunch
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
        public string SelectPunch { get; set; } = null!;
        public DateTime? InPunch { get; set; }
        public DateTime? OutPunch { get; set; }
        public string Remarks { get; set; } = null!;
    }

    public class OnDutyRequest
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? TotalDays { get; set; }
        public string? TotalHours { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class Business
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public decimal? TotalDays { get; set; }
        public string? TotalHours { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class WorkFrom
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; } 
        public string? Status2 { get; set; } 
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public decimal? TotalDays { get; set; }
        public string? TotalHours { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class ShiftChan
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
        public string ShiftName { get; set; } = null!;
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class ShiftExte
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string? DurationHours { get; set; }
        public DateTime? BeforeShiftHours { get; set; }
        public DateTime? AfterShiftHours { get; set; }
        public string? Remarks { get; set; }
    }

    public class WeeklyOffHoliday
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; } 
        public string? Status2 { get; set; } 
        public string SelectShiftType { get; set; } = null!;
        public DateOnly TxnDate { get; set; }
        public string? ShiftName { get; set; } 
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? ShiftOutTime { get; set; }
    }

    public class CompOffAvai
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string? ApplicationTypeName { get; set; }
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
        public DateOnly WorkedDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string FromDuration { get; set; } = null!;
        public string ToDuration { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public decimal TotalDays { get; set; }
    }

    public class CompOffCred
    {
        public int Id { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
        public DateOnly WorkedDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class Reimbursements
    {
        public int Id { get; set; }
        public DateOnly BillDate { get; set; }
        public string BillNo { get; set; } = null!;
        public string? Description { get; set; }
        public string BillPeriod { get; set; } = null!;
        public decimal Amount { get; set; }
        public string ReimbursementTypeName { get; set; } = null!;
        public string UploadFilePath { get; set; } = null!;
        public string? Status1 { get; set; }
        public string? Status2 { get; set; }
    }
}