using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IWorkstationMasterService
    {
        Task<List<WorkStationResponse>> GetAllWorkstationsAsync();
        Task<string> CreateWorkstationAsync(WorkStationRequest workstationRequest);
        Task<string> UpdateWorkstationAsync(UpdateWorkStation updatedWorkstation);
    }
}
