namespace AttendanceManagement.Input_Models
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public int UserId { get; set; }
    }

    public class ResetPasswordModel
    {
        public string NewPassword { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public int UserId { get; set; }
    }
}
