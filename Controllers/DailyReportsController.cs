using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DailyReportsController : ControllerBase
{
    private readonly DailyReportsService _dailyReportsService;
    private readonly LoggingService _loggingService;

    public DailyReportsController(DailyReportsService dailyReportsService, LoggingService loggingService)
    {
        _dailyReportsService = dailyReportsService;
        _loggingService = loggingService;

    }

    [HttpGet("GetReportType")]
    public async Task<IActionResult> GetReportType()
    {
        try
        {
            var result = await _dailyReportsService.GetReportType();
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

    [HttpPost("GetDailyReport")]
    public async Task<IActionResult> GetDailyReport(DailyReportRequest request)
    {
        try
        {
            var result = await _dailyReportsService.GetDailyReports(request);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Daily Report", "POST", "/api/DailyReports/GetDailyReport", "Report retrieved successfully", request.CreatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Daily Report", "POST", "/api/DailyReports/GetDailyReport", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Daily Report", "POST", "/api/DailyReports/GetDailyReport", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddWorkingTypeAmount")]
    public async Task<IActionResult> AddWorkingTypeAmount(WorkingTypeAmountRequest request)
    {
        try
        {
            var result = await _dailyReportsService.AddWorkingTypeAmount(request);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Working Type Amount", "POST", "/api/Attendance/AddWorkingTypeAmount", result, request.CreatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Working Type Amount", "POST", "/api/Attendance/AddWorkingTypeAmount", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Working Type Amount", "POST", "/api/Attendance/AddWorkingTypeAmount", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetWorkingTypeAmount")]
    public async Task<IActionResult> GetWorkingTypeAmount()
    {
        try
        {
            var result = await _dailyReportsService.GetWorkingTypeAmount();
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

    [HttpPost("UpdateWorkingTypeAmount")]
    public async Task<IActionResult> UpdateWorkingTypeAmount(UpdateWorkingTypeAmount request)
    {
        try
        {
            var result = await _dailyReportsService.UpdateWorkingTypeAmount(request);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Working Type Amount", "POST", "/api/Attendance/UpdateWorkingTypeAmount", result, request.UpdatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Working Type Amount", "POST", "/api/Attendance/UpdateWorkingTypeAmount", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Working Type Amount", "POST", "/api/Attendance/UpdateWorkingTypeAmount", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}