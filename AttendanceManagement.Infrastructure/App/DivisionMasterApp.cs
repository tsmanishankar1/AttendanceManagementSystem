using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class DivisionMasterApp : IDivisionMasterApp
    {
        private readonly IDivisionMasterInfra _divisionMasterInfra;
        public DivisionMasterApp(IDivisionMasterInfra divisionMasterInfra)
        {
            _divisionMasterInfra = divisionMasterInfra;
        }

        public async Task<string> AddDivision(DivisionRequest divisionRequest)
            => await _divisionMasterInfra.AddDivision(divisionRequest);

        public async Task<List<DivisionResponse>> GetAllDivisions()
            => await _divisionMasterInfra.GetAllDivisions();

        public async Task<string> UpdateDivision(UpdateDivision division)
            => await _divisionMasterInfra.UpdateDivision(division);
    }
}