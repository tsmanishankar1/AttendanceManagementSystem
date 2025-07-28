using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class ApplicationApp : IApplicationApp
{
    private readonly IApplicationInfra _applicationInfra;

    public ApplicationApp(IApplicationInfra applicationInfra)
    {
        _applicationInfra = applicationInfra;
    }

    public async Task<string> AddCommonPermissionAsync(CommonPermissionRequest commonPermissionRequest)
        => await _applicationInfra.AddCommonPermissionAsync(commonPermissionRequest);

    public async Task<string> AddReimbursement(ReimbursementRequestModel request)
        => await _applicationInfra.AddReimbursement(request);

    public async Task<bool> CancelAppliedLeave(CancelAppliedLeave cancel)
        => await _applicationInfra.CancelAppliedLeave(cancel);

    public async Task<string> CreateAsync(CompOffCreditDto compOffCreditDto)
        => await _applicationInfra.CreateAsync(compOffCreditDto);

    public async Task<string> CreateAsync(CompOffAvailRequest request)
        => await _applicationInfra.CreateAsync(request);

    public async Task<string> CreateBusinessTravelAsync(BusinessTravelRequestDto request)
        => await _applicationInfra.CreateBusinessTravelAsync(request);

    public async Task<string> CreateLeaveRequisitionAsync(LeaveRequisitionRequest leaveRequisitionRequest)
        => await _applicationInfra.CreateLeaveRequisitionAsync(leaveRequisitionRequest);

    public async Task<string> CreateManualPunchAsync(ManualPunchRequestDto request)
        => await _applicationInfra.CreateManualPunchAsync(request);

    public async Task<string> CreateOnDutyRequisitionAsync(OnDutyRequisitionRequest request)
        => await _applicationInfra.CreateOnDutyRequisitionAsync(request);

    public async Task<string> CreateShiftChangeAsync(ShiftChangeDto request)
        => await _applicationInfra.CreateShiftChangeAsync(request);

    public async Task<string> CreateShiftExtensionAsync(ShiftExtensionDto request)
        => await _applicationInfra.CreateShiftExtensionAsync(request);

    public async Task<string> CreateWeeklyOffHolidayWorkingAsync(WeeklyOffHolidayWorkingDto request)
        => await _applicationInfra.CreateWeeklyOffHolidayWorkingAsync(request);

    public async Task<string> CreateWorkFromHomeAsync(WorkFromHomeDto request)
        => await _applicationInfra.CreateWorkFromHomeAsync(request);

    public async Task<IEnumerable<object>> GetApplicationDetails(int staffId, int applicationTypeId)
        => await _applicationInfra.GetApplicationDetails(staffId, applicationTypeId);

    public async Task<List<object>> GetApplicationRequisition(int approverId, List<int>? staffIds, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate)
        => await _applicationInfra.GetApplicationRequisition(approverId, staffIds, applicationTypeId, fromDate, toDate);

    public async Task<List<ApprovalNotificationResponse>> GetApprovalNotifications(int staffId)
        => await _applicationInfra.GetApprovalNotifications(staffId);

    public async Task<List<CompOffAvailDto>> GetCompOffAvail()
        => await _applicationInfra.GetCompOffAvail();

    public async Task<List<CompOffCreditResponseDto>> GetCompOffCredit()
        => await _applicationInfra.GetCompOffCredit();

    public async Task<List<CompOffCreditResponseDto>> GetCompOffCreditAllAsync()
        => await _applicationInfra.GetCompOffCreditAllAsync();

    public async Task<List<object>> GetLeaveDetailsWithDefaultsAsync(int StaffCreationId)
        => await _applicationInfra.GetLeaveDetailsWithDefaultsAsync(StaffCreationId);

    public async Task<object> GetMonthlyDetailsAsync(int staffId, int month, int year)
        => await _applicationInfra.GetMonthlyDetailsAsync(staffId, month, year);

    public async Task<List<RequestNotificationResponse>> GetRequestNotifications(int staffId)
        => await _applicationInfra.GetRequestNotifications(staffId);

    public async Task<List<object>> GetShiftsByStaffAndDateRange(int staffId, DateOnly fromDate, DateOnly toDate)
        => await _applicationInfra.GetShiftsByStaffAndDateRange(staffId, fromDate, toDate);

    public async Task<List<StaffPermissionResponse>> GetStaffCommonPermission(int? staffId)
        => await _applicationInfra.GetStaffCommonPermission(staffId);

    public async Task<List<CommonPermissionResponse>> GetStaffPermissions(GetCommonStaff getStaff)
        => await _applicationInfra.GetStaffPermissions(getStaff);

    public async Task<bool> RevokeAppliedLeave(CancelAppliedLeave cancel)
        => await _applicationInfra.RevokeAppliedLeave(cancel);

    public async Task<string> UpdateApprovalNotifications(int staffId, int notificationId)
        => await _applicationInfra.UpdateApprovalNotifications(staffId, notificationId);
}
