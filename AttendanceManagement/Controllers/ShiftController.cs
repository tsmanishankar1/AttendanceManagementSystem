using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShiftController : ControllerBase
{
    private readonly IShiftInfra _shiftService;
    private readonly ILoggingInfra _loggingService;

    public ShiftController(IShiftInfra shiftService, ILoggingInfra loggingService)
    {
        _shiftService = shiftService;
        _loggingService = loggingService;
    }

    [HttpGet("GetByDivision")]
    public async Task<IActionResult> GetStaffByDivision(int divisionId)
    {
        try
        {
            var staffList = await _shiftService.GetStaffByDivisionIdAsync(divisionId);
            var response = new
            {
                Success = true,
                Message = staffList
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
            await _loggingService.AuditLog("Shift", "POST", "/api/Shift/CreatShift", createdShift, newShift.CreatedBy, newShift);
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/CreateShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, newShift.CreatedBy, newShift);
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/CreateShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, newShift.CreatedBy, newShift);
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/CreateShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, newShift.CreatedBy, newShift);
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
            await _loggingService.AuditLog("Shift", "POST", "/api/Shift/UpdateShift", updated, updatedShift.UpdatedBy, updatedShift);
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/UpdateShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedShift.UpdatedBy, updatedShift);
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/UpdateShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedShift.UpdatedBy, updatedShift);
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/UpdateShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedShift.UpdatedBy, updatedShift);
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
            await _loggingService.AuditLog("Shift", "POST", "/api/Shift/AddRegularShift", createdShift, regularShift.CreatedBy, regularShift);
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/AddRegularShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, regularShift.CreatedBy, regularShift);
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/AddRegularShift", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, regularShift.CreatedBy, regularShift);
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
            await _loggingService.AuditLog("Shift", "POST", "/api/Shift/AssignShiftToStaff", createdShift, assignShift.CreatedBy, assignShift);
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/AssignShiftToStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignShift.CreatedBy, assignShift);
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/AssignShiftToStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignShift.CreatedBy, assignShift);
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/AssignShiftToStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignShift.CreatedBy, assignShift);
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Shift", "POST", "/api/Shift/AssignShiftToStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignShift.CreatedBy, assignShift);
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAllAssignedShifts")]
    public async Task<IActionResult> GetAllAssignedShifts(int approverId)
    {
        try
        {
            var shifts = await _shiftService.GetAllAssignedShifts(approverId);
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
}