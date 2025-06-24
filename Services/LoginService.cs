using AttendanceManagement;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LoginService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<UserManagement> _passwordHasher;
    public LoginService(AttendanceManagementSystemContext context, IConfiguration configuration, IPasswordHasher<UserManagement> passwordHasher)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = passwordHasher;

    }

    public async Task<(string AccessToken, string RefreshToken)> ValidateUserAsync(Login login)
    {
        var user = await _context.UserManagements.FirstOrDefaultAsync(e => e.Username == login.Username && e.IsActive);
        if (user == null) throw new MessageNotFoundException("Invalid username");
        if (!VerifyPassword(user, user.Password, login.Password)) throw new MessageNotFoundException("Invalid password");
        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == user.StaffCreationId && s.IsActive == true);
        if (staff == null) throw new MessageNotFoundException("Staff not found");
        var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Id == staff.DesignationId && d.IsActive);
        if (designation == null) throw new MessageNotFoundException("Designation not found");
        var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Id == staff.DepartmentId && d.IsActive);
        if (department == null) throw new MessageNotFoundException("Department not found");
        var division = _context.DivisionMasters.FirstOrDefault(d => d.Id == staff.DivisionId && d.IsActive);
        if (division == null) throw new MessageNotFoundException("Division not found");
        var accessToken = GenerateJwtToken(user.Username, user.StaffCreationId, staff.DesignationId, designation.Name, staff.DepartmentId, department.Name, staff.DivisionId, division.Name);
        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(user.StaffCreationId),
            UserId = user.StaffCreationId,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsActive = true,
            CreatedBy = user.StaffCreationId,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return (accessToken, refreshToken.Token);
    }

    private string GenerateRefreshToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secret)) throw new InvalidOperationException("JWT SecretKey is missing in configuration");
        var key = Encoding.UTF8.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("UserManagementId", userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
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

    public async Task<(string AccessToken, string RefreshToken)> RefreshAccessToken(RefreshTokenDto refreshTokenDto)
    {
        if (string.IsNullOrEmpty(refreshTokenDto.RefreshToken)) throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshTokenDto.RefreshToken));
        var refreshTokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshTokenDto.RefreshToken);
        if (refreshTokenEntity == null || refreshTokenEntity.ExpiryDate < DateTime.UtcNow) throw new InvalidOperationException("Invalid or expired refresh token");
        var user = await _context.UserManagements.FirstOrDefaultAsync(u => u.Id == refreshTokenEntity.UserId);
        if (user == null) throw new Exception("User not found");
        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == user.StaffCreationId && s.IsActive == true);
        if (staff == null) throw new MessageNotFoundException("Staff not found");
        var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Id == staff.DesignationId && d.IsActive);
        if (designation == null) throw new MessageNotFoundException("Designation not found");
        var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Id == staff.DepartmentId && d.IsActive);
        if (department == null) throw new MessageNotFoundException("Department not found");
        var division = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Id == staff.DivisionId && d.IsActive);
        if (division == null) throw new MessageNotFoundException("Division not found");
        var newAccessToken = GenerateJwtToken(user.Username, user.StaffCreationId, staff.DesignationId, designation.Name, staff.DepartmentId, department.Name, staff.DivisionId, division.Name);

        refreshTokenEntity.IsActive = false;
        refreshTokenEntity.UpdatedBy = user.StaffCreationId;
        refreshTokenEntity.UpdatedUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var newRefreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(user.StaffCreationId),
            UserId = user.StaffCreationId,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsActive = true,
            CreatedBy = user.StaffCreationId,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.RefreshTokens.AddAsync(newRefreshToken);
        await _context.SaveChangesAsync();
        return (newAccessToken, newRefreshToken.Token);
    }

    private string GenerateJwtToken(string userName, int staffId, int designationId, string designation, int departmentId, string department, int divisionId, string divisionName)
    {
        try
        {
            var user = _context.UserManagements.FirstOrDefault(e => e.Username == userName && e.IsActive);
            if (user == null) throw new MessageNotFoundException("User not found");
            var userName1 = _context.StaffCreations.FirstOrDefault(e => e.Id == user.StaffCreationId && e.IsActive == true);
            if (userName1 == null) throw new MessageNotFoundException("User not found");
            var userName2 = $"{userName1.FirstName}{(string.IsNullOrWhiteSpace(userName1.LastName) ? "" : " " + userName1.LastName)}";
            var staff = _context.StaffCreations.FirstOrDefault(e => e.Id == staffId && e.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Staff not found");
            var role = _context.AccessLevels.FirstOrDefault(e => e.Name == staff.AccessLevel && e.IsActive == true);
            if (role == null) throw new MessageNotFoundException("Role not found");

/*            var designations = _context.DesignationMasters.FirstOrDefault(e => e.Id == designationId && e.IsActive == true);
            if (designations == null)
            {
                throw new MessageNotFoundException("Designation not found");
            }
*/
            var approver = _context.StaffCreations.FirstOrDefault(e => e.Id == staff.ApprovalLevel1 && e.IsActive == true);
            if (approver == null) throw new MessageNotFoundException("Approver not found");
            string approverFullName = approver != null ? $"{approver.FirstName}{(string.IsNullOrWhiteSpace(approver.LastName) ? "" : " " + approver.LastName)}" : "N/A";
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secret)) throw new InvalidOperationException("JWT SecretKey is missing in configuration");
            var key = Encoding.UTF8.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(key);
            string profilePhoto = staff.ProfilePhoto ?? "";

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserName", userName2),
                    new Claim("UserManagementId", user.Id.ToString()),
                    new Claim("StaffId", staff.Id.ToString()),
                    new Claim("StaffCreationId", staff.StaffId),
                    new Claim("DesignationId", designationId.ToString()),
                    new Claim("DesignationName", designation),
                    new Claim("DepartmentId", departmentId.ToString()),
                    new Claim("DepartmentName", department),
                    new Claim("DivisionId", divisionId.ToString()),
                    new Claim("DivisionName", divisionName),
                    new Claim("RoleId", role.Id.ToString()),
                    new Claim("Role", role.Name),
                    new Claim("ProfilePhoto", profilePhoto),
                    new Claim("ApproverId", staff.ApprovalLevel1.ToString()), 
                    new Claim("ApproverName", approverFullName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}