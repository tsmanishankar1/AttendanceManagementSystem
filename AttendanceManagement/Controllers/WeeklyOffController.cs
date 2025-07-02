using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeeklyOffController : ControllerBase
{
    private readonly IWeeklyOffService _weeklyOffService;
    private readonly ILoggingService _loggingService;

    public WeeklyOffController(IWeeklyOffService weeklyOffService, ILoggingService loggingService)
    {
        _weeklyOffService = weeklyOffService;
        _loggingService = loggingService;
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
            await _loggingService.AuditLog("Create WeeklyOff", "POST", "/api/WeeklyOff/AddWeeklyOff", createdWeeklyOff, weeklyOff.CreatedBy, JsonSerializer.Serialize(weeklyOff));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Create WeeklyOff", "POST", "/api/WeeklyOff/AddWeeklyOff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, weeklyOff.CreatedBy, JsonSerializer.Serialize(weeklyOff));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Create WeeklyOff", "POST", "/api/WeeklyOff/AddWeeklyOff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, weeklyOff.CreatedBy, JsonSerializer.Serialize(weeklyOff));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Create WeeklyOff", "POST", "/api/WeeklyOff/AddWeeklyOff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, weeklyOff.CreatedBy, JsonSerializer.Serialize(weeklyOff));
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
            await _loggingService.AuditLog("Update WeeklyOff", "POST", "/api/WeeklyOff/UpdateWeeklyOff", updated, updatedWeeklyOff.UpdatedBy, JsonSerializer.Serialize(updatedWeeklyOff));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Update WeeklyOff", "POST", "/api/WeeklyOff/UpdateWeeklyOff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedWeeklyOff.UpdatedBy, JsonSerializer.Serialize(updatedWeeklyOff));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Update WeeklyOff", "POST", "/api/WeeklyOff/UpdateWeeklyOff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedWeeklyOff.UpdatedBy, JsonSerializer.Serialize(updatedWeeklyOff));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Update WeeklyOff", "POST", "/api/WeeklyOff/UpdateWeeklyOff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedWeeklyOff.UpdatedBy, JsonSerializer.Serialize(updatedWeeklyOff));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}