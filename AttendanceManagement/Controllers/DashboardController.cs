using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILoggingService _loggingService;
    public DashboardController(IDashboardService dashboardService, ILoggingService loggingService)
    {
        _dashboardService = dashboardService;
        _loggingService = loggingService;

    }

    [HttpGet("Anniversaries")]
    public async Task<IActionResult> GetTodaysAnniversaries(int eventTypeId)
    {
        try
        {
            var anniversaries = await _dashboardService.GetTodaysAnniversaries(eventTypeId);
            var response = new
            {
                Success = true,
                Message = anniversaries
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

    [HttpGet("GetNewJoinee")]
    public async Task<IActionResult> GetNewJoinee()
    {
        try
        {
            var newJoinee = await _dashboardService.GetNewJoinee();
            var response = new
            {
                Success = true,
                Message = newJoinee
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

    [HttpGet("GetAllHolidays")]
    public async Task<IActionResult> GetAllHolidays(int staffId, int shiftTypeId)
    {
        try
        {
            var holidays = await _dashboardService.GetAllHolidaysAsync(staffId, shiftTypeId);
            var response = new
            {
                Success = true,
                Message = holidays
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

    [HttpPost("CreateAnnouncement")]
    public async Task<IActionResult> CreateAnnouncement(AnnouncementDto announcementDto)
    {
        try
        {
            var holidays = await _dashboardService.CreateAnnouncement(announcementDto);
            var response = new
            {
                Success = true,
                Message = holidays
            };
            await _loggingService.AuditLog("Announcement", "POST", "/api/Dashboard/CreateAnnouncement", holidays, announcementDto.CreatedBy, JsonSerializer.Serialize(announcementDto));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Announcement", "POST", "/api/Dashboard/CreateAnnouncement", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, announcementDto.CreatedBy, JsonSerializer.Serialize(announcementDto));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAnnouncement")]
    public async Task<IActionResult> GetAnnouncement()
    {
        try
        {
            var holidays = await _dashboardService.GetAnnouncement();
            var response = new
            {
                Success = true,
                Message = holidays
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

    [HttpGet("GetActiveAnnouncement")]
    public async Task<IActionResult> GetActiveAnnouncement()
    {
        try
        {
            var holidays = await _dashboardService.GetActiveAnnouncement();
            var response = new
            {
                Success = true,
                Message = holidays
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

    [HttpPost("UpdateAnnouncement")]
    public async Task<IActionResult> UpdateAnnouncement(AnnouncementResponse announcementResponse)
    {
        try
        {
            var holidays = await _dashboardService.UpdateAnnouncement(announcementResponse);
            var response = new
            {
                Success = true,
                Message = holidays
            };
            await _loggingService.AuditLog("Announcement", "POST", "/api/Dashboard/UpdateAnnouncement", holidays, announcementResponse.CreatedBy, JsonSerializer.Serialize(announcementResponse));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Announcement", "POST", "/api/Dashboard/UpdateAnnouncement", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, announcementResponse.CreatedBy, JsonSerializer.Serialize(announcementResponse));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Announcement", "POST", "/api/Dashboard/UpdateAnnouncement", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, announcementResponse.CreatedBy, JsonSerializer.Serialize(announcementResponse));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("HeadcountByDepartment")]
    public async Task<IActionResult> GetHeadCountByDepartment()
    {
        try
        {
            var result = await _dashboardService.GetHeadCountByDepartmentAsync();
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

    [HttpGet("GetLeaveDetails")]
    public async Task<IActionResult> GetLeaveDetails(int StaffId)
    {
        try
        {
            var leaveDetails = await _dashboardService.GetLeaveDetailsWithDefaultsAsync(StaffId);
            var response = new
            {
                Success = true,
                Message = leaveDetails
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

    [HttpGet("UpcomingShifts")]
    public async Task<IActionResult> GetUpcomingShifts(int staffId)
    {
        try
        {
            var upcomingShifts = await _dashboardService.GetUpcomingShiftsForStaffAsync(staffId);
            var response = new
            {
                Success = true,
                Message = upcomingShifts
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch(Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}
