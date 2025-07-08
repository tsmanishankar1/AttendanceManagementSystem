using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Infrastructure.Infra;

public class WorkStationMasterApp : IWorkstationMasterApp
{
    private readonly IWorkstationMasterInfra _workstationMasterInfra;
    public WorkStationMasterApp(IWorkstationMasterInfra workstationMasterInfra)
    {
        _workstationMasterInfra = workstationMasterInfra;
    }

    public async Task<string> CreateWorkstationAsync(WorkStationRequest workstationRequest)
        => await _workstationMasterInfra.CreateWorkstationAsync(workstationRequest);

    public async Task<List<WorkStationResponse>> GetAllWorkstationsAsync()
        => await _workstationMasterInfra.GetAllWorkstationsAsync();

    public async Task<string> UpdateWorkstationAsync(UpdateWorkStation updatedWorkstation)
        => await _workstationMasterInfra.UpdateWorkstationAsync(updatedWorkstation);
}