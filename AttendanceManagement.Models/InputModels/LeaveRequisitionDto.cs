using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.InputModels
{
    public class LeaveRequisitionDto
    {
        public int LeaveRequestId { get; set; }
        public string ApplicationType { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string StartDuration { get; set; } = null!;
        public string EndDuration { get; set; } = null!;
        public string LeaveType { get; set; } = null!;
        public DateOnly FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class LeaveRequisitionRequest
    {
        [MaxLength(50)]
        public string StartDuration { get; set; } = null!;
        [MaxLength(50)]
        public string? EndDuration { get; set; } = null!;
        public int? StaffId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; } = null!;
        public int CreatedBy { get; set; }
        public int ApplicationTypeId { get; set; }
        public decimal TotalDays { get; set; }
    }

    public class UpdateLeaveRequest
    {
        public int LeaveRequestId { get; set; }
        [MaxLength(50)]
        public string StartDuration { get; set; } = null!;
        [MaxLength(50)]
        public string EndDuration { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public int ApplicationTypeId { get; set; }
    }
}