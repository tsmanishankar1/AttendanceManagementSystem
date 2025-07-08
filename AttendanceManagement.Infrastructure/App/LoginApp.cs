using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class LoginApp : ILoginApp
{
    private readonly ILoginInfra _loginInfra;
    public LoginApp(ILoginInfra loginInfra)
    {
        _loginInfra = loginInfra;
    }

    public async Task<(string AccessToken, string RefreshToken)> RefreshAccessToken(RefreshTokenDto refreshTokenDto)
        => await _loginInfra.RefreshAccessToken(refreshTokenDto);

    public async Task<(string AccessToken, string RefreshToken)> ValidateUserAsync(Login login)
        => await _loginInfra.ValidateUserAsync(login);
}