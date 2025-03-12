using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DailyReportsController : ControllerBase
{
    private readonly DailyReportsService _dailyReportsService;
    private readonly ILogger<DailyReportsController> _logger;

    public DailyReportsController(DailyReportsService dailyReportsService, ILogger<DailyReportsController> logger)
    {
        _dailyReportsService = dailyReportsService;
        _logger = logger;
    }

    [HttpPost("GetDailyReport")]
    public async Task<IActionResult> GetDailyReport([FromBody] DailyReportRequest request)
    {
        try
        {
            _logger.LogInformation("Fetching daily reports: {@Request}", request);

            var result = await _dailyReportsService.GetDailyReports(request);

            if (result == null || result.Count == 0)
            {
                _logger.LogWarning("No records found for the given criteria.");
                return NotFound(new { Message = "No records found." });
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid request parameters.");
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching daily reports.");
            return StatusCode(500, new { Message = "An error occurred while fetching data.", Error = ex.Message });
        }
    }

}
