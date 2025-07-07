
using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Infrastructure.Interface
{
    public interface ILeaveGroupInfra
    {
        Task<List<LeaveGroupResponse>> GetAllLeaveGroups();
        Task<string> AddLeaveGroupWithTransactionsAsync(AddLeaveGroupDto addLeaveGroupDto);
        Task<string> UpdateLeaveGroup(UpdateLeaveGroup leaveGroup);
    }
}
