using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserManagementController : ControllerBase
{
    private readonly UserManagementService _userService;
    private readonly AttendanceManagementSystemContext _context;

    public UserManagementController(UserManagementService userService, AttendanceManagementSystemContext context)
    {
        _userService = userService;
        _context = context;

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

            AuditLog log = new AuditLog
            {
                Module = "RegisterUser",
                HttpMethod = "POST",
                ApiEndpoint = "/api/User/RegisterUser",
                SuccessMessage = "User registered successfully",
                Payload = System.Text.Json.JsonSerializer.Serialize(userRequest),
                StaffId = userRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "RegisterUser",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/User/RegisterUser",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = userRequest.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(userRequest),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "RegisterUser",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/User/RegisterUser",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = userRequest.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(userRequest),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
    public async Task<IActionResult> DeactivateStaff(int StaffId)
    {
        try
        {
            var result = await _userService.DeactivateStaffByUserManagementIdAsync(StaffId);
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
}
