using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly ApplicationService _service;
    private readonly LoggingService _loggingService;

    public ApplicationController(ApplicationService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpPost("GetStaffApplicationStatus")]
    public async Task<IActionResult> GetApplicationDetails(ApplicationDetails applicationDetails)
    {
        try
        {
            var applicationTypes = await _service.GetApplicationDetails(applicationDetails.StaffId, applicationDetails.ApplicationTypeId);
            var response = new
            {
                Success = true,
                Message = applicationTypes
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
    [HttpGet("GetCompOffAvail")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _service.GetAllAsync();
            var response = new
            {
                Success = true,
                Message = result
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
    [HttpPost("CreateCompOffAvail")]
    public async Task<IActionResult> Create(CompOffAvailRequest request)
    {
        try
        {
            var result = await _service.CreateAsync(request);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog(
                  "CompOff Availability",
                  "POST",
                  "/CompOffAvail/Create",
                  "CompOff request submitted successfully",
                  request.CreatedBy,
                  JsonSerializer.Serialize(request));

            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError(" CompOff Availability",
            "POST",
            "/CompOffAvail/Create",
            ex.Message,
            ex.StackTrace ?? string.Empty,
            ex.InnerException?.ToString() ?? string.Empty,
            request.CreatedBy,
            JsonSerializer.Serialize(request));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("CompOff Availability",
            "POST",
            "/CompOffAvail/Create",
            ex.Message,
            ex.StackTrace ?? string.Empty,
            ex.InnerException?.ToString() ?? string.Empty,
            request.CreatedBy,
            JsonSerializer.Serialize(request));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
    [HttpPost("CancelAppliedLeave")]
    public async Task<IActionResult> CancelAppliedLeave(CancelAppliedLeave cancel)
    {
        try
        {
            var result = await _service.CancelAppliedLeave(cancel.ApplicationTypeId, cancel.Id);
            if (!result)
            {
                return NotFound(new { Success = false, Message = "Application request not found or already cancelled" });
            }
            var response = new
            {
                Success = true,
                Message = "Application request successfully cancelled"
            };

            await _loggingService.AuditLog(
                "Application Cancellation",
                "POST",
                "/CancelAppliedLeave",
                "Application request cancelled successfully",
                cancel.Id,
                JsonSerializer.Serialize(new { cancel.ApplicationTypeId, cancel.Id })
            );
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError(
                "Application Cancellation",
                "POST",
                "/CancelAppliedLeave",
                ex.Message,
                ex.StackTrace ?? string.Empty,
                ex.InnerException?.ToString() ?? string.Empty,
                cancel.Id, 
                JsonSerializer.Serialize(new { cancel.ApplicationTypeId, cancel.Id })
            );
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetCompOffCredit")]
    public async Task<IActionResult> GetCompOffCreditAll()
    {
        try
        {
            var result = await _service.GetAllAsync();
            var response = new
            {
                Success = true,
                Message = result
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
    [HttpPost("CreateCompOffCredit")]
    public async Task<IActionResult> CreateCompOffCredit(CompOffCreditDto request)
    {
        try
        {
            var result = await _service.CreateAsync(request);
            var response = new
            {
                Success = true,
                Message = result
            };

            await _loggingService.AuditLog(
                "CompOff Credit",
                "POST",
                "/CompOffCredit/CreateCompOffCredit",
                "CompOffCredit request submitted successfully",
                request.CreatedBy,
                JsonSerializer.Serialize(request)
            );

            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError(
                "CompOff Credit",
                "POST",
                "/CompOffCredit/CreateCompOffCredit",
                ex.Message,
                ex.StackTrace ?? string.Empty,
                ex.InnerException?.ToString() ?? string.Empty,
                request.CreatedBy,
                JsonSerializer.Serialize(request)
            );
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError(
                "CompOff Credit",
                "POST",
                "/CompOffCredit/CreateCompOffCredit",
                ex.Message,
                ex.StackTrace ?? string.Empty,
                ex.InnerException?.ToString() ?? string.Empty,
                request.CreatedBy,
                JsonSerializer.Serialize(request)
            );
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
    [HttpPost("GetMonthlyCalendarDetails")]
    public async Task<IActionResult> GetMonthlyDetails(MonthlyCalendar calendar)
    {
        try
        {
            var monthlyCalendar = await _service.GetMonthlyDetailsAsync(calendar.StaffId, calendar.Month, calendar.Year);
            var response = new
            {
                Success = true,
                Message = monthlyCalendar
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
    [HttpGet("GetShifts")]
    public IActionResult GetShiftsByStaffAndDateRange(int staffId, DateOnly fromDate, DateOnly toDate)
    {
        try
        {
            var shifts = _service.GetShiftsByStaffAndDateRange(staffId, fromDate, toDate);
            var response = new
            {
                Success = true,
                Message = shifts
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
    [HttpGet("GetApplicationTypeName")]
    public async Task<IActionResult> GetAllApplicationTypes()
    {
        try
        {
            var applicationTypes = await _service.GetAllApplicationTypesAsync();
            var response = new
            {
                Success = true,
                Message = applicationTypes
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

    [HttpPost("GetApplicationRequisition")]
    public async Task<IActionResult> GetApplicationRequisition(GetLeaveRequest getLeaveRequest)
    {
        try
        {
            var getLeave = await _service.GetApplicationRequisition(getLeaveRequest.ApproverId, getLeaveRequest.StaffIds, getLeaveRequest.ApplicationTypeId, getLeaveRequest.FromDate, getLeaveRequest.ToDate);
            var response = new
            {
                Success = true,
                Message = getLeave
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

    [HttpPost("ApproveApplicationRequisition")]
    public async Task<IActionResult> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest)
    {
        try
        {
            var approveLeave = await _service.ApproveApplicationRequisition(approveLeaveRequest);
            var response = new
            {
                Success = true,
                Message = approveLeave
            };
            await _loggingService.AuditLog("Application Approval", "POST", "/Application/ApproveApplication", approveLeave, approveLeaveRequest.ApprovedBy, JsonSerializer.Serialize(approveLeaveRequest));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Application Approval", "POST", "/Application/ApproveApplication", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, approveLeaveRequest.ApprovedBy, JsonSerializer.Serialize(approveLeaveRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetApprovalNotifications")]
    public async Task<IActionResult> GetApprovalNotifications(int staffId)
    {
        try
        {
            var notifications = await _service.GetApprovalNotifications(staffId);
            var response = new
            {
                Success = true,
                Message = notifications
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

    [HttpPost("UpdateApprovalNotifications")]
    public async Task<IActionResult> UpdateApprovalNotifications(int staffId, int notificationId)
    {
        try
        {
            var notifications = await _service.UpdateApprovalNotifications(staffId, notificationId);
            var response = new
            {
                Success = true,
                Message = notifications
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

    [HttpPost("CreateLeaveRequisition")]
    public async Task<IActionResult> CreateLeaveRequisition(LeaveRequisitionRequest leaveRequisition)
    {
        try
        {
            var createdLeaveRequisition = await _service.CreateLeaveRequisitionAsync(leaveRequisition);
            var response = new
            {
                Success = true,
                Message = createdLeaveRequisition
            };
            await _loggingService.AuditLog("Leave Request", "POST", "/Application/CreateLeaveRequisition", createdLeaveRequisition, leaveRequisition.CreatedBy, JsonSerializer.Serialize(leaveRequisition));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Leave Request", "POST", "/Application/CreateLeaveRequisition", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveRequisition.CreatedBy, JsonSerializer.Serialize(leaveRequisition));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Leave Request", "POST", "/Application/CreateLeaveRequisition", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, leaveRequisition.CreatedBy, JsonSerializer.Serialize(leaveRequisition));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddCommonPermission")]
    public async Task<IActionResult> CreateCommonPermission(CommonPermissionRequest commonPermission)
    {
        try
        {
            var result = await _service.AddCommonPermissionAsync(commonPermission);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Common Permission", "POST", "/Application/AddCommonPermission", result, commonPermission.CreatedBy, JsonSerializer.Serialize(commonPermission));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Common Permission", "POST", "/Application/AddCommonPermission", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, commonPermission.CreatedBy, JsonSerializer.Serialize(commonPermission));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Common Permission", "POST", "/Application/AddCommonPermission", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, commonPermission.CreatedBy, JsonSerializer.Serialize(commonPermission));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetStaffPermissionById")]
    public async Task<IActionResult> GetStaffCommonPermission(int staffId)
    {
        try
        {
            var result = await _service.GetStaffCommonPermission(staffId);
            var response = new
            {
                Success = true,
                Message = result
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

    [HttpPost("SearchByNames")]
    public async Task<IActionResult> GetStaffPermissions(GetCommonStaff getStaff)
    {
        try
        {
            var result = await _service.GetStaffPermissions(getStaff);
            var response = new
            {
                Success = true,
                Message = result
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

    [HttpGet("GetLeaveCountbyStaffId")]
    public async Task<IActionResult> GetLeaveDetails(int staffId)
    {
        try
        {
            var leaveDetails = await _service.GetLeaveDetailsWithDefaultsAsync(staffId);
            var response = new
            {
                Success = true,
                Message = leaveDetails
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

    [HttpPost("CreateManualPunch")]
    public async Task<IActionResult> CreateManualPunch(ManualPunchRequestDto createManualPunch)
    {
        try
        {

            var result = await _service.CreateManualPunchAsync(createManualPunch);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Manual Punch", "POST", "/Application/CreateManualPunch", "Manual punch updated successfully", createManualPunch.CreatedBy, JsonSerializer.Serialize(createManualPunch));
            return Ok(response);

        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Manual Punch", "POST", "/Application/CreateManualPunch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createManualPunch.CreatedBy, JsonSerializer.Serialize(createManualPunch));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Manual Punch", "POST", "/Application/CreateManualPunch", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createManualPunch.CreatedBy, JsonSerializer.Serialize(createManualPunch));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateOnDutyRequistion")]
    public async Task<IActionResult> CreateOnDutyRequisition(OnDutyRequisitionRequest onDutyRequisitionRequest)
    {
        try
        {
            var result = await _service.CreateOnDutyRequisitionAsync(onDutyRequisitionRequest);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("On Duty Request", "POST", "/Application/CreateOnDutyRequistion", "On duty request created successfully", onDutyRequisitionRequest.CreatedBy, JsonSerializer.Serialize(onDutyRequisitionRequest));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("On Duty Request", "POST", "/Application/CreateOnDutyRequistion", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, onDutyRequisitionRequest.CreatedBy, JsonSerializer.Serialize(onDutyRequisitionRequest));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("On Duty Request", "POST", "/Application/CreateOnDutyRequistion", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, onDutyRequisitionRequest.CreatedBy, JsonSerializer.Serialize(onDutyRequisitionRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateBusinessTravel")]
    public async Task<IActionResult> CreateBusinessTravel(BusinessTravelRequestDto createBusinessTravel)
    {
        try
        {
            var result = await _service.CreateBusinessTravelAsync(createBusinessTravel);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Business Travel", "POST", "/Application/CreateBusinessTravel", "Business travel created successfully", createBusinessTravel.CreatedBy, JsonSerializer.Serialize(createBusinessTravel));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Business Travel", "POST", "/Application/CreateBusinessTravel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createBusinessTravel.CreatedBy, JsonSerializer.Serialize(createBusinessTravel));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Business Travel", "POST", "/Application/CreateBusinessTravel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createBusinessTravel.CreatedBy, JsonSerializer.Serialize(createBusinessTravel));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateWorkFromHome")]
    public async Task<IActionResult> CreateWorkFromHome(WorkFromHomeDto createWorkFromHome)
    {
        try
        {
            var result = await _service.CreateWorkFromHomeAsync(createWorkFromHome);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Work From Home", "POST", "/Application/CreateWorkFromHome", "Work From Home created successfully", createWorkFromHome.CreatedBy, JsonSerializer.Serialize(createWorkFromHome));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Work From Home", "POST", "/Application/CreateWorkFromHome", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createWorkFromHome.CreatedBy, JsonSerializer.Serialize(createWorkFromHome));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Work From Home", "POST", "/Application/CreateWorkFromHome", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createWorkFromHome.CreatedBy, JsonSerializer.Serialize(createWorkFromHome));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateShiftChange")]
    public async Task<IActionResult> CreateShiftChange(ShiftChangeDto createShiftChange)
    {
        try
        {
            var result = await _service.CreateShiftChangeAsync(createShiftChange);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Shift Change", "POST", "/Application/CreateShiftChange", "Shift change request submitted successfully", createShiftChange.CreatedBy, JsonSerializer.Serialize(createShiftChange));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Shift Change", "POST", "/Application/CreateShiftChange", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createShiftChange.CreatedBy, JsonSerializer.Serialize(createShiftChange));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Shift Change", "POST", "/Application/CreateShiftChange", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createShiftChange.CreatedBy, JsonSerializer.Serialize(createShiftChange));
            return ErrorClass.ErrorResponse(ex.Message);
        }

    }

    [HttpPost("CreateShiftExtension")]
    public async Task<IActionResult> CreateShiftExtension(ShiftExtensionDto createShiftExtension)
    {
        try
        {
            var result = await _service.CreateShiftExtensionAsync(createShiftExtension);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Shift Extension", "POST", "/Application/CreateShiftExtension", "Shift extension request submitted successfully", createShiftExtension.CreatedBy, JsonSerializer.Serialize(createShiftExtension));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Shift Extension", "POST", "/Application/CraeteShiftExtension", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createShiftExtension.CreatedBy, JsonSerializer.Serialize(createShiftExtension));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Shift Extension", "POST", "/Application/CraeteShiftExtension", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createShiftExtension.CreatedBy, JsonSerializer.Serialize(createShiftExtension));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateWeeklyOff/HolidayWorking")]
    public async Task<IActionResult> CreateWeeklyOffHolidayWorking(WeeklyOffHolidayWorkingDto createWeeklyoffHolidayWorking)
    {
        try
        {
            var result = await _service.CreateWeeklyOffHolidayWorkingAsync(createWeeklyoffHolidayWorking);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Weekly Off/Holiday Working", "POST", "/Application/CreateWeeklyOff/HolidayWorking", "Weekly off request submitted successfully", createWeeklyoffHolidayWorking.CreatedBy, JsonSerializer.Serialize(createWeeklyoffHolidayWorking));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Weekly Off/Holiday Working", "POST", "/Application/CreateWeeklyOff/HolidayWorking", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createWeeklyoffHolidayWorking.CreatedBy, JsonSerializer.Serialize(createWeeklyoffHolidayWorking));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Weekly Off/Holiday Working", "POST", "/Application/CreateWeeklyOff/HolidayWorking", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createWeeklyoffHolidayWorking.CreatedBy, JsonSerializer.Serialize(createWeeklyoffHolidayWorking));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}