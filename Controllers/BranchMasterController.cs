using AttendanceManagement.AtrakModels;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BranchMasterController : ControllerBase
{
    private readonly BranchMasterService _service;
    private readonly LoggingService _loggingService;

    public BranchMasterController(BranchMasterService service, LoggingService loggingService)
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

    [HttpGet("GetBranchById")]
    public async Task<IActionResult> GetById(int branchMasterId)
    {
        try
        {
            var branch = await _service.GetBranchById(branchMasterId);
            var response = new
            {
                Success = true,
                Message = branch
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
            await _loggingService.AuditLog("Branch Master", "POST", "/BranchMaster/CreateBranch", createdBranch, branchMasterRequest.CreatedBy, JsonSerializer.Serialize(branchMasterRequest));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/BranchMaster/CreateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMasterRequest.CreatedBy, JsonSerializer.Serialize(branchMasterRequest));
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
            await _loggingService.AuditLog("Branch Master", "POST", "/BranchMaster/UpdateBranch", updatedBranch, branchMaster.UpdatedBy, JsonSerializer.Serialize(branchMaster));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/BranchMaster/UpdateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMaster.UpdatedBy, JsonSerializer.Serialize(branchMaster));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Branch Master", "POST", "/BranchMaster/UpdateBranch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, branchMaster.UpdatedBy, JsonSerializer.Serialize(branchMaster));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

/*    [HttpGet("GetStaffAttendance")]
    public async Task<IActionResult> GetStaffAttendance(string staffId)
    {
        try
        {
            var staffAttendance = await _service.GetStaffAttendance(staffId);
            var response = new
            {
                Success = true,
                Message = staffAttendance
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

    [HttpPost("UpdateStaffAttendance")]
    public async Task<IActionResult> UpdateStaffAttendance(AttendanceDatum attendanceDatum)
    {
        try
        {
            var updatedAttendance = await _service.UpdateStaffAttendance(attendanceDatum);
            var response = new
            {
                Success = true,
                Message = updatedAttendance
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
*/}