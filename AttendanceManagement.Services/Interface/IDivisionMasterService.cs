using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IDivisionMasterService
    {
        Task<List<DivisionResponse>> GetAllDivisions();
        Task<string> AddDivision(DivisionRequest divisionRequest);
        Task<string> UpdateDivision(UpdateDivision division);
    }
}
