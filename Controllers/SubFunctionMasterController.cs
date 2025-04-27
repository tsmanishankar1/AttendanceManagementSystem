using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubFunctionMasterController : ControllerBase
{
    private readonly SubFunctionMasterService _service;
    private readonly LoggingService _loggingService;
    public SubFunctionMasterController(SubFunctionMasterService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllSubFunctions")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var subFunctions = await _service.GetAllSubFunctionsAsync();
            var response = new
            {
                Success = true,
                Message = subFunctions
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

    [HttpPost("CreateSubFunction")]
    public async Task<IActionResult> Create(SubFunctionRequest subFunctionMaster)
    {
        try
        {
            var createdSubFunction = await _service.CreateSubFunctionAsync(subFunctionMaster);
            var response = new
            {
                Success = true,
                Message = createdSubFunction
            };
            await _loggingService.AuditLog("SubFunction Master", "POST", "/api/SubFunctionMaster/CreateSubFunction", createdSubFunction, subFunctionMaster.CreatedBy, JsonSerializer.Serialize(subFunctionMaster));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("SubFunction Master", "POST", "/api/SubFunctionMaster/CreateSubFunction", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, subFunctionMaster.CreatedBy, JsonSerializer.Serialize(subFunctionMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateSubFunction")]
    public async Task<IActionResult> Update(UpdateSubFunction subFunctionMaster)
    {
        try
        {
            var updatedSubFunction = await _service.UpdateSubFunctionAsync(subFunctionMaster);
            var response = new
            {
                Success = true,
                Message = updatedSubFunction
            };
            await _loggingService.AuditLog("SubFunction Master", "POST", "/api/SubFunctionMaster/UpdateSubFunction", updatedSubFunction, subFunctionMaster.UpdatedBy, JsonSerializer.Serialize(subFunctionMaster));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("SubFunction Master", "POST", "/api/SubFunctionMaster/UpdateSubFunction", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, subFunctionMaster.UpdatedBy, JsonSerializer.Serialize(subFunctionMaster));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("SubFunction Master", "POST", "/api/SubFunctionMaster/UpdateSubFunction", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, subFunctionMaster.UpdatedBy, JsonSerializer.Serialize(subFunctionMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}