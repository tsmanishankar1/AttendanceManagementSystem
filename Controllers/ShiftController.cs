using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShiftController : ControllerBase
{
    private readonly ShiftService _shiftService;
    private readonly AttendanceManagementSystemContext _context;

    public ShiftController(ShiftService shiftService, AttendanceManagementSystemContext context)
    {
        _shiftService = shiftService;
        _context = context;
    }

    [HttpGet("GetAllShifts")]
    public async Task<IActionResult> GetShifts()
    {
        try
        {
            var shifts = await _shiftService.GetAllShiftsAsync();
            var response = new
            {
                Success = true,
                Message = shifts
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

    [HttpGet("GetShiftById")]
    public async Task<IActionResult> GetShiftById(int shiftId)
    {
        try
        {
            var shift = await _shiftService.GetShiftByIdAsync(shiftId);
            var response = new
            {
                Success = true,
                Message = shift
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

    [HttpPost("CreateShift")]
    public async Task<IActionResult> CreateShift(ShiftRequest newShift)
    {
        try
        {
            var createdShift = await _shiftService.CreateShiftAsync(newShift);
            var response = new
            {
                Success = true,
                Message = createdShift
            };
            AuditLog log = new AuditLog
            {
                Module = "Shifts",
                HttpMethod = "POST",
                ApiEndpoint = "/Shifts/CreatShifts",
                SuccessMessage = createdShift,
                Payload = System.Text.Json.JsonSerializer.Serialize(newShift),
                StaffId = newShift.CreatedBy,
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
                    Module = "Shift",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shifts/CreateShift",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = newShift.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(newShift),
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
                    Module = "Shift",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shifts/CreateShift",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = newShift.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(newShift),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateShift")]
    public async Task<IActionResult> UpdateShift(UpdateShift updatedShift)
    {
        try
        {
            var updated = await _shiftService.UpdateShiftAsync(updatedShift);
            var response = new
            {
                Success = true,
                Message = updated
            };
            AuditLog log = new AuditLog
            {
                Module = "Shift",
                HttpMethod = "POST",
                ApiEndpoint = "/Shift/UpdateShift",
                SuccessMessage = updated,
                Payload = System.Text.Json.JsonSerializer.Serialize(updatedShift),
                StaffId = updatedShift.UpdatedBy,
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
                    Module = "Shift",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shift/UpdateShift",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedShift.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedShift),
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
                    Module = "Shift",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shift/UpdateShift",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedShift.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedShift),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddRegularShift")]
    public async Task<IActionResult> CreateRegularShift(RegularShiftRequest regularShift)
    {
        try
        {
            var createdShift = await _shiftService.CreateRegularShiftAsync(regularShift);
            var response = new
            {
                Success = true,
                Message = createdShift
            };
            var auditLogs = regularShift.StaffIds.Select(staffId => new AuditLog
            {
                Module = "Shifts",
                HttpMethod = "POST",
                ApiEndpoint = "/Shifts/CreateRegularShifts",
                SuccessMessage = createdShift,
                Payload = System.Text.Json.JsonSerializer.Serialize(regularShift),
                StaffId = staffId, 
                CreatedUtc = DateTime.UtcNow
            }).ToList();

            _context.AuditLogs.AddRange(auditLogs);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                var errorLogs = regularShift.StaffIds.Select(staffId => new ErrorLog
                {
                    Module = "Shift",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shifts/CreateRegularShift",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = staffId,
                    Payload = System.Text.Json.JsonSerializer.Serialize(regularShift),
                    CreatedUtc = DateTime.UtcNow
                }).ToList();

                logContext.ErrorLogs.AddRange(errorLogs);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AssignShiftToStaff")]
    public async Task<IActionResult> AssignShiftToStaff(AssignShiftRequest assignShift)
    {
        try
        {
            var createdShift = await _shiftService.AssignShiftToStaffAsync(assignShift);
            var response = new
            {
                Success = true,
                Message = createdShift
            };
            AuditLog log = new AuditLog
            {
                Module = "Shifts",
                HttpMethod = "POST",
                ApiEndpoint = "/Shifts/CreatAssignedShifts",
                SuccessMessage = createdShift,
                Payload = System.Text.Json.JsonSerializer.Serialize(assignShift),
                StaffId = assignShift.CreatedBy,
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
                    Module = "Shift",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shifts/CreateAssignedShift",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = assignShift.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(assignShift),
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
                    Module = "Shift",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shifts/CreateAssignedShift",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = assignShift.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(assignShift),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}
