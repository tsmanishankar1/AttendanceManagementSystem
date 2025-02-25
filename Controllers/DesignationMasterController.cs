using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DesignationMasterController : ControllerBase
{
    private readonly DesignationMasterService _service;
    private readonly LoggingService _loggingService;

    public DesignationMasterController(DesignationMasterService service, LoggingService loggingService)
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

    [HttpGet("GetDesignationById")]
    public async Task<IActionResult> GetDesignationById(int designationMasterId)
    {
        try
        {
            var designation = await _service.GetDesignationById(designationMasterId);
            var response = new
            {
                Success = true,
                Message = designation
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
            await _loggingService.AuditLog("Designation Master", "POST", "/DesignationMaster/CreateDesignation", createdDesignation, designation.CreatedBy, JsonSerializer.Serialize(designation));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/DesignationMaster/CreateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.CreatedBy, JsonSerializer.Serialize(designation));
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
            await _loggingService.AuditLog("Designation Master", "POST", "/DesignationMaster/UpdateDesignation", success, designation.UpdatedBy, JsonSerializer.Serialize(designation));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/DesignationMaster/UpdateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.UpdatedBy, JsonSerializer.Serialize(designation));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Designation Master", "POST", "/DesignationMaster/UpdateDesignation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, designation.UpdatedBy, JsonSerializer.Serialize(designation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}