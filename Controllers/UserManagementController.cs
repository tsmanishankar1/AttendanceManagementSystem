using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[Route("api/[controller]")]
[ApiController]
public class UserManagementController : ControllerBase
{
    private readonly UserManagementService _userService;
    private readonly LoggingService _loggingService;
    public UserManagementController(UserManagementService userService, LoggingService loggingService)
    {
        _userService = userService;
        _loggingService = loggingService;

    }

    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUser(UserManagementRequest userRequest)
    {
        try
        {
            var result = await _userService.RegisterUser(userRequest);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Register User", "POST", "/api/UserManagement/RegisterUser", result, userRequest.CreatedBy, JsonSerializer.Serialize(userRequest));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Register User", "POST", "/api/UserManagement/RegisterUser", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, userRequest.CreatedBy, JsonSerializer.Serialize(userRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
        catch(InvalidOperationException ex)
        {
            await _loggingService.LogError("Register User", "POST", "/api/UserManagement/RegisterUser", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, userRequest.CreatedBy, JsonSerializer.Serialize(userRequest));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Register User", "POST", "/api/UserManagement/RegisterUser", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, userRequest.CreatedBy, JsonSerializer.Serialize(userRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("RegisterUserDetails")]
    public async Task<IActionResult> GetUserByUserId(int StaffId)
    {
        try
        {
            var result = await _userService.GetUserByUserId(StaffId);
            var response = new
            {
                Success = true,
                Message = result
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

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        try
        {
            var result = await _userService.ChangePasswordAsync(model);
            var response = new
            {
                Success = true,
                Message = result
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

    [HttpGet("GetStaffDetails")]
    public async Task<IActionResult> GetStaffDetailsByUsername(string staffname)
    {
        try
        {
            var result = await _userService.GetStaffDetailsByStaffName(staffname);
            var response = new
            {
                Success = true,
                Message = result
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

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        try
        {
            var result = await _userService.ResetPasswordAsync(model);
            var response = new
            {
                Success = true,
                Message = result
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

    [HttpGet("GetStaffDetailsByStaffId")]
    public async Task<IActionResult> GetByStaffId(int staffId)
    {
        try
        {
            var result = await _userService.GetByUserId(staffId);
            var response = new
            {
                Success = true,
                Message = result
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
    [HttpPost("RemoveUser")]
    public async Task<IActionResult> DeactivateStaff(int staffId, int deletedBy)
    {
        try
        {
            var result = await _userService.DeactivateStaffByUserManagementIdAsync(staffId, deletedBy);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Remove User", "POST", "/api/UserManagement/RemoveUser", result, deletedBy, JsonSerializer.Serialize(new { staffId, deletedBy }));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Remove User", "POST", "/api/UserManagement/RemoveUser", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { staffId, deletedBy }));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Remove User", "POST", "/api/UserManagement/RemoveUser", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { staffId, deletedBy}));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
    [HttpGet("GetMenusByRoleId")]
    public async Task<IActionResult> GetMenusByRoleId(int roleId)
    {
        try
        {
            var menus = await _userService.GetMenusByRoleIdAsync(roleId);
            var response = new
            {
                Success = true,
                Menus = menus
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
