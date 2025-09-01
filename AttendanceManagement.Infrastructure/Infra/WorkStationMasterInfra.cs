using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra;

public class WorkStationMasterInfra : IWorkstationMasterInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IMemoryCache _cache;
    private const string WorkstationCacheKey = "AllWorkstations";
    public WorkStationMasterInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<WorkStationResponse>> GetAllWorkstationsAsync()
    {
        if (_cache.TryGetValue(WorkstationCacheKey, out var cachedObj) && cachedObj is List<WorkStationResponse> cachedWorkstations)
        {
            return cachedWorkstations;
        }

        var allWorkstations = await _context.WorkstationMasters
            .Select(ws => new WorkStationResponse
            {
                WorkstationMasterId = ws.Id,
                FullName = ws.Name,
                ShortName = ws.ShortName,
                IsActive = ws.IsActive,
                CreatedBy = ws.CreatedBy
            })
            .ToListAsync();

        if (allWorkstations.Count == 0)
        {
            throw new MessageNotFoundException("No Workstations found");
        }

        _cache.Set(WorkstationCacheKey, allWorkstations);

        return allWorkstations;
    }

    public async Task<string> CreateWorkstationAsync(WorkStationRequest workstationRequest)
    {
        var message = "Workstation added successfully.";

        var isDuplicate = await _context.WorkstationMasters
            .AnyAsync(ws => ws.Name.ToLower() == workstationRequest.FullName.ToLower());
        if (isDuplicate) throw new ConflictException("Workstation name already exists");

        var workstation = new WorkstationMaster
        {
            Name = workstationRequest.FullName,
            ShortName = workstationRequest.ShortName,
            IsActive = workstationRequest.IsActive,
            CreatedBy = workstationRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };

        await _context.WorkstationMasters.AddAsync(workstation);
        await _context.SaveChangesAsync();

        _cache.Remove(WorkstationCacheKey);

        return message;
    }

    public async Task<string> UpdateWorkstationAsync(UpdateWorkStation updatedWorkstation)
    {
        var message = "Workstation updated successfully.";

        var workstation = await _context.WorkstationMasters
            .FirstOrDefaultAsync(ws => ws.Id == updatedWorkstation.WorkstationMasterId);
        if (workstation == null) throw new MessageNotFoundException("Workstation not found");

        if (!string.IsNullOrWhiteSpace(updatedWorkstation.FullName))
        {
            var isDuplicate = await _context.WorkstationMasters
                .AnyAsync(ws => ws.Id != updatedWorkstation.WorkstationMasterId &&
                                ws.Name.ToLower() == updatedWorkstation.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Workstation name already exists");
        }

        workstation.Name = updatedWorkstation.FullName;
        workstation.IsActive = updatedWorkstation.IsActive;
        workstation.ShortName = updatedWorkstation.ShortName;
        workstation.UpdatedBy = updatedWorkstation.UpdatedBy;
        workstation.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _cache.Remove(WorkstationCacheKey);

        return message;
    }
}