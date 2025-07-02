using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ILeaveGroupConfigurationService
    {
        Task<List<LeaveGroupConfigurationDto>> GetAllConfigurations();
        Task<LeaveGroupConfigurationDto> GetConfigurationDetailsById(int leaveGroupConfigurationId);
        Task<string> CreateConfigurations(LeaveGroupConfigurationRequest configurationRequeset);
        Task<string> UpdateConfigurations(UpdateLeaveGroupConfiguration configuration);
    }
}
