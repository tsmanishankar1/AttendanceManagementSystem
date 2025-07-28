using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace AttendanceManagement.Infrastructure.Infra;

public class UserManagementInfra : IUserManagementInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IPasswordHasher<UserManagement> _passwordHasher;

    public UserManagementInfra(AttendanceManagementSystemContext context, IPasswordHasher<UserManagement> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    public async Task<string> RegisterUser(UserManagementRequest userRequest)
    {
        var userNameExists = await _context.UserManagements.AnyAsync(u => u.Username == userRequest.Username && u.IsActive);
        if (userNameExists) throw new ConflictException("User name already exists");
        var staffWithOrgType = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == userRequest.StaffCreationId && s.IsActive == true);
        if (staffWithOrgType == null) throw new MessageNotFoundException("Staff not found");
        var userExists = await _context.UserManagements.AnyAsync(u => u.StaffCreationId == userRequest.StaffCreationId && u.IsActive);
        if (userExists) throw new ConflictException("Selected user is already registered");
        var user = new UserManagement
        {
            Username = userRequest.Username,
            IsActive = true,
            CreatedBy = userRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            StaffCreationId = userRequest.StaffCreationId
        };
        user.Password = _passwordHasher.HashPassword(user, userRequest.Password);
        await _context.UserManagements.AddAsync(user);
        await _context.SaveChangesAsync();
        return "User registered successfully";
    }

    public async Task<object> GetUserByUserId(int StaffId)
    {
        var user = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == StaffId && u.IsActive == true);
        if (user == null) throw new MessageNotFoundException("User not found");
        return new
        {
            StaffCreationId = user.Id,
            StaffName = $"{user.FirstName}{(string.IsNullOrWhiteSpace(user.LastName) ? "" : " " + user.LastName)}",
            CreatedBy = user.CreatedBy
        };
    }

    public async Task<string> ChangePasswordAsync(ChangePasswordModel model)
    {
        var message = "Password changed successfully";
        var user = await _context.UserManagements.FirstOrDefaultAsync(u => u.StaffCreationId == model.UserId && u.IsActive);
        if (user == null) throw new MessageNotFoundException("User not found");
        if (!VerifyPassword(user, user.Password, model.CurrentPassword))
        {
            throw new InvalidOperationException("Current password is incorrect");
        }
        if (model.CurrentPassword == model.NewPassword)
        {
            throw new InvalidOperationException("Current password and New password should not match");
        }
        if (model.NewPassword != model.ConfirmPassword)
        {
            throw new ArgumentException("New password and confirm password must match");
        }
        var passwordHistory = await _context.PasswordHistories
            .Where(ph => ph.CreatedBy == user.Id)
            .OrderByDescending(ph => ph.CreatedUtc)
            .Take(3)
            .ToListAsync();
        var reused = passwordHistory.Any(ph => VerifyPassword(user, ph.NewPassword, model.NewPassword));
        if (reused)
        {
            throw new InvalidOperationException("You have recently used this password. Please choose a different one");
        }
        var hashedNewPassword = _passwordHasher.HashPassword(user, model.NewPassword);
        var passwordHistoryEntry = new PasswordHistory
        {
            OldPassword = user.Password,
            NewPassword = hashedNewPassword,
            IsActive = true,
            CreatedBy = user.Id,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.PasswordHistories.AddAsync(passwordHistoryEntry);
        user.Password = hashedNewPassword;
        user.UpdatedUtc = DateTime.UtcNow;
        user.UpdatedBy = user.StaffCreationId;
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<UserManagementResponse> GetStaffDetailsByStaffName(string staffname)
    {
        var staffList = await (
            from staff in _context.StaffCreations
            join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
            where staff.IsActive == true
            select new
            {
                staff.Id,
                staff.FirstName,
                staff.LastName,
                staff.CreatedBy,
                DepartmentName = department.Name
            }
        ).ToListAsync();
        var user = staffList
            .Select(s => new UserManagementResponse
            {
                UserManagementId = s.Id,
                StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                DepartmentName = s.DepartmentName,
                CreatedBy = s.CreatedBy
            })
            .FirstOrDefault(u => u.StaffName == staffname);
        if (user == null) throw new MessageNotFoundException("Staff not found");
        return user;
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordModel model)
    {
        var message = "Password reset successfully";
        var user = await _context.UserManagements.FirstOrDefaultAsync(u => u.StaffCreationId == model.UserId && u.IsActive);
        if (user == null) throw new MessageNotFoundException("User not found");
        if (model.NewPassword != model.ConfirmPassword) throw new ArgumentException("New password and confirm password must match");
        var hashedPassword = _passwordHasher.HashPassword(user, model.NewPassword);
        user.Password = hashedPassword;
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
                              StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                              DepartmentName = department.Name,
                              CreatedBy = staff.CreatedBy
                          }).FirstOrDefaultAsync();
        if (user == null) throw new MessageNotFoundException("User not found.");
        return user;
    }

    public async Task<string> DeactivateStaffByUserManagementIdAsync(int staffCreationId, int deletedBy)
    {
        var message = "Staff deactivated successfully";
        var userManagement = await _context.UserManagements.FirstOrDefaultAsync(u => u.StaffCreationId == staffCreationId && u.IsActive);
        if (userManagement == null) throw new MessageNotFoundException("User not found");
        userManagement.IsActive = false;
        userManagement.UpdatedBy = deletedBy;
        userManagement.UpdatedUtc = DateTime.UtcNow;
        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffCreationId && s.IsActive == true);
        if (staff == null) throw new MessageNotFoundException("Staff not found");
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
                menuDict[menu.ParentMenuId.Value].Children?.Add(menu);
            }
            else
            {
                rootMenus.Add(menu);
            }
        }
        return rootMenus;
    }

    private bool VerifyPassword(UserManagement user, string storedPassword, string providedPassword)
    {
        try
        {
            var result = _passwordHasher.VerifyHashedPassword(user, storedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
        catch (FormatException)
        {
            return storedPassword == providedPassword;
        }
    }
}