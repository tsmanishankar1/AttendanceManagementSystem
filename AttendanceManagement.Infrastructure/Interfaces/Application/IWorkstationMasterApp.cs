using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IWorkstationMasterApp
    {
        Task<List<WorkStationResponse>> GetAllWorkstationsAsync();
        Task<string> CreateWorkstationAsync(WorkStationRequest workstationRequest);
        Task<string> UpdateWorkstationAsync(UpdateWorkStation updatedWorkstation);
    }
}
