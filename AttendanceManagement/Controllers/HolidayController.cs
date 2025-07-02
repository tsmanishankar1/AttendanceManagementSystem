using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.DTOs;
using AttendanceManagement.InputModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using AttendanceManagement.Services.Interface;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HolidayController : ControllerBase
{
    private readonly IHolidayService _service;
    private readonly ILoggingService _loggingService;

    public HolidayController(IHolidayService service, ILoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
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
            await _loggingService.AuditLog("Holiday", "POST", "/api/Holiday/AddHolidayMaster", createdHoliday, holiday.CreatedBy, JsonSerializer.Serialize(holiday));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Holiday", "POST", "/api/Holiday/AddHolidayMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, holiday.CreatedBy, JsonSerializer.Serialize(holiday));
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
            await _loggingService.AuditLog("Holiday", "POST", "/api/Holiday/UpdateHolidayMaster", updated, updatedHoliday.UpdatedBy, JsonSerializer.Serialize(updatedHoliday));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Holiday", "POST", "/api/Holiday/UpdateHolidayMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedHoliday.UpdatedBy, JsonSerializer.Serialize(updatedHoliday));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Holiday", "POST", "/api/Holiday/UpdateHolidayMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedHoliday.UpdatedBy, JsonSerializer.Serialize(updatedHoliday));
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
            await _loggingService.AuditLog("Holiday Calendar", "POST", "/api/Holiday/AddHolidayCalendarGroup", addHoliday, request.CreatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Holiday Calendar", "POST", "/api/Holiday/AddHolidayCalendarGroup", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.CreatedBy, JsonSerializer.Serialize(request));
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
            await _loggingService.AuditLog("Holiday Calendar Group", "POST", "/api/Holiday/UpdateCalendarGroupById", update, request.UpdatedBy, JsonSerializer.Serialize(request));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Holiday Calendar Group", "POST", "/api/Holiday/UpdateCalendarGroupById", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Holiday Calendar Group", "POST", "/api/Holiday/UpdateCalendarGroupById", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
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
            await _loggingService.AuditLog("Holiday Zone", "POST", "/api/Holiday/AddHolidayZone", createdZone, holidayZone.CreatedBy, JsonSerializer.Serialize(holidayZone));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Holiday Zone", "POST", "/api/Holiday/AddHolidayZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, holidayZone.CreatedBy, JsonSerializer.Serialize(holidayZone));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateHolidayZone")]
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
            await _loggingService.AuditLog("Holiday Zone", "POST", "/api/Holiday/UpdateHolidayZone", updatedZone, holidayZone.UpdatedBy, JsonSerializer.Serialize(holidayZone));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Holiday Zone", "POST", "/api/Holiday/UpdateHolidayZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, holidayZone.UpdatedBy, JsonSerializer.Serialize(holidayZone));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Holiday Zone", "POST", "/api/Holiday/UpdateHolidayZone", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, holidayZone.UpdatedBy, JsonSerializer.Serialize(holidayZone));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}