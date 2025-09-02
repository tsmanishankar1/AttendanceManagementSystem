using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra;

public class CostCentreMasterInfra : ICostCentreInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IMemoryCache _cache;
    private const string CostCentreCacheKey = "AllCostCentres";

    public CostCentreMasterInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<CostMasterResponse>> GetAllCostCentres()
    {
        if (_cache.TryGetValue(CostCentreCacheKey, out var cachedObj) && cachedObj is List<CostMasterResponse> cachedCostCentres)
        {
            return cachedCostCentres;
        }

        var allCostMaster = await (from cost in _context.CostCentreMasters
                                   select new CostMasterResponse
                                   {
                                       CostCentreMasterId = cost.Id,
                                       FullName = cost.Name,
                                       ShortName = cost.ShortName,
                                       IsActive = cost.IsActive,
                                       CreatedBy = cost.CreatedBy
                                   })
                                   .ToListAsync();

        if (allCostMaster.Count == 0)
        {
            throw new MessageNotFoundException("No cost centre found");
        }

        _cache.Set(CostCentreCacheKey, allCostMaster);

        return allCostMaster;
    }

    public async Task<string> CreateCostCentre(CostMasterRequest costCentreMaster)
    {
        var message = "Cost centre created successfully";

        var isDuplicate = await _context.CostCentreMasters
            .AnyAsync(c => c.Name.ToLower() == costCentreMaster.FullName.ToLower());
        if (isDuplicate) throw new ConflictException("Cost centre name already exists");

        var costMaster = new CostCentreMaster
        {
            Name = costCentreMaster.FullName,
            ShortName = costCentreMaster.ShortName,
            IsActive = costCentreMaster.IsActive,
            CreatedBy = costCentreMaster.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };

        await _context.CostCentreMasters.AddAsync(costMaster);
        await _context.SaveChangesAsync();

        _cache.Remove(CostCentreCacheKey);

        return message;
    }

    public async Task<string> UpdateCostCentre(UpdateCostMaster costCentreMaster)
    {
        var message = "Cost centre updated successfully";

        var existingCostCentre = await _context.CostCentreMasters
            .FirstOrDefaultAsync(c => c.Id == costCentreMaster.CostCentreMasterId);

        if (existingCostCentre == null)
        {
            throw new MessageNotFoundException("Cost centre not found");
        }

        if (!string.IsNullOrWhiteSpace(costCentreMaster.FullName))
        {
            var isDuplicate = await _context.CostCentreMasters
                .AnyAsync(c => c.Id != costCentreMaster.CostCentreMasterId &&
                               c.Name.ToLower() == costCentreMaster.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Cost centre name already exists");
        }

        existingCostCentre.Name = costCentreMaster.FullName ?? existingCostCentre.Name;
        existingCostCentre.ShortName = costCentreMaster.ShortName ?? existingCostCentre.ShortName;
        existingCostCentre.UpdatedBy = costCentreMaster.UpdatedBy;
        existingCostCentre.IsActive = costCentreMaster.IsActive;
        existingCostCentre.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _cache.Remove(CostCentreCacheKey);

        return message;
    }
}