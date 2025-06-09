using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Services;

public class DepartmentMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public DepartmentMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentResponse>> GetAllDepartments()
    {
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
        return allDepartment;
    }
    public async Task<string> CreateDepartment(DepartmentRequest departmentRequest)
    {
        var message = "Department created successfully";
        var isDuplicate = await _context.DepartmentMasters.AnyAsync(d => d.Name.ToLower() == departmentRequest.FullName.ToLower());
        if (isDuplicate) throw new ConflictException("Department name already exists");
        DepartmentMaster department = new DepartmentMaster();
        department.Name = departmentRequest.FullName;
        department.ShortName = departmentRequest.ShortName;
        department.Phone = departmentRequest.Phone;
        department.Fax = departmentRequest.Fax;
        department.Email = departmentRequest.Email;
        department.CreatedBy = departmentRequest.CreatedBy;
        department.CreatedUtc = DateTime.UtcNow;
        department.IsActive = departmentRequest.IsActive;

        await _context.DepartmentMasters.AddAsync(department);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateDepartment(UpdateDepartment department)
    {
        var message = "Department updated successfully";
        var existingDepartment = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Id == department.DepartmentMasterId);
        if (existingDepartment == null) throw new MessageNotFoundException("Department not found");
        if (!string.IsNullOrWhiteSpace(department.FullName))
        {
            var isDuplicate = await _context.DepartmentMasters.AnyAsync(d => d.Id != department.DepartmentMasterId && d.Name.ToLower() == department.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Department name already exists");
        }
        existingDepartment.Name = department.FullName;
        existingDepartment.ShortName = department.ShortName;
        existingDepartment.Phone = department.Phone;
        existingDepartment.Fax = department.Fax;
        existingDepartment.Email = department.Email;
        existingDepartment.UpdatedBy = department.UpdatedBy;
        existingDepartment.UpdatedUtc = DateTime.UtcNow;
        existingDepartment.IsActive = department.IsActive;

        await _context.SaveChangesAsync();
        return message;
    }
}