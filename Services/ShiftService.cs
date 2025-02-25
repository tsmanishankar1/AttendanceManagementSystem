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

        public async Task<IEnumerable<ShiftResponse>> GetAllShiftsAsync()
        {
            var allShift = await (from shift in _context.Shifts
                                  select new ShiftResponse
                                  {
                                      ShiftId = shift.Id,
                                      ShiftName = shift.ShiftName,
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

        public async Task<ShiftResponse> GetShiftByIdAsync(int shiftId)
        {
            var allShift = await (from shift in _context.Shifts
                                  where shift.Id == shiftId
                                  select new ShiftResponse
                                  {
                                      ShiftId = shift.Id,
                                      ShiftName = shift.ShiftName,
                                      ShortName = shift.ShortName,
                                      StartTime = shift.StartTime,
                                      EndTime = shift.EndTime,
                                      IsActive = shift.IsActive,
                                      CreatedBy = shift.CreatedBy
                                  })
                                  .FirstOrDefaultAsync();
            if (allShift == null)
            {
                throw new MessageNotFoundException("Shift not found");
            }
            return allShift;
        }

        public async Task<string> CreateShiftAsync(ShiftRequest newShift)
        {
            var message = "Shift added successfully";

            var shift = new Shift
            {
                ShiftName = newShift.ShiftName,
                ShortName = newShift.ShortName,
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
            if (existingShift == null)
                throw new MessageNotFoundException("Shift not found");

            existingShift.ShiftName = updatedShift.ShiftName;
            existingShift.ShortName = updatedShift.ShortName;
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
                if (shift != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == item.Id && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    if (staff != null)
                    {
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
                    }
                }
                await _context.SaveChangesAsync();
            }
            return message;
        }
    }
}
