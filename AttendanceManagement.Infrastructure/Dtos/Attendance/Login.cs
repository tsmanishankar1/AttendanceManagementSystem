using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
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