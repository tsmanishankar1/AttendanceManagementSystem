using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ILocationApp
    {
        Task<List<LocationResponse>> GetAllLocationMastersAsync();
        Task<string> CreateLocationMasterAsync(LocationRequest locationMaster);
        Task<string> UpdateLocationMasterAsync(UpdateLocation locationMaster);
    }
}
