using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DesignationMasterController : ControllerBase
{
    private readonly IDesignationMasterInfra _service;
    private readonly ILoggingInfra _loggingService;

    public DesignationMasterController(IDesignationMasterInfra service, ILoggingInfra loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllDesignations")]
    public async Task<IActionResult> GetAllDesignations()
    {
        try
        {
            var designations = await _service.GetAllDesignations();
            var response = new
            {
                Success = true,
                Message = designations
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

    [HttpPost("CreateDesignation")]
    public async Task<IActionResult> AddDesignation(DesignationRequest designation)
    {
        try
        {
            var createdDesignation = await _service.AddDesignation(designation);
            var response = new
            {
                Success = true,
                Message = createdDesignation
            };
            await _loggingService.AuditLog("Designation Master", "POST", "/api/DesignationMaster/CreateDesignation", createdDesignation, designation.CreatedBy, JsonSerializer.Serialize(designation));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/api/DesignationMaster/CreateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.CreatedBy, JsonSerializer.Serialize(designation));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/api/DesignationMaster/CreateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.CreatedBy, JsonSerializer.Serialize(designation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateDesignation")]
    public async Task<IActionResult> UpdateDesignation(UpdateDesignation designation)
    {
        try
        {
            var success = await _service.UpdateDesignation(designation);
            var response = new
            {
                Success = true,
                Message = success
            };
            await _loggingService.AuditLog("Designation Master", "POST", "/api/DesignationMaster/UpdateDesignation", success, designation.UpdatedBy, JsonSerializer.Serialize(designation));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/api/DesignationMaster/UpdateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.UpdatedBy, JsonSerializer.Serialize(designation));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/api/DesignationMaster/UpdateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.UpdatedBy, JsonSerializer.Serialize(designation));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/api/DesignationMaster/UpdateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.UpdatedBy, JsonSerializer.Serialize(designation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}