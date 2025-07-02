using AttendanceManagement.InputModels;
using AttendanceManagement.Services;
using AttendanceManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BranchMasterController : ControllerBase
{
    private readonly IBranchMasterService _service;
    private readonly ILoggingService _loggingService;

    public BranchMasterController(IBranchMasterService service, ILoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllBranches")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var branches = await _service.GetAllBranches();
            var response = new
            {
                Success = true,
                Message = branches
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

    [HttpPost("CreateBranch")]
    public async Task<IActionResult> Create(BranchMasterRequest branchMasterRequest)
    {
        try
        {
            var createdBranch = await _service.CreateBranch(branchMasterRequest);
            var response = new
            {
                Success = true,
                Message = createdBranch
            };
            await _loggingService.AuditLog("Branch Master", "POST", "/api/BranchMaster/CreateBranch", createdBranch, branchMasterRequest.CreatedBy, JsonSerializer.Serialize(branchMasterRequest));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/api/BranchMaster/CreateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMasterRequest.CreatedBy, JsonSerializer.Serialize(branchMasterRequest));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/api/BranchMaster/CreateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMasterRequest.CreatedBy, JsonSerializer.Serialize(branchMasterRequest));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/api/BranchMaster/CreateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMasterRequest.CreatedBy, JsonSerializer.Serialize(branchMasterRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateBranch")]
    public async Task<IActionResult> Update(UpdateBranch branchMaster)
    {
        try
        {
            var updatedBranch = await _service.UpdateBranch(branchMaster);
            var response = new
            {
                Success = true,
                Message = updatedBranch
            };
            await _loggingService.AuditLog("Branch Master", "POST", "/api/BranchMaster/UpdateBranch", updatedBranch, branchMaster.UpdatedBy, JsonSerializer.Serialize(branchMaster));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/api/BranchMaster/UpdateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMaster.UpdatedBy, JsonSerializer.Serialize(branchMaster));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/api/BranchMaster/UpdateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMaster.UpdatedBy, JsonSerializer.Serialize(branchMaster));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/api/BranchMaster/UpdateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMaster.UpdatedBy, JsonSerializer.Serialize(branchMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}