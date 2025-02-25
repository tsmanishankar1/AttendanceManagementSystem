using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveGroupController : ControllerBase
{
    private readonly LeaveGroupService _service;
    private readonly AttendanceManagementSystemContext _context;

    public LeaveGroupController(LeaveGroupService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
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

    [HttpGet("GetLeaveGroupById")]
    public async Task<IActionResult> GetLeaveGroupById(int leaveGroupId)
    {
        try
        {
            var leaveGroup = await _service.GetLeaveGroupDetailsById(leaveGroupId);
            var response = new
            {
                Success = true,
                Message = leaveGroup
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
            AuditLog log = new AuditLog
            {
                Module = "Leave Group",
                HttpMethod = "POST",
                ApiEndpoint = "/LeaveGroup/CreateLeaveGroup",
                SuccessMessage = addLeave,
                Payload = System.Text.Json.JsonSerializer.Serialize(addLeaveGroupDto),
                StaffId = addLeaveGroupDto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Leave Group",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveGroup/CreateLeaveGroup",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = addLeaveGroupDto.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(addLeaveGroupDto),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "Leave Group",
                HttpMethod = "POST",
                ApiEndpoint = "/LeaveGroup/UpdateLeaveGroup",
                SuccessMessage = updatedLeaveGroup,
                Payload = System.Text.Json.JsonSerializer.Serialize(leaveGroup),
                StaffId = leaveGroup.UpdatedBy,
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
                    Module = "Leave Group",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveGroup/UpdateLeaveGroup",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = leaveGroup.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(leaveGroup),
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
                    Module = "Leave Group",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveGroup/UpdateLeaveGroup",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = leaveGroup.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(leaveGroup),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}