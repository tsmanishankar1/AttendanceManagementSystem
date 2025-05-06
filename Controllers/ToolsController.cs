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

    [HttpGet("GetStaffInfoByOrganizationType")]
    public async Task<IActionResult> GetStaffInfoByOrganizationType(int organizationTypeId)
    {
        try
        {
            var staffInfo = await _service.GetStaffInfoByOrganizationTypeAsync(organizationTypeId);
            var response = new
            {
                Success = true,
                Message = staffInfo
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

    [HttpPost("GetStaffInfo")]
    public async Task<IActionResult> GetStaffInfoByStaffId(List<int> staffIds)
    {
        try
        {
            var staffInfo = await _service.GetStaffInfoByStaffId(staffIds);
            var response = new
            {
                Success = true,
                Message = staffInfo
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
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
            await _loggingService.AuditLog("Assign Leave Type", "POST", "/api/Tools/CreateAssignLeaveType", createdAssignLeaveType, assignLeaveType.CreatedBy, JsonSerializer.Serialize(assignLeaveType));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Assign Leave Type", "POST", "/api/Tools/CreateAssignLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignLeaveType.CreatedBy, JsonSerializer.Serialize(assignLeaveType));
            return ErrorClass.ErrorResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Assign Leave Type", "POST", "/api/Tools/CreateAssignLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignLeaveType.CreatedBy, JsonSerializer.Serialize(assignLeaveType));
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
            await _loggingService.AuditLog("Assign Leave Type", "POST", "/api/Tools/UpdateAssignLeaveType", updatedAssignLeaveType, assignLeaveType.UpdatedBy, JsonSerializer.Serialize(assignLeaveType));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Assign Leave Type", "POST", "/api/Tools/UpdateAssignLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignLeaveType.UpdatedBy, JsonSerializer.Serialize(assignLeaveType));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Assign Leave Type", "POST", "/api/Tools/UpdateAssignLeaveType", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignLeaveType.UpdatedBy, JsonSerializer.Serialize(assignLeaveType));
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
            await _loggingService.AuditLog("Leave Credit Debit", "POST", "/api/Tools/AddMultipleLeaveCreditDebit", message, leaveCreditDebit.CreatedBy, JsonSerializer.Serialize(leaveCreditDebit));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Credit Debit", "POST", "/api/Tools/AddMultipleLeaveCreditDebit", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveCreditDebit.CreatedBy, JsonSerializer.Serialize(leaveCreditDebit));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("ReaderConfiguration")]
    public async Task<IActionResult> CreateReader(ReaderConfigurationRequest readerConfiguration)
    {
        try
        {
            var ReaderConfig = await _service.AddReaderConfigurationAsync(readerConfiguration);
            var response = new
            {
                Success = true,
                Message = ReaderConfig
            };
            await _loggingService.AuditLog("Reader Configuration", "POST", "/api/Tools/ReaderConfiguration", ReaderConfig, readerConfiguration.CreatedBy, JsonSerializer.Serialize(readerConfiguration));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Reader Configuration", "POST", "/api/Tools/ReaderConfiguration", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, readerConfiguration.CreatedBy, JsonSerializer.Serialize(readerConfiguration));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAllReaderConfigurations")]
    public async Task<IActionResult> GetReaderConfigurations()
    {
        try
        {
            var result = await _service.GetReaderConfigurationsAsync();
            var response = new
            {
                Success = true,
                Message = result
            };
            return Ok(result);
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

    [HttpPost("AttendanceRegularization")]
    public async Task<IActionResult> UpdateAttendanceStatus(UpdateAttendanceStatusRequest request)
    {
        try
        {
            string result = await _service.UpdateAttendanceStatusAsync(request);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Attendance Regularization", "POST", "/api/Tools/AtendanceStatus", result, request.CreatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Attendance Regularization", "POST", "/api/Tools/AttendanceStatus", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AttendanceStatusColor")]
    public async Task<IActionResult> CreateAttendanceStatusColor( AttendanceStatusColorDto dto)
    {
        try
        {
            var createdAttendanceStatus = await _service.CreateAttendanceStatusColorAsync(dto);

            var response = new
            {
                Success = true,
                Message = createdAttendanceStatus
            };

            await _loggingService.AuditLog("Attendance Regularization", "POST", "/api/Tools/AttendanceStatus", JsonSerializer.Serialize(response), dto.CreatedBy, JsonSerializer.Serialize(dto));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError( "Attendance Regularization", "POST", "/api/Tools/AttendanceStatus",  ex.Message, ex.StackTrace ?? string.Empty,    ex.InnerException?.ToString() ?? string.Empty,dto.CreatedBy,   JsonSerializer.Serialize(dto)  );
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAttendanceStatusColor")]
    public async Task<IActionResult> GetAttendanceStatusColor()
    {
        try
        {
            var result = await _service.GetAttendanceStatusColor();
            var response = new
            {
                Success = true,
                Message = result
            };
            return Ok(response);
        }
        catch(MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch(Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateAttendanceStatusColor")]
    public async Task<IActionResult> UpdateAttendanceStatusColor(UpdateAttendanceStatusColor dto)
    {
        try
        {
            var createdAttendanceStatus = await _service.UpdateAttendanceStatusColor(dto);

            var response = new
            {
                Success = true,
                Message = createdAttendanceStatus
            };

            await _loggingService.AuditLog("Attendance Regularization", "POST", "/api/Tools/UpdateAttendanceStatusColor", JsonSerializer.Serialize(response), dto.UpdatedBy, JsonSerializer.Serialize(dto));
            return Ok(response);
        }
        catch(MessageNotFoundException ex)
        {
            await _loggingService.LogError("Attendance Regularization", "POST", "/api/Tools/UpdateAttendanceStatusColor", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dto.UpdatedBy, JsonSerializer.Serialize(dto));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Attendance Regularization", "POST", "/api/Tools/UpdateAttendanceStatusColor", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dto.UpdatedBy, JsonSerializer.Serialize(dto));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}