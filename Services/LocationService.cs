using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class LocationService
    {
        private readonly AttendanceManagementSystemContext _context;

        public LocationService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<LocationResponse>> GetAllLocationMastersAsync()
        {
            var allLocation = await (from location in _context.LocationMasters
                                     select new LocationResponse
                                     {
                                         LocationMasterId = location.Id,
                                         FullName = location.Name,
                                         ShortName = location.ShortName,
                                         IsActive = location.IsActive,
                                         CreatedBy = location.CreatedBy
                                     })
                                    .ToListAsync();
            if (allLocation.Count == 0)
            {
                throw new MessageNotFoundException("No locations found");
            }
            return allLocation;
        }

        public async Task<string> CreateLocationMasterAsync(LocationRequest locationMaster)
        {
            var message = "Location added successfully";
            var location = new LocationMaster
            {
                Name = locationMaster.FullName,
                ShortName = locationMaster.ShortName,
                IsActive = locationMaster.IsActive,
                CreatedBy = locationMaster.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.LocationMasters.Add(location);
            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<string> UpdateLocationMasterAsync(UpdateLocation locationMaster)
        {
            var message = "Location updated successfully";
            var existingLocation = await _context.LocationMasters.FirstOrDefaultAsync(l => l.Id == locationMaster.LocationMasterId);
            if (existingLocation == null)
            {
                throw new MessageNotFoundException("Location not found");
            }
            existingLocation.Name = locationMaster.FullName;
            existingLocation.ShortName = locationMaster.ShortName;
            existingLocation.IsActive = locationMaster.IsActive;
            existingLocation.UpdatedBy = locationMaster.UpdatedBy;
            existingLocation.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return message;
        }
    }
}