using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AttendanceManagement.Services;

public class BranchMasterService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IConfiguration _configuration;
    public BranchMasterService(AttendanceManagementSystemContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;

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
}