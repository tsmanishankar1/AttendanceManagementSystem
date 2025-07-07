using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IDivisionMasterInfra
    {
        Task<List<DivisionResponse>> GetAllDivisions();
        Task<string> AddDivision(DivisionRequest divisionRequest);
        Task<string> UpdateDivision(UpdateDivision division);
    }
}
