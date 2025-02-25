using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services;

public class GradeMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public GradeMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GradeMasterResponse>> GetAllGrades()
    {
        var allGrade = await (from grade in _context.GradeMasters
                        select new GradeMasterResponse
                        {
                            GradeMasterId = grade.Id,
                            FullName = grade.FullName,
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


    public async Task<GradeMasterResponse> GetGradeById(int gradeMasterId)
    {
        var allGrade = await (from grade in _context.GradeMasters
                        where grade.Id == gradeMasterId
                        select new GradeMasterResponse
                        {
                            GradeMasterId = grade.Id,
                            FullName = grade.FullName,
                            ScreenOption = grade.ScreenOption,
                            IsActive = grade.IsActive,
                            CreatedBy = grade.CreatedBy
                        })
                        .FirstOrDefaultAsync();
        if (allGrade == null)
        {
            throw new MessageNotFoundException("Grade not found");
        }
        return allGrade;
    }

    public async Task<string> CreateGrade(GradeMasterRequest gradeMasterRequest)
    {
        var message = "Grade added successfully";

        var gradeMaster = new GradeMaster
        {
            FullName = gradeMasterRequest.FullName,
            ScreenOption = gradeMasterRequest.ScreenOption,
            IsActive = gradeMasterRequest.IsActive,
            CreatedBy = gradeMasterRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };

        _context.GradeMasters.Add(gradeMaster);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateGrade(UpdateGradeMaster gradeMaster)
    {
        var message = "Grade updated successfully";

        var existingGrade = _context.GradeMasters.FirstOrDefault(g => g.Id == gradeMaster.GradeMasterId);
        if (existingGrade == null)
        {
            throw new MessageNotFoundException("Grade not found");
        }

        existingGrade.FullName = gradeMaster.FullName;
        existingGrade.ScreenOption = gradeMaster.ScreenOption;
        existingGrade.IsActive = gradeMaster.IsActive;
        existingGrade.UpdatedBy = gradeMaster.UpdatedBy;
        existingGrade.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return message;
    }
}

