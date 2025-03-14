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

}
