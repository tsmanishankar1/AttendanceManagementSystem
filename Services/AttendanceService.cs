using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanceManagement.AtrakModels;
using AttendanceManagement.Atrak_InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Input_Models;
using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
public class AttendanceService
{
    private readonly AtrakContext _atrakContext;
    private readonly AttendanceManagementSystemContext _attendanceContext;
    private readonly StoredProcedureDbContext _storedProcedureDbContext;

    public AttendanceService(AtrakContext atrakContext, AttendanceManagementSystemContext attendanceContext, StoredProcedureDbContext storedProcedureDbContext)
    {
        _atrakContext = atrakContext;
        _attendanceContext = attendanceContext;
        _storedProcedureDbContext = storedProcedureDbContext;

    }
    public async Task<SmaxTransactionResponse?> GetCheckInCheckOutAsync(int staffId)
    {
        var staff = await _attendanceContext.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId);
        if (staff == null) throw new MessageNotFoundException("Staff not found for the given ID.");
        var assignedShift = await _attendanceContext.AssignShifts.Where(a => a.StaffId == staffId).OrderByDescending(a => a.FromDate).FirstOrDefaultAsync();
        var shift = assignedShift != null ? await _attendanceContext.Shifts.FirstOrDefaultAsync(s => s.Id == assignedShift.ShiftId) : null;
        string shiftName = shift?.Name ?? "Not Assigned";
        TimeSpan? fromTime = TimeSpan.TryParse(shift?.StartTime, out var from) ? from : (TimeSpan?)null;
        TimeSpan? toTime = TimeSpan.TryParse(shift?.EndTime, out var to) ? to : (TimeSpan?)null;
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        List<SmaxTransaction> transactions;
        if (fromTime.HasValue && toTime.HasValue && fromTime > toTime)
        {
            var shiftStartDateTime = yesterday.Add(fromTime.Value);
            var shiftEndDateTime = today.Add(toTime.Value);
            var bufferEndTime = shiftEndDateTime.AddHours(1);
            transactions = await _atrakContext.SmaxTransactions
                .Where(t => t.TrChId == staff.StaffId
                    && t.TrDate.HasValue
                    && (t.TrDate.Value.Date == yesterday || t.TrDate.Value.Date == today))
                .OrderBy(t => t.TrDate)
                .ThenBy(t => t.TrTime)
                .ToListAsync();
            transactions = transactions.Where(t =>
            {
                var date = t.TrDate?.Date ?? DateTime.MinValue;
                var time = t.TrTime?.TimeOfDay ?? TimeSpan.Zero;
                var dateTime = date + time;

                return dateTime >= shiftStartDateTime && dateTime <= bufferEndTime;
            }).ToList();
        }
        else
        {
            transactions = await _atrakContext.SmaxTransactions
                .Where(t => t.TrChId == staff.StaffId
                    && t.TrDate.HasValue
                    && t.TrDate.Value.Date == today)
                .OrderBy(t => t.TrTime)
                .ToListAsync();
        }

