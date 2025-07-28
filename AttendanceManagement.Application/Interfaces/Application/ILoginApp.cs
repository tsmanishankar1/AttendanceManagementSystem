using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ILoginApp
    {
        Task<(string AccessToken, string RefreshToken)> ValidateUserAsync(Login login);
        Task<(string AccessToken, string RefreshToken)> RefreshAccessToken(RefreshTokenDto refreshTokenDto);
    }
}
