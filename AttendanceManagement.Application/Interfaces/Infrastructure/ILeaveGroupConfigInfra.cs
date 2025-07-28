using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ILeaveGroupConfigurationInfra
    {
        Task<List<LeaveGroupConfigurationDto>> GetAllConfigurations();
        Task<LeaveGroupConfigurationDto> GetConfigurationDetailsById(int leaveGroupConfigurationId);
        Task<string> CreateConfigurations(LeaveGroupConfigurationRequest configurationRequeset);
        Task<string> UpdateConfigurations(UpdateLeaveGroupConfiguration configuration);
    }
}
