using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class ManualPunchRequestDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        [MaxLength(100)]
        public string SelectPunch { get; set; } = null!;
        public DateTime? InPunch { get; set; }
        public DateTime? OutPunch { get; set; }
        [MaxLength(255)]
        public string Remarks { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
}