using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IDivisionMasterApp
    {
        Task<List<DivisionResponse>> GetAllDivisions();
        Task<string> AddDivision(DivisionRequest divisionRequest);
        Task<string> UpdateDivision(UpdateDivision division);
    }
}
