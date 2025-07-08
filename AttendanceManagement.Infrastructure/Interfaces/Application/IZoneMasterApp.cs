using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IZoneMasterApp
    {
        Task<List<ZoneMasterResponse>> GetAllZonesAsync();
        Task<string> CreateZoneAsync(ZoneMasterRequest zoneMaster);
        Task<string> UpdateZoneAsync(UpdateZoneMaster zoneMaster);
    }
}
