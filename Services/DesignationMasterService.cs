using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<DesignationResponse>> GetAllDesignations()
        {
            var allDesignation = await (from designation in _context.DesignationMasters
                                  select new DesignationResponse
                                  {
                                      DesignationMasterId = designation.Id,
                                      FullName = designation.FullName,
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
            var message = "Designation added successfully";

            DesignationMaster designation = new DesignationMaster
            {
                FullName = designationRequest.FullName,
                ShortName = designationRequest.ShortName,
                IsActive = designationRequest.IsActive,
                CreatedBy = designationRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.DesignationMasters.Add(designation);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateDesignation(UpdateDesignation designation)
        {
            var message = "Designation updated successfully";

            // Find the existing designation by DesignationMasterId and check if it's active
            var existingDesignation = _context.DesignationMasters
                .FirstOrDefault(d => d.Id == designation.DesignationMasterId);

            if(existingDesignation == null)
            {
                throw new MessageNotFoundException("Designation not found");
            }

            existingDesignation.FullName = designation.FullName;
            existingDesignation.ShortName = designation.ShortName;
            existingDesignation.IsActive = designation.IsActive;
            existingDesignation.UpdatedBy = designation.UpdatedBy;
            existingDesignation.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
    }
}
