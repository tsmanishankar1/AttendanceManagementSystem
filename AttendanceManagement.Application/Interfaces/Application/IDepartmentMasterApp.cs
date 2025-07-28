using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IDepartmentMasterApp
    {
        Task<List<DepartmentResponse>> GetAllDepartments();
        Task<string> CreateDepartment(DepartmentRequest departmentRequest);
        Task<string> UpdateDepartment(UpdateDepartment department);
    }
}
