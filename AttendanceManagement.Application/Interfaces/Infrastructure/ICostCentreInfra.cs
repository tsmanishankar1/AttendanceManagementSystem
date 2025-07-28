using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ICostCentreInfra
    {
        Task<List<CostMasterResponse>> GetAllCostCentres();
        Task<string> CreateCostCentre(CostMasterRequest costCentreMaster);
        Task<string> UpdateCostCentre(UpdateCostMaster costCentreMaster);
    }
}
