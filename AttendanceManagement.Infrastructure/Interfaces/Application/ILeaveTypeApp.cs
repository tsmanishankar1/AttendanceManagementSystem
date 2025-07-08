using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ILeaveTypeApp
    {
        Task<List<LeaveTypeResponse>> GetAllLeaveTypesAsync();
        Task<string> CreateLeaveTypeAsync(LeaveTypeRequest leaveTypeRequest);
        Task<string> UpdateLeaveTypeAsync(UpdateLeaveType leaveType);
    }
}
