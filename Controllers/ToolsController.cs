using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Input_Models;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToolsController : ControllerBase
{
    private readonly ToolsService _service;
    private readonly AttendanceManagementSystemContext _context;

    public ToolsController(ToolsService toolsService, AttendanceManagementSystemContext context)
    {
        _service = toolsService;
        _context = context;
    }

    [HttpGet("GetAllApplicationTypes")]
    public async Task<IActionResult> GetAllApplicationTypes()
    {
        try
        {
            var applicationTypes = await _service.GetAllApplicationTypesAsync();
            var response = new
            {
                Success = true,
                Message = applicationTypes
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

    [HttpGet("GetStaffInfoByOrganizationType")]
    public async Task<IActionResult> GetStaffInfoByOrganizationType(int organizationTypeId)
    {
        try
        {
            var staffInfo = await _service.GetStaffInfoByOrganizationTypeAsync(organizationTypeId);
            if (staffInfo == null || staffInfo.Count == 0)
            {
                return NotFound("No staff found for the given organization type.");
            }
            return Ok(staffInfo);
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

    [HttpPost("AddMultipleLeaveCreditDebit")]
    public async Task<IActionResult> AddLeaveCreditDebitForMultipleStaff(LeaveCreditDebitRequest leaveCreditDebit)
    {
        try
        {
            var message = await _service.AddLeaveCreditDebitForMultipleStaffAsync(leaveCreditDebit);
            var response = new
            {
                Success = true,
                Message = message
            };
            AuditLog log = new AuditLog
            {
                Module = "Leave Credit Debit",
                HttpMethod = "POST",
                ApiEndpoint = "/LeaveCreditDebit/CreateLeaveCreditDebitForMultipleStaff",
                SuccessMessage = message,
                Payload = System.Text.Json.JsonSerializer.Serialize(leaveCreditDebit),
                StaffId = leaveCreditDebit.CreatedBy,
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
                    Module = "Leave Credit Debit",
                    HttpMethod = "POST",
                    ApiEndpoint = "/LeaveCreditDebit/CreateLeaveCreditDebitForMultipleStaff",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = leaveCreditDebit.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(leaveCreditDebit),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddApplicationType")]
    public async Task<IActionResult> CreateApplicationType(ApplicationTypeRequest applicationType)
    {
        try
        {
            var createdApplicationType = await _service.AddApplicationTypeAsync(applicationType);
            var response = new
            {
                Success = true,
                Message = createdApplicationType
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}