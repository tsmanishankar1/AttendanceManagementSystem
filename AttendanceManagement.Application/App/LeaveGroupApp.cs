using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Infrastructure.Interface;

namespace AttendanceManagement.Application.App
{
    public class LeaveGroupApp : ILeaveGroupApp
    {
        private readonly ILeaveGroupInfra _leaveGroupInfra;
        public LeaveGroupApp(ILeaveGroupInfra leaveGroupInfra)
        {
            _leaveGroupInfra = leaveGroupInfra;
        }

        public async Task<string> AddLeaveGroupWithTransactionsAsync(AddLeaveGroupDto addLeaveGroupDto)
            => await _leaveGroupInfra.AddLeaveGroupWithTransactionsAsync(addLeaveGroupDto);

        public async Task<List<LeaveGroupResponse>> GetAllLeaveGroups()
            => await _leaveGroupInfra.GetAllLeaveGroups();

        public async Task<string> UpdateLeaveGroup(UpdateLeaveGroup leaveGroup)
            => await _leaveGroupInfra.UpdateLeaveGroup(leaveGroup);
    }
}