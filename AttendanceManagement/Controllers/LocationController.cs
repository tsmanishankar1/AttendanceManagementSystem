using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationMasterController : ControllerBase
{
    private readonly ILocationApp _service;
    private readonly ILoggingApp _loggingService;

    public LocationMasterController(ILocationApp service, ILoggingApp loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllLocations")]
    public async Task<IActionResult> GetAllLocations()
    {
        try
        {
            var locations = await _service.GetAllLocationMastersAsync();
            var response = new
            {
                Success = true,
                Message = locations
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

    [HttpPost("CreateLocation")]
    public async Task<IActionResult> CreateLocation(LocationRequest location)
    {
        try
        {
            var createdLocation = await _service.CreateLocationMasterAsync(location);
            var response = new
            {
                Success = true,
                Message = createdLocation
            };
            await _loggingService.AuditLog("Location Master", "POST", "/api/Location/CreateLocation", createdLocation, location.CreatedBy, JsonSerializer.Serialize(location));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Location Master", "POST", "/api/Location/CreateLocation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, location.CreatedBy, JsonSerializer.Serialize(location));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Location Master", "POST", "/api/Location/CreateLocation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, location.CreatedBy, JsonSerializer.Serialize(location));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateLocation")]
    public async Task<IActionResult> UpdateLocation(UpdateLocation location)
    {
        try
        {
            var updatedLocation = await _service.UpdateLocationMasterAsync(location);
            var response = new
            {
                Success = true,
                Message = updatedLocation
            };
            await _loggingService.AuditLog("Location Master", "POST", "/api/Location/UpdateLocation", updatedLocation, location.UpdatedBy, JsonSerializer.Serialize(location));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Location Master", "POST", "/api/Location/UpdateLocation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, location.UpdatedBy, JsonSerializer.Serialize(location));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Location Master", "POST", "/api/Location/UpdateLocation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, location.UpdatedBy, JsonSerializer.Serialize(location));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Location Master", "POST", "/api/Location/UpdateLocation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, location.UpdatedBy, JsonSerializer.Serialize(location));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}