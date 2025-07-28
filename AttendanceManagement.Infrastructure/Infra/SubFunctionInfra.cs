using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Infrastructure.Infra;

public class SubFunctionMasterService : ISubFunctionMasterInfra
{
    private readonly AttendanceManagementSystemContext _context;

    public SubFunctionMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<List<SubFunctionResponse>> GetAllSubFunctionsAsync()
    {
        var allSubFunctions = await (from subFunction in _context.SubFunctionMasters
                                     select new SubFunctionResponse
                                     {
                                         SubFunctionMasterId = subFunction.Id,
                                         FullName = subFunction.FullName,
                                         ShortName = subFunction.ShortName,
                                         IsActive = subFunction.IsActive,
                                         CreatedBy = subFunction.CreatedBy
                                     })
                               .ToListAsync();
        if (allSubFunctions.Count == 0)
        {
            throw new MessageNotFoundException("No sub functions found");
        }
        return allSubFunctions;
    }

    public async Task<string> CreateSubFunctionAsync(SubFunctionRequest subFunctionMaster)
    {
        var message = "Sub function added successfully";
        var isDuplicate = await _context.SubFunctionMasters.AnyAsync(s => s.FullName.ToLower() == subFunctionMaster.FullName.ToLower());
        if (isDuplicate) throw new ConflictException("Sub function name already exists");
        var subFunction = new SubFunctionMaster
        {
            FullName = subFunctionMaster.FullName,
            ShortName = subFunctionMaster.ShortName,
            IsActive = subFunctionMaster.IsActive,
            CreatedBy = subFunctionMaster.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.SubFunctionMasters.AddAsync(subFunction);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateSubFunctionAsync(UpdateSubFunction subFunctionMaster)
    {
        var message = "Sub function updated successfully";
        var existingSubFunction = await _context.SubFunctionMasters.FirstOrDefaultAsync(s => s.Id == subFunctionMaster.SubFunctionMasterId);
        if (existingSubFunction == null)
        {
            throw new MessageNotFoundException("Sub function not found");
        }
        if (!string.IsNullOrWhiteSpace(subFunctionMaster.FullName))
        {
            var isDuplicate = await _context.SubFunctionMasters.AnyAsync(s => s.Id != subFunctionMaster.SubFunctionMasterId && s.FullName.ToLower() == subFunctionMaster.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Sub function name already exists");
        }
        existingSubFunction.FullName = subFunctionMaster.FullName;
        existingSubFunction.ShortName = subFunctionMaster.ShortName;
        existingSubFunction.IsActive = subFunctionMaster.IsActive;
        existingSubFunction.UpdatedBy = subFunctionMaster.UpdatedBy;
        existingSubFunction.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return message;
    }
}