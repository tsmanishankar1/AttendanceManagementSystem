using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services;

public class GradeMasterService : IGradeMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public GradeMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<List<GradeMasterResponse>> GetAllGrades()
    {
        var allGrade = await (from grade in _context.GradeMasters
                        select new GradeMasterResponse
                        {
                            GradeMasterId = grade.Id,
                            FullName = grade.Name,
                            ScreenOption = grade.ScreenOption,
                            IsActive = grade.IsActive,
                            CreatedBy = grade.CreatedBy
                        })
                        .ToListAsync();
        if (allGrade.Count == 0)
        {
            throw new MessageNotFoundException("No grades found");
        }
        return allGrade;
    }


    public async Task<string> CreateGrade(GradeMasterRequest gradeMasterRequest)
    {
        var message = "Grade added successfully";
        var isDuplicate = await _context.GradeMasters.AnyAsync(g => g.Name.ToLower() == gradeMasterRequest.FullName.ToLower());
        if (isDuplicate) throw new ConflictException("Grade name already exists");
        var gradeMaster = new GradeMaster
        {
            Name = gradeMasterRequest.FullName,
            ScreenOption = gradeMasterRequest.ScreenOption,
            IsActive = gradeMasterRequest.IsActive,
            CreatedBy = gradeMasterRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.GradeMasters.AddAsync(gradeMaster);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateGrade(UpdateGradeMaster gradeMaster)
    {
        var message = "Grade updated successfully";
        var existingGrade = await _context.GradeMasters.FirstOrDefaultAsync(g => g.Id == gradeMaster.GradeMasterId);
        if (existingGrade == null)
        {
            throw new MessageNotFoundException("Grade not found");
        }
        if (!string.IsNullOrWhiteSpace(gradeMaster.FullName))
        {
            var isDuplicate = await _context.GradeMasters.AnyAsync(g => g.Id != gradeMaster.GradeMasterId && g.Name.ToLower() == gradeMaster.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Grade name already exists");
        }
        existingGrade.Name = gradeMaster.FullName;
        existingGrade.ScreenOption = gradeMaster.ScreenOption;
        existingGrade.IsActive = gradeMaster.IsActive;
        existingGrade.UpdatedBy = gradeMaster.UpdatedBy;
        existingGrade.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return message;
    }
}