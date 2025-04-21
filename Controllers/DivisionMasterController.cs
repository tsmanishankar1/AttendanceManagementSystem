using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DivisionMasterController : ControllerBase
{
    private readonly DivisionMasterService _service;
    private readonly LoggingService _loggingService;

    public DivisionMasterController(DivisionMasterService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllDivisions")]
    public async Task<IActionResult> GetAllDivisions()
    {
        try
        {
            var divisions = await _service.GetAllDivisions();
            var response = new
            {
                Success = true,
                Message = divisions
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

    [HttpPost("CreateDivision")]
    public async Task<IActionResult> AddDivision(DivisionRequest division)
    {
        try
        {
            var addDivision = await _service.AddDivision(division);
            var response = new
            {
                Success = true,
                Message = addDivision
            };
            await _loggingService.AuditLog("Division Master", "POST", "/api/DivisionMaster/CreateDivision", addDivision, division.CreatedBy, JsonSerializer.Serialize(division));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Division Master", "POST", "/api/DivisionMaster/CreateDivision", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, division.CreatedBy, JsonSerializer.Serialize(division));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateDivision")]
    public async Task<IActionResult> UpdateDivision(UpdateDivision division)
    {
        try
        {
            var update = await _service.UpdateDivision(division);
            var response = new
            {
                Success = true,
                Message = update
            };
            await _loggingService.AuditLog("Division Master", "POST", "/api/DivisionMaster/UpdateDivision", update, division.UpdatedBy, JsonSerializer.Serialize(division));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Division Master", "POST", "/api/DivisionMaster/UpdateDivision", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, division.UpdatedBy, JsonSerializer.Serialize(division));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Division Master", "POST", "/api/DivisionMaster/UpdateDivision", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, division.UpdatedBy, JsonSerializer.Serialize(division));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}