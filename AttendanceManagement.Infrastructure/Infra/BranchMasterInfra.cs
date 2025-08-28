using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AttendanceManagement.Infrastructure.Infra;

public class BranchMasterInfra : IBranchMasterInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IWebHostEnvironment _env;
    public BranchMasterInfra(AttendanceManagementSystemContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<List<BranchMasterResponse>> GetAllBranches()
    {
        var allBranch = await (from b in _context.BranchMasters
                               join c in _context.CompanyMasters
                               on b.CompanyMasterId equals c.Id
                               select new BranchMasterResponse
                               {
                                   BranchMasterId = b.Id,
                                   FullName = b.Name,
                                   ShortName = b.ShortName,
                                   Address = b.Address,
                                   City = b.City,
                                   District = b.District,
                                   State = b.State,
                                   Country = b.Country,
                                   PostalCode = b.PostalCode,
                                   PhoneNumber = b.PhoneNumber,
                                   Fax = b.Fax,
                                   Email = b.Email,
                                   CompanyMasterId = b.CompanyMasterId,
                                   CompanyMasterName = c.Name,
                                   IsHeadOffice = b.IsHeadOffice,
                                   IsActive = b.IsActive,
                                   CreatedBy = b.CreatedBy
                               })
                          .ToListAsync();
        if (allBranch.Count == 0)
        {
            throw new MessageNotFoundException("No branches found");
        }
        return allBranch;
    }

    public async Task<string> CreateBranch(BranchMasterRequest branchMasterRequest)
    {
        var message = "Branch created successfully.";
        var companyId = await _context.CompanyMasters.AnyAsync(d => d.Id == branchMasterRequest.CompanyMasterId && d.IsActive);
        if (!companyId) throw new MessageNotFoundException("Company not found");
        var duplicateBranch = await _context.BranchMasters.AnyAsync(b => b.Name.ToLower() == branchMasterRequest.FullName.ToLower());
        if (duplicateBranch) throw new ConflictException("Branch name already exists");
        var branchMaster = new BranchMaster
        {
            Name = branchMasterRequest.FullName,
            ShortName = branchMasterRequest.ShortName,
            Address = branchMasterRequest.Address,
            City = branchMasterRequest.City,
            District = branchMasterRequest.District,
            State = branchMasterRequest.State,
            Country = branchMasterRequest.Country,
            PostalCode = branchMasterRequest.PostalCode,
            PhoneNumber = branchMasterRequest.PhoneNumber,
            Fax = branchMasterRequest.Fax,
            Email = branchMasterRequest.Email,
            IsHeadOffice = branchMasterRequest.IsHeadOffice,
            IsActive = branchMasterRequest.IsActive,
            CreatedBy = branchMasterRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            CompanyMasterId = branchMasterRequest.CompanyMasterId
        };
        await _context.BranchMasters.AddAsync(branchMaster);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> UpdateBranch(UpdateBranch branchMasterRequest)
    {
        var message = "Branch updated successfully.";
        var companyId = await _context.CompanyMasters.AnyAsync(d => d.Id == branchMasterRequest.CompanyMasterId && d.IsActive);
        if (!companyId) throw new MessageNotFoundException("Company not found");
        var existingBranch = await _context.BranchMasters.FirstOrDefaultAsync(b => b.Id == branchMasterRequest.BranchMasterId);
        if (existingBranch == null)
        {
            throw new MessageNotFoundException("Branch not found");
        }
        if (!string.IsNullOrWhiteSpace(branchMasterRequest.FullName))
        {
            var duplicateBranch = await _context.BranchMasters.AnyAsync(b => b.Id != branchMasterRequest.BranchMasterId && b.Name.ToLower() == branchMasterRequest.FullName.ToLower());
            if (duplicateBranch) throw new ConflictException("Branch name already exists");
        }
        existingBranch.Name = branchMasterRequest.FullName ?? existingBranch.Name;
        existingBranch.ShortName = branchMasterRequest.ShortName ?? existingBranch.ShortName;
        existingBranch.Address = branchMasterRequest.Address ?? existingBranch.Address;
        existingBranch.City = branchMasterRequest.City ?? existingBranch.City;
        existingBranch.District = branchMasterRequest.District ?? existingBranch.District;
        existingBranch.State = branchMasterRequest.State ?? existingBranch.State;
        existingBranch.Country = branchMasterRequest.Country ?? existingBranch.Country;
        existingBranch.PostalCode = branchMasterRequest.PostalCode;
        existingBranch.PhoneNumber = branchMasterRequest.PhoneNumber;
        existingBranch.Fax = branchMasterRequest.Fax ?? existingBranch.Fax;
        existingBranch.Email = branchMasterRequest.Email ?? existingBranch.Email;
        existingBranch.IsHeadOffice = branchMasterRequest.IsHeadOffice;
        existingBranch.IsActive = branchMasterRequest.IsActive;
        existingBranch.UpdatedBy = branchMasterRequest.UpdatedBy;
        existingBranch.UpdatedUtc = DateTime.UtcNow;
        existingBranch.CompanyMasterId = branchMasterRequest.CompanyMasterId;

        await _context.SaveChangesAsync();
        return message;
    }

    public string GetAppsettings()
    {
        var path = Path.Combine(_env.ContentRootPath, "appsettings.json");
        if (!File.Exists(path)) throw new FileNotFoundException("appsettings.json not found");
        return File.ReadAllText(path);
    }

    public List<string> GetWorkspaceFile()
    {
        var rootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
        if (!Directory.Exists(rootPath)) throw new DirectoryNotFoundException("Workspace folder not found");
        var allFiles = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories);
        var relativePaths = allFiles.Select(file => Path.GetRelativePath(rootPath, file).Replace("\\", "/")).ToList();
        return relativePaths;
    }

    public async Task<List<Goal>> GetGoals()
    {
        return await _context.Goals.ToListAsync();
    }

    public async Task<List<KraSelfReview>> KraSelfReviews()
    {
        return await _context.KraSelfReviews.ToListAsync();
    }

    public async Task<List<KraManagerReview>> KraManagerReviews()
    {
        return await _context.KraManagerReviews.ToListAsync();
    }

    public async Task<List<UserManagement>> GetUserManagement()
    {
        return await _context.UserManagements.ToListAsync();
    }

    public async Task<List<Probation>> GetProbations()
    {
        return await _context.Probations.ToListAsync();
    }

    public async Task<List<Feedback>> GetFeedbacks()
    {
        return await _context.Feedbacks.ToListAsync();
    }

    public async Task<List<ProbationReport>> GetProbationReports()
    {
        return await _context.ProbationReports.ToListAsync();
    }

    public async Task<List<RefreshToken>> GetRefreshToken()
    {
        return await _context.RefreshTokens.ToListAsync();
    }

    public async Task<List<AssignShift>> GetAssignShift()
    {
        return await _context.AssignShifts.ToListAsync();
    }

    public async Task<List<AuditLog>> GetAuditLog()
    {
        return await _context.AuditLogs.ToListAsync();
    }

    public async Task<List<ErrorLog>> GetErrorLog()
    {
        return await _context.ErrorLogs.ToListAsync();
    }
}