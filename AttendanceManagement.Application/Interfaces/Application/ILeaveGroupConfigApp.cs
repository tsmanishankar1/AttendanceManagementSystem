using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ILeaveGroupConfigurationApp
    {
        Task<List<LeaveGroupConfigurationDto>> GetAllConfigurations();
        Task<LeaveGroupConfigurationDto> GetConfigurationDetailsById(int leaveGroupConfigurationId);
        Task<string> CreateConfigurations(LeaveGroupConfigurationRequest configurationRequeset);
        Task<string> UpdateConfigurations(UpdateLeaveGroupConfiguration configuration);
    }
}
