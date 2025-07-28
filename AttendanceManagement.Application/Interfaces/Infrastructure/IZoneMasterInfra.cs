using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IZoneMasterInfra
    {
        Task<List<ZoneMasterResponse>> GetAllZonesAsync();
        Task<string> CreateZoneAsync(ZoneMasterRequest zoneMaster);
        Task<string> UpdateZoneAsync(UpdateZoneMaster zoneMaster);
    }
}
