using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CostCentreMasterController : ControllerBase
{
    private readonly CostCentreMasterService _service;
    private readonly LoggingService _loggingService;

    public CostCentreMasterController(CostCentreMasterService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllCostCentres")]
    public async Task<IActionResult> GetAllCostCentres()
    {
        try
        {
            var costCentres = await _service.GetAllCostCentres();
            var response = new
            {
                Success = true,
                Message = costCentres
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

    [HttpGet("GetCostCentreById")]
    public async Task<IActionResult> GetCostCentreById(int costCentreMasterId)
    {
        try
        {
            var costCentre = await _service.GetCostCentreById(costCentreMasterId);
            var response = new
            {
                Success = true,
                Message = costCentre
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

    [HttpPost("CreateCostCentre")]
    public async Task<IActionResult> CreateCostCentre(CostMasterRequest costCentreMaster)
    {
        try
        {
            var createdCostCentre = await _service.CreateCostCentre(costCentreMaster);
            var response = new
            {
                Success = true,
                Message = createdCostCentre
            };
            await _loggingService.AuditLog("Cost Centre Master", "POST", "/CostCentreMaster/CreateCostCentre", createdCostCentre, costCentreMaster.CreatedBy, JsonSerializer.Serialize(costCentreMaster));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Cost Centre Master", "POST", "/CostCentreMaster/CreateCostCentre", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, costCentreMaster.CreatedBy, JsonSerializer.Serialize(costCentreMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateCostCentre")]
    public async Task<IActionResult> UpdateCostCentre(UpdateCostMaster costCentreMaster)
    {
        try
        {
            var updatedCostCentre = await _service.UpdateCostCentre(costCentreMaster);
            var response = new
            {
                Success = true,
                Message = updatedCostCentre
            };
            await _loggingService.AuditLog("Cost Centre Master", "POST", "/CostCentreMaster/UpdateCostCentre", updatedCostCentre ?? string.Empty, costCentreMaster.UpdatedBy, JsonSerializer.Serialize(costCentreMaster));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Cost Centre Master", "POST", "/CostCentreMaster/UpdateCostCentre", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, costCentreMaster.UpdatedBy, JsonSerializer.Serialize(costCentreMaster));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Cost Centre Master", "POST", "/CostCentreMaster/UpdateCostCentre", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, costCentreMaster.UpdatedBy, JsonSerializer.Serialize(costCentreMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}