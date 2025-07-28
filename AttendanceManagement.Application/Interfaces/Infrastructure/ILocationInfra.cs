using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ILocationInfra
    {
        Task<List<LocationResponse>> GetAllLocationMastersAsync();
        Task<string> CreateLocationMasterAsync(LocationRequest locationMaster);
        Task<string> UpdateLocationMasterAsync(UpdateLocation locationMaster);
    }
}
