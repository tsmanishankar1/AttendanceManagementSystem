using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveGroupConfigurationController : ControllerBase
{
    private readonly LeaveGroupConfigurationService _service;
    private readonly LoggingService _loggingService;

    public LeaveGroupConfigurationController(LeaveGroupConfigurationService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllGroupConfigurations")]
    public async Task<IActionResult> GetAllConfigurations()
    {
        try
        {
            var transactions = await _service.GetAllConfigurations();
            var response = new
            {
                Success = true,
                Message = transactions
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

    [HttpGet("GetGroupConfigurationById")]
    public async Task<IActionResult> GetConfigurationById(int leaveGroupConfigurationId)
    {
        try
        {
            var transaction = await _service.GetConfigurationDetailsById(leaveGroupConfigurationId);
            var response = new
            {
                Success = true,
                Message = transaction
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

    [HttpPost("AddLeaveGroupConfiguration")]
    public async Task<IActionResult> CreateConfiguration(LeaveGroupConfigurationRequest configuration)
    {
        try
        {
            var createdConfiguration = await _service.CreateConfigurations(configuration);
            var response = new
            {
                Success = true,
                Message = createdConfiguration
            };
            await _loggingService.AuditLog("Leave Group Configuration", "POST", "/api/LeaveGroupConfiguration/AddLeaveGroupConfiguration", createdConfiguration, configuration.CreatedBy, JsonSerializer.Serialize(configuration));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Group Configuration", "POST", "/api/LeaveGroupConfiguration/AddLeaveGroupConfiguration", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, configuration.CreatedBy, JsonSerializer.Serialize(configuration));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateLeaveGroupConfiguration")]
    public async Task<IActionResult> UpdateConfiguration(UpdateLeaveGroupConfiguration transaction)
    {
        try
        {
            var updatedConfiguration = await _service.UpdateConfigurations(transaction);
            var response = new
            {
                Success = true,
                Message = updatedConfiguration
            };
            await _loggingService.AuditLog("Leave Group Configuration", "POST", "/api/LeaveGroupConfiguration/UpdateLeaveGroupConfiguration", updatedConfiguration, transaction.UpdatedBy, JsonSerializer.Serialize(transaction));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Leave Group Configuration", "POST", "/api/LeaveGroupConfiguration/UpdateLeaveGroupConfiguration", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, transaction.UpdatedBy, JsonSerializer.Serialize(transaction));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Group Configuration", "POST", "/api/LeaveGroupConfiguration/UpdateLeaveGroupConfiguration", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, transaction.UpdatedBy, JsonSerializer.Serialize(transaction));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}