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
        var staff = await _attendanceContext.StaffCreations
            .FirstOrDefaultAsync(s => s.Id == staffId);

        if (staff == null)
            throw new MessageNotFoundException("Staff not found for the given ID.");
        var assignedShift = await _attendanceContext.AssignShifts
            .Where(a => a.StaffId == staffId)
            .OrderByDescending(a => a.FromDate)
            .FirstOrDefaultAsync();
        var shift = assignedShift != null ? await _attendanceContext.Shifts.FirstOrDefaultAsync(s => s.ShiftTypeId == assignedShift.ShiftId) : null;

        string shiftName = shift?.ShiftName ?? "Not Assigned"; 
            var transactions = await _atrakContext.SmaxTransactions
            .Where(t => t.TrChId == staff.StaffId && t.TrDate.HasValue && t.TrDate.Value.Date == DateTime.Today)
            .OrderBy(t => t.TrTime)
            .ToListAsync();

        if (!transactions.Any())
            throw new MessageNotFoundException("No record found for today's date and given Staff ID.");

        var checkIn = transactions.FirstOrDefault();
        var checkOut = transactions.LastOrDefault();
        string? trIpAddressIn = checkIn?.TrIpaddress;
        string? trIpAddressOut = checkOut?.TrIpaddress;

        var readerIn = trIpAddressIn != null
            ? await _attendanceContext.ReaderConfigurations
                .FirstOrDefaultAsync(r => r.ReaderIpAddress == trIpAddressIn)
            : null;

        var readerOut = trIpAddressOut != null
            ? await _attendanceContext.ReaderConfigurations
                .FirstOrDefaultAsync(r => r.ReaderIpAddress == trIpAddressOut)
            : null;
        TimeSpan? duration = checkIn?.TrTime != null && checkOut?.TrTime != null
            ? checkOut.TrTime - checkIn.TrTime
            : null;

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
        var message = "";
        if (request.GraceTimeId == 1) message = "Grace time added successfully";
        else if (request.GraceTimeId == 2) message = "Extra break time added successfully";
        var graceTime = new AttendanceGraceTimeCalc
        {
            GraceTimeId = request.GraceTimeId,
            Value = request.Value,
            IsActive = request.IsActive,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _attendanceContext.AttendanceGraceTimeCalcs.Add(graceTime);
        await _attendanceContext.SaveChangesAsync();
        return message;
    }
    public async Task<List<AttendanceGraceTimeCalcResponse>> GetGraceTimeAndBreakTime()
    {
        var graceTime = await (from g in _attendanceContext.AttendanceGraceTimeCalcs
                               select new AttendanceGraceTimeCalcResponse
                               {
                                   Id = g.Id,
                                   GraceTimeId = g.GraceTimeId,
                                   Value = g.Value,
                                   IsActive = g.IsActive
                               }).ToListAsync();
        if (graceTime.Count == 0)
        {
            throw new MessageNotFoundException("No grace time and extra break time found");
        }
        return graceTime;
    }

    public async Task<string> UpdateGraceTimeAndBreakTime(UpdateAttendanceGraceTimeCalc request)
    {
        var message = "Grace time and extra break time updated successfully";
        if (request.GraceTimeId == 2) message = "Extra break time updated successfully";
        else if (request.GraceTimeId == 1) message = "Grace time updated successfully";
        var graceTime = await _attendanceContext.AttendanceGraceTimeCalcs.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsActive);
        if (graceTime == null)
        {
            throw new MessageNotFoundException("Grace time and extra break time not found");
        }
        graceTime.GraceTimeId = request.GraceTimeId;
        graceTime.Value = request.Value;
        graceTime.IsActive = request.IsActive;
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

    public async Task<List<AttendanceRecordDto>> GetAttendanceRecords(AttendanceStatusResponse attendanceStatus)
    {
        var result = await (from ar in _attendanceContext.AttendanceRecords
                            join staff in _attendanceContext.StaffCreations on ar.StaffId equals staff.Id
                            where (attendanceStatus.StaffId == ar.StaffId && attendanceStatus.DepartmentId == staff.DepartmentId
                            && attendanceStatus.DivisionId == staff.DivisionId || (attendanceStatus.FromDate >= ar.AttendanceDate
                            && attendanceStatus.ToDate <= ar.AttendanceDate) || (attendanceStatus.FromMonth >= ar.AttendanceDate.Month
                            && attendanceStatus.ToMonth <= ar.AttendanceDate.Month)) && staff.IsActive == true && !ar.IsDeleted && ar.IsFreezed == null
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
        if(result.Count == 0)
        {
            throw new MessageNotFoundException("No attendance record found");
        }
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

