using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BranchMasterController : ControllerBase
{
    private readonly IBranchMasterApp _service;
    private readonly ILoggingApp _loggingService;

    public BranchMasterController(IBranchMasterApp service, ILoggingApp loggingService)
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

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetAppsettings")]
    public IActionResult GetAppsettings()
    {
        try
        {
            var branches = _service.GetAppsettings();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetWorkspaceFile")]
    public IActionResult GetWorkspaceFile()
    {
        try
        {
            var branches = _service.GetWorkspaceFile();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (DirectoryNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetGoals")]
    public async Task<IActionResult> GetGoals()
    {
        try
        {
            var branches = await _service.GetGoals();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetManagerReview")]
    public async Task<IActionResult> GetManagerReview()
    {
        try
        {
            var branches = await _service.KraManagerReviews();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetSelfReview")]
    public async Task<IActionResult> GetSelfReview()
    {
        try
        {
            var branches = await _service.KraSelfReviews();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetUserManagement")]
    public async Task<IActionResult> GetUserManagement()
    {
        try
        {
            var branches = await _service.GetUserManagement();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetProbbations")]
    public async Task<IActionResult> GetProbations()
    {
        try
        {
            var branches = await _service.GetProbations();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetFeedbacks")]
    public async Task<IActionResult> GetFeedbacks()
    {
        try
        {
            var branches = await _service.GetFeedbacks();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetProbationReport")]
    public async Task<IActionResult> GetProbationReport()
    {
        try
        {
            var branches = await _service.GetProbationReports();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("GetRefreshToken")]
    public async Task<IActionResult> GetRefreshToken()
    {
        try
        {
            var branches = await _service.GetRefreshToken();
            var response = new
            {
                Success = true,
                Message = branches
            };
            return Ok(response);
        }
        catch (FileNotFoundException ex)
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