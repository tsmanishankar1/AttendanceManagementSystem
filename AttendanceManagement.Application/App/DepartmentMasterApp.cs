using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class DepartmentMasterApp : IDepartmentMasterApp
{
    private readonly IDepartmentMasterInfra _departmentMasterInfra;
    public DepartmentMasterApp(IDepartmentMasterInfra departmentMasterInfra)
    {
        _departmentMasterInfra = departmentMasterInfra;

    }

    public async Task<string> CreateDepartment(DepartmentRequest departmentRequest)
        => await _departmentMasterInfra.CreateDepartment(departmentRequest);

    public async Task<List<DepartmentResponse>> GetAllDepartments()
        => await _departmentMasterInfra.GetAllDepartments();

    public async Task<string> UpdateDepartment(UpdateDepartment department)
        => await _departmentMasterInfra.UpdateDepartment(department);
}