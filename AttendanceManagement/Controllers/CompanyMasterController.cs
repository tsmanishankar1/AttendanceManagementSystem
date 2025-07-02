using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyMasterController : ControllerBase
{
    private readonly ICompanyMaster _service;
    private readonly ILoggingService _loggingService;

    public CompanyMasterController(ICompanyMaster service, ILoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllCompanys")]
    public async Task<IActionResult> GetAllCompanyMasters()
    {
        try
        {
            var companies = await _service.GetAll();
            var response = new
            {
                Success = true,
                Message = companies
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

    [HttpPost("CreateCompany")]
    public async Task<IActionResult> AddCompanyMaster(CompanyMasterRequest companyMaster)
    {
        try
        {
            var createdCompany = await _service.Add(companyMaster);
            var response = new
            {
                Success = true,
                Message = createdCompany
            };
            await _loggingService.AuditLog("Company Master", "POST", "/api/CompanyMaster/CreateCompany", createdCompany, companyMaster.CreatedBy, JsonSerializer.Serialize(companyMaster));
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Company Master", "POST", "/api/CompanyMaster/CreateCompany", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, companyMaster.CreatedBy, JsonSerializer.Serialize(companyMaster));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Company Master", "POST", "/api/CompanyMaster/CreateCompany", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, companyMaster.CreatedBy, JsonSerializer.Serialize(companyMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateCompany")]
    public async Task<IActionResult> UpdateCompanyMaster(CompanyMasterDto companyMaster)
    {
        try
        {
            var updatedCompany = await _service.Update(companyMaster);
            var response = new
            {
                Success = true,
                Message = updatedCompany
            };
            await _loggingService.AuditLog("Company Master", "POST", "/api/CompanyMaster/UpdateCompany", updatedCompany, companyMaster.UpdatedBy, JsonSerializer.Serialize(companyMaster));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Company Master", "POST", "/api/CompanyMaster/UpdateCompany", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, companyMaster.UpdatedBy, JsonSerializer.Serialize(companyMaster));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Company Master", "POST", "/api/CompanyMaster/UpdateCompany", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, companyMaster.UpdatedBy, JsonSerializer.Serialize(companyMaster));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Company Master", "POST", "/api/CompanyMaster/UpdateCompany", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty,  companyMaster.UpdatedBy, JsonSerializer.Serialize(companyMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}