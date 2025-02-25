using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("GetAllEventTypes")]
    public async Task<IActionResult> GetAllEventTypes()
    {
        try
        {
            var events = await _dashboardService.GetAllEventTypes();
            var response = new
            {
                Success = true,
                Message = events
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
    public async Task<IActionResult> GetAllHolidays()
    {
        try
        {
            var holidays = await _dashboardService.GetAllHolidaysAsync();
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
}