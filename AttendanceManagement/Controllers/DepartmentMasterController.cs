using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentMasterController : ControllerBase
{
    private readonly IDepartmentMasterInfra _service;
    private readonly ILoggingInfra _loggingService;

    public DepartmentMasterController(IDepartmentMasterInfra service, ILoggingInfra loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllDepartments")]
    public async Task<IActionResult> GetAllDepartments()
    {
        try
        {
            var departments = await _service.GetAllDepartments();
            var response = new
            {
                Success = true,
                Message = departments
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

    [HttpPost("CreateDepartment")]
    public async Task<IActionResult> CreateDepartment(DepartmentRequest department)
    {
        try
        {
            var createdDepartment = await _service.CreateDepartment(department);
            var response = new
            {
                Success = true,
                Message = createdDepartment
            };
            await _loggingService.AuditLog("Department Master", "POST", "/api/DepartmentMaster/CreateDepartment", createdDepartment, department.CreatedBy, JsonSerializer.Serialize(department));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Department Master", "POST", "/api/DepartmentMaster/CreateDepartment", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, department.CreatedBy, JsonSerializer.Serialize(department));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Department Master", "POST", "/api/DepartmentMaster/CreateDepartment", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, department.CreatedBy, JsonSerializer.Serialize(department));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateDepartment")]
    public async Task<IActionResult> UpdateDepartment(UpdateDepartment updatedDepartment)
    {
        try
        {
            var success = await _service.UpdateDepartment(updatedDepartment);
            var response = new
            {
                Success = true,
                Message = success
            };
            await _loggingService.AuditLog("Department Master", "POST", "/api/DepartmentMaster/UpdateDepartment", success, updatedDepartment.UpdatedBy, JsonSerializer.Serialize(updatedDepartment));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Department Master", "POST", "/api/DepartmentMaster/UpdateDepartment", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedDepartment.UpdatedBy, JsonSerializer.Serialize(updatedDepartment));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Department Master", "POST", "/api/DepartmentMaster/UpdateDepartment", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedDepartment.UpdatedBy, JsonSerializer.Serialize(updatedDepartment));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Department Master", "POST", "/api/DepartmentMaster/UpdateDepartment", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedDepartment.UpdatedBy, JsonSerializer.Serialize(updatedDepartment));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}