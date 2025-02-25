using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeeklyOffController : ControllerBase
{
    private readonly WeeklyOffService _weeklyOffService;
    private readonly AttendanceManagementSystemContext _context;

    public WeeklyOffController(WeeklyOffService weeklyOffService, AttendanceManagementSystemContext context)
    {
        _weeklyOffService = weeklyOffService;
        _context = context;
    }

    [HttpGet("GetAllWeeklyOffs")]
    public async Task<IActionResult> GetAllWeeklyOffs()
    {
        try
        {
            var weeklyOffs = await _weeklyOffService.GetAllWeeklyOffsAsync();
            var response = new
            {
                Success = true,
                Message = weeklyOffs
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

    [HttpGet("GetWeeklyOffById")]
    public async Task<IActionResult> GetWeeklyOffById(int weeklyOffId)
    {
        try
        {
            var weeklyOff = await _weeklyOffService.GetWeeklyOffByIdAsync(weeklyOffId);
            var response = new
            {
                Success = true,
                Message = weeklyOff
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

    [HttpPost("AddWeeklyOff")]
    public async Task<IActionResult> CreateWeeklyOff(WeeklyOffRequest weeklyOff)
    {
        try
        {
            var createdWeeklyOff = await _weeklyOffService.CreateWeeklyOffAsync(weeklyOff);
            var response = new
            {
                Success = true,
                Message = createdWeeklyOff
            };

            AuditLog log = new AuditLog
            {
                Module = "CreateWeeklyOff",
                HttpMethod = "POST",
                ApiEndpoint = "/api/WeeklyOff/AddWeeklyOff",
                SuccessMessage = "Created weekly off successfully",
                Payload = System.Text.Json.JsonSerializer.Serialize(weeklyOff),
                StaffId = weeklyOff.CreatedBy,
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
                    Module = "CreateWeeklyOff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WeeklyOff/AddWeeklyOff",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = weeklyOff.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(weeklyOff),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "CreateWeeklyOff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WeeklyOff/AddWeeklyOff",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = weeklyOff.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(weeklyOff),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateWeeklyOff")]
    public async Task<IActionResult> UpdateWeeklyOff(UpdateWeeklyOff updatedWeeklyOff)
    {
        try
        {
            var updated = await _weeklyOffService.UpdateWeeklyOffAsync(updatedWeeklyOff);
            var response = new
            {
                Success = true,
                Message = updated
            };
            AuditLog log = new AuditLog
            {
                Module = "UpdateWeeklyOff",
                HttpMethod = "POST",
                ApiEndpoint = "/api/WeeklyOff/UpdateWeeklyOff",
                SuccessMessage = "Updated weekly off successfully",
                Payload = System.Text.Json.JsonSerializer.Serialize(updatedWeeklyOff),
                StaffId = updatedWeeklyOff.UpdatedBy,
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
                    Module = "UpdateWeeklyOff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WeeklyOff/UpdateWeeklyOff",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedWeeklyOff.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedWeeklyOff),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "UpdateWeeklyOff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WeeklyOff/UpdateWeeklyOff",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedWeeklyOff.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedWeeklyOff),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}