        if (!transactions.Any()) throw new MessageNotFoundException("No record found for the given Staff ID and shift timings.");
        var checkIn = transactions.FirstOrDefault();
        var checkOut = transactions.LastOrDefault();
        string? trIpAddressIn = checkIn?.TrIpaddress;
        string? trIpAddressOut = checkOut?.TrIpaddress;
        var readerIn = trIpAddressIn != null ? await _attendanceContext.ReaderConfigurations.FirstOrDefaultAsync(r => r.ReaderIpAddress == trIpAddressIn) : null;
        var readerOut = trIpAddressOut != null ? await _attendanceContext.ReaderConfigurations.FirstOrDefaultAsync(r => r.ReaderIpAddress == trIpAddressOut) : null;
        TimeSpan? duration = checkIn?.TrTime != null && checkOut?.TrTime != null ? checkOut.TrTime - checkIn.TrTime : null;
        return new SmaxTransactionResponse
        {
            StaffCreationId = checkIn?.TrChId,
            StaffId = staff.Id,
            ShiftName = shiftName,
            ReaderNameIn = readerIn?.ReaderName,
            ReaderNameOut = readerOut?.ReaderName,
            Date = checkIn?.TrDate?.ToString("dd-MMM-yyyy"),
            CheckInTime = checkIn?.TrTime?.ToString("dd-MMM-yyyy HH:mm:ss"),
            CheckOutTime = checkOut?.TrTime?.ToString("dd-MMM-yyyy HH:mm:ss"),
            Duration = duration?.ToString(@"hh\:mm\:ss") ?? "-"
        };
    }

    public async Task<string> AddGraceTimeAndBreakTime(AttendanceGraceTimeCalcRequest request)
    {
        var typeName = await _attendanceContext.GraceTimeDropdowns
            .Where(w => w.Id == request.GraceTimeId && w.IsActive)
            .Select(w => w.Name)
            .FirstOrDefaultAsync();
        if (typeName == null) throw new MessageNotFoundException("Grace time type not found");
        var message = $"{typeName} added successfully";
        var existingActiveEntries = await _attendanceContext.AttendanceGraceTimeCalcs.Where(x => x.GraceTimeId == request.GraceTimeId && x.IsActive).ToListAsync();
        foreach (var entry in existingActiveEntries)
        {
            entry.IsActive = false;
            entry.UpdatedBy = request.CreatedBy;
            entry.UpdatedUtc = DateTime.UtcNow;
        }
        var graceTime = new AttendanceGraceTimeCalc
        {
            GraceTimeId = request.GraceTimeId,
            Value = request.Value,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _attendanceContext.AttendanceGraceTimeCalcs.AddAsync(graceTime);
        await _attendanceContext.SaveChangesAsync();
        return message;
    }

    public async Task<List<AttendanceGraceTimeCalcResponse>> GetGraceTimeAndBreakTime()
    {
        var graceTime = await (from g in _attendanceContext.AttendanceGraceTimeCalcs
                               where g.IsActive
                               select new AttendanceGraceTimeCalcResponse
                               {
                                   Id = g.Id,
                                   GraceTimeId = g.GraceTimeId,
                                   Value = g.Value
                               }).ToListAsync();
        if (graceTime.Count == 0) throw new MessageNotFoundException("No grace time types found");
        return graceTime;
    }

    public async Task<string> UpdateGraceTimeAndBreakTime(UpdateAttendanceGraceTimeCalc request)
    {
        var typeName = await _attendanceContext.GraceTimeDropdowns
            .Where(w => w.Id == request.GraceTimeId && w.IsActive)
            .Select(w => w.Name)
            .FirstOrDefaultAsync();
        if (typeName == null) throw new MessageNotFoundException("Grace time type not found");
        var message = $"{typeName} updated successfully";
        var graceTime = await _attendanceContext.AttendanceGraceTimeCalcs.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsActive);
        if (graceTime == null) throw new MessageNotFoundException($"Grace time not found");
        graceTime.GraceTimeId = request.GraceTimeId;
        graceTime.Value = request.Value;
        graceTime.UpdatedBy = request.UpdatedBy;
        graceTime.UpdatedUtc = DateTime.UtcNow;
        await _attendanceContext.SaveChangesAsync();
        return message;
    }

    public async Task<List<AttendanceRecordDto>> AttendanceRecords()
    {
        var attendanceList = await _storedProcedureDbContext.attendanceRecordDtos
            .FromSqlRaw("EXEC AttendanceRecords")
            .ToListAsync();
        if (attendanceList.Count == 0)
        {
            throw new MessageNotFoundException("No attendance records found");
        }
        return attendanceList;
    }

    public async Task<List<StaffInfoDto>> GetAllStaffsByDepartmentAndDivision(GetStaffByDepartmentDivision staff)
    {
        var staffInfo = await (from staffs in _attendanceContext.StaffCreations
                               join dept in _attendanceContext.DepartmentMasters on staffs.DepartmentId equals dept.Id
                               join div in _attendanceContext.DivisionMasters on staffs.DivisionId equals div.Id
                               where staffs.IsActive == true && dept.IsActive && div.IsActive
                                     && ((!staff.DepartmentId.HasValue || staffs.DepartmentId == staff.DepartmentId)) && ((!staff.DivisionId.HasValue || staffs.DivisionId == staff.DivisionId))
                               select new StaffInfoDto
                               {
                                   StaffId = staffs.Id,
                                   StaffName = $"{staffs.FirstName} {staffs.LastName}",
                                   DepartmentName = dept.Name
                               })
                               .ToListAsync();

        if (staffInfo.Count == 0) throw new MessageNotFoundException("No staffs found");
        return staffInfo;
    }

    public async Task<List<AttendanceRecordDto>> GetAttendanceRecords(AttendanceStatusResponse attendanceStatus)
    {
        var result = await (
                            from ar in _attendanceContext.AttendanceRecords
                            join staff in _attendanceContext.StaffCreations on ar.StaffId equals staff.Id
                            where staff.IsActive == true
                                  && !ar.IsDeleted
                                  && ar.IsFreezed == null
                                  && ar.StaffId == attendanceStatus.StaffId
                                  && staff.DepartmentId == attendanceStatus.DepartmentId
                                  && staff.DivisionId == attendanceStatus.DivisionId
                                  && (
                                        (attendanceStatus.FromDate.HasValue && attendanceStatus.ToDate.HasValue &&
                                         ar.AttendanceDate >= attendanceStatus.FromDate.Value &&
                                         ar.AttendanceDate <= attendanceStatus.ToDate.Value)
                                        ||
                                        (attendanceStatus.FromMonth.HasValue && attendanceStatus.ToMonth.HasValue &&
                                         ar.AttendanceDate.Month >= attendanceStatus.FromMonth.Value &&
                                         ar.AttendanceDate.Month <= attendanceStatus.ToMonth.Value)
                                     )
                            select new AttendanceRecordDto
                            {
                                Id = ar.Id,
                                StaffId = ar.StaffId,
                                StaffCreationId = staff.StaffId,
                                StaffName = $"{staff.FirstName}{staff.LastName}",
                                FirstIn = ar.FirstIn,
                                LastOut = ar.LastOut,
                                ShiftId = ar.ShiftId,
                                IsEarlyComing = ar.IsEarlyComing,
                                IsLateComing = ar.IsLateComing,
                                IsEarlyGoing = ar.IsEarlyGoing,
                                IsLateGoing = ar.IsLateGoing,
                                BreakHours = ar.BreakHour,
                                ExtraBreakHours = ar.ExtraBreakHours,
                                IsBreakHoursExceed = ar.IsBreakHoursExceed,
                                StatusId = ar.StatusId,
                                IsHolidayWorkingEligible = ar.IsHolidayWorkingEligible,
                                Norm = ar.Norm,
                                CompletedFileCouunt = ar.CompletedFileCount,
                                TotalFte = ar.TotalFte,
                                IsFteAchieved = ar.IsFteAchieved,
                                AttendanceDate = ar.AttendanceDate
                            })
                            .ToListAsync();
        if(result.Count == 0) throw new MessageNotFoundException("No attendance record found");
        return result;
    }

    public async Task<string> FreezeAttendanceRecords(AttendanceFreezeRequest attendanceFreezeRequest)
    {
        var message = "Selected attendance records freezed successfully";
        var selectedRows = attendanceFreezeRequest.SelectedRows;
        foreach(var item in selectedRows)
        {
            var existingAttendanceRecord = await _attendanceContext.AttendanceRecords.FirstOrDefaultAsync(a => a.Id == item.Id && !a.IsDeleted);
            if(existingAttendanceRecord != null)
            {
                existingAttendanceRecord.IsFreezed = attendanceFreezeRequest.IsFreezed;
                existingAttendanceRecord.FreezedBy = attendanceFreezeRequest.FreezedBy;
                existingAttendanceRecord.FreezedOn = DateTime.UtcNow;
            }
            await _attendanceContext.SaveChangesAsync();
        }
        return message;
    }
}