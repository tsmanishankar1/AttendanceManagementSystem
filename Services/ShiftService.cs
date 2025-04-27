using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceManagement.Services
{
    public class ShiftService
    {
        private readonly AttendanceManagementSystemContext _context;

        public ShiftService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<List<StaffsDto>> GetStaffByDivisionIdAsync(int divisionId)
        {
            var staffList = await _context.StaffCreations
                .Where(s => s.DivisionId == divisionId)
                .Select(s => new StaffsDto
                {
                    StaffId=s.Id,
                    StaffCreationId = s.StaffId,
                    FullName = $"{s.FirstName} {s.LastName ?? ""}".Trim(),
                    DepartmentId = s.DepartmentId,
                    DepartmentName = _context.DepartmentMasters
                                    .Where(d => d.Id == s.DepartmentId)
                                    .Select(d => d.Name)
                                    .FirstOrDefault() ?? "Unknown",
                    DesignationId = s.DesignationId,
                    DesignationName = _context.DesignationMasters
                                    .Where(des => des.Id == s.DesignationId)
                                    .Select(des => des.Name)
                                    .FirstOrDefault() ?? "Unknown"
                })
                .ToListAsync();
            return staffList;
        }

        public async Task<IEnumerable<ShiftResponse>> GetAllShiftsAsync()
        {
            var allShift = await (from shift in _context.Shifts
                                  join shiftType in _context.ShiftTypeDropDowns
                                  on shift.ShiftTypeId equals shiftType.Id into shiftTypeGroup
                                  from shiftType in shiftTypeGroup.DefaultIfEmpty() 
                                  select new ShiftResponse
                                  {
                                      ShiftId = shift.Id,
                                      ShiftName = shift.Name,
                                      ShiftTypeId = shift.ShiftTypeId,
                                      ShiftTypeName = shiftType != null ? shiftType.Name : null, 
                                      ShortName = shift.ShortName,
                                      StartTime = shift.StartTime,
                                      EndTime = shift.EndTime,
                                      IsActive = shift.IsActive,
                                      CreatedBy = shift.CreatedBy
                                  })
                                  .ToListAsync();
            if (allShift.Count == 0)
            {
                throw new MessageNotFoundException("No shifts found");
            }
            return allShift;
        }

        public async Task<string> CreateShiftAsync(ShiftRequest newShift)
        {
            var message = "Shift added successfully";
            var shift = new Shift
            {
                Name = newShift.ShiftName,
                ShortName = newShift.ShortName,
                ShiftTypeId = newShift.ShiftTypeId,
                StartTime = newShift.StartTime,
                EndTime = newShift.EndTime,
                IsActive = newShift.IsActive,
                CreatedBy = newShift.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateShiftAsync(UpdateShift updatedShift)
        {
            var message = "Shift updated successfully";
            var existingShift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == updatedShift.ShiftId);
            if (existingShift == null) throw new MessageNotFoundException("Shift not found");
            existingShift.Name = updatedShift.ShiftName;
            existingShift.ShortName = updatedShift.ShortName;
            existingShift.ShiftTypeId = updatedShift.ShiftTypeId;
            existingShift.StartTime = updatedShift.StartTime;
            existingShift.EndTime = updatedShift.EndTime;
            existingShift.IsActive = updatedShift.IsActive;
            existingShift.UpdatedBy = updatedShift.UpdatedBy;
            existingShift.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> CreateRegularShiftAsync(RegularShiftRequest regularShift)
        {
            var message = "Regular shifts added successfully";
            var shifts = regularShift.StaffIds.Select(staffId => new RegularShift
            {
                ShiftType = regularShift.ShiftType,
                WeeklyOffType = regularShift.WeeklyOffType,
                DayPattern = regularShift.DayPattern,
                ChangeEffectFrom = regularShift.ChangeEffectFrom,
                Reason = regularShift.Reason,
                ShiftId = regularShift.ShiftId,
                ShiftPattern = regularShift.ShiftPattern,
                StaffCreationId = staffId, 
                IsActive = true,
                CreatedBy = regularShift.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            })
            .ToList();
            _context.RegularShifts.AddRange(shifts);
            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<string> AssignShiftToStaffAsync(AssignShiftRequest assignShift)
        {
            var message = "Shift assigned successfully";
            var selectedRows = assignShift.selectedRows;
            foreach (var item in selectedRows)
            {
                var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == assignShift.ShiftId && s.IsActive);
                if (shift == null) throw new MessageNotFoundException("Shift not found");
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == item.Id && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = staff.FirstName + " " + staff.LastName;
                var existingAssignedShift = await _context.AssignShifts
                    .Where(a => a.FromDate == assignShift.FromDate && a.ToDate == assignShift.ToDate && a.IsActive)
                    .ToListAsync();
                if(existingAssignedShift.Count > 0)
                {
                    throw new MessageNotFoundException($"Shift already assigned for staff {staffName}");
                }
                var existingAssignments = await _context.AssignShifts
                           .Where(a => a.StaffId == item.Id && a.IsActive)
                           .ToListAsync();
                foreach (var existingShift in existingAssignments)
                {
                    if (existingShift.ToDate >= assignShift.FromDate)
                    {
                        if (existingShift.FromDate < assignShift.FromDate && existingShift.ToDate >= assignShift.FromDate)
                        {
                            existingShift.ToDate = assignShift.FromDate.AddDays(-1);
                        }
                        else
                        {
                            existingShift.IsActive = false;
                        }
                        existingShift.UpdatedBy = assignShift.CreatedBy;
                        existingShift.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                var shiftAssign = new AssignShift
                {
                    FromDate = assignShift.FromDate,
                    ToDate = assignShift.ToDate,
                    ShiftId = assignShift.ShiftId,
                    StaffId = item.Id,
                    IsActive = true,
                    CreatedBy = assignShift.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                _context.AssignShifts.Add(shiftAssign);
                await _context.SaveChangesAsync();
            }
            await MarkExpiredShiftsInactive(assignShift.CreatedBy);
            return message;
        }

        private async Task MarkExpiredShiftsInactive(int updatedBy)
        {
            DateTime date = DateTime.Now;
            var expiredShifts = await _context.AssignShifts
                .Where(s => s.IsActive && s.ToDate < DateOnly.FromDateTime(date))
                .ToListAsync();
            foreach (var shift in expiredShifts)
            {
                shift.IsActive = false;
                shift.UpdatedBy = updatedBy;
                shift.UpdatedUtc = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<AssignedShiftResponse>> GetAllAssignedShifts(int approverId)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverId)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN";
            var getAssignedShifts = await (from ass in _context.AssignShifts
                                           join sh in _context.Shifts on ass.ShiftId equals sh.Id
                                           join st in _context.StaffCreations on ass.StaffId equals st.Id
                                           where (isSuperAdmin || st.ApprovalLevel1 == approverId || st.ApprovalLevel2 == approverId)
                                           && ass.IsActive && sh.IsActive && st.IsActive == true
                                           select new AssignedShiftResponse
                                           {
                                               ShiftName = sh.Name,
                                               FromDate = ass.FromDate,
                                               ToDate = ass.ToDate,
                                               StaffName = $"{st.FirstName} {st.LastName}"
                                           })
                                           .ToListAsync();
            if (getAssignedShifts.Count == 0) throw new MessageNotFoundException("No assigned shifts found");
            return getAssignedShifts;
        }
    }
}