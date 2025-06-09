using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Services;

public class ZoneMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public ZoneMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<List<ZoneMasterResponse>> GetAllZonesAsync()
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
        var isDuplicate = await _context.ZoneMasters.AnyAsync(z => z.FullName.ToLower() == zoneMaster.FullName.ToLower());
        if (isDuplicate) throw new ValidationException("Zone name already exists");
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
        if (!string.IsNullOrWhiteSpace(zoneMaster.FullName))
        {
            var isDuplicate = await _context.ZoneMasters.AnyAsync(z => z.Id != zoneMaster.ZoneMasterId && z.FullName.ToLower() == zoneMaster.FullName.ToLower());
            if (isDuplicate) throw new ValidationException("Zone name already exists");
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