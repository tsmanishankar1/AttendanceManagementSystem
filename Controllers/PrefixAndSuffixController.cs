using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Input_Models;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrefixAndSuffixController : ControllerBase
{
    private readonly PrefixAndSuffixService _prefixAndSuffixService;
    private readonly AttendanceManagementSystemContext _context;

    public PrefixAndSuffixController(PrefixAndSuffixService prefixAndSuffixService, AttendanceManagementSystemContext context)
    {
        _prefixAndSuffixService = prefixAndSuffixService;
        _context = context;
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
            AuditLog log = new AuditLog
            {
                Module = "Prefix And Suffix",
                HttpMethod = "POST",
                ApiEndpoint = "/PrefixAndSuffix/AddPrefixAndSuffix",
                SuccessMessage = createdPrefixAndSuffix,
                Payload = System.Text.Json.JsonSerializer.Serialize(prefixAndSuffix),
                StaffId = prefixAndSuffix.CreatedBy,
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
                    Module = "Prefix And Suffix",
                    HttpMethod = "POST",
                    ApiEndpoint = "/PrefixAndSuffix/AddPrefixAndSuffix",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = prefixAndSuffix.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(prefixAndSuffix),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "Prefix And Suffix",
                HttpMethod = "POST",
                ApiEndpoint = "/PrefixAndSuffix/UpdatePrefixAndSuffix",
                SuccessMessage = updatedRecord,
                Payload = System.Text.Json.JsonSerializer.Serialize(updatedPrefixAndSuffix),
                StaffId = updatedPrefixAndSuffix.UpdatedBy,
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
                    Module = "Prefix And Suffix",
                    HttpMethod = "POST",
                    ApiEndpoint = "/PrefixAndSuffix/UpdatePrefixAndSuffix",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedPrefixAndSuffix.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedPrefixAndSuffix),
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
                    Module = "Prefix And Suffix",
                    HttpMethod = "POST",
                    ApiEndpoint = "/PrefixAndSuffix/UpdatePrefixAndSuffix",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedPrefixAndSuffix.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedPrefixAndSuffix),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}