using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Infrastructure.Infra;

public class ZoneMasterApp : IZoneMasterApp
{
    private readonly IZoneMasterInfra _zoneMasterInfra;
    public ZoneMasterApp(IZoneMasterInfra zoneMasterInfra)
    {
        _zoneMasterInfra = zoneMasterInfra;
    }

    public async Task<string> CreateZoneAsync(ZoneMasterRequest zoneMaster)
        => await _zoneMasterInfra.CreateZoneAsync(zoneMaster);

    public async Task<List<ZoneMasterResponse>> GetAllZonesAsync()
        => await _zoneMasterInfra.GetAllZonesAsync();

    public async Task<string> UpdateZoneAsync(UpdateZoneMaster zoneMaster)
        => await _zoneMasterInfra.UpdateZoneAsync(zoneMaster);
}