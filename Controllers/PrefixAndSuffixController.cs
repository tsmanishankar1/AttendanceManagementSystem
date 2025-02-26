using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using System.Text.Json;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrefixAndSuffixController : ControllerBase
{
    private readonly PrefixAndSuffixService _prefixAndSuffixService;
    private readonly LoggingService _loggingService;

    public PrefixAndSuffixController(PrefixAndSuffixService prefixAndSuffixService, LoggingService loggingService)
    {
        _prefixAndSuffixService = prefixAndSuffixService;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllSuffixLeaveType")]
    public async Task<IActionResult> GetAllSuffixLeaveType()
    {
        try
        {
            var suffixLeaveType = await _prefixAndSuffixService.GetAllSuffixLeaveType();
            var response = new
            {
                Success = true,
                Message = suffixLeaveType
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

    [HttpPost("AddSuffixLeaveType")]
    public async Task<IActionResult> Create(SuffixLeaveRequest suffixLeaveType)
    {
        try
        {
            var createdSuffixLeaveType = await _prefixAndSuffixService.Create(suffixLeaveType);
            var response = new
            {
                Success = true,
                Message = createdSuffixLeaveType
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAllPrefixLeaveType")]
    public async Task<IActionResult> GetAllPrefixLeaveType()
    {
        try
        {
            var result = await _prefixAndSuffixService.GetAllPrefixLeaveType();
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

    [HttpPost("CreatePrefixLeaveType")]
    public async Task<IActionResult> AddPrefixLeaveType(PrefixLeaveRequest prefixLeaveType)
    {
        try
        {
            var message = await _prefixAndSuffixService.AddPrefixLeaveType(prefixLeaveType);
            var response = new
            {
                Success = true,
                Message = message
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAllPrefixAndSuffix")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var prefixAndSuffixes = await _prefixAndSuffixService.GetAllPrefixAndSuffixAsync();
            var response = new
            {
                Success = true,
                Message = prefixAndSuffixes
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

    [HttpGet("GetPrefixAndSuffixById")]
    public async Task<IActionResult> GetPrefixAndSuffixById(int prefixAndSuffixId)
    {
        try
        {
            var prefixAndSuffixes = await _prefixAndSuffixService.GetPrefixAndSuffixById(prefixAndSuffixId);
            var response = new
            {
                Success = true,
                Message = prefixAndSuffixes
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

    [HttpPost("AddPrefixAndSuffix")]
    public async Task<IActionResult> Create(PrefixAndSuffixRequest prefixAndSuffix)
    {
        try
        {
            var createdPrefixAndSuffix = await _prefixAndSuffixService.Create(prefixAndSuffix);
            var response = new
            {
                Success = true,
                Message = createdPrefixAndSuffix
            };
            await _loggingService.AuditLog("Prefix And Suffix", "POST", "/PrefixAndSuffix/AddPrefixAndSuffix", createdPrefixAndSuffix, prefixAndSuffix.CreatedBy, JsonSerializer.Serialize(prefixAndSuffix));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Prefix And Suffix", "POST", "/PrefixAndSuffix/AddPrefixAndSuffix", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, prefixAndSuffix.CreatedBy, JsonSerializer.Serialize(prefixAndSuffix));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdatePrefixAndSuffix")]
    public async Task<IActionResult> Update(UpdatePrefixAndSuffix updatedPrefixAndSuffix)
    {
        try
        {
            var updatedRecord = await _prefixAndSuffixService.Update(updatedPrefixAndSuffix);
            var response = new
            {
                Success = true,
                Message = updatedRecord
            };
            await _loggingService.AuditLog("Prefix And Suffix", "POST", "/PrefixAndSuffix/UpdatePrefixAndSuffix", updatedRecord, updatedPrefixAndSuffix.UpdatedBy, JsonSerializer.Serialize(updatedPrefixAndSuffix));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Prefix And Suffix", "POST", "/PrefixAndSuffix/UpdatePrefixAndSuffix", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedPrefixAndSuffix.UpdatedBy, JsonSerializer.Serialize(updatedPrefixAndSuffix));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Prefix And Suffix", "POST", "/PrefixAndSuffix/UpdatePrefixAndSuffix", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedPrefixAndSuffix.UpdatedBy, JsonSerializer.Serialize(updatedPrefixAndSuffix));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}