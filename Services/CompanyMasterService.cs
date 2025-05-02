using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class CompanyMasterService
    {
        private readonly AttendanceManagementSystemContext _context;

        public CompanyMasterService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyMasterResponse>> GetAll()
        {
            var allCompany = await (from company in _context.CompanyMasters
                              select new CompanyMasterResponse
                              {
                                  CompanyMasterId = company.Id,
                                  FullName = company.Name,
                                  ShortName = company.ShortName,
                                  LegalName = company.LegalName,
                                  Address = company.Address,
                                  Website = company.Website,
                                  RegisterNumber = company.RegisterNumber,
                                  Tngsnumber = company.Tngsnumber,
                                  Cstnumber = company.Cstnumber,
                                  Tinnumber = company.Tinnumber,
                                  ServiceTaxNo = company.ServiceTaxNo,
                                  Pannumber = company.Pannumber,
                                  Pfnumber = company.Pfnumber,
                                  IsActive = company.IsActive,
                                  CreatedBy = company.CreatedBy
                              })
                              .ToListAsync();
            if (allCompany.Count == 0)
            {
                throw new MessageNotFoundException("No company found");
            }
            return allCompany;
        }

        public async Task<string> Add(CompanyMasterRequest companyMasterRequest)
        {
            var message = "Company created successfully";
            CompanyMaster company = new CompanyMaster();
            company.Name = companyMasterRequest.FullName;
            company.ShortName = companyMasterRequest.ShortName;
            company.LegalName = companyMasterRequest.LegalName;
            company.Address = companyMasterRequest.Address;
            company.Website = companyMasterRequest.Website;
            company.RegisterNumber = companyMasterRequest.RegisterNumber;
            company.Tngsnumber = companyMasterRequest.Tngsnumber;
            company.Cstnumber = companyMasterRequest.Cstnumber;
            company.Tinnumber = companyMasterRequest.Tinnumber;
            company.ServiceTaxNo = companyMasterRequest.ServiceTaxNo;
            company.Pannumber = companyMasterRequest.Pannumber;
            company.Pfnumber = companyMasterRequest.Pfnumber;
            company.IsActive = companyMasterRequest.IsActive;
            company.CreatedBy = companyMasterRequest.CreatedBy;
            company.CreatedUtc = DateTime.UtcNow;

            await _context.CompanyMasters.AddAsync(company);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> Update(CompanyMasterDto companyMaster)
        {
            var message = "Company updated successfully";
            var existingCompany = await _context.CompanyMasters.FirstOrDefaultAsync(d => d.Id == companyMaster.CompanyMasterId);
            if (existingCompany == null)
            {
                throw new MessageNotFoundException("Category not found");
            }
            existingCompany.Name = companyMaster.FullName ?? existingCompany.Name;
            existingCompany.ShortName = companyMaster.ShortName ?? existingCompany.ShortName;
            existingCompany.LegalName = companyMaster.LegalName ?? existingCompany.LegalName;
            existingCompany.Address = companyMaster.Address ?? existingCompany.Address;
            existingCompany.Website = companyMaster.Website ?? existingCompany.Website;
            existingCompany.RegisterNumber = companyMaster.RegisterNumber ?? existingCompany.RegisterNumber;
            existingCompany.Tngsnumber = companyMaster.Tngsnumber ?? existingCompany.Tngsnumber;
            existingCompany.Cstnumber = companyMaster.Cstnumber ?? existingCompany.Cstnumber;
            existingCompany.Tinnumber = companyMaster.Tinnumber ?? existingCompany.Tinnumber;
            existingCompany.ServiceTaxNo = companyMaster.ServiceTaxNo ?? existingCompany.ServiceTaxNo;
            existingCompany.Pannumber = companyMaster.Pannumber ?? existingCompany.Pannumber;
            existingCompany.Pfnumber = companyMaster.Pfnumber ?? existingCompany.Pfnumber;
            existingCompany.IsActive = companyMaster.IsActive;
            existingCompany.UpdatedBy = companyMaster.UpdatedBy;
            existingCompany.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
    }
}