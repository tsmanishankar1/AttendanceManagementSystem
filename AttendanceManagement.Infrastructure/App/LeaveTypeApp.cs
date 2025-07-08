using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class LeaveTypeApp : ILeaveTypeApp
    {
        private readonly ILeaveTypeInfra _leaveTypeInfra;
        public LeaveTypeApp(ILeaveTypeInfra leaveTypeInfra)
        {
            _leaveTypeInfra = leaveTypeInfra;
        }

        public async Task<string> CreateLeaveTypeAsync(LeaveTypeRequest leaveTypeRequest)
            => await _leaveTypeInfra.CreateLeaveTypeAsync(leaveTypeRequest);

        public async Task<List<LeaveTypeResponse>> GetAllLeaveTypesAsync()
            => await _leaveTypeInfra.GetAllLeaveTypesAsync();

        public async Task<string> UpdateLeaveTypeAsync(UpdateLeaveType leaveType)
            => await _leaveTypeInfra.UpdateLeaveTypeAsync(leaveType);
    }
}