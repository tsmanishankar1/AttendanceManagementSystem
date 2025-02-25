using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceManagement.Services
{
    public class LeaveGroupService
    {
        private readonly AttendanceManagementSystemContext _context;

        public LeaveGroupService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveGroupResponse>> GetAllLeaveGroups()
        {
            var leaveGroups = await (from leave in _context.LeaveGroups
                                     select new
                                     {
                                         leave.Id,
                                         leave.LeaveGroupName,
                                         leave.IsActive,
                                         leave.CreatedBy
                                     })
                                   .ToListAsync();

            if (leaveGroups.Count == 0)
            {
                throw new MessageNotFoundException("No leave groups found");
            }

            var leaveGroupResponses = leaveGroups.Select(leave => new LeaveGroupResponse
            {
                LeaveGroupId = leave.Id,
                LeaveGroupName = leave.LeaveGroupName,
                IsActive = leave.IsActive,
                CreatedBy = leave.CreatedBy,
                LeaveTypeIds = _context.LeaveGroupTransactions
                                       .Where(transaction => transaction.LeaveGroupId == leave.Id && transaction.IsActive)
                                       .Select(transaction => transaction.LeaveTypeId)
                                       .ToList()
            }).ToList();

            return leaveGroupResponses;
        }

        public async Task<LeaveGroupResponse> GetLeaveGroupDetailsById(int leaveGroupId)
        {
            var leaveGroup = await (from leave in _context.LeaveGroups
                                    where leave.Id == leaveGroupId
                                    select new LeaveGroupResponse
                                    {
                                        LeaveGroupId = leave.Id,
                                        LeaveGroupName = leave.LeaveGroupName,
                                        IsActive = leave.IsActive,
                                        CreatedBy = leave.CreatedBy,
                                        LeaveTypeIds = (from transaction in _context.LeaveGroupTransactions
                                                        where transaction.LeaveGroupId == leaveGroupId && transaction.IsActive
                                                        select transaction.LeaveTypeId).ToList()
                                    })
                                  .FirstOrDefaultAsync();

            if (leaveGroup == null)
            {
                throw new MessageNotFoundException("Leave group not found");
            }

            return leaveGroup;
        }
        public async Task<string> AddLeaveGroupWithTransactionsAsync(AddLeaveGroupDto addLeaveGroupDto)
        {
            var message = "Leave group added successfully";
            // Create a new LeaveGroup entry
            var leaveGroup = new LeaveGroup
            {
                LeaveGroupName = addLeaveGroupDto.LeaveGroupName,
                IsActive = addLeaveGroupDto.IsActive,
                CreatedBy = addLeaveGroupDto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            // Add LeaveGroup to the database
            _context.LeaveGroups.Add(leaveGroup);
            await _context.SaveChangesAsync(); // Save to get LeaveGroupId

            // Add LeaveGroupTransaction entries
            foreach (var leaveTypeId in addLeaveGroupDto.LeaveTypeIds)
            {
                var leaveGroupTransaction = new LeaveGroupTransaction
                {
                    LeaveGroupId = leaveGroup.Id, // Use the new LeaveGroupId
                    LeaveTypeId = leaveTypeId,
                    IsActive = addLeaveGroupDto.IsActive,
                    CreatedBy = addLeaveGroupDto.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };

                _context.LeaveGroupTransactions.Add(leaveGroupTransaction);
            }

            // Save the LeaveGroupTransaction entries
            await _context.SaveChangesAsync();
            return message;
        }


        public async Task<string> UpdateLeaveGroup(UpdateLeaveGroup leaveGroup)
        {
            var message = "Leave group updated successfully";

            // Fetch the existing LeaveGroup
            var existingLeaveGroup = await _context.LeaveGroups
                .FirstOrDefaultAsync(lg => lg.Id == leaveGroup.LeaveGroupId);

            if (existingLeaveGroup == null)
            {
                throw new MessageNotFoundException("Leave group not found");
            }

            // Update the LeaveGroup properties
            existingLeaveGroup.LeaveGroupName = leaveGroup.LeaveGroupName ?? existingLeaveGroup.LeaveGroupName;
            existingLeaveGroup.IsActive = leaveGroup.IsActive;
            existingLeaveGroup.UpdatedBy = leaveGroup.UpdatedBy;
            existingLeaveGroup.UpdatedUtc = DateTime.UtcNow;

            // Save the updated LeaveGroup
            await _context.SaveChangesAsync();

            // Validate LeaveType associations
            if (leaveGroup.LeaveTypeIds != null)
            {
                // Fetch existing LeaveGroupTransaction associations
                var existingLeaveGroupTransactions = await _context.LeaveGroupTransactions
                    .Where(lgt => lgt.LeaveGroupId == leaveGroup.LeaveGroupId)
                    .ToListAsync();

                // Determine which leave types to reactivate, deactivate, and add
                var leaveTypeIdsToReactivate = existingLeaveGroupTransactions
                    .Where(lgt => !lgt.IsActive && leaveGroup.LeaveTypeIds.Contains(lgt.LeaveTypeId))
                    .Select(lgt => lgt.LeaveTypeId)
                    .ToList();

                var leaveTypeIdsToDeactivate = existingLeaveGroupTransactions
                    .Where(lgt => lgt.IsActive && !leaveGroup.LeaveTypeIds.Contains(lgt.LeaveTypeId))
                    .Select(lgt => lgt.LeaveTypeId)
                    .ToList();

                var leaveTypeIdsToAdd = leaveGroup.LeaveTypeIds
                    .Where(leaveTypeId => !existingLeaveGroupTransactions.Any(lgt => lgt.LeaveTypeId == leaveTypeId))
                    .ToList();

                // Reactivate existing associations that need to be reactivated
                foreach (var transaction in existingLeaveGroupTransactions.Where(lgt => leaveTypeIdsToReactivate.Contains(lgt.LeaveTypeId)))
                {
                    transaction.IsActive = true;
                    transaction.UpdatedBy = leaveGroup.UpdatedBy;
                    transaction.UpdatedUtc = DateTime.UtcNow;
                }

                // Deactivate existing associations that are no longer relevant
                foreach (var transaction in existingLeaveGroupTransactions.Where(lgt => leaveTypeIdsToDeactivate.Contains(lgt.LeaveTypeId)))
                {
                    transaction.IsActive = false;
                    transaction.UpdatedBy = leaveGroup.UpdatedBy;
                    transaction.UpdatedUtc = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(); // Save changes after updating existing associations

                // Add new LeaveGroupTransaction associations
                foreach (var leaveTypeId in leaveTypeIdsToAdd)
                {
                    var newLeaveGroupTransaction = new LeaveGroupTransaction
                    {
                        LeaveGroupId = leaveGroup.LeaveGroupId,
                        LeaveTypeId = leaveTypeId,
                        IsActive = leaveGroup.IsActive,
                        CreatedBy = leaveGroup.UpdatedBy, // Use CreatedBy for new transactions
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.LeaveGroupTransactions.Add(newLeaveGroupTransaction);
                }

                // Save the new LeaveGroupTransaction entries
                await _context.SaveChangesAsync();
            }

            return message;
        }
    }
}

