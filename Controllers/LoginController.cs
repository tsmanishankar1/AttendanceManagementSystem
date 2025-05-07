using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Input_Models;
using Azure.Core;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginService _loginService;

    public LoginController(LoginService loginService)
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
    public IActionResult RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var newAccessToken = _loginService.RefreshAccessToken(refreshTokenDto.RefreshToken);
            var response = new
            {
                Success = true,
                AccessToken = newAccessToken
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