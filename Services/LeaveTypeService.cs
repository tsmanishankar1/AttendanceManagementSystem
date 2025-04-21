using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class LeaveTypeService
    {
        private readonly AttendanceManagementSystemContext _context;

        public LeaveTypeService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveTypeResponse>> GetAllLeaveTypesAsync()
        {
            var allLeaveType = await (from leaveType in _context.LeaveTypes
                                      select new LeaveTypeResponse
                                      {
                                          LeaveTypeId = leaveType.Id,
                                          Name = leaveType.Name,
                                          ShortName = leaveType.ShortName,
                                          Accountable = leaveType.Accountable,
                                          Encashable = leaveType.Encashable,
                                          PaidLeave = leaveType.PaidLeave,
                                          CommonType = leaveType.CommonType,
                                          PermissionType = leaveType.PermissionType,
                                          CarryForward = leaveType.CarryForward,
                                          IsActive = leaveType.IsActive,
                                          CreatedBy = leaveType.CreatedBy
                                      })
                                     .ToListAsync();
            if (allLeaveType.Count == 0)
            {
                throw new MessageNotFoundException("No leave types found");
            }
            return allLeaveType;
        }

        public async Task<string> CreateLeaveTypeAsync(LeaveTypeRequest leaveTypeRequest)
        {
            var message = "Leave type added successfully";

            var leaveType = new LeaveType
            {
                Name = leaveTypeRequest.Name,
                ShortName = leaveTypeRequest.ShortName,
                Accountable = leaveTypeRequest.Accountable,
                Encashable = leaveTypeRequest.Encashable,
                PaidLeave = leaveTypeRequest.PaidLeave,
                CommonType = leaveTypeRequest.CommonType,
                PermissionType = leaveTypeRequest.PermissionType,
                CarryForward = leaveTypeRequest.CarryForward,
                IsActive = leaveTypeRequest.IsActive,
                CreatedBy = leaveTypeRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateLeaveTypeAsync(UpdateLeaveType leaveType)
        {
            var message = "Leave type updated successfully";

            var existingLeaveType = await _context.LeaveTypes.FirstOrDefaultAsync(s => s.Id == leaveType.LeaveTypeId);
            if (existingLeaveType == null)
                throw new MessageNotFoundException("Leave type not found");

            existingLeaveType.Name = leaveType.Name;
            existingLeaveType.ShortName = leaveType.ShortName;
            existingLeaveType.Accountable = leaveType.Accountable;
            existingLeaveType.Encashable = leaveType.Encashable;
            existingLeaveType.PaidLeave = leaveType.PaidLeave;
            existingLeaveType.CommonType = leaveType.CommonType;
            existingLeaveType.PermissionType = leaveType.PermissionType;
            existingLeaveType.CarryForward = leaveType.CarryForward;
            existingLeaveType.IsActive = leaveType.IsActive;
            existingLeaveType.UpdatedBy = leaveType.UpdatedBy;
            existingLeaveType.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
    }
}

