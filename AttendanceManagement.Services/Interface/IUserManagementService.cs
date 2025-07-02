using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IUserManagementService
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
