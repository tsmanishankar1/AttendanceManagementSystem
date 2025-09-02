using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra;

public class ZoneMasterInfra : IZoneMasterInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IMemoryCache _cache;
    private const string ZoneCacheKey = "AllZones";
    public ZoneMasterInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<ZoneMasterResponse>> GetAllZonesAsync()
    {
        if (_cache.TryGetValue(ZoneCacheKey, out var cachedObj) && cachedObj is List<ZoneMasterResponse> cachedZones)
        {
            return cachedZones;
        }

        var allZones = await (from zone in _context.ZoneMasters
                              select new ZoneMasterResponse
                              {
                                  ZoneMasterId = zone.Id,
                                  FullName = zone.FullName,
                                  ShortName = zone.ShortName,
                                  IsActive = zone.IsActive,
                                  CreatedBy = zone.CreatedBy
                              })
                              .ToListAsync();

        if (allZones.Count == 0)
        {
            throw new MessageNotFoundException("No zones found");
        }

        _cache.Set(ZoneCacheKey, allZones);

        return allZones;
    }

    public async Task<string> CreateZoneAsync(ZoneMasterRequest zoneMaster)
    {
        var message = "Zone added successfully.";

        var isDuplicate = await _context.ZoneMasters
            .AnyAsync(z => z.FullName.ToLower() == zoneMaster.FullName.ToLower());
        if (isDuplicate) throw new ConflictException("Zone name already exists");

        var zone = new ZoneMaster
        {
            FullName = zoneMaster.FullName,
            ShortName = zoneMaster.ShortName,
            IsActive = zoneMaster.IsActive,
            CreatedBy = zoneMaster.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };

        await _context.ZoneMasters.AddAsync(zone);
        await _context.SaveChangesAsync();

        _cache.Remove(ZoneCacheKey);

        return message;
    }

    public async Task<string> UpdateZoneAsync(UpdateZoneMaster zoneMaster)
    {
        var message = "Zone updated successfully.";

        var existingZone = await _context.ZoneMasters
            .FirstOrDefaultAsync(z => z.Id == zoneMaster.ZoneMasterId);
        if (existingZone == null)
        {
            throw new MessageNotFoundException("Zone not found");
        }

        if (!string.IsNullOrWhiteSpace(zoneMaster.FullName))
        {
            var isDuplicate = await _context.ZoneMasters
                .AnyAsync(z => z.Id != zoneMaster.ZoneMasterId &&
                               z.FullName.ToLower() == zoneMaster.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Zone name already exists");
        }

        existingZone.FullName = zoneMaster.FullName;
        existingZone.ShortName = zoneMaster.ShortName;
        existingZone.IsActive = zoneMaster.IsActive;
        existingZone.UpdatedBy = zoneMaster.UpdatedBy;
        existingZone.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _cache.Remove(ZoneCacheKey);

        return message;
    }
}