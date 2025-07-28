using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ILoginInfra
    {
        Task<(string AccessToken, string RefreshToken)> ValidateUserAsync(Login login);
        Task<(string AccessToken, string RefreshToken)> RefreshAccessToken(RefreshTokenDto refreshTokenDto);
    }
}
