using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class ChangePasswordModel
    {
        [MaxLength(50)]
        public string CurrentPassword { get; set; } = null!;
        [MaxLength(50)]
        public string NewPassword { get; set; } = null!;
        [MaxLength(50)]
        public string ConfirmPassword { get; set; } = null!;
        public int UserId { get; set; }
    }

    public class ResetPasswordModel
    {
        [MaxLength(50)]
        public string NewPassword { get; set; } = null!;
        [MaxLength(50)]
        public string ConfirmPassword { get; set; } = null!;
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
    }
}