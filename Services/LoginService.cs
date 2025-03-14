using AttendanceManagement;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class LoginService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IConfiguration _configuration;

    public LoginService(AttendanceManagementSystemContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<string> ValidateUserAsync(Login login)
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
            var staff = _context.StaffCreations
                .AsEnumerable()
                .FirstOrDefault(s => s.Id == user.StaffCreationId && s.IsActive.GetValueOrDefault());

            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            var designation = _context.DesignationMasters
                .AsEnumerable()
                .FirstOrDefault(d => d.Id == staff.DesignationId && d.IsActive);

            if (designation == null)
            {
                throw new MessageNotFoundException("Designation not found");
            }

            var token = GenerateJwtToken(user.Username, user.StaffCreationId, staff.DesignationId, designation.FullName);
            return token;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string GenerateJwtToken(string userName, int staffId, int designationId, string role)
    {
        try
        {
            var user = _context.UserManagements.FirstOrDefault(e => e.Username == userName && e.IsActive);

            if (user == null)
            {
                throw new MessageNotFoundException("User not found");
            }
            var staff = _context.StaffCreations.FirstOrDefault(e => e.Id == staffId && e.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            var staffCreationId = $"{_context.OrganizationTypes.Where(o => o.Id == staff.OrganizationTypeId).Select(o => o.ShortName).FirstOrDefault()}{staff.Id}";
            var roleId = _context.AccessLevels.FirstOrDefault(e => e.Name == staff.AccessLevel && e.IsActive == true);
            if(roleId == null)
            {
                throw new MessageNotFoundException("Role not found");
            }
            var designation = _context.DesignationMasters.FirstOrDefault(e => e.Id == designationId && e.IsActive == true);
            if (designation == null)
            {
                throw new MessageNotFoundException("Designation not found");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new byte[32];
            RandomNumberGenerator.Fill(key);
            var signingKey = new SymmetricSecurityKey(key);
            string profilePhoto = staff.ProfilePhoto ?? "";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserName", user.Username),
                    new Claim("UserManagementId", user.Id.ToString()),
                    new Claim("StaffId", staff.Id.ToString()),
                    new Claim("StaffCreationId", staffCreationId),
                    new Claim("DesignationId", designation.Id.ToString()),
                    new Claim("DesignationName", designation.FullName),
                    new Claim("RoleId", roleId.Id.ToString()),
                    new Claim("Role", roleId.Name.ToString()),
                    new Claim("ProfilePhoto", profilePhoto)
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