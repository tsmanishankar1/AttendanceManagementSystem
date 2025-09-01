using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra;

public class CategoryMasterInfra : ICategoryMasterInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IMemoryCache _cache;
    private readonly string _categoryCacheKey = "AllCategories";
    public CategoryMasterInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;

    }

    public async Task<List<CategoryMasterResponse>> GetAllCategoriesAsync()
    {
        if (_cache.TryGetValue(_categoryCacheKey, out var cachedObj) && cachedObj is List<CategoryMasterResponse> cachedCategories)
        {
            return cachedCategories;
        }

        var allCategory = await _context.CategoryMasters
            .Select(category => new CategoryMasterResponse
            {
                CategoryMasterId = category.Id,
                FullName = category.Name,
                ShortName = category.ShortName,
                IsActive = category.IsActive
            })
            .ToListAsync();

        if (allCategory.Count == 0)
        {
            throw new MessageNotFoundException("No Categories found");
        }

        _cache.Set(_categoryCacheKey, allCategory);

        return allCategory;
    }

    public async Task<string> CreateCategoryAsync(CategoryMasterRequest request)
    {
        var message = "Category created successfully";

        var duplicateCategory = await _context.CategoryMasters
            .AnyAsync(c => c.Name.ToLower() == request.FullName.ToLower());
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

        _cache.Remove(_categoryCacheKey);

        return message;
    }

    public async Task<string> UpdateCategoryAsync(UpdateCategory request)
    {
        var message = "Category updated successfully";

        var existingCategory = await _context.CategoryMasters
            .FirstOrDefaultAsync(c => c.Id == request.CategoryMasterId);

        if (existingCategory == null)
        {
            throw new MessageNotFoundException("Category not found");
        }

        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            var duplicateCategory = await _context.CategoryMasters
                .AnyAsync(c => c.Id != request.CategoryMasterId &&
                               c.Name.ToLower() == request.FullName.ToLower());
            if (duplicateCategory) throw new ConflictException("Category name already exists");
        }

        existingCategory.Name = request.FullName ?? existingCategory.Name;
        existingCategory.ShortName = request.ShortName ?? existingCategory.ShortName;
        existingCategory.IsActive = request.IsActive;
        existingCategory.UpdatedBy = request.UpdatedBy;
        existingCategory.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _cache.Remove(_categoryCacheKey);

        return message;
    }
}