using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffCreationController : ControllerBase
{
    private readonly IStaffCreationApp _service;
    private readonly ILoggingApp _loggingService;

    public StaffCreationController(IStaffCreationApp service, ILoggingApp loggingService)
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
    public async Task<IActionResult> SearchStaff(GetStaff getStaff)
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
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
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
            await _loggingService.AuditLog("Update Staff", "POST", "/api/StaffCreation/UpdateStaffCreation", success, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Update Staff", "POST", "/api/StaffCreation/UpdateStaffCreation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            await _loggingService.LogError("Update Staff", "POST", "/api/StaffCreation/UpdateStaffCreation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return ErrorClass.BadResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Update Staff", "POST", "/api/StaffCreation/UpdateStaffCreation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await _loggingService.LogError("Update Staff", "POST", "/api/StaffCreation/UpdateStaffCreation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Update Staff", "POST", "/api/StaffCreation/UpdateStaffCreation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedStaff.UpdatedBy, JsonSerializer.Serialize(updatedStaff));
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
            await _loggingService.AuditLog("Create Staff", "POST", "/api/StaffCreation/AddStaff", addStaff, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Create Staff", "POST", "/api/StaffCreation/AddStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Create Staff", "POST", "/api/StaffCreation/AddStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            await _loggingService.LogError("Create Staff", "POST", "/api/StaffCreation/AddStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return ErrorClass.BadResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await _loggingService.LogError("Create Staff", "POST", "/api/StaffCreation/AddStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Create Staff", "POST", "/api/StaffCreation/AddStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, staffInput.CreatedBy, JsonSerializer.Serialize(staffInput));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetMyProfile")]
    public async Task<IActionResult> GetMyProfile(int staffId)
    {
        try
        {
            var myProfile = await _service.GetMyProfile(staffId);
            var response = new
            {
                Success = true,
                Message = myProfile
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

    [HttpPost("UpdateMyProfile")]
    public async Task<IActionResult> UpdateMyProfile(IndividualStaffUpdate updateMyProfile)
    {
        try
        {
            var updatedProfile = await _service.UpdateMyProfile(updateMyProfile);
            var response = new
            {
                Success = true,
                Message = updatedProfile
            };
            await _loggingService.AuditLog("Update Profile", "POST", "/api/StaffCreation/UpdateMyProfile", updatedProfile, updateMyProfile.UpdatedBy, JsonSerializer.Serialize(updateMyProfile));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Update Profile", "POST", "/api/StaffCreation/UpdateMyProfile", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updateMyProfile.UpdatedBy, JsonSerializer.Serialize(updateMyProfile));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Update Profile", "POST", "/api/StaffCreation/UpdateMyProfile", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updateMyProfile.UpdatedBy, JsonSerializer.Serialize(updateMyProfile));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetStaffRecordsByApprovalLevel")]
    public async Task<IActionResult> GetStaffRecordsByApprovalLevel(int currentApprover1, bool? isApprovalLevel1, bool? isApprovalLevel2, int? divisionId = null)
    {
        try
        {
            var records = await _service.GetStaffRecordsByApprovalLevelAsync(currentApprover1, isApprovalLevel1, isApprovalLevel2, divisionId);
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
        try
        {
            var result = await _service.UpdateApproversAsync(request.StaffIds, request.ApproverId1, request.ApproverId2, request.UpdatedBy);
            await _loggingService.AuditLog("Update Staff Approver", "POST", "/api/StaffCreation/UpdateApprovers", result, request.UpdatedBy, JsonSerializer.Serialize(request));
            return Ok(new { Success = true, Message = result });
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Update Staff Approver", "POST", "/api/StaffCreation/UpdateApprovers", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await _loggingService.LogError("Update Staff Approver", "POST", "/api/StaffCreation/UpdateApprovers", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Update Staff Approver", "POST", "/api/StaffCreation/UpdateApprovers", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, request.UpdatedBy, JsonSerializer.Serialize(request));
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
            await _loggingService.AuditLog("Approve Pending Staffs", "POST", "/api/StaffCreation/ApprovePendingStaffs", approveStaff, approvePendingStaff.ApprovedBy, JsonSerializer.Serialize(approvePendingStaff));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Approve Pending Staffs", "POST", "/api/StaffCreation/ApprovePendingStaffs", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, approvePendingStaff.ApprovedBy, JsonSerializer.Serialize(approvePendingStaff));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Approve Pending Staffs", "POST", "/api/StaffCreation/ApprovePendingStaffs", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, approvePendingStaff.ApprovedBy, JsonSerializer.Serialize(approvePendingStaff));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Approve Pending Staffs", "POST", "/api/StaffCreation/ApprovePendingStaffs", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, approvePendingStaff.ApprovedBy, JsonSerializer.Serialize(approvePendingStaff));
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
            await _loggingService.AuditLog("DropDown Master", "POST", "/api/StaffCreation/CreateDropDownMaster", createdDropDownMaster, dropDownRequest.CreatedBy, JsonSerializer.Serialize(dropDownRequest));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("DropDown Master", "POST", "/api/StaffCreation/CreateDropDownMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownRequest.CreatedBy, JsonSerializer.Serialize(dropDownRequest));
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
            await _loggingService.AuditLog("DropDown Master", "POST", "/api/StaffCreation/UpdateDropDownMaster", updatedDropDownMaster, updateDropDown.UpdatedBy, JsonSerializer.Serialize(updateDropDown));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("DropDown Master", "POST", "/api/StaffCreation/UpdateDropDownMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updateDropDown.UpdatedBy, JsonSerializer.Serialize(updateDropDown));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("DropDown Master", "POST", "/api/StaffCreation/UpdateDropDownMaster", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updateDropDown.UpdatedBy, JsonSerializer.Serialize(updateDropDown));
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
            await _loggingService.AuditLog("DropDown Details", "POST", "/api/StaffCreation/CreateDropDownDetails", dropDown, dropDownDetailsRequest.CreatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return Ok(response);
        }
        catch(MessageNotFoundException ex)
        {
            await _loggingService.LogError("DropDown Details", "POST", "/api/StaffCreation/CreateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.CreatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch(Exception ex)
        {
            await _loggingService.LogError("DropDown Details", "POST", "/api/StaffCreation/CreateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.CreatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
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
            await _loggingService.AuditLog("DropDown Details", "POST", "/api/StaffCreation/UpdateDropDownDetails", dropDown, dropDownDetailsRequest.UpdatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("DropDown Details", "POST", "/api/StaffCreation/UpdateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.UpdatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("DropDown Details", "POST", "/api/StaffCreation/UpdateDropDownDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, dropDownDetailsRequest.UpdatedBy, JsonSerializer.Serialize(dropDownDetailsRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}