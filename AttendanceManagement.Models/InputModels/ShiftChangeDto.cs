using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.InputModels
{
    public class ShiftChangeDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public int ShiftId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
}