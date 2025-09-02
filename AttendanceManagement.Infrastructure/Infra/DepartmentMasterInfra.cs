using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra;

public class DepartmentMasterInfra : IDepartmentMasterInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IMemoryCache _cache;
    private const string DepartmentCacheKey = "AllDepartments";

    public DepartmentMasterInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<DepartmentResponse>> GetAllDepartments()
    {
        if (_cache.TryGetValue(DepartmentCacheKey, out var cachedObj) && cachedObj is List<DepartmentResponse> cachedDepartments)
        {
            return cachedDepartments;
        }

        var allDepartment = await (from department in _context.DepartmentMasters
                                   select new DepartmentResponse
                                   {
                                       DepartmentMasterId = department.Id,
                                       FullName = department.Name,
                                       ShortName = department.ShortName,
                                       Phone = department.Phone,
                                       Fax = department.Fax,
                                       Email = department.Email,
                                       IsActive = department.IsActive,
                                       CreatedBy = department.CreatedBy
                                   })
                                   .ToListAsync();

        if (allDepartment.Count == 0)
        {
            throw new MessageNotFoundException("No department found");
        }

        _cache.Set(DepartmentCacheKey, allDepartment);

        return allDepartment;
    }

    public async Task<string> CreateDepartment(DepartmentRequest departmentRequest)
    {
        var message = "Department created successfully";

        var isDuplicate = await _context.DepartmentMasters
            .AnyAsync(d => d.Name.ToLower() == departmentRequest.FullName.ToLower());
        if (isDuplicate) throw new ConflictException("Department name already exists");

        var department = new DepartmentMaster
        {
            Name = departmentRequest.FullName,
            ShortName = departmentRequest.ShortName,
            Phone = departmentRequest.Phone,
            Fax = departmentRequest.Fax,
            Email = departmentRequest.Email,
            CreatedBy = departmentRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            IsActive = departmentRequest.IsActive
        };

        await _context.DepartmentMasters.AddAsync(department);
        await _context.SaveChangesAsync();

        _cache.Remove(DepartmentCacheKey);

        return message;
    }

    public async Task<string> UpdateDepartment(UpdateDepartment department)
    {
        var message = "Department updated successfully";

        var existingDepartment = await _context.DepartmentMasters
            .FirstOrDefaultAsync(d => d.Id == department.DepartmentMasterId);

        if (existingDepartment == null)
            throw new MessageNotFoundException("Department not found");

        if (!string.IsNullOrWhiteSpace(department.FullName))
        {
            var isDuplicate = await _context.DepartmentMasters
                .AnyAsync(d => d.Id != department.DepartmentMasterId &&
                               d.Name.ToLower() == department.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Department name already exists");
        }

        existingDepartment.Name = department.FullName ?? existingDepartment.Name;
        existingDepartment.ShortName = department.ShortName ?? existingDepartment.ShortName;
        existingDepartment.Phone = department.Phone;
        existingDepartment.Fax = department.Fax ?? existingDepartment.Fax;
        existingDepartment.Email = department.Email ?? existingDepartment.Email;
        existingDepartment.UpdatedBy = department.UpdatedBy;
        existingDepartment.UpdatedUtc = DateTime.UtcNow;
        existingDepartment.IsActive = department.IsActive;

        await _context.SaveChangesAsync();

        _cache.Remove(DepartmentCacheKey);

        return message;
    }
}