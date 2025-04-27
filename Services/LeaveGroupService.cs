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
                                         leave.Name,
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
                LeaveGroupName = leave.Name,
                IsActive = leave.IsActive,
                CreatedBy = leave.CreatedBy,
                LeaveTypeIds = _context.LeaveGroupTransactions
                                       .Where(transaction => transaction.LeaveGroupId == leave.Id && transaction.IsActive)
                                       .Select(transaction => transaction.LeaveTypeId)
                                       .ToList()
            }).ToList();
            return leaveGroupResponses;
        }

        public async Task<string> AddLeaveGroupWithTransactionsAsync(AddLeaveGroupDto addLeaveGroupDto)
        {
            var message = "Leave group added successfully";
            var leaveGroup = new LeaveGroup
            {
                Name = addLeaveGroupDto.LeaveGroupName,
                IsActive = addLeaveGroupDto.IsActive,
                CreatedBy = addLeaveGroupDto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.LeaveGroups.Add(leaveGroup);
            await _context.SaveChangesAsync();

            foreach (var leaveTypeId in addLeaveGroupDto.LeaveTypeIds)
            {
                var leaveGroupTransaction = new LeaveGroupTransaction
                {
                    LeaveGroupId = leaveGroup.Id,
                    LeaveTypeId = leaveTypeId,
                    IsActive = addLeaveGroupDto.IsActive,
                    CreatedBy = addLeaveGroupDto.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };

                _context.LeaveGroupTransactions.Add(leaveGroupTransaction);
            }
            await _context.SaveChangesAsync();
            return message;
        }


        public async Task<string> UpdateLeaveGroup(UpdateLeaveGroup leaveGroup)
        {
            var message = "Leave group updated successfully";
            var existingLeaveGroup = await _context.LeaveGroups.FirstOrDefaultAsync(lg => lg.Id == leaveGroup.LeaveGroupId);
            if (existingLeaveGroup == null)
            {
                throw new MessageNotFoundException("Leave group not found");
            }
            existingLeaveGroup.Name = leaveGroup.LeaveGroupName ?? existingLeaveGroup.Name;
            existingLeaveGroup.IsActive = leaveGroup.IsActive;
            existingLeaveGroup.UpdatedBy = leaveGroup.UpdatedBy;
            existingLeaveGroup.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            if (leaveGroup.LeaveTypeIds != null)
            {
                var existingLeaveGroupTransactions = await _context.LeaveGroupTransactions.Where(lgt => lgt.LeaveGroupId == leaveGroup.LeaveGroupId && lgt.IsActive).ToListAsync();
                foreach (var transaction in existingLeaveGroupTransactions)
                {
                    transaction.IsActive = false;
                    transaction.UpdatedBy = leaveGroup.UpdatedBy;
                    transaction.UpdatedUtc = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync(); 
                var newLeaveGroupTransactions = leaveGroup.LeaveTypeIds.Select(leaveTypeId => new LeaveGroupTransaction
                {
                    LeaveGroupId = leaveGroup.LeaveGroupId,
                    LeaveTypeId = leaveTypeId,
                    IsActive = leaveGroup.IsActive,
                    CreatedBy = leaveGroup.UpdatedBy,
                    CreatedUtc = DateTime.UtcNow
                }).ToList();

                await _context.LeaveGroupTransactions.AddRangeAsync(newLeaveGroupTransactions);
                await _context.SaveChangesAsync();
            }
            return message;
        }
    }
}