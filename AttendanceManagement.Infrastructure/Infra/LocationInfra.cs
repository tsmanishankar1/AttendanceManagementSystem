using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class LocationInfra : ILocationInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IMemoryCache _cache;
        private const string LocationCacheKey = "AllLocations";
        public LocationInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<LocationResponse>> GetAllLocationMastersAsync()
        {
            if (_cache.TryGetValue(LocationCacheKey, out var cachedObj) && cachedObj is List<LocationResponse> cachedLocations)
            {
                return cachedLocations;
            }

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

            _cache.Set(LocationCacheKey, allLocation);

            return allLocation;
        }

        public async Task<string> CreateLocationMasterAsync(LocationRequest locationMaster)
        {
            var message = "Location added successfully";

            var isDuplicate = await _context.LocationMasters
                .AnyAsync(l => l.Name.ToLower() == locationMaster.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Location name already exists");

            var location = new LocationMaster
            {
                Name = locationMaster.FullName,
                ShortName = locationMaster.ShortName,
                IsActive = locationMaster.IsActive,
                CreatedBy = locationMaster.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            await _context.LocationMasters.AddAsync(location);
            await _context.SaveChangesAsync();

            _cache.Remove(LocationCacheKey);

            return message;
        }

        public async Task<string> UpdateLocationMasterAsync(UpdateLocation locationMaster)
        {
            var message = "Location updated successfully";

            var existingLocation = await _context.LocationMasters
                .FirstOrDefaultAsync(l => l.Id == locationMaster.LocationMasterId);

            if (existingLocation == null)
            {
                throw new MessageNotFoundException("Location not found");
            }

            if (!string.IsNullOrWhiteSpace(locationMaster.FullName))
            {
                var isDuplicate = await _context.LocationMasters
                    .AnyAsync(l => l.Id != locationMaster.LocationMasterId &&
                                   l.Name.ToLower() == locationMaster.FullName.ToLower());
                if (isDuplicate) throw new ConflictException("Location name already exists");
            }

            existingLocation.Name = locationMaster.FullName;
            existingLocation.ShortName = locationMaster.ShortName;
            existingLocation.IsActive = locationMaster.IsActive;
            existingLocation.UpdatedBy = locationMaster.UpdatedBy;
            existingLocation.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _cache.Remove(LocationCacheKey);

            return message;
        }
    }
}