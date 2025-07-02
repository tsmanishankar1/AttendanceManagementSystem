using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ICostCentre
    {
        Task<List<CostMasterResponse>> GetAllCostCentres();
        Task<string> CreateCostCentre(CostMasterRequest costCentreMaster);
        Task<string> UpdateCostCentre(UpdateCostMaster costCentreMaster);
    }
}
