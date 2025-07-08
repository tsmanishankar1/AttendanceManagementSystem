using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ICostCentreApp
    {
        Task<List<CostMasterResponse>> GetAllCostCentres();
        Task<string> CreateCostCentre(CostMasterRequest costCentreMaster);
        Task<string> UpdateCostCentre(UpdateCostMaster costCentreMaster);
    }
}
