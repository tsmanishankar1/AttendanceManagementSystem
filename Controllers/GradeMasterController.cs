using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GradeMasterController : ControllerBase
{
    private readonly GradeMasterService _service;
    private readonly AttendanceManagementSystemContext _context;

    public GradeMasterController(GradeMasterService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet("GetAllGrades")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var grades = await _service.GetAllGrades();
            var response = new
            {
                Success = true,
                Message = grades
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

    [HttpGet("GetGradeById")]
    public async Task<IActionResult> GetById(int gradeMasterId)
    {
        try
        {
            var grade = await _service.GetGradeById(gradeMasterId);
            var response = new
            {
                Success = true,
                Message = grade
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

    [HttpPost("CreateGrade")]
    public async Task<IActionResult> Create(GradeMasterRequest gradeMaster)
    {
        try
        {
            var createdGrade = await _service.CreateGrade(gradeMaster);
            var response = new
            {
                Success = true,
                Message = createdGrade
            };
            AuditLog log = new AuditLog
            {
                Module = "Grade Master",
                HttpMethod = "POST",
                ApiEndpoint = "/GradeMaster/CreateGrade",
                SuccessMessage = createdGrade,
                Payload = System.Text.Json.JsonSerializer.Serialize(gradeMaster),
                StaffId = gradeMaster.CreatedBy,
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
                    Module = "Grade Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/GradeMaster/CreateGrade",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = gradeMaster.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(gradeMaster),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateGrade")]
    public async Task<IActionResult> Update(UpdateGradeMaster gradeMaster)
    {
        try
        {
            var updatedGrade = await _service.UpdateGrade(gradeMaster);
            var response = new
            {
                Success = true,
                Message = updatedGrade
            };
            AuditLog log = new AuditLog
            {
                Module = "Grade Master",
                HttpMethod = "POST",
                ApiEndpoint = "/GradeMaster/UpdateGrade",
                SuccessMessage = updatedGrade,
                Payload = System.Text.Json.JsonSerializer.Serialize(gradeMaster),
                StaffId = gradeMaster.UpdatedBy,
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
                    Module = "Grade Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/GradeMaster/UpdateGrade",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = gradeMaster.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(gradeMaster),
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
                    Module = "Grade Master",
                    HttpMethod = "POST",
                    ApiEndpoint = "/GradeMaster/UpdateGrade",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = gradeMaster.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(gradeMaster),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}