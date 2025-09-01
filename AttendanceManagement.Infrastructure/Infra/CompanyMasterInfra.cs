using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class CompanyMasterInfra : ICompanyMasterInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IMemoryCache _cache;
        private const string CompanyCacheKey = "AllCompanies";
        public CompanyMasterInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<CompanyMasterResponse>> GetAll()
        {
            if (_cache.TryGetValue(CompanyCacheKey, out var cachedObj) && cachedObj is List<CompanyMasterResponse> cachedCompanies)
            {
                return cachedCompanies;
            }

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

            _cache.Set(CompanyCacheKey, allCompany);

            return allCompany;
        }

        public async Task<string> Add(CompanyMasterRequest companyMasterRequest)
        {
            var message = "Company created successfully";

            var duplicateCompany = await _context.CompanyMasters
                .AnyAsync(c => c.Name.ToLower() == companyMasterRequest.FullName.ToLower());
            if (duplicateCompany) throw new ConflictException("Company name already exists");

            var company = new CompanyMaster
            {
                Name = companyMasterRequest.FullName,
                ShortName = companyMasterRequest.ShortName,
                LegalName = companyMasterRequest.LegalName,
                Address = companyMasterRequest.Address,
                Website = companyMasterRequest.Website,
                RegisterNumber = companyMasterRequest.RegisterNumber,
                Tngsnumber = companyMasterRequest.Tngsnumber,
                Cstnumber = companyMasterRequest.Cstnumber,
                Tinnumber = companyMasterRequest.Tinnumber,
                ServiceTaxNo = companyMasterRequest.ServiceTaxNo,
                Pannumber = companyMasterRequest.Pannumber,
                Pfnumber = companyMasterRequest.Pfnumber,
                IsActive = companyMasterRequest.IsActive,
                CreatedBy = companyMasterRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            await _context.CompanyMasters.AddAsync(company);
            await _context.SaveChangesAsync();

            _cache.Remove(CompanyCacheKey);

            return message;
        }

        public async Task<string> Update(CompanyMasterDto companyMaster)
        {
            var message = "Company updated successfully";

            var existingCompany = await _context.CompanyMasters
                .FirstOrDefaultAsync(d => d.Id == companyMaster.CompanyMasterId);

            if (existingCompany == null)
            {
                throw new MessageNotFoundException("Company not found");
            }

            if (!string.IsNullOrWhiteSpace(companyMaster.FullName))
            {
                var duplicateCompany = await _context.CompanyMasters
                    .AnyAsync(c => c.Id != companyMaster.CompanyMasterId &&
                                   c.Name.ToLower() == companyMaster.FullName.ToLower());
                if (duplicateCompany) throw new ConflictException("Company name already exists");
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

            _cache.Remove(CompanyCacheKey);

            return message;
        }
    }
}