using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.DTOs;
using AttendanceManagement.Input_Models;
using Swashbuckle.AspNetCore.Annotations;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HolidayController : ControllerBase
{
    private readonly HolidayService _service;
    private readonly AttendanceManagementSystemContext _context;

    public HolidayController(HolidayService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet("GetAllHolidayMasters")]
    public async Task<IActionResult> GetAllHolidaysAsync()
    {
        try
        {
            var holidays = await _service.GetAllHolidaysAsync();
            var response = new
            {
                Success = true,
                Message = holidays
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

    [HttpGet("GetAllHolidayType")]
    public async Task<IActionResult> GetAllHolidayType()
    {
        try
        {
            var holidays = await _service.GetAllHolidayType();
            var response = new
            {
                Success = true,
                Message = holidays
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

    [HttpGet("GetHolidayMasterById")]
    public async Task<IActionResult> GetHolidayById(int holidayMasterId)
    {
        try
        {
            var holiday = await _service.GetHolidayById(holidayMasterId);
            var response = new
            {
                Success = true,
                Message = holiday
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

    [HttpPost("AddHolidayMaster")]
    public async Task<IActionResult> CreateHoliday(HolidayRequest holiday)
    {
        try
        {
            var createdHoliday = await _service.CreateHoliday(holiday);
            var response = new
            {
                Success = true,
                Message = createdHoliday
            };
            AuditLog log = new AuditLog
            {
                Module = "Holiday",
                HttpMethod = "POST",
                ApiEndpoint = "/Holiday/CreateHoliday",
                SuccessMessage = createdHoliday,
                Payload = System.Text.Json.JsonSerializer.Serialize(holiday),
                StaffId = holiday.CreatedBy,
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
                    Module = "Holiday",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Holiday/CreateHoliday",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = holiday.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(holiday),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateHolidayMaster")]
    public async Task<IActionResult> UpdateHoliday(UpdateHoliday updatedHoliday)
    {
        try
        {
            var updated = await _service.UpdateHoliday(updatedHoliday);
            var response = new
            {
                Success = true,
                Message = updated
            };
            AuditLog log = new AuditLog
            {
                Module = "Holiday",
                HttpMethod = "POST",
                ApiEndpoint = "/Holiday/UpdateHoliday",
                SuccessMessage = updated,
                Payload = System.Text.Json.JsonSerializer.Serialize(updatedHoliday),
                StaffId = updatedHoliday.UpdatedBy,
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
                    Module = "Holiday",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Holiday/UpdateHoliday",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedHoliday.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedHoliday),
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
                    Module = "Holiday",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Holiday/UpdateHoliday",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedHoliday.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedHoliday),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddHolidayCalendarGroup")]
    public async Task<IActionResult> CreateHolidayCalendar(HolidayCalendarRequestDto request)
    {
        try
        {
            var addHoliday = await _service.CreateHolidayCalendar(request);
            var response = new
            {
                Success = true,
                Message = addHoliday
            };
            AuditLog log = new AuditLog
            {
                Module = "Holiday Calendar",
                HttpMethod = "POST",
                ApiEndpoint = "/HolidayCalendar/CreateHolidayCalendar",
                SuccessMessage = addHoliday,
                Payload = System.Text.Json.JsonSerializer.Serialize(request),
                StaffId = request.CreatedBy,
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
                    Module = "Holiday Calendar",
                    HttpMethod = "POST",
                    ApiEndpoint = "/HolidayCalendar/CreateHolidayCalendar",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = request.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(request),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetHolidayGroup")]
    public async Task<IActionResult> GetHolidayCalendars()
    {
        try
        {
            var holidayCalendars = await _service.GetAllHolidayCalendarsAsync();
            var response = new
            {
                Success = true,
                Message = holidayCalendars
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

    [HttpPost("UpdateCalendarGroupById")]
    public async Task<IActionResult> UpdateHolidayCalendar(UpdateHolidayCalanderDto request)
    {
        try
        {
            var update = await _service.UpdateHolidayCalendar(request);
            var response = new
            {
                Success = true,
                Message = update
            };
            AuditLog log = new AuditLog
            {
                Module = "Holiday Calendar Group",
                HttpMethod = "POST",
                ApiEndpoint = "/HolidayGroup/UpdateHolidayGroupCalendar",
                SuccessMessage = update,
                Payload = System.Text.Json.JsonSerializer.Serialize(request),
                StaffId = request.UpdatedBy,
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
                    Module = "Holiday Calendar Group",
                    HttpMethod = "POST",
                    ApiEndpoint = "/HolidayGroup/UpdateHolidayGroupCalendar",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = request.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(request),
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
                    Module = "Holiday Calendar Group",
                    HttpMethod = "POST",
                    ApiEndpoint = "/HolidayGroup/UpdateHolidayGroupCalendar",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = request.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(request),
                    CreatedUtc = DateTime.UtcNow
                };
                    
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetHolidayZone")]
    public async Task<IActionResult> GetHolidayZones()
    {
        try
        {
            var holidayZones = await _service.GetAllHolidayZonesAsync();
            var response = new
            {
                Success = true,
                Message = holidayZones
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

    [HttpGet("GetHolidayZoneById")]
    public async Task<IActionResult> GetHolidayZone(int holidayZoneId)
    {
        try
        {
            var holidayZone = await _service.GetHolidayZoneByIdAsync(holidayZoneId);
            var response = new
            {
                Success = true,
                Message = holidayZone
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

    [HttpPost("AddHolidayZone")]
    public async Task<IActionResult> PostHolidayZone(HolidayZoneRequest holidayZone)
    {
        try
        {
            var createdZone = await _service.CreateHolidayZoneAsync(holidayZone);
            var response = new
            {
                Success = true,
                Message = createdZone
            };
            AuditLog log = new AuditLog
            {
                Module = "Holiday Zone",
                HttpMethod = "POST",
                ApiEndpoint = "/HolidayZone/CreateHolidayZone",
                SuccessMessage = createdZone,
                Payload = System.Text.Json.JsonSerializer.Serialize(holidayZone),
                StaffId = holidayZone.CreatedBy,
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
                    Module = "Holiday Zone",
                    HttpMethod = "POST",
                    ApiEndpoint = "/HolidayZone/CreateHolidayZone",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = holidayZone.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(holidayZone),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateHolidayZoneby")]
    public async Task<IActionResult> UpdateHolidayZoneby(UpdateHolidayZone holidayZone)
    {
        try
        {
            var updatedZone = await _service.UpdateHolidayZoneAsync(holidayZone);
            var response = new
            {
                Success = true,
                Message = updatedZone
            };
            AuditLog log = new AuditLog
            {
                Module = "Holiday Zone",
                HttpMethod = "POST",
                ApiEndpoint = "/HolidayZone/UpdateHolidayZone",
                SuccessMessage = updatedZone,
                Payload = System.Text.Json.JsonSerializer.Serialize(holidayZone),
                StaffId = holidayZone.UpdatedBy,
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
                    Module = "Holiday Zone",
                    HttpMethod = "POST",
                    ApiEndpoint = "/HolidayZone/UpdateHolidayZone",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = holidayZone.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(holidayZone),
                    CreatedUtc = DateTime.UtcNow
                };

                _context.ErrorLogs.Add(log);
                await _context.SaveChangesAsync();
            }
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Holiday Zone",
                    HttpMethod = "POST",
                    ApiEndpoint = "/HolidayZone/UpdateHolidayZone",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = holidayZone.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(holidayZone),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}