using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace AttendanceManagement.Input_Models
{
    public class LeaveReq
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
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
        public int? ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string TotalHours { get; set; } = null!;
        public DateOnly PermissionDate { get; set; }
        public string PermissionType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Remarks { get; set; }
    }

    public class ManualPunch
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string SelectPunch { get; set; } = null!;
        public DateTime? InPunch { get; set; }
        public DateTime? OutPunch { get; set; }
        public string Remarks { get; set; } = null!;
    }

    public class OnDutyRequest
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Reason { get; set; } = null!;
    }
    public class Business
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public string Reason { get; set; } = null!;
    }
    public class WorkFrom
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StartDuration { get; set; } = null!;
        public string? EndDuration { get; set; }
        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public string Reason { get; set; } = null!;
    }
    public class ShiftChan
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string ShiftName { get; set; } = null!;
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string Reason { get; set; } = null!;
    }
    public class ShiftExte
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateOnly TransactionDate { get; set; }

        public string? DurationHours { get; set; }

        public DateTime? BeforeShiftHours { get; set; }

        public DateTime? AfterShiftHours { get; set; }

        public string? Remarks { get; set; }
    }
    public class WeeklyOffHoliday
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string SelectShiftType { get; set; } = null!;

        public DateOnly TxnDate { get; set; }

        public string ShiftName { get; set; } = null!;

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? ShiftInTime { get; set; }

        public DateTime? ShiftOutTime { get; set; }
    }
    public class CompOffAvai
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; }
        public string Status { get; set; }
        public DateOnly WorkedDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string FromDuration { get; set; } = null!;
        public string ToDuration { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public int TotalDays { get; set; }
    }
    public class CompOffCred
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; }
        public string Status { get; set; }
        public DateOnly WorkedDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; } = null!;
    }
}

