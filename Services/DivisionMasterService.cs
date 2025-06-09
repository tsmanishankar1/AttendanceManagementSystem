using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Services
{
    public class DivisionMasterService
    {
        private readonly AttendanceManagementSystemContext _context;

        public DivisionMasterService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<DivisionResponse>> GetAllDivisions()
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
            var message = "Division created successfully";
            var isDuplicate = await _context.DivisionMasters.AnyAsync(d => d.Name.ToLower() == divisionRequest.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Division name already exists");
            DivisionMaster division = new DivisionMaster
            {
                Name = divisionRequest.FullName,
                ShortName = divisionRequest.ShortName,
                IsActive = divisionRequest.IsActive,
                CreatedBy = divisionRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.DivisionMasters.AddAsync(division);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateDivision(UpdateDivision division)
        {
            var message = "Division updated successfully";
            var existingDivision = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Id == division.DivisionMasterId);
            if (existingDivision == null)
            {
                throw new MessageNotFoundException("Division not found");
            }
            if (!string.IsNullOrWhiteSpace(division.FullName))
            {
                var isDuplicate = await _context.DivisionMasters.AnyAsync(d => d.Id != division.DivisionMasterId && d.Name.ToLower() == division.FullName.ToLower());
                if (isDuplicate) throw new ConflictException("Division name already exists");
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