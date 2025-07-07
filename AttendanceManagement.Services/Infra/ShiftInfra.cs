using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class ShiftInfra : IShiftInfra
    {
        private readonly AttendanceManagementSystemContext _context;

        public ShiftInfra(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<List<StaffsDto>> GetStaffByDivisionIdAsync(int divisionId)
        {
            var staffList = await _context.StaffCreations
                .Where(s => s.DivisionId == divisionId && s.IsActive == true)
                .Select(s => new StaffsDto
                {
                    StaffId=s.Id,
                    StaffCreationId = s.StaffId,
                    FullName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                    DepartmentId = s.DepartmentId,
                    DepartmentName = _context.DepartmentMasters
                                    .Where(d => d.Id == s.DepartmentId && d.IsActive)
                                    .Select(d => d.Name)
                                    .FirstOrDefault() ?? "Unknown",
                    DesignationId = s.DesignationId,
                    DesignationName = _context.DesignationMasters
                                    .Where(des => des.Id == s.DesignationId && des.IsActive)
                                    .Select(des => des.Name)
                                    .FirstOrDefault() ?? "Unknown"
                })
                .ToListAsync();
            return staffList;
        }

        public async Task<List<ShiftResponse>> GetAllShiftsAsync()
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
                                      ShiftTypeName = shiftType.Name, 
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
            var message = "Shift created successfully";
            var shiftType = await _context.ShiftTypeDropDowns.AnyAsync(p => p.Id == newShift.ShiftTypeId && p.IsActive);
            if (!shiftType) throw new MessageNotFoundException("Shift type not found");
            var duplicateShiftName = await _context.Shifts.AnyAsync(s => s.Name.ToLower() == newShift.ShiftName.ToLower());
            if (duplicateShiftName) throw new ConflictException("Shift name already exists");
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
            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateShiftAsync(UpdateShift updatedShift)
        {
            var message = "Shift updated successfully";
            var existingShift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == updatedShift.ShiftId);
            if (existingShift == null) throw new MessageNotFoundException("Shift not found");
            var shiftType = await _context.ShiftTypeDropDowns.AnyAsync(p => p.Id == updatedShift.ShiftTypeId && p.IsActive);
            if (!shiftType) throw new MessageNotFoundException("Shift type not found");
            var duplicateShiftName = await _context.Shifts.AnyAsync(s => s.Id != updatedShift.ShiftId && s.Name.ToLower() == updatedShift.ShiftName.ToLower());
            if (duplicateShiftName) throw new ConflictException("Shift name already exists");
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
            var shift = await _context.Shifts.AnyAsync(p => p.Id == regularShift.ShiftId && p.IsActive);
            if (!shift) throw new MessageNotFoundException("Shift not found");
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
            await _context.RegularShifts.AddRangeAsync(shifts);
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
                var staffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
                var fromDate = assignShift.FromDate;
                var toDate = assignShift.ToDate;
                for (var date = fromDate; date <= toDate; date = date.AddDays(1))
                {
                    await AttendanceFreezeDate(item.Id, date);
                    var existingAssignedShift = await _context.AssignShifts
                        .Where(a => a.FromDate == date &&
                                    a.StaffId == item.Id &&
                                    a.IsActive)
                        .ToListAsync();
                    if (existingAssignedShift.Count > 0)
                    {
                        throw new ConflictException($"Shift already assigned for staff {staffName}");
                    }
                    var shiftAssign = new AssignShift
                    {
                        FromDate = date,
                        ShiftId = assignShift.ShiftId,
                        StaffId = item.Id,
                        IsActive = true,
                        CreatedBy = assignShift.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.AssignShifts.AddAsync(shiftAssign);
                }
                await _context.SaveChangesAsync();
            }
            return message;
        }

        public async Task AttendanceFreezeDate(int staffId, DateOnly date)
        {
            var hasUnfreezed = await _context.AttendanceRecords.AnyAsync(f => f.IsFreezed == true && f.StaffId == staffId && f.AttendanceDate == date);
            if (hasUnfreezed) throw new InvalidOperationException("Shift cannot be assign attendance records are frozen");
        }

        public async Task<List<AssignedShiftResponse>> GetAllAssignedShifts(int approverId)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverId && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
            var getAssignedShifts = await (from ass in _context.AssignShifts
                                           join sh in _context.Shifts on ass.ShiftId equals sh.Id
                                           join st in _context.StaffCreations on ass.StaffId equals st.Id
                                           where (isSuperAdmin || st.ApprovalLevel1 == approverId || st.ApprovalLevel2 == approverId)
                                           && ass.IsActive && sh.IsActive && st.IsActive == true
                                           select new AssignedShiftResponse
                                           {
                                               ShiftName = sh.Name,
                                               Date = ass.FromDate,
                                               StaffName = $"{st.FirstName}{(string.IsNullOrWhiteSpace(st.LastName) ? "" : " " + st.LastName)}"
                                           })
                                           .ToListAsync();
            if (getAssignedShifts.Count == 0) throw new MessageNotFoundException("No assigned shifts found");
            return getAssignedShifts;
        }
    }
}