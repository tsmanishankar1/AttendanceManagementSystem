using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkstationMasterController : ControllerBase
{
    private readonly WorkstationMasterService _service;
    private readonly AttendanceManagementSystemContext _context;

    public WorkstationMasterController(WorkstationMasterService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet("GetAllWorkStations")]
    public async Task<IActionResult> GetAllWorkstations()
    {
        try
        {
            var workstations = await _service.GetAllWorkstationsAsync();
            var response = new
            {
                Success = true,
                Message = workstations
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

    [HttpGet("GetWorkstationById")]
    public async Task<IActionResult> GetWorkstationById(int workstationMasterId)
    {
        try
        {
            var workstation = await _service.GetWorkstationByIdAsync(workstationMasterId);
            var response = new
            {
                Success = true,
                Message = workstation
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

    [HttpPost("CreateWorkstation")]
    public async Task<IActionResult> CreateWorkstation(WorkStationRequest workstation)
    {
        try
        {
            var createdWorkstation = await _service.CreateWorkstationAsync(workstation);
            var response = new
            {
                Success = true,
                Message = createdWorkstation
            };

            AuditLog log = new AuditLog
            {
                Module = "CreateWorkstation",
                HttpMethod = "POST",
                ApiEndpoint = "/api/WorkstationMaster/CreateWorkstation",
                SuccessMessage = "Created workstation successfully",
                Payload = System.Text.Json.JsonSerializer.Serialize(workstation),
                StaffId = workstation.CreatedBy,
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
                    Module = "CreateWorkstation",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WorkstationMaster/CreateWorkstation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = workstation.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(workstation),
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
                    Module = "CreateWorkstation",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WorkstationMaster/CreateWorkstation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = workstation.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(workstation),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateWorkstation")]
    public async Task<IActionResult> UpdateWorkstation(UpdateWorkStation updatedWorkstation)
    {
        try
        {
            var success = await _service.UpdateWorkstationAsync(updatedWorkstation);
            var response = new
            {
                Success = true,
                Message = success
            };

            AuditLog log = new AuditLog
            {
                Module = "UpdateWorkstation",
                HttpMethod = "POST",
                ApiEndpoint = "/api/WorkstationMaster/UpdateWorkstation",
                SuccessMessage = "Updated workstation successfully",
                Payload = System.Text.Json.JsonSerializer.Serialize(updatedWorkstation),
                StaffId = updatedWorkstation.UpdatedBy,
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
                    Module = "UpdateWorkstation",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WorkstationMaster/UpdateWorkstation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedWorkstation.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedWorkstation),
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
                    Module = "UpdateWorkstation",
                    HttpMethod = "POST",
                    ApiEndpoint = "/api/WorkstationMaster/UpdateWorkstation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedWorkstation.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedWorkstation),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}
