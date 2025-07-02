using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ILoginService
    {
        Task<(string AccessToken, string RefreshToken)> ValidateUserAsync(Login login);
        Task<(string AccessToken, string RefreshToken)> RefreshAccessToken(RefreshTokenDto refreshTokenDto);
    }
}
