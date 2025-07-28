using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class ShiftExtensionDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public DateOnly TransactionDate { get; set; }
        [MaxLength(255)]
        public string? DurationHours { get; set; }
        public DateTime? BeforeShiftHours { get; set; }
        public DateTime? AfterShiftHours { get; set; }
        [MaxLength(255)]
        public string Remarks { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
}