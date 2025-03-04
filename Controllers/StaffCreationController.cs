using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffCreationController : ControllerBase
{
    private readonly StaffCreationService _service;
    private readonly LoggingService _loggingService;

    public StaffCreationController(StaffCreationService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;

    }

    [HttpGet("GetByStaffId")]
    public async Task<IActionResult> GetByUserManagementId(int StaffId)
    {
        try
        {
            var staff = await _service.GetByUserManagementIdAsync(StaffId);
            var response = new
            {
                Success = true,
                Message = staff
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

    [HttpPost("search")]
    public async Task<ActionResult<List<StaffDto>>> SearchStaff(GetStaff getStaff)
    {
        try
        {
            var staffList = await _service.GetStaffAsync(getStaff);
            var response = new
            {
                Success = true,
                Data = staffList
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return NotFound(new { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("UpdateStaffCreation")]
    public async Task<IActionResult> UpdateStaffCreation(UpdateStaff updatedStaff)
    {
        try
        {
            var success = await _service.UpdateStaffCreationAsync(updatedStaff);
            var response = new
            {
                Success = true,
                Message = success
            };

            await _loggingService.AuditLog("UpdateStaff", "POST", "/Staff/UpdateStaffById", success, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("UpdateStaff", "POST", "/Staff/UpdateStaffById", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("UpdateStaff", "POST", "/Staff/UpdateStaffById", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddStaff")]
    public async Task<IActionResult> AddStaff(StaffCreationInputModel staffInput)
    {
        try
        {
            var addStaff = await _service.AddStaff(staffInput);
            var response = new
            {
                Success = true,
                Message = addStaff
            };
            await _loggingService.AuditLog("CreateStaff", "POST", "/Staff/CreateStaff", addStaff, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("CreateStaff", "POST", "/Staff/CreateStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("CreateStaff", "POST", "/Staff/CreateStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetStaffRecordsByApprovalLevel")]
    public async Task<IActionResult> GetStaffRecordsByApprovalLevel(int currentApprover1)
    {
        try
        {
            var records = await _service.GetStaffRecordsByApprovalLevelAsync(currentApprover1);
            var response = new
            {
                Success = true,
                Message = records
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

    [HttpPost("UpdateApprovers")]
    public async Task<IActionResult> UpdateApprovers(ApproverUpdateRequest request)
    {
        if (request == null || request.StaffIds == null || !request.StaffIds.Any() || request.UpdatedBy <= 0)
        {
            return BadRequest(new { Success = false, Message = "Invalid input parameters" });
        }

        try
        {
            var result = await _service.UpdateApproversAsync(request.StaffIds, request.ApproverId1, request.ApproverId2, request.UpdatedBy);

            if (result != "Approvers updated successfully")
            {
                return BadRequest(new { Success = false, Message = result });
            }

            await _loggingService.AuditLog("Staff", "POST", "/api/Staff/update-approvers", result, request.UpdatedBy, JsonSerializer.Serialize(request));
            return Ok(new { Success = true, Message = "Approvers updated successfully" });
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Staff", "POST", "/api/Staff/update-approvers", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Staff", "POST", "/api/Staff/update-approvers", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetPendingStaffForManagerApproval")]
    public async Task<IActionResult> GetPendingStaffForManagerApproval(int approverId)
    {
        try
        {
            var pendingStaff = await _service.GetPendingStaffForManagerApproval(approverId);
            var response = new
            {
                Success = true,
                Message = pendingStaff
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

    [HttpPost("ApprovePendingStaffs")]
    public async Task<IActionResult> ApprovePendingStaffs(ApprovePendingStaff approvePendingStaff)
    {
        try
        {
            var approveStaff = await _service.ApprovePendingStaffs(approvePendingStaff);
            var response = new
            {
                Success = true,
                Message = approveStaff
            };
            await _loggingService.AuditLog("ApprovePendingStaffs", "POST", "/Staff/ApprovePendingStaffs", approveStaff, approvePendingStaff.ApprovedBy, JsonSerializer.Serialize(approvePendingStaff));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("ApprovePendingStaffs", "POST", "/Staff/ApprovePendingStaffs", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, approvePendingStaff.ApprovedBy, JsonSerializer.Serialize(approvePendingStaff));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("ApprovePendingStaffs", "POST", "/Staff/ApprovePendingStaffs", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, approvePendingStaff.ApprovedBy, JsonSerializer.Serialize(approvePendingStaff));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateDropDownMaster")]
    public async Task<IActionResult> CreateDropDownMaster(DropDownRequest dropDownRequest)
    {
        try
        {
            var createdDropDownMaster = await _service.CreateDropDownMaster(dropDownRequest);
            var response = new
            {
                Success = true,
                Message = createdDropDownMaster
            };
            await _loggingService.AuditLog("DropDownMaster", "POST", "/Staff/CreateDropDownMaster", createdDropDownMaster, dropDownRequest.CreatedBy, JsonSerializer.Serialize(dropDownRequest));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("DropDownMaster", "POST", "/Staff/CreateDropDownMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownRequest.CreatedBy, JsonSerializer.Serialize(dropDownRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetDropDownMaster")]
    public async Task<IActionResult> GetDropDownMaster()
    {
        try
        {
            var dropDownMaster = await _service.GetDropDownMaster();
            var response = new
            {
                Success = true,
                Message = dropDownMaster
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

    [HttpPost("UpdateDropDownMaster")]
    public async Task<IActionResult> UpdateDropDownMaster(UpdateDropDown updateDropDown)
    {
        try
        {
            var updatedDropDownMaster = await _service.UpdateDropDownMaster(updateDropDown);
            var response = new
            {
                Success = true,
                Message = updatedDropDownMaster
            };
            await _loggingService.AuditLog("DropDownMaster", "POST", "/Staff/UpdateDropDownMaster", updatedDropDownMaster, updateDropDown.UpdatedBy, JsonSerializer.Serialize(updateDropDown));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("DropDownMaster", "POST", "/Staff/UpdateDropDownMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updateDropDown.UpdatedBy, JsonSerializer.Serialize(updateDropDown));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("DropDownMaster", "POST", "/Staff/UpdateDropDownMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updateDropDown.UpdatedBy, JsonSerializer.Serialize(updateDropDown));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateDropDownDetails")]
    public async Task<IActionResult> CreateDropDownDetails(DropDownDetailsRequest dropDownDetailsRequest)
    {
        try
        {
            var dropDown = await _service.CreateDropDownDetails(dropDownDetailsRequest);
            var response = new
            {
                Success = true,
                Message = dropDown
            };
            await _loggingService.AuditLog("DropDownDetails", "POST", "/Staff/CreateDropDownDetails", dropDown, dropDownDetailsRequest.CreatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return Ok(response);
        }
        catch(MessageNotFoundException ex)
        {
            await _loggingService.LogError("DropDownDetails", "POST", "/Staff/CreateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.CreatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch(Exception ex)
        {
            await _loggingService.LogError("DropDownDetails", "POST", "/Staff/CreateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.CreatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAllDropDowns")]
    public async Task<IActionResult> GetAllDropDowns(int id)
    {
        try
        {
            var dropDownMaster = await _service.GetAllDropDowns(id);
            var response = new
            {
                Success = true,
                Message = dropDownMaster
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

    [HttpPost("UpdateDropDownDetails")]
    public async Task<IActionResult> UpdateDropDownDetails(DropDownDetailsUpdate dropDownDetailsRequest)
    {
        try
        {
            var dropDown = await _service.UpdateDropDownDetails(dropDownDetailsRequest);
            var response = new
            {
                Success = true,
                Message = dropDown
            };
            await _loggingService.AuditLog("DropDownDetails", "POST", "/Staff/UpdateDropDownDetails", dropDown, dropDownDetailsRequest.UpdatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("DropDownDetails", "POST", "/Staff/UpdateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.UpdatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("DropDownDetails", "POST", "/Staff/UpdateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.UpdatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}