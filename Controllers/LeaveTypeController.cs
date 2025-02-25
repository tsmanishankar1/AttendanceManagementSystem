using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveTypeController : ControllerBase
{
    private readonly LeaveTypeService _leaveTypeService;
    private readonly AttendanceManagementSystemContext _context;

    public LeaveTypeController(LeaveTypeService leaveTypeService, AttendanceManagementSystemContext context)
    {
        _leaveTypeService = leaveTypeService;
        _context = context;
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

    [HttpGet("GetLeaveTypeById")]
    public async Task<IActionResult> GetLeaveTypeById(int leaveTypeId)
    {
        try
        {
            var leaveType = await _leaveTypeService.GetLeaveTypeByIdAsync(leaveTypeId);
            var response = new
            {
                Success = true,
                Message = leaveType
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
            AuditLog log = new AuditLog
            {
                Module = "Leave Type",
                HttpMethod = "POST",
                ApiEndpoint = "/LeaveType/CreateLeaveType",
                SuccessMessage = createdLeaveType,
                Payload = System.Text.Json.JsonSerializer.Serialize(leaveType),
                StaffId = leaveType.CreatedBy,
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
                    Module = "Leave Type",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveType/CreateLeaveType",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = leaveType.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(leaveType),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "Leave Type",
                HttpMethod = "POST",
                ApiEndpoint = "/LeaveType/UpdateLeaveType",
                SuccessMessage = updated,
                Payload = System.Text.Json.JsonSerializer.Serialize(leaveType),
                StaffId = leaveType.UpdatedBy,
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
                    Module = "Leave Type",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveType/UpdateLeaveType",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = leaveType.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(leaveType),
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
                    Module = "Leave Type",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveType/UpdateLeaveType",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = leaveType.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(leaveType),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}