using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class LocationApp : ILocationApp
    {
        private readonly ILocationInfra _locationInfra;
        public LocationApp(ILocationInfra locationInfra)
        {
            _locationInfra = locationInfra;
        }

        public async Task<string> CreateLocationMasterAsync(LocationRequest locationMaster)
            => await _locationInfra.CreateLocationMasterAsync(locationMaster);

        public async Task<List<LocationResponse>> GetAllLocationMastersAsync()
            => await _locationInfra.GetAllLocationMastersAsync();

        public async Task<string> UpdateLocationMasterAsync(UpdateLocation locationMaster)
            => await _locationInfra.UpdateLocationMasterAsync(locationMaster);
    }
}