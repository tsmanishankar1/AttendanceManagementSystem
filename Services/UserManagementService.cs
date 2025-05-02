using AttendanceManagement;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

public class UserManagementService
{
    private readonly AttendanceManagementSystemContext _context;

    public UserManagementService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }
    public async Task<string> RegisterUser(UserManagementRequest userRequest)
    {
        var userName = await _context.UserManagements.AnyAsync(u => u.Username == userRequest.Username && u.IsActive);
        if (userName)
        {
            throw new InvalidOperationException("User name already exists");
        }
        var staffWithOrgType = await (from s in _context.StaffCreations
                                      join o in _context.OrganizationTypes on s.OrganizationTypeId equals o.Id
                                      where s.Id == userRequest.StaffCreationId
                                      select new { s.Id, o.ShortName }).FirstOrDefaultAsync();
        if (staffWithOrgType == null)
        {
            throw new MessageNotFoundException("StaffCreationId not found.");
        }
        var userExists = await _context.UserManagements.AnyAsync(u => u.StaffCreationId == userRequest.StaffCreationId && u.IsActive);
        if (userExists)
        {
            throw new InvalidOperationException("User is already exists.");
        }
        var user = new UserManagement
        {
            Username = userRequest.Username,
            Password = userRequest.Password,
            IsActive = true,
            CreatedBy = userRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            StaffCreationId = userRequest.StaffCreationId
        };
        await _context.UserManagements.AddAsync(user);
        await _context.SaveChangesAsync();

        return "User registered successfully";
    }

    public async Task<object> GetUserByUserId(int StaffId)
    {
        var user = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == StaffId && u.IsActive == true);
        if (user == null) throw new MessageNotFoundException("User not found.");
        return new
        {
            StaffCreationId = user.Id,
            StaffName = $"{user.FirstName} {user.LastName}",
            CreatedBy = user.CreatedBy
        };
    }

    public async Task<string> ChangePasswordAsync(ChangePasswordModel model)
    {
        var message = "Password changed successfully.";
        var user = await _context.UserManagements.FirstOrDefaultAsync(u => u.StaffCreationId == model.UserId && u.IsActive);
        if (user == null)
        {
            throw new MessageNotFoundException("User not found.");
        }
        if (user.Password != model.CurrentPassword)
        {
            throw new Exception("Current password is incorrect.");
        }
        if (model.NewPassword != model.ConfirmPassword)
        {
            throw new Exception("New password and confirm password must match.");
        }
        var passwordHistory = await _context.PasswordHistories
            .Join(_context.UserManagements,
                ph => ph.CreatedBy, 
                u => u.Id,
                (ph, u) => new
                {
                    u.Username,
                    ph.OldPassword,
                    ph.NewPassword,
                    ph.CreatedUtc
                })
            .Where(p => p.Username == user.Username)
            .OrderByDescending(p => p.CreatedUtc)
            .Take(3)
            .ToListAsync();
        var usageCount = passwordHistory.Count(ph => ph.NewPassword == model.NewPassword);
        if (usageCount >= 1) 
        {
            throw new Exception("You cannot reuse this password immediately. Try a different one.");
        }
        var passwordHistoryEntry = new PasswordHistory
        {
            OldPassword = user.Password,
            NewPassword = model.NewPassword,
            IsActive = true,
            CreatedBy = user.Id,  
            CreatedUtc = DateTime.UtcNow
        };
        await _context.PasswordHistories.AddAsync(passwordHistoryEntry);
        user.Password = model.NewPassword;
        user.UpdatedUtc = DateTime.UtcNow;
        user.UpdatedBy = user.StaffCreationId; 
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<UserManagementResponse> GetStaffDetailsByStaffName(string staffname)
    {
        var user = await(from staff in _context.StaffCreations
                         join department in _context.DepartmentMasters
                         on staff.DepartmentId equals department.Id
                         where staff.FirstName + " " + staff.LastName == staffname && staff.IsActive==true
                         select new UserManagementResponse
                         {
                             UserManagementId = staff.Id,
                             StaffName = staff.FirstName + " " + staff.LastName,
                             DepartmentName = department.Name,
                             CreatedBy = staff.CreatedBy
                         }).FirstOrDefaultAsync();
        if (user == null) throw new MessageNotFoundException("Staff not found.");
        return user;
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordModel model)
    {
        var message = "Password reset successfully";
        var user = await _context.UserManagements.FirstOrDefaultAsync(u => u.StaffCreationId == model.UserId && u.IsActive);
        if (user == null)
        {
            throw new MessageNotFoundException("User not found");
        }
        if (model.NewPassword != model.ConfirmPassword)
        {
            throw new Exception("New password and confirm password must be match");
        }
        user.Password = model.NewPassword;
        user.UpdatedUtc = DateTime.UtcNow;
        user.UpdatedBy = model.UserId;
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<UserManagementResponse> GetByUserId(int StaffCreationId)
    {
        var user = await (from staff in _context.StaffCreations
                          join department in _context.DepartmentMasters
                          on staff.DepartmentId equals department.Id
                          where staff.Id == StaffCreationId && staff.IsActive==true && department.IsActive
                          select new UserManagementResponse
                          {
                              UserManagementId = staff.Id,
                              StaffName = staff.FirstName + " " + staff.LastName,
                              DepartmentName = department.Name,
                              CreatedBy = staff.CreatedBy
                          }).FirstOrDefaultAsync();
        if (user == null) throw new MessageNotFoundException("User not found.");
        return user;
    }

    public async Task<string> DeactivateStaffByUserManagementIdAsync(int staffCreationId, int deletedBy)
    {
        var message = "Staff deactivated successfully";
        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffCreationId && s.IsActive==true);
        if (staff == null)
        {
            throw new MessageNotFoundException("Staff not found");
        }
        staff.IsActive = false;
        staff.UpdatedUtc = DateTime.UtcNow;
        staff.UpdatedBy = deletedBy;
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<MenuResponse>> GetMenusByRoleIdAsync(int roleId)
    {
        var flatMenus = await _context.RoleMenuMappings
            .Where(rm => rm.RoleId == roleId && rm.IsActive)
            .Select(rm => rm.Menu)
            .Where(m => m.IsActive)
            .OrderBy(m => m.Id)
            .ToListAsync();
        var menuResponses = flatMenus.Select(m => new MenuResponse
        {
            Id = m.Id,
            Name = m.Name,
            ParentMenuId = m.ParentMenuId,
            CreatedBy = m.CreatedBy,
            Children = new List<MenuResponse>()
        }).ToList();
        var menuDict = menuResponses.ToDictionary(m => m.Id);
        List<MenuResponse> rootMenus = new List<MenuResponse>();
        foreach (var menu in menuResponses)
        {
            if (menu.ParentMenuId.HasValue && menuDict.ContainsKey(menu.ParentMenuId.Value))
            {
                menuDict[menu.ParentMenuId.Value].Children.Add(menu);
            }
            else
            {
                rootMenus.Add(menu);
            }
        }
        return rootMenus;
    }
}