using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class CostCentreMasterApp : ICostCentreApp
{
    private readonly ICostCentreInfra _costCentreInfra;
    public CostCentreMasterApp(ICostCentreInfra costCentreInfra)
    {
        _costCentreInfra = costCentreInfra;
    }

    public async Task<string> CreateCostCentre(CostMasterRequest costCentreMaster)
        => await _costCentreInfra.CreateCostCentre(costCentreMaster);

    public async Task<List<CostMasterResponse>> GetAllCostCentres()
        => await _costCentreInfra.GetAllCostCentres();

    public async Task<string> UpdateCostCentre(UpdateCostMaster costCentreMaster)
        => await _costCentreInfra.UpdateCostCentre(costCentreMaster);
}