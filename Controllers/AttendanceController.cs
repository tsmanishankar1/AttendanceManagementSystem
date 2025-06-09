using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AttendanceManagement.Services;
using AttendanceManagement.AtrakModels;
using AttendanceManagement.Input_Models;
using System.Text.Json;

[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly AttendanceService _smaxTransactionService;
    private readonly LoggingService _loggingService;

    public AttendanceController(AttendanceService smaxTransactionService, LoggingService loggingService)
    {
        _smaxTransactionService = smaxTransactionService;
        _loggingService = loggingService;

    }

    [HttpGet("AttendanceDetails")]
    public async Task<IActionResult> GetFirstCheckin(int staffId)
    {
        try
        {
            var checkinRecord = await _smaxTransactionService.GetCheckInCheckOutAsync(staffId);
            var response = new
            {
                Success = true,
                Message = checkinRecord
            };
            return Ok(response);
        }
        catch(MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddGraceTimeAndBreakTime")]
    public async Task<IActionResult> AddGraceTimeAndBreakTime(AttendanceGraceTimeCalcRequest request)
    {
        try
        {
            var result = await _smaxTransactionService.AddGraceTimeAndBreakTime(request);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Grace and Extra Breack Time", "POST", "/api/Attendance/AddGraceTimeAndBreakTime", result, request.CreatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Grace and Extra Breack Time", "POST", "/api/Attendance/AddGraceTimeAndBreakTime", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Grace and Extra Breack Time", "POST", "/api/Attendance/AddGraceTimeAndBreakTime", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetGraceTimeAndBreakTime")]
    public async Task<IActionResult> GetGraceTimeAndBreakTime()
    {
        try
        {
            var result = await _smaxTransactionService.GetGraceTimeAndBreakTime();
            var response = new
            {
                Success = true,
                Message = result
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

    [HttpPost("UpdateGraceTimeAndBreakTime")]
    public async Task<IActionResult> UpdateGraceTimeAndBreakTime(UpdateAttendanceGraceTimeCalc request)
    {
        try
        {
            var result = await _smaxTransactionService.UpdateGraceTimeAndBreakTime(request);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Grace and Extra Breack Time", "POST", "/api/Attendance/UpdateGraceTimeAndBreakTime", result, request.UpdatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Grace and Extra Breack Time", "POST", "/api/Attendance/UpdateGraceTimeAndBreakTime", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Grace and Extra Breack Time", "POST", "/api/Attendance/UpdateGraceTimeAndBreakTime", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("AttendanceRecords")]
    public async Task<IActionResult> AttendanceRecords()
    {
        try
        {
            var results = await _smaxTransactionService.AttendanceRecords();
            var response = new
            {
                Success = true,
                Message = results
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

    [HttpPost("GetAllStaffsByDepartmentAndDivision")]
    public async Task<IActionResult> GetAllStaffsByDepartmentAndDivision(GetStaffByDepartmentDivision staff)
    {
        try
        {
            var results = await _smaxTransactionService.GetAllStaffsByDepartmentAndDivision(staff);
            var response = new
            {
                Success = true,
                Message = results
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

    [HttpPost("GetAttendanceRecords")]
    public async Task<IActionResult> GetAttendanceRecords(AttendanceStatusResponse attendanceStatus)
    {
        try
        {
            var results = await _smaxTransactionService.GetAttendanceRecords(attendanceStatus);
            var response = new
            {
                Success = true,
                Message = results
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

    [HttpPost("FreezeAttendanceRecords")]
    public async Task<IActionResult> FreezeAttendanceRecords(AttendanceFreezeRequest attendanceFreeze)
    {
        try
        {
            var results = await _smaxTransactionService.FreezeAttendanceRecords(attendanceFreeze);
            var response = new
            {
                Success = true,
                Message = results
            };
            await _loggingService.AuditLog("Freeze Attendance Records", "POST", "/api/Attendance/FreezeAttendanceRecords", results, attendanceFreeze.FreezedBy, JsonSerializer.Serialize(attendanceFreeze));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Freeze Attendance Records", "POST", "/api/Attendance/FreezeAttendanceRecords", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, attendanceFreeze.FreezedBy, JsonSerializer.Serialize(attendanceFreeze));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Freeze Attendance Records", "POST", "/api/Attendance/FreezeAttendanceRecords", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, attendanceFreeze.FreezedBy, JsonSerializer.Serialize(attendanceFreeze));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}