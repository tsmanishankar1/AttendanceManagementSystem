using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveGroupController : ControllerBase
{
    private readonly ILeaveGroupApp _service;
    private readonly ILoggingApp _loggingService;

    public LeaveGroupController(ILeaveGroupApp service, ILoggingApp loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllLeaveGroups")]
    public async Task<IActionResult> GetAllLeaveGroups()
    {
        try
        {
            var leaveGroups = await _service.GetAllLeaveGroups();
            var response = new
            {
                Success = true,
                Message = leaveGroups
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

    [HttpPost("AddLeaveGroup")]
    public async Task<IActionResult> AddLeaveGroup(AddLeaveGroupDto addLeaveGroupDto)
    {
        try
        {
            var addLeave = await _service.AddLeaveGroupWithTransactionsAsync(addLeaveGroupDto);
            var response = new
            {
                Success = true,
                Message = addLeave
            };
            await _loggingService.AuditLog("Leave Group", "POST", "/api/LeaveGroup/AddLeaveGroup", addLeave, addLeaveGroupDto.CreatedBy, JsonSerializer.Serialize(addLeaveGroupDto));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Leave Group", "POST", "/api/LeaveGroup/AddLeaveGroup", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, addLeaveGroupDto.CreatedBy, JsonSerializer.Serialize(addLeaveGroupDto));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Group", "POST", "/api/LeaveGroup/AddLeaveGroup", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, addLeaveGroupDto.CreatedBy, JsonSerializer.Serialize(addLeaveGroupDto));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateLeaveGroup")]
    public async Task<IActionResult> UpdateLeaveGroup(UpdateLeaveGroup leaveGroup)
    {
        try
        {
            var updatedLeaveGroup = await _service.UpdateLeaveGroup(leaveGroup);
            var response = new
            {
                Success = true,
                Message = updatedLeaveGroup
            };
            await _loggingService.AuditLog("Leave Group", "POST", "/api/LeaveGroup/UpdateLeaveGroup", updatedLeaveGroup, leaveGroup.UpdatedBy, JsonSerializer.Serialize(leaveGroup));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Leave Group", "POST", "/api/LeaveGroup/UpdateLeaveGroup", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveGroup.UpdatedBy, JsonSerializer.Serialize(leaveGroup));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Leave Group", "POST", "/api/LeaveGroup/UpdateLeaveGroup", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveGroup.UpdatedBy, JsonSerializer.Serialize(leaveGroup));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Group", "POST", "/api/LeaveGroup/UpdateLeaveGroup", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveGroup.UpdatedBy, JsonSerializer.Serialize(leaveGroup));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}