using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Input_Models;
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
            var user = await _loginService.ValidateUserAsync(login);
            var response = new
            {
                Success = true,
                AccessToken = user
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
}