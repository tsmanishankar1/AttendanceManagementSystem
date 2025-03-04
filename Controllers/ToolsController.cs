using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Input_Models;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToolsController : ControllerBase
{
    private readonly ToolsService _service;
    private readonly LoggingService _loggingService;

    public ToolsController(ToolsService toolsService, LoggingService loggingService)
    {
        _service = toolsService;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllApplicationTypes")]
    public async Task<IActionResult> GetAllApplicationTypes()
    {
        try
        {
            var applicationTypes = await _service.GetAllApplicationTypesAsync();
            var response = new
            {
                Success = true,
                Message = applicationTypes
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

    [HttpGet("GetStaffInfoByOrganizationType")]
    public async Task<IActionResult> GetStaffInfoByOrganizationType(int organizationTypeId)
    {
        try
        {
            var staffInfo = await _service.GetStaffInfoByOrganizationTypeAsync(organizationTypeId);
            if (staffInfo == null || staffInfo.Count == 0)
            {
                return NotFound("No staff found for the given organization type.");
            }
            return Ok(staffInfo);
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
    [HttpPost("GetStaffInfo")]
    public async Task<IActionResult> GetStaffInfoByStaffId([FromBody] List<int> staffIds)
    {
        try
        {
            if (staffIds == null || !staffIds.Any())
            {
                return BadRequest("Staff ID list cannot be empty.");
            }
            var staffInfo = await _service.GetStaffInfoByStaffId(staffIds);
            return Ok(staffInfo);
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

    [HttpGet("GetAllAssignLeaveTypes")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var assignLeaveTypes = await _service.GetAllAssignLeaveTypes();
            var response = new
            {
                Success = true,
                Message = assignLeaveTypes
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

    [HttpGet("GetAssignLeaveTypeById")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var assignLeaveType = await _service.GetAssignLeaveTypeById(id);
            var response = new
            {
                Success = true,
                Message = assignLeaveType
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

    [HttpPost("CreateAssignLeaveType")]
    public async Task<IActionResult> Create(CreateAssignLeaveTypeDTO assignLeaveType)
    {
        try
        {
            var createdAssignLeaveType = await _service.CreateAssignLeaveType(assignLeaveType);
            var response = new
            {
                Success = true,
                Message = createdAssignLeaveType
            };
            await _loggingService.AuditLog("Assign Leave Type", "POST", "/AssignLeaveType/CreateAssignLeaveType", "Assign LeaveType Created Successfully", assignLeaveType.CreatedBy, JsonSerializer.Serialize(assignLeaveType));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Assign Leave Type", "POST", "/AssignLeaveType/CreateAssignLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignLeaveType.CreatedBy, JsonSerializer.Serialize(assignLeaveType));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateAssignLeaveType")]
    public async Task<IActionResult> Update(UpdateAssignLeaveTypeDTO assignLeaveType)
    {
        try
        {
            var updatedAssignLeaveType = await _service.UpdateAssignLeaveType(assignLeaveType);
            var response = new
            {
                Success = true,
                Message = updatedAssignLeaveType
            };
            await _loggingService.AuditLog("Assign Leave Type", "POST", "/AssignLeaveType/UpdateAssignLeaveType", "AssignLeaveType Updated Successfully", assignLeaveType.UpdatedBy, JsonSerializer.Serialize(assignLeaveType));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Assign Leave Type", "POST", "/AssignLeaveType/UpdateAssignLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignLeaveType.UpdatedBy, JsonSerializer.Serialize(assignLeaveType));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Assign Leave Type", "POST", "/AssignLeaveType/UpdateAssignLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignLeaveType.UpdatedBy, JsonSerializer.Serialize(assignLeaveType));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
    [HttpPost("AddMultipleLeaveCreditDebit")]
    public async Task<IActionResult> AddLeaveCreditDebitForMultipleStaff(LeaveCreditDebitRequest leaveCreditDebit)
    {
        try
        {
            var message = await _service.AddLeaveCreditDebitForMultipleStaffAsync(leaveCreditDebit);
            var response = new
            {
                Success = true,
                Message = message
            };
            await _loggingService.AuditLog("Leave Credit Debit", "POST", "/LeaveCreditDebit/CreateLeaveCreditDebitForMultipleStaff", message, leaveCreditDebit.CreatedBy, JsonSerializer.Serialize(leaveCreditDebit));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Credit Debit", "POST", "/LeaveCreditDebit/CreateLeaveCreditDebitForMultipleStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveCreditDebit.CreatedBy, JsonSerializer.Serialize(leaveCreditDebit));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddApplicationType")]
    public async Task<IActionResult> CreateApplicationType(ApplicationTypeRequest applicationType)
    {
        try
        {
            var createdApplicationType = await _service.AddApplicationTypeAsync(applicationType);
            var response = new
            {
                Success = true,
                Message = createdApplicationType
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}