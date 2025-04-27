using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services;

public class ZoneMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public ZoneMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ZoneMasterResponse>> GetAllZonesAsync()
    {
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
        return allZones;
    }

    public async Task<string> CreateZoneAsync(ZoneMasterRequest zoneMaster)
    {
        var message = "Zone added successfully.";
        var zone = new ZoneMaster
        {
            FullName = zoneMaster.FullName,
            ShortName = zoneMaster.ShortName,
            IsActive = zoneMaster.IsActive,
            CreatedBy = zoneMaster.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.ZoneMasters.Add(zone);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateZoneAsync(UpdateZoneMaster zoneMaster)
    {
        var message = "Zone updated successfully.";
        var existingZone = await _context.ZoneMasters.FirstOrDefaultAsync(z => z.Id == zoneMaster.ZoneMasterId);
        if (existingZone == null)
        {
            throw new MessageNotFoundException("Zone not found");
        }
        existingZone.FullName = zoneMaster.FullName;
        existingZone.ShortName = zoneMaster.ShortName;
        existingZone.UpdatedBy = zoneMaster.UpdatedBy;
        existingZone.IsActive = zoneMaster.IsActive;
        existingZone.UpdatedUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return message;
    }
}