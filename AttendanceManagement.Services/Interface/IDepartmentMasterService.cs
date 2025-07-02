using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IDepartmentMasterService
    {
        Task<List<DepartmentResponse>> GetAllDepartments();
        Task<string> CreateDepartment(DepartmentRequest departmentRequest);
        Task<string> UpdateDepartment(UpdateDepartment department);
    }
}
