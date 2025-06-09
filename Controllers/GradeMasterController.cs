using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GradeMasterController : ControllerBase
{
    private readonly GradeMasterService _service;
    private readonly LoggingService _loggingService;

    public GradeMasterController(GradeMasterService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
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
            await _loggingService.AuditLog("Grade Master", "POST", "/api/GradeMaster/CreateGrade", createdGrade, gradeMaster.CreatedBy, JsonSerializer.Serialize(gradeMaster));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Grade Master", "POST", "/api/GradeMaster/CreateGrade", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, gradeMaster.CreatedBy, JsonSerializer.Serialize(gradeMaster));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Grade Master", "POST", "/api/GradeMaster/CreateGrade", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, gradeMaster.CreatedBy, JsonSerializer.Serialize(gradeMaster));
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
            await _loggingService.AuditLog("Grade Master", "POST", "/api/GradeMaster/UpdateGrade", updatedGrade, gradeMaster.UpdatedBy, JsonSerializer.Serialize(gradeMaster));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Grade Master", "POST", "/api/GradeMaster/UpdateGrade", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, gradeMaster.UpdatedBy, JsonSerializer.Serialize(gradeMaster));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Grade Master", "POST", "/api/GradeMaster/UpdateGrade", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, gradeMaster.UpdatedBy, JsonSerializer.Serialize(gradeMaster));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Grade Master", "POST", "/api/GradeMaster/UpdateGrade", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, gradeMaster.UpdatedBy, JsonSerializer.Serialize(gradeMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}