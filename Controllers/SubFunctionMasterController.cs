using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubFunctionMasterController : ControllerBase
{
    private readonly SubFunctionMasterService _service;
    private readonly AttendanceManagementSystemContext _context;

    public SubFunctionMasterController(SubFunctionMasterService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet("GetAllSubFunctions")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var subFunctions = await _service.GetAllSubFunctionsAsync();
            var response = new
            {
                Success = true,
                Message = subFunctions
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

    [HttpGet("GetSubFunctionById")]
    public async Task<IActionResult> GetById(int subFunctionMasterId)
    {
        try
        {
            var subFunction = await _service.GetSubFunctionByIdAsync(subFunctionMasterId);
            var response = new
            {
                Success = true,
                Message = subFunction
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

    [HttpPost("CreateSubFunction")]
    public async Task<IActionResult> Create(SubFunctionRequest subFunctionMaster)
    {
        try
        {
            var createdSubFunction = await _service.CreateSubFunctionAsync(subFunctionMaster);
            var response = new
            {
                Success = true,
                Message = createdSubFunction
            };
            AuditLog log = new AuditLog
            {
                Module = "SubFunction Master",
                HttpMethod = "POST",
                ApiEndpoint = "/SubFunction/CreateSubFunction",
                SuccessMessage = createdSubFunction,
                Payload = System.Text.Json.JsonSerializer.Serialize(subFunctionMaster),
                StaffId = subFunctionMaster.CreatedBy,
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
                    Module = "SubFunction Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/SubFunction/CreateSubFunction",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = subFunctionMaster.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(subFunctionMaster),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateSubFunction")]
    public async Task<IActionResult> Update(UpdateSubFunction subFunctionMaster)
    {
        try
        {
            var updatedSubFunction = await _service.UpdateSubFunctionAsync(subFunctionMaster);
            var response = new
            {
                Success = true,
                Message = updatedSubFunction
            };
            AuditLog log = new AuditLog
            {
                Module = "SubFunction Master",
                HttpMethod = "POST",
                ApiEndpoint = "/SubFunction/UpdateSubFunction",
                SuccessMessage = updatedSubFunction,
                Payload = System.Text.Json.JsonSerializer.Serialize(subFunctionMaster),
                StaffId = subFunctionMaster.UpdatedBy,
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
                    Module = "SubFunction Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/SubFunction/UpdateSubFunction",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = subFunctionMaster.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(subFunctionMaster),
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
                    Module = "SubFunction Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/SubFunction/UpdateSubFunction",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = subFunctionMaster.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(subFunctionMaster),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}
