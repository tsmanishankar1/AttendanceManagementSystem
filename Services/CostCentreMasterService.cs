using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services;

public class CostCentreMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public CostCentreMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CostMasterResponse>> GetAllCostCentres()
    {
        var allCostMaster = await (from cost in _context.CostCentreMasters
                             select new CostMasterResponse
                             {
                                 CostCentreMasterId = cost.Id,
                                 FullName = cost.FullName,
                                 ShortName = cost.ShortName,
                                 IsActive = cost.IsActive,
                                 CreatedBy = cost.CreatedBy
                             })
                             .ToListAsync();
        if (allCostMaster.Count == 0)
        {
            throw new MessageNotFoundException("No cost centre found");
        }

        return allCostMaster;
    }

    public async Task<CostMasterResponse> GetCostCentreById(int costCentreMasterId)
    {
        var allCostMaster = await (from cost in _context.CostCentreMasters
                             where cost.Id == costCentreMasterId
                             select new CostMasterResponse
                             {
                                 CostCentreMasterId = cost.Id,
                                 FullName = cost.FullName,
                                 ShortName = cost.ShortName,
                                 IsActive = cost.IsActive,
                                 CreatedBy = cost.CreatedBy
                             })
                             .FirstOrDefaultAsync();
        if (allCostMaster == null)
        {
            throw new MessageNotFoundException("Cost centre not found");
        }

        return allCostMaster;
    }

    public async Task<string> CreateCostCentre(CostMasterRequest costCentreMaster)
    {
        var message = "Cost centre added successfully";

        CostCentreMaster costMaster = new CostCentreMaster();
        costMaster.FullName = costCentreMaster.FullName;
        costMaster.ShortName = costCentreMaster.ShortName;
        costMaster.IsActive = costCentreMaster.IsActive;
        costMaster.CreatedBy = costCentreMaster.CreatedBy;
        costMaster.CreatedUtc = DateTime.UtcNow;

        _context.CostCentreMasters.Add(costMaster);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateCostCentre(UpdateCostMaster costCentreMaster)
    {
        var message = "Cost centre updated successfully";

        var existingCostCentre = _context.CostCentreMasters
            .FirstOrDefault(c => c.Id == costCentreMaster.CostCentreMasterId);

        if (existingCostCentre == null)
        {
            throw new MessageNotFoundException("Cost centre not found");
        }
        existingCostCentre.FullName = costCentreMaster.FullName ?? existingCostCentre.FullName;
        existingCostCentre.ShortName = costCentreMaster.ShortName ?? existingCostCentre.ShortName;
        existingCostCentre.UpdatedBy = costCentreMaster.UpdatedBy;
        existingCostCentre.IsActive = costCentreMaster.IsActive;
        existingCostCentre.UpdatedUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return message;
    }

}