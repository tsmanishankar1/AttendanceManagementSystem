using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services;

public class DepartmentMasterService
{
    private readonly AttendanceManagementSystemContext _context;

    public DepartmentMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentResponse>> GetAllDepartments()
    {
        var allDepartment = await (from department in _context.DepartmentMasters
                             select new DepartmentResponse
                             {
                                 DepartmentMasterId = department.Id,
                                 FullName = department.FullName,
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

    public async Task<DepartmentResponse> GetDepartmentById(int departmentMasterId)
    {
        var allDepartment = await (from department in _context.DepartmentMasters
                             where department.Id == departmentMasterId
                             select new DepartmentResponse
                             {
                                 DepartmentMasterId = department.Id,
                                 FullName = department.FullName,
                                 ShortName = department.ShortName,
                                 Phone = department.Phone,
                                 Fax = department.Fax,
                                 Email = department.Email,
                                 IsActive = department.IsActive,
                                 CreatedBy = department.CreatedBy
                             })
                             .FirstOrDefaultAsync();
        if (allDepartment == null)
        {
            throw new MessageNotFoundException("Department not found");
        }
        return allDepartment;
    }

    public async Task<string> CreateDepartment(DepartmentRequest departmentRequest)
    {
        var message = "Department added successfully";

        DepartmentMaster department = new DepartmentMaster();
        department.FullName = departmentRequest.FullName;
        department.ShortName = departmentRequest.ShortName;
        department.Phone = departmentRequest.Phone;
        department.Fax = departmentRequest.Fax;
        department.Email = departmentRequest.Email;
        department.CreatedBy = departmentRequest.CreatedBy;
        department.CreatedUtc = DateTime.UtcNow;
        department.IsActive = departmentRequest.IsActive;

        _context.DepartmentMasters.Add(department);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<string> UpdateDepartment(UpdateDepartment department)
    {
        var message = "Department updated successfully";
        var existingDepartment = _context.DepartmentMasters.FirstOrDefault(d => d.Id == department.DepartmentMasterId);

        if (existingDepartment == null)
            throw new MessageNotFoundException("Department not found");

        existingDepartment.FullName = department.FullName;
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
