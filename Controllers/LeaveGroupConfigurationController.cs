using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveGroupConfigurationController : ControllerBase
{
    private readonly LeaveGroupConfigurationService _service;
    private readonly AttendanceManagementSystemContext _context;

    public LeaveGroupConfigurationController(LeaveGroupConfigurationService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
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
            AuditLog log = new AuditLog
            {
                Module = "Leave Group Configuration",
                HttpMethod = "POST",
                ApiEndpoint = "/LeaveGroupConfiguration/CreateLeaveGroupConfiguration",
                SuccessMessage = createdConfiguration,
                Payload = System.Text.Json.JsonSerializer.Serialize(configuration),
                StaffId = configuration.CreatedBy,
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
                    Module = "Leave Group Configuration",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveGroupConfiguration/CreateLeaveGroupConfiguration",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = configuration.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(configuration),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "Leave Group Configuration",
                HttpMethod = "POST",
                ApiEndpoint = "/LeaveGroupConfiguration/UpdateLeaveGroupConfiguration",
                SuccessMessage = updatedConfiguration,
                Payload = System.Text.Json.JsonSerializer.Serialize(transaction),
                StaffId = transaction.UpdatedBy,
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
                    Module = "Leave Group Configuration",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveGroupConfiguration/UpdateLeaveGroupConfiguration",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = transaction.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(transaction),
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
                    Module = "Leave Group Configuration",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveGroupConfiguration/UpdateLeaveGroupConfiguration",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = transaction.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(transaction),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}