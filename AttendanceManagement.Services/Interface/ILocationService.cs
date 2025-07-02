using AttendanceManagement.InputModels;

namespace AttendanceManagement.Services.Interface
{
    public interface ILocationService
    {
        Task<List<LocationResponse>> GetAllLocationMastersAsync();
        Task<string> CreateLocationMasterAsync(LocationRequest locationMaster);
        Task<string> UpdateLocationMasterAsync(UpdateLocation locationMaster);
    }
}
