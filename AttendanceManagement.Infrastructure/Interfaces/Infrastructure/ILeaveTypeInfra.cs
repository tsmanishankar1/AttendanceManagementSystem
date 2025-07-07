using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ILeaveTypeInfra
    {
        Task<List<LeaveTypeResponse>> GetAllLeaveTypesAsync();
        Task<string> CreateLeaveTypeAsync(LeaveTypeRequest leaveTypeRequest);
        Task<string> UpdateLeaveTypeAsync(UpdateLeaveType leaveType);
    }
}
