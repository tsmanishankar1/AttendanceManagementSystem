using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationMasterController : ControllerBase
{
    private readonly LocationService _service;
    private readonly LoggingService _loggingService;

    public LocationMasterController(LocationService service, LoggingService loggingService)
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
        catch (Exception ex)
        {
            await _loggingService.LogError("Location Master", "POST", "/api/Location/UpdateLocation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, location.UpdatedBy, JsonSerializer.Serialize(location));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}