using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationMasterController : ControllerBase
{
    private readonly LocationService _service;
    private readonly AttendanceManagementSystemContext _context;

    public LocationMasterController(LocationService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
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

    [HttpGet("GetLocationById")]
    public async Task<IActionResult> GetLocationById(int locationMasterId)
    {
        try
        {
            var location = await _service.GetLocationMasterByIdAsync(locationMasterId);
            var response = new
            {
                Success = true,
                Message = location
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
            AuditLog log = new AuditLog
            {
                Module = "Location Master",
                HttpMethod = "POST",
                ApiEndpoint = "/LocationMaster/CreateLocation",
                SuccessMessage = createdLocation,
                Payload = System.Text.Json.JsonSerializer.Serialize(location),
                StaffId = location.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Location Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LocationMaster/CreateLocation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = location.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(location),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "Location Master",
                HttpMethod = "POST",
                ApiEndpoint = "/LocationMaster/UpdateLocation",
                SuccessMessage = updatedLocation,
                Payload = System.Text.Json.JsonSerializer.Serialize(location),
                StaffId = location.UpdatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Location Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LocationMaster/UpdateLocation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = location.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(location),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Location Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LocationMaster/UpdateLocation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = location.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(location),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}