using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class UserManagementApp : IUserManagementApp
    {
        private readonly IUserManagementInfra _userManagementInfra;

        public UserManagementApp(IUserManagementInfra userManagementInfra)
        {
            _userManagementInfra = userManagementInfra;
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordModel model)
            => await _userManagementInfra.ChangePasswordAsync(model);

        public async Task<string> DeactivateStaffByUserManagementIdAsync(int staffCreationId, int deletedBy)
            => await _userManagementInfra.DeactivateStaffByUserManagementIdAsync(staffCreationId, deletedBy);

        public async Task<UserManagementResponse> GetByUserId(int staffCreationId)
            => await _userManagementInfra.GetByUserId(staffCreationId);

        public async Task<List<MenuResponse>> GetMenusByRoleIdAsync(int roleId)
            => await _userManagementInfra.GetMenusByRoleIdAsync(roleId);

        public async Task<UserManagementResponse> GetStaffDetailsByStaffName(string staffname)
            => await _userManagementInfra.GetStaffDetailsByStaffName(staffname);

        public async Task<object> GetUserByUserId(int staffId)
            => await _userManagementInfra.GetUserByUserId(staffId);

        public async Task<string> RegisterUser(UserManagementRequest userRequest)
            => await _userManagementInfra.RegisterUser(userRequest);

        public async Task<string> ResetPasswordAsync(ResetPasswordModel model)
            => await _userManagementInfra.ResetPasswordAsync(model);
    }
}