using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Services;

public class WorkstationMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public WorkstationMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<List<WorkStationResponse>> GetAllWorkstationsAsync()
    {
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
        return allWorkstations;
    }

    public async Task<string> CreateWorkstationAsync(WorkStationRequest workstationRequest)
    {
        var message = "Workstation added successfully.";
        var isDuplicate = await _context.WorkstationMasters.AnyAsync(ws => ws.Name.ToLower() == workstationRequest.FullName.ToLower());
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
        return message;
    }

    public async Task<string> UpdateWorkstationAsync(UpdateWorkStation updatedWorkstation)
    {
        var message = "Workstation updated successfully.";
        var workstation = await _context.WorkstationMasters.FirstOrDefaultAsync(ws => ws.Id == updatedWorkstation.WorkstationMasterId);
        if (workstation == null) throw new MessageNotFoundException("WorkStation not found");
        if (!string.IsNullOrWhiteSpace(updatedWorkstation.FullName))
        {
            var isDuplicate = await _context.WorkstationMasters.AnyAsync(ws => ws.Id != updatedWorkstation.WorkstationMasterId && ws.Name.ToLower() == updatedWorkstation.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Workstation name already exists");
        }
        workstation.Name = updatedWorkstation.FullName;
        workstation.IsActive = updatedWorkstation.IsActive;
        workstation.ShortName = updatedWorkstation.ShortName;
        workstation.UpdatedBy = updatedWorkstation.UpdatedBy;
        workstation.UpdatedUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return message;
    }
}