using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AttendanceManagement.Services
{
    public class DivisionMasterService
    {
        private readonly AttendanceManagementSystemContext _context;

        public DivisionMasterService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DivisionResponse>> GetAllDivisions()
        {
            var allDivision = await (from division in _context.DivisionMasters
                               select new DivisionResponse
                               {
                                   DivisionMasterId = division.Id,
                                   FullName = division.Name,
                                   ShortName = division.ShortName,
                                   IsActive = division.IsActive,
                                   CreatedBy = division.CreatedBy
                               })
                               .ToListAsync();
            if (allDivision.Count == 0)
            {
                throw new MessageNotFoundException("No divisions found");
            }
            return allDivision;
        }

        public async Task<string> AddDivision(DivisionRequest divisionRequest)
        {
            var message = "Division added successfully";
            DivisionMaster division = new DivisionMaster
            {
                Name = divisionRequest.FullName,
                ShortName = divisionRequest.ShortName,
                IsActive = divisionRequest.IsActive,
                CreatedBy = divisionRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.DivisionMasters.Add(division);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateDivision(UpdateDivision division)
        {
            var message = "Division updated successfully";
            var existingDivision = _context.DivisionMasters.FirstOrDefault(d => d.Id == division.DivisionMasterId);
            if (existingDivision == null)
            {
                throw new MessageNotFoundException("Division not found");
            }
            existingDivision.Name = division.FullName ?? existingDivision.Name;
            existingDivision.ShortName = division.ShortName ?? existingDivision.ShortName;
            existingDivision.IsActive = division.IsActive;
            existingDivision.UpdatedBy = division.UpdatedBy;
            existingDivision.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
    }
}