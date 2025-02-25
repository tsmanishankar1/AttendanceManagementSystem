using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class ManualPunchRequestDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public string SelectPunch { get; set; } = null!;
        public DateTime? InPunch { get; set; }
        public DateTime? OutPunch { get; set; }
        public string Remarks { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
}
