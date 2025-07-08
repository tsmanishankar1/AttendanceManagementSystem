
using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ILeaveGroupApp
    {
        Task<List<LeaveGroupResponse>> GetAllLeaveGroups();
        Task<string> AddLeaveGroupWithTransactionsAsync(AddLeaveGroupDto addLeaveGroupDto);
        Task<string> UpdateLeaveGroup(UpdateLeaveGroup leaveGroup);
    }
}
