using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services;

public class CategoryMasterService : ICategoryMaster
{
    private readonly AttendanceManagementSystemContext _context;
    public CategoryMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }
    public async Task<List<CategoryMasterResponse>> GetAllCategoriesAsync()
    {
        var allCategory = await _context.CategoryMasters
            .Select(category => new CategoryMasterResponse
            {
                CategoryMasterId = category.Id,
                FullName = category.Name,
                ShortName = category.ShortName,
                IsActive = category.IsActive
            }).ToListAsync();
        if (allCategory.Count == 0)
        {
            throw new MessageNotFoundException("No Categories found");
        }
        return allCategory;
    }

    public async Task<string> CreateCategoryAsync(CategoryMasterRequest request)
    {
        var message = "Category created successfully";
        var duplicateCategory = await _context.CategoryMasters.AnyAsync(c => c.Name.ToLower() == request.FullName.ToLower());
        if (duplicateCategory) throw new ConflictException("Category name already exists");
        var newCategory = new CategoryMaster
        {
            Name = request.FullName,
            ShortName = request.ShortName,
            IsActive = request.IsActive,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.CategoryMasters.AddAsync(newCategory);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateCategoryAsync(UpdateCategory request)
    {
        var message = "Category created successfully";
        var existingCategory = await _context.CategoryMasters.FirstOrDefaultAsync(c => c.Id == request.CategoryMasterId);
        if (existingCategory == null)
        {
            throw new MessageNotFoundException("Category not found");
        }
        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            var duplicateCategory = await _context.CategoryMasters.AnyAsync(c => c.Id != request.CategoryMasterId &&c.Name.ToLower() == request.FullName.ToLower());
            if (duplicateCategory) throw new ConflictException("Category name already exists");
        }
        existingCategory.Name = request.FullName ?? existingCategory.Name;
        existingCategory.ShortName = request.ShortName ?? existingCategory.ShortName;
        existingCategory.IsActive = request.IsActive;
        existingCategory.UpdatedBy = request.UpdatedBy;
        existingCategory.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return message;
    }
}