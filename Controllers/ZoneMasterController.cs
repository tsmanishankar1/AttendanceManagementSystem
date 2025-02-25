using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using ErrorLog = AttendanceManagement.Models.ErrorLog;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ZoneMasterController : ControllerBase
{
    private readonly ZoneMasterService _service;
    private readonly AttendanceManagementSystemContext _context;

    public ZoneMasterController(ZoneMasterService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
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

    [HttpGet("GetZoneById")]
    public async Task<IActionResult> GetById(int zoneMasterId)
    {
        try
        {
            var zone = await _service.GetZoneByIdAsync(zoneMasterId);
            var response = new
            {
                Success = true,
                Message = zone
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
            AuditLog log = new AuditLog
            {
                Module = "CreateZone",
                HttpMethod = "POST",
                ApiEndpoint = "/Skills/CreateZone",
                SuccessMessage = createdZone,
                Payload = System.Text.Json.JsonSerializer.Serialize(zoneMaster),
                StaffId = zoneMaster.CreatedBy,
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
                    Module = "CreateZone",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Skills/CreateZone",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = zoneMaster.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(zoneMaster),
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
                    Module = "CreateZone",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Skills/CreateZone",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = zoneMaster.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(zoneMaster),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateZone")]
    public async Task<IActionResult> Update(UpdateZoneMaster zoneMaster)
    {
        try
        {
            var updatedZone = await _service.UpdateZoneAsync(zoneMaster);
            var response = new
            {
                Success = true,
                Message = updatedZone
            };
            AuditLog log = new AuditLog
            {
                Module = "UpdateZone",
                HttpMethod = "POST",
                ApiEndpoint = "/Zone/UpdateZone",
                SuccessMessage = updatedZone,
                Payload = System.Text.Json.JsonSerializer.Serialize(zoneMaster),
                StaffId = zoneMaster.UpdatedBy,
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
                    Module = "UpdateZone",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Zone/UpdateZone",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = zoneMaster.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(zoneMaster),
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
                    Module = "UpdateZone",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Zone/UpdateZone",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = zoneMaster.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(zoneMaster),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}


