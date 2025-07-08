using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class LeaveGroupConfigurationApp : ILeaveGroupConfigurationApp
    {
        private readonly ILeaveGroupConfigurationInfra _leaveGroupConfigurationApp;
        public LeaveGroupConfigurationApp(ILeaveGroupConfigurationInfra leaveGroupConfigurationApp)
        {
            _leaveGroupConfigurationApp = leaveGroupConfigurationApp;
        }

        public async Task<string> CreateConfigurations(LeaveGroupConfigurationRequest configurationRequeset)
            => await _leaveGroupConfigurationApp.CreateConfigurations(configurationRequeset);

        public async Task<List<LeaveGroupConfigurationDto>> GetAllConfigurations()
            => await _leaveGroupConfigurationApp.GetAllConfigurations();

        public async Task<LeaveGroupConfigurationDto> GetConfigurationDetailsById(int leaveGroupConfigurationId)
            => await _leaveGroupConfigurationApp.GetConfigurationDetailsById(leaveGroupConfigurationId);

        public async Task<string> UpdateConfigurations(UpdateLeaveGroupConfiguration configuration)
            => await _leaveGroupConfigurationApp.UpdateConfigurations(configuration);
    }
}