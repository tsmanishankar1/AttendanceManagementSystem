using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.InputModels;
using AttendanceManagement.Services.Interface;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Login login)
    {
        try
        {
            var (accessToken, refreshToken) = await _loginService.ValidateUserAsync(login);
            var response = new
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var (newAccessToken, refreshToken) = await _loginService.RefreshAccessToken(refreshTokenDto);
            var response = new
            {
                Success = true,
                AccessToken = newAccessToken,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return ErrorClass.BadResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}