using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application;

public interface IApplicationApp
{
    Task<bool> CancelAppliedLeave(CancelAppliedLeave cancel);
    Task<bool> RevokeAppliedLeave(CancelAppliedLeave cancel);
    Task<IEnumerable<object>> GetApplicationDetails(int staffId, int applicationTypeId);
    Task<object> GetMonthlyDetailsAsync(int staffId, int month, int year);
    Task<List<CompOffCreditResponseDto>> GetCompOffCreditAllAsync();
    Task<string> CreateAsync(CompOffCreditDto compOffCreditDto);
    Task<string> CreateAsync(CompOffAvailRequest request);
    Task<List<CompOffAvailDto>> GetCompOffAvail();
    Task<List<CompOffCreditResponseDto>> GetCompOffCredit();
    Task<List<object>> GetApplicationRequisition(int approverId, List<int>? staffIds, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate);
    Task<List<ApprovalNotificationResponse>> GetApprovalNotifications(int staffId);
    Task<List<RequestNotificationResponse>> GetRequestNotifications(int staffId);
    Task<string> UpdateApprovalNotifications(int staffId, int notificationId);
    Task<string> CreateLeaveRequisitionAsync(LeaveRequisitionRequest leaveRequisitionRequest);
    Task<string> AddCommonPermissionAsync(CommonPermissionRequest commonPermissionRequest);
    Task<List<StaffPermissionResponse>> GetStaffCommonPermission(int? staffId);
    Task<List<CommonPermissionResponse>> GetStaffPermissions(GetCommonStaff getStaff);
    Task<List<object>> GetLeaveDetailsWithDefaultsAsync(int StaffCreationId);
    Task<string> CreateManualPunchAsync(ManualPunchRequestDto request);
    Task<string> CreateOnDutyRequisitionAsync(OnDutyRequisitionRequest request);
    Task<string> CreateBusinessTravelAsync(BusinessTravelRequestDto request);
    Task<string> CreateWorkFromHomeAsync(WorkFromHomeDto request);
    Task<List<object>> GetShiftsByStaffAndDateRange(int staffId, DateOnly fromDate, DateOnly toDate);
    Task<string> CreateShiftChangeAsync(ShiftChangeDto request);
    Task<string> CreateShiftExtensionAsync(ShiftExtensionDto request);
    Task<string> CreateWeeklyOffHolidayWorkingAsync(WeeklyOffHolidayWorkingDto request);
    Task<string> AddReimbursement(ReimbursementRequestModel request);
}
