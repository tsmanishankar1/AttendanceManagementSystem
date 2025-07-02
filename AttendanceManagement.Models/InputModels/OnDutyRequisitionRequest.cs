using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.InputModels
{
    public class OnDutyRequisitionRequest
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        [MaxLength(20)]
        public string StartDuration { get; set; } = null!;
        [MaxLength(20)]
        public string? EndDuration { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; } = null!;
        [MaxLength(50)]
        public string? TotalHours { get; set; }
        public decimal? TotalDays { get; set; }
        public int CreatedBy { get; set; }
    }
}