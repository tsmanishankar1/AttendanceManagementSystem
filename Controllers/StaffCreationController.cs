using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffCreationController : ControllerBase
{
    private readonly StaffCreationService _service;
    private readonly AttendanceManagementSystemContext _context;

    public StaffCreationController(StaffCreationService service, AttendanceManagementSystemContext context)
    {
        _service = service;
        _context = context;
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

            AuditLog log = new AuditLog
            {
                Module = "UpdateStaff",
                HttpMethod = "POST",
                ApiEndpoint = "/Staff/UpdateStaffById",
                SuccessMessage = success,
                Payload = System.Text.Json.JsonSerializer.Serialize(updatedStaff),
                StaffId = updatedStaff.UpdatedBy,
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
                    Module = "UpdateStaff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/UpdateStaffById",
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedStaff.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedStaff),
                    CreatedUtc = DateTime.UtcNow
                };
                _context.ErrorLogs.Add(log);
                await _context.SaveChangesAsync();
            }

            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "UpdateStaff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/UpdateStaffById",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedStaff.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedStaff),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "CreateStaff",
                HttpMethod = "POST",
                ApiEndpoint = "/Staff/CreateStaff",
                SuccessMessage = addStaff,
                Payload = System.Text.Json.JsonSerializer.Serialize(staffInput),
                StaffId = staffInput.CreatedBy,
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
                    Module = "CreateStaff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/CreateStaff",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = staffInput.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(staffInput),
                    CreatedUtc = DateTime.UtcNow
                };

                _context.ErrorLogs.Add(log);
                await _context.SaveChangesAsync();
            }
                return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "CreateStaff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/CreateStaff",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = staffInput.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(staffInput),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
        if (request == null || request.StaffId <= 0 || request.UpdatedBy <= 0)
        {
            return BadRequest(new { Success = false, Message = "Invalid input parameters" });
        }

        try
        {
            var result = await _service.UpdateApproversAsync(request.StaffId, request.ApproverId1, request.ApproverId2, request.UpdatedBy);

            if (result != "Success")
            {
                return BadRequest(new { Success = false, Message = result });
            }
            var auditLog = new AuditLog
            {
                Module = "Staff",
                HttpMethod = "POST",
                ApiEndpoint = "/api/Staff/update-approvers",
                SuccessMessage = result,
                Payload = System.Text.Json.JsonSerializer.Serialize(request),
                StaffId = request.UpdatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Approvers updated successfully" });
        }
        catch (MessageNotFoundException ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "UpdateStaff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/UpdateStaffByApprovers",
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = request.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(request),
                    CreatedUtc = DateTime.UtcNow
                };
                _context.ErrorLogs.Add(log);
                await _context.SaveChangesAsync();
            }

            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "UpdateStaff",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/UpdateStaffByApprovers",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = request.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(request),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "ApprovePendingStaffs",
                HttpMethod = "POST",
                ApiEndpoint = "/Staff/ApprovePendingStaffs",
                SuccessMessage = approveStaff,
                Payload = System.Text.Json.JsonSerializer.Serialize(approvePendingStaff),
                StaffId = approvePendingStaff.ApprovedBy,
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
                    Module = "ApprovePendingStaffs",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/ApprovePendingStaffs",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = approvePendingStaff.ApprovedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(approvePendingStaff),
                    CreatedUtc = DateTime.UtcNow
                };

                _context.ErrorLogs.Add(log);
                await _context.SaveChangesAsync();
            }
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "ApprovePendingStaffs",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Staff/ApprovePendingStaffs",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = approvePendingStaff.ApprovedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(approvePendingStaff),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            return Ok(response);
        }
        catch (Exception ex)
        {
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
            return Ok(response);
        }
        catch(MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch(Exception ex)
        {
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
}