using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IWorkstationMasterInfra
    {
        Task<List<WorkStationResponse>> GetAllWorkstationsAsync();
        Task<string> CreateWorkstationAsync(WorkStationRequest workstationRequest);
        Task<string> UpdateWorkstationAsync(UpdateWorkStation updatedWorkstation);
    }
}
