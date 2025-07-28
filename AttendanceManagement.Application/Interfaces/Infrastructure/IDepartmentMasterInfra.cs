using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IDepartmentMasterInfra
    {
        Task<List<DepartmentResponse>> GetAllDepartments();
        Task<string> CreateDepartment(DepartmentRequest departmentRequest);
        Task<string> UpdateDepartment(UpdateDepartment department);
    }
}
