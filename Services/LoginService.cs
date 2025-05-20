using AttendanceManagement;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
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
    private readonly IMemoryCache _memoryCache;
    public LoginService(AttendanceManagementSystemContext context, IConfiguration configuration, IMemoryCache memoryCache)
    {
        _context = context;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }

    public async Task<(string AccessToken, string RefreshToken)> ValidateUserAsync(Login login)
    {
        try
        {
            var user = await _context.UserManagements.FirstOrDefaultAsync(e => e.Username == login.Username && e.IsActive);
            if (user == null)
            {
                throw new MessageNotFoundException("Invalid username");
            }
            if (user.Password != login.Password)
            {
                throw new MessageNotFoundException("Invalid password");
            }
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == user.StaffCreationId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Id == staff.DesignationId && d.IsActive);
            if (designation == null)
            {
                throw new MessageNotFoundException("Designation not found");
            }
            var accessToken = GenerateJwtToken(user.Username, user.StaffCreationId, staff.DesignationId, designation.Name);
            var refreshToken = GenerateRefreshToken(user.StaffCreationId);
            var tokenResponse = new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            var refreshTokenExpiry = DateTime.UtcNow.AddHours(1);
            _memoryCache.Set(refreshToken, refreshTokenExpiry, refreshTokenExpiry);

            return (accessToken, refreshToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string GenerateRefreshToken(int userId)
    {
        var refreshToken = GenerateRandomToken();
        var expiryDate = DateTime.UtcNow.AddHours(1);
        RefreshTokenStore.AddRefreshToken(refreshToken, userId, expiryDate);
        return refreshToken;
    }

    private string GenerateRandomToken()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var tokenData = new byte[32];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }
    }

    public string RefreshAccessToken(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));
        var refreshTokenData = RefreshTokenStore.GetRefreshToken(refreshToken);
        if (refreshTokenData == null || refreshTokenData.Value.ExpiryDate < DateTime.UtcNow) throw new InvalidOperationException("Invalid or expired refresh token");
        var user = _context.UserManagements.FirstOrDefault(u => u.Id == refreshTokenData.Value.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var staff = _context.StaffCreations.FirstOrDefault(s => s.Id == user.StaffCreationId && s.IsActive == true);
        if (staff == null) throw new MessageNotFoundException("Staff not found");
        var designation = _context.DesignationMasters.FirstOrDefault(d => d.Id == staff.DesignationId);
        if (designation == null) throw new MessageNotFoundException("Designation not found");
        var newAccessToken = GenerateJwtToken(user.Username, user.StaffCreationId, staff.DesignationId, designation.Name);

        return newAccessToken;
    }

    public static class RefreshTokenStore
    {
        private static readonly Dictionary<string, (int UserId, DateTime ExpiryDate)> _refreshTokens = new();
        public static void AddRefreshToken(string token, int userId, DateTime expiryDate)
        {
            _refreshTokens[token] = (userId, expiryDate);
        }        public static (int UserId, DateTime ExpiryDate)? GetRefreshToken(string token)
        {
            if (_refreshTokens.TryGetValue(token, out var tokenData))
            {
                return tokenData;
            }
            return null;
        }        public static void RemoveRefreshToken(string token)
        {
            _refreshTokens.Remove(token);
        }
    }

    private string GenerateJwtToken(string userName, int staffId, int designationId, string designation)
    {
        try
        {
            var user = _context.UserManagements.FirstOrDefault(e => e.Username == userName && e.IsActive);
            if (user == null)
            {
                throw new MessageNotFoundException("User not found");
            }
            var userName1 = _context.StaffCreations.FirstOrDefault(e => e.Id == user.StaffCreationId && e.IsActive == true);
            if (userName1 == null)
            {
                throw new MessageNotFoundException("User not found");
            }
            var userName2 = $"{userName1.FirstName}{(string.IsNullOrWhiteSpace(userName1.LastName) ? "" : " " + userName1.LastName)}";
            var staff = _context.StaffCreations.FirstOrDefault(e => e.Id == staffId && e.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            var role = _context.AccessLevels.FirstOrDefault(e => e.Name == staff.AccessLevel && e.IsActive == true);
            if (role == null)
            {
                throw new MessageNotFoundException("Role not found");
            }
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
            var key = new byte[32];
            RandomNumberGenerator.Fill(key);
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
                    new Claim("RoleId", role.Id.ToString()),
                    new Claim("Role", role.Name.ToString()),
                    new Claim("ProfilePhoto", profilePhoto),
                    new Claim("ApproverId", staff.ApprovalLevel1.ToString()), 
                    new Claim("ApproverName", approverFullName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
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