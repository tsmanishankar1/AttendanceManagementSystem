using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ZoneMasterController : ControllerBase
{
    private readonly IZoneMasterApp _service;
    private readonly ILoggingApp _loggingService;

    public ZoneMasterController(IZoneMasterApp service, ILoggingApp loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllZones")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var zones = await _service.GetAllZonesAsync();
            var response = new 
            { 
                Success = true, 
                Message = zones 
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

    [HttpPost("CreateZone")]
    public async Task<IActionResult> Create(ZoneMasterRequest zoneMaster)
    {
        try
        {
            var createdZone = await _service.CreateZoneAsync(zoneMaster);
            var response = new 
            { 
                Success = true, 
                Message = createdZone 
            };
            await _loggingService.AuditLog("Zone Master", "POST", "/api/ZoneMaster/CreateZone", createdZone, zoneMaster.CreatedBy, JsonSerializer.Serialize(zoneMaster));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Zone Master", "POST", "/api/ZoneMaster/CreateZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, zoneMaster.CreatedBy, JsonSerializer.Serialize(zoneMaster));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Zone Master", "POST", "/api/ZoneMaster/CreateZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, zoneMaster.CreatedBy, JsonSerializer.Serialize(zoneMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateZone")]
    public async Task<IActionResult> Update(UpdateZoneMaster zoneMaster)
    {
        try
        {
            var updatedZone = await _service.UpdateZoneAsync(zoneMaster);
            var response = new { Success = true, Message = updatedZone };
            await _loggingService.AuditLog("Zone Master", "POST", "/api/ZoneMaster/UpdateZone", updatedZone, zoneMaster.UpdatedBy, JsonSerializer.Serialize(zoneMaster));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Zone Master", "POST", "/api/ZoneMaster/UpdateZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, zoneMaster.UpdatedBy, JsonSerializer.Serialize(zoneMaster));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Zone Master", "POST", "/api/ZoneMaster/UpdateZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, zoneMaster.UpdatedBy, JsonSerializer.Serialize(zoneMaster));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Zone Master", "POST", "/api/ZoneMaster/UpdateZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, zoneMaster.UpdatedBy, JsonSerializer.Serialize(zoneMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}