using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkstationMasterController : ControllerBase
{
    private readonly WorkstationMasterService _service;
    private readonly LoggingService _loggingService;

    public WorkstationMasterController(WorkstationMasterService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllWorkStations")]
    public async Task<IActionResult> GetAllWorkstations()
    {
        try
        {
            var workstations = await _service.GetAllWorkstationsAsync();
            var response = new
            {
                Success = true,
                Message = workstations
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

    [HttpGet("GetWorkstationById")]
    public async Task<IActionResult> GetWorkstationById(int workstationMasterId)
    {
        try
        {
            var workstation = await _service.GetWorkstationByIdAsync(workstationMasterId);
            var response = new
            {
                Success = true,
                Message = workstation
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

    [HttpPost("CreateWorkstation")]
    public async Task<IActionResult> CreateWorkstation(WorkStationRequest workstation)
    {
        try
        {
            var createdWorkstation = await _service.CreateWorkstationAsync(workstation);
            var response = new
            {
                Success = true,
                Message = createdWorkstation
            };
            await _loggingService.AuditLog("Workstation Master", "POST", "/api/WorkstationMaster/CreateWorkstation", createdWorkstation, workstation.CreatedBy, JsonSerializer.Serialize(workstation));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Workstation Master", "POST", "/api/WorkstationMaster/CreateWorkstation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, workstation.CreatedBy, JsonSerializer.Serialize(workstation));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Workstation Master", "POST", "/api/WorkstationMaster/CreateWorkstation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, workstation.CreatedBy, JsonSerializer.Serialize(workstation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateWorkstation")]
    public async Task<IActionResult> UpdateWorkstation(UpdateWorkStation updatedWorkstation)
    {
        try
        {
            var success = await _service.UpdateWorkstationAsync(updatedWorkstation);
            var response = new
            {
                Success = true,
                Message = success
            };
            await _loggingService.AuditLog("Workstation Master", "POST", "/api/WorkstationMaster/UpdateWorkstation", success, updatedWorkstation.UpdatedBy, JsonSerializer.Serialize(updatedWorkstation));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Workstation Master", "POST", "/api/WorkstationMaster/UpdateWorkstation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedWorkstation.UpdatedBy, JsonSerializer.Serialize(updatedWorkstation));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Workstation Master", "POST", "/api/WorkstationMaster/UpdateWorkstation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedWorkstation.UpdatedBy, JsonSerializer.Serialize(updatedWorkstation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}
