using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveTypeController : ControllerBase
{
    private readonly ILeaveTypeInfra _leaveTypeService;
    private readonly ILoggingInfra _loggingService;

    public LeaveTypeController(ILeaveTypeInfra leaveTypeService, ILoggingInfra loggingService)
    {
        _leaveTypeService = leaveTypeService;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllLeaveTypes")]
    public async Task<IActionResult> GetAllLeaveTypes()
    {
        try
        {
            var leaveTypes = await _leaveTypeService.GetAllLeaveTypesAsync();
            var response = new
            {
                Success = true,
                Message = leaveTypes
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

    [HttpPost("CreateLeaveType")]
    public async Task<IActionResult> CreateLeaveType(LeaveTypeRequest leaveType)
    {
        try
        {
            var createdLeaveType = await _leaveTypeService.CreateLeaveTypeAsync(leaveType);
            var response = new
            {
                Success = true,
                Message = createdLeaveType
            };
            await _loggingService.AuditLog("Leave Type", "POST", "/api/LeaveType/CreateLeaveType", createdLeaveType, leaveType.CreatedBy, JsonSerializer.Serialize(leaveType));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Type", "POST", "/api/LeaveType/CreateLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveType.CreatedBy, JsonSerializer.Serialize(leaveType));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateLeaveType")]
    public async Task<IActionResult> UpdateLeaveType(UpdateLeaveType leaveType)
    {
        try
        {
            var updated = await _leaveTypeService.UpdateLeaveTypeAsync(leaveType);
            var response = new
            {
                Success = true,
                Message = updated
            };
            await _loggingService.AuditLog("Leave Type", "POST", "/api/LeaveType/UpdateLeaveType", updated, leaveType.UpdatedBy, JsonSerializer.Serialize(leaveType));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Leave Type", "POST", "/api/LeaveType/UpdateLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveType.UpdatedBy, JsonSerializer.Serialize(leaveType));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Type", "POST", "/api/LeaveType/UpdateLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveType.UpdatedBy, JsonSerializer.Serialize(leaveType));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}