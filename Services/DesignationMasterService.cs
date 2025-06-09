using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AttendanceManagement.Services
{
    public class DesignationMasterService
    {
        private readonly AttendanceManagementSystemContext _context;

        public DesignationMasterService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<DesignationResponse>> GetAllDesignations()
        {
            var allDesignation = await (from designation in _context.DesignationMasters
                                  select new DesignationResponse
                                  {
                                      DesignationMasterId = designation.Id,
                                      FullName = designation.Name,
                                      ShortName = designation.ShortName,
                                      IsActive = designation.IsActive,
                                      CreatedBy = designation.CreatedBy
                                  })
                                  .ToListAsync();
            if(allDesignation.Count == 0)
            {
                throw new MessageNotFoundException("No designations found");
            }
            return allDesignation;
        }

        public async Task<string> AddDesignation(DesignationRequest designationRequest)
        {
            var message = "Designation created successfully";
            var isDuplicate = await _context.DesignationMasters.AnyAsync(d => d.Name.ToLower() == designationRequest.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Designation name already exists");
            DesignationMaster designation = new DesignationMaster
            {
                Name = designationRequest.FullName,
                ShortName = designationRequest.ShortName,
                IsActive = designationRequest.IsActive,
                CreatedBy = designationRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.DesignationMasters.AddAsync(designation);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateDesignation(UpdateDesignation designation)
        {
            var message = "Designation updated successfully";
            var existingDesignation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Id == designation.DesignationMasterId);
            if(existingDesignation == null)
            {
                throw new MessageNotFoundException("Designation not found");
            }
            if (!string.IsNullOrWhiteSpace(designation.FullName))
            {
                var isDuplicate = await _context.DesignationMasters.AnyAsync(d => d.Id != designation.DesignationMasterId && d.Name.ToLower() == designation.FullName.ToLower());
                if (isDuplicate) throw new ConflictException("Designation name already exists");
            }
            existingDesignation.Name = designation.FullName;
            existingDesignation.ShortName = designation.ShortName;
            existingDesignation.IsActive = designation.IsActive;
            existingDesignation.UpdatedBy = designation.UpdatedBy;
            existingDesignation.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
    }
}