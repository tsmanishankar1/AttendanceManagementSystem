using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class Login
    {
        [MaxLength(100)]
        public string Username { get; set; } = null!;
        [MaxLength(50)]
        public string Password { get; set; } = null!;
    }

    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = null!;
    }
}