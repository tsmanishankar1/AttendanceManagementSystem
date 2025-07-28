using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IUserManagementApp
    {
        Task<string> RegisterUser(UserManagementRequest userRequest);
        Task<object> GetUserByUserId(int StaffId);
        Task<string> ChangePasswordAsync(ChangePasswordModel model);
        Task<UserManagementResponse> GetStaffDetailsByStaffName(string staffname);
        Task<string> ResetPasswordAsync(ResetPasswordModel model);
        Task<UserManagementResponse> GetByUserId(int StaffCreationId);
        Task<string> DeactivateStaffByUserManagementIdAsync(int staffCreationId, int deletedBy);
        Task<List<MenuResponse>> GetMenusByRoleIdAsync(int roleId);
    }
}
