using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security;

namespace AttendanceManagement.Services;

public class ApplicationService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly StoredProcedureDbContext _storedProcedureDbContext;

    public ApplicationService(AttendanceManagementSystemContext context, StoredProcedureDbContext storedProcedureDbContext)
    {
        _context = context;
        _storedProcedureDbContext = storedProcedureDbContext;
    }
    public async Task<bool> CancelAppliedLeave(int applicationTypeId, int id, int updatedBy)
    {
        object? entity = null;
        try
        {
            switch (applicationTypeId)
            {
                case 1:
                    entity = await _context.LeaveRequisitions.FirstOrDefaultAsync(l => l.Id == id && l.IsActive);
                    break;
                case 2:
                    entity = await _context.CommonPermissions.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
                    break;
                case 3:
                    entity = await _context.ManualPunchRequistions.FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
                    break;
                case 4:
                    entity = await _context.OnDutyRequisitions.FirstOrDefaultAsync(o => o.Id == id && o.IsActive);
                    break;
                case 5:
                    entity = await _context.BusinessTravels.FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
                    break;
                case 6:
                    entity = await _context.WorkFromHomes.FirstOrDefaultAsync(w => w.Id == id && w.IsActive);
                    break;
                case 7:
                    entity = await _context.ShiftChanges.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
                    break;
                case 8:
                    entity = await _context.ShiftExtensions.FirstOrDefaultAsync(se => se.Id == id && se.IsActive);
                    break;
                case 9:
                    entity = await _context.WeeklyOffHolidayWorkings.FirstOrDefaultAsync(wh => wh.Id == id && wh.IsActive);
                    break;
                case 10:
                    entity = await _context.CompOffAvails.FirstOrDefaultAsync(ca => ca.Id == id && ca.IsActive);
                    break;
                case 11:
                    entity = await _context.CompOffCredits.FirstOrDefaultAsync(cc => cc.Id == id && cc.IsActive);
                    break;
                default:
                    return false;
            }

            if (entity == null)
            {
                Console.WriteLine($"Active entity not found for ApplicationTypeId: {applicationTypeId}, Id: {id}");
                return false;
            }

            var entityType = entity.GetType();
            var isCancelledProperty = entityType.GetProperty("IsCancelled");
            var updatedUtcProperty = entityType.GetProperty("UpdatedUtc");
            var updatedByProperty = entityType.GetProperty("UpdatedBy");
            var isActiveProperty = entityType.GetProperty("IsActive");

            if (isCancelledProperty == null)
            {
                Console.WriteLine($"IsCancelled property not found in entity type: {entityType.Name}");
                return false;
            }

            bool isAlreadyCancelled = (bool)(isCancelledProperty.GetValue(entity) ?? false);
            if (isAlreadyCancelled)
            {
                Console.WriteLine($"Application already cancelled for Id: {id}");
                return false;
            }
            isCancelledProperty.SetValue(entity, true);
            updatedUtcProperty?.SetValue(entity, DateTime.UtcNow);
            updatedByProperty?.SetValue(entity, updatedBy);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CancelAppliedLeave: {ex.Message}");
            return false;
        }
    }
    public async Task<IEnumerable<object>> GetApplicationDetails(int staffId, int applicationTypeId)
    {
        var application = applicationTypeId switch
        {
            1 => await _context.LeaveRequisitions
                        .Where(lr => lr.IsCancelled == null)
         .Join(_context.ApplicationTypes, lr => lr.ApplicationTypeId, at => at.Id,
               (lr, at) => new { lr, at })
         .Join(_context.LeaveTypes, temp => temp.lr.LeaveTypeId, lt => lt.Id,
               (temp, lt) => new LeaveReq
               {
                   ApplicationTypeId = temp.lr.ApplicationTypeId,
                   ApplicationTypeName = temp.at.ApplicationTypeName,
                   Status = temp.lr.Status1.HasValue ? (temp.lr.Status1.Value ? "Approved" : "Rejected") : "Pending",
                   StartDuration = temp.lr.StartDuration,
                   EndDuration = temp.lr.EndDuration,
                   LeaveTypeName = lt.Name,
                   FromDate = temp.lr.FromDate,
                   ToDate = temp.lr.ToDate,
                   TotalDays = temp.lr.TotalDays,
                   Reason = temp.lr.Reason
               })
         .ToListAsync(),

            2 => await _context.CommonPermissions
                        .Where(lr => lr.IsCancelled == null)
             .Join(_context.ApplicationTypes, cp => cp.ApplicationTypeId, at => at.Id,
                   (cp, at) => new PermissionDto
                   {
                       ApplicationTypeId = cp.ApplicationTypeId,
                       ApplicationTypeName = at.ApplicationTypeName,
                       Status = cp.Status.HasValue ? (cp.Status.Value ? "Approved" : "Rejected") : "Pending",
                       PermissionDate = cp.PermissionDate,
                       TotalHours = cp.TotalHours,
                       StartTime = cp.StartTime,
                       EndTime = cp.EndTime,
                       PermissionType = cp.PermissionType,
                       Remarks = cp.Remarks
                   })
             .ToListAsync(),

            3 => await _context.ManualPunchRequistions
                   .Where(lr => lr.IsCancelled == null)
            .Select(mp => new ManualPunch
            {
                ApplicationTypeId = mp.ApplicationTypeId,
                ApplicationTypeName = mp.ApplicationType.ApplicationTypeName,
                Status = mp.Status1.HasValue ? (mp.Status1.Value ? "Approved" : "Rejected") : "Pending",
                SelectPunch = mp.SelectPunch,
                InPunch = mp.InPunch,
                OutPunch = mp.OutPunch,
                Remarks = mp.Remarks
            })
            .ToListAsync(),

            4 => await _context.OnDutyRequisitions
                    .Where(lr => lr.IsCancelled == null)
             .Select(od => new OnDutyRequest
             {
                 ApplicationTypeId = od.ApplicationTypeId,
                 ApplicationTypeName = od.ApplicationType.ApplicationTypeName,
                 Status = od.Status1.HasValue ? (od.Status1.Value ? "Approved" : "Rejected") : "Pending",
                 StartDuration = od.StartDuration,
                 EndDuration = od.EndDuration,
                 StartDate = od.StartDate,
                 EndDate = od.EndDate,
                 StartTime = od.StartTime,
                 EndTime = od.EndTime,
                 Reason = od.Reason
             })
             .ToListAsync(),

            5 => await _context.BusinessTravels
                       .Where(lr => lr.IsCancelled == null)
                .Select(bt => new Business
                {
                    ApplicationTypeId = bt.ApplicationTypeId,
                    ApplicationTypeName = bt.ApplicationType.ApplicationTypeName,
                    Status = bt.Status1.HasValue ? (bt.Status1.Value ? "Approved" : "Rejected") : "Pending",
                    StartDuration = bt.StartDuration,
                    EndDuration = bt.EndDuration,
                    FromTime = bt.FromTime,
                    ToTime = bt.ToTime,
                    FromDate = bt.FromDate,
                    ToDate = bt.ToDate,
                    Reason = bt.Reason
                })
                .ToListAsync(),
            6 => await _context.WorkFromHomes
                       .Where(lr => lr.IsCancelled == null)
                 .Select(wfh => new WorkFrom
                 {
                     ApplicationTypeId = wfh.ApplicationTypeId,
                     ApplicationTypeName = wfh.ApplicationType.ApplicationTypeName,
                     Status = wfh.Status1.HasValue ? (wfh.Status1.Value ? "Approved" : "Rejected") : "Pending",
                     StartDuration = wfh.StartDuration,
                     EndDuration = wfh.EndDuration,
                     FromTime = wfh.FromTime,
                     ToTime = wfh.ToTime,
                     FromDate = wfh.FromDate,
                     ToDate = wfh.ToDate,
                     Reason = wfh.Reason
                 })
             .ToListAsync(),

            7 => await _context.ShiftChanges
                      .Where(lr => lr.IsCancelled == null)
              .Join(_context.ApplicationTypes, sc => sc.ApplicationTypeId, at => at.Id,
                    (sc, at) => new { sc, at })
              .Join(_context.Shifts, temp => temp.sc.ShiftId, s => s.Id,
                    (temp, s) => new ShiftChan
                    {
                        ApplicationTypeId = temp.sc.ApplicationTypeId,
                        ApplicationTypeName = temp.at.ApplicationTypeName,
                        Status = temp.sc.Status1.HasValue ? (temp.sc.Status1.Value ? "Approved" : "Rejected") : "Pending",
                        ShiftName = s.ShiftName,
                        FromDate = temp.sc.FromDate,
                        ToDate = temp.sc.ToDate,
                        Reason = temp.sc.Reason
                    })
              .ToListAsync(),

            8 => await _context.ShiftExtensions
                .Where(lr => lr.IsCancelled == null)
              .Select(se => new ShiftExte
              {
                  ApplicationTypeId = se.ApplicationTypeId,
                  ApplicationTypeName = se.ApplicationType.ApplicationTypeName,
                  Status = se.Status1.HasValue ? (se.Status1.Value ? "Approved" : "Rejected") : "Pending",
                  TransactionDate = se.TransactionDate,
                  DurationHours = se.DurationHours,
                  BeforeShiftHours = se.BeforeShiftHours,
                  AfterShiftHours = se.AfterShiftHours,
                  Remarks = se.Remarks
              })
              .ToListAsync(),

            9 => await _context.WeeklyOffHolidayWorkings
                   .Where(lr => lr.IsCancelled == null)
              .Join(_context.ApplicationTypes,
                    wh => wh.ApplicationTypeId, at => at.Id, (wh, at) => new { wh, at })
              .Join(_context.Shifts,
                    temp => temp.wh.ShiftId, s => s.Id, (temp, s) => new WeeklyOffHoliday
                    {
                        ApplicationTypeId = temp.wh.ApplicationTypeId,
                        ApplicationTypeName = temp.at.ApplicationTypeName,
                        Status = temp.wh.Status1.HasValue ? (temp.wh.Status1.Value ? "Approved" : "Rejected") : "Pending",
                        SelectShiftType = temp.wh.SelectShiftType,
                        TxnDate = temp.wh.TxnDate,
                        ShiftName = s.ShiftName,
                        ShiftInTime = temp.wh.ShiftInTime,
                        ShiftOutTime = temp.wh.ShiftOutTime
                    })
              .ToListAsync(),

            10 => await _context.CompOffAvails
                       .Where(lr => lr.IsCancelled == null)
                 .Select(coa => new CompOffAvai
                 {
                     ApplicationTypeId = coa.ApplicationTypeId,
                     ApplicationTypeName = coa.ApplicationType.ApplicationTypeName,
                     Status = coa.Status.HasValue ? (coa.Status.Value ? "Approved" : "Rejected") : "Pending",
                     WorkedDate = coa.WorkedDate,
                     FromDate = coa.FromDate,
                     ToDate = coa.ToDate,
                     FromDuration = coa.FromDuration,
                     ToDuration = coa.ToDuration,
                     Reason = coa.Reason,
                     TotalDays = coa.TotalDays
                 })
                 .ToListAsync(),

            11 => await _context.CompOffCredits
                       .Where(lr => lr.IsCancelled == null)
                 .Select(coc => new CompOffCred
                 {
                     ApplicationTypeId = coc.ApplicationTypeId,
                     ApplicationTypeName = coc.ApplicationType.ApplicationTypeName,
                     Status = coc.Status.HasValue ? (coc.Status.Value ? "Approved" : "Rejected") : "Pending",
                     WorkedDate = coc.WorkedDate,
                     TotalDays = coc.TotalDays,
                     Reason = coc.Reason
                 })
                 .ToListAsync(),
            _ => Enumerable.Empty<object>()
        };
        return application;
    }
    public async Task<object> GetMonthlyDetailsAsync(string staffId, int month, int year)
    {
        if (!int.TryParse(staffId, out int parsedStaffId))
        {
            throw new ArgumentException("Invalid StaffId format.");
        }
        var staff = await (from staffs in _context.StaffCreations
                           join org in _context.OrganizationTypes on staffs.OrganizationTypeId equals org.Id
                           where staffs.Id == parsedStaffId
                           select new
                           {
                               staffs.Id,
                               FullName = $"{staffs.FirstName} {staffs.LastName}",
                               OrgShortName = org.ShortName
                           })
                      .FirstOrDefaultAsync();

        if (staff == null)
        {
            throw new ArgumentException("Staff not found.");
        }
        var leaveRecords = await _context.LeaveRequisitions
            .Where(lr => lr.Id == parsedStaffId &&
                         lr.CreatedUtc.Month == month &&
                         lr.CreatedUtc.Year == year)
            .Select(lr => new
            {
                lr.FromDate,
                lr.ToDate,
                LeaveTypeName = _context.LeaveTypes
            .Where(lt => lt.Id == lr.LeaveTypeId)
            .Select(lt => lt.Name)
            .FirstOrDefault(),
                lr.Reason
            })
            .ToListAsync();

        var workFromHomeRecords = await _context.WorkFromHomes
            .Where(wfh => wfh.CreatedBy == parsedStaffId &&
                   wfh.CreatedUtc.Month == month &&
                   wfh.CreatedUtc.Year == year &&
                   wfh.ApplicationTypeId != 0)
            .Select(wfh => new
            {
                ApplicationTypeName = _context.ApplicationTypes
             .Where(lt => lt.Id == wfh.ApplicationTypeId)
             .Select(lt => lt.ApplicationTypeName)
             .FirstOrDefault(),
                wfh.FromDate,
                wfh.ToDate,
                wfh.Reason
            })
            .ToListAsync();

        var onDutyRecords = await _context.OnDutyRequisitions
            .Where(od => od.CreatedBy == parsedStaffId &&
                         od.CreatedUtc.Month == month &&
                         od.CreatedUtc.Year == year &&
                         od.ApplicationTypeId != 0)
            .Select(od => new
            {
                ApplicationTypeName = _context.ApplicationTypes
                    .Where(lt => lt.Id == od.ApplicationTypeId)
                    .Select(lt => lt.ApplicationTypeName)
                    .FirstOrDefault(),
                od.StartDate,
                od.EndDate,
                od.Reason
            })
            .ToListAsync();

        var businessTravelRecords = await _context.BusinessTravels
            .Where(bt => bt.CreatedBy == parsedStaffId &&
                         bt.CreatedUtc.Month == month &&
                         bt.CreatedUtc.Year == year &&
                         bt.ApplicationTypeId != 0)
            .Select(bt => new
            {
                ApplicationTypeName = _context.ApplicationTypes
                    .Where(lt => lt.Id == bt.ApplicationTypeId)
                    .Select(lt => lt.ApplicationTypeName)
                    .FirstOrDefault(),
                bt.FromDate,
                bt.ToDate,
                bt.Reason
            })
            .ToListAsync();

        var compOffRecords = await _context.CompOffAvails
            .Where(bt => bt.CreatedBy == parsedStaffId &&
                         bt.CreatedUtc.Month == month &&
                         bt.CreatedUtc.Year == year &&
                         bt.ApplicationTypeId != 0)
            .Select(bt => new
            {
                ApplicationTypeName = _context.ApplicationTypes
                    .Where(lt => lt.Id == bt.ApplicationTypeId)
                    .Select(lt => lt.ApplicationTypeName)
                    .FirstOrDefault(),
                bt.FromDate,
                bt.ToDate
            })
            .ToListAsync();

        var weeklyOffRecords = await _context.WeeklyOffHolidayWorkings
            .Where(bt => bt.CreatedBy == parsedStaffId &&
                         bt.CreatedUtc.Month == month &&
                         bt.CreatedUtc.Year == year &&
                         bt.ApplicationTypeId != 0)
            .Select(bt => new
            {
                ApplicationTypeName = _context.ApplicationTypes
                    .Where(lt => lt.Id == bt.ApplicationTypeId)
                    .Select(lt => lt.ApplicationTypeName)
                    .FirstOrDefault(),
                bt.ShiftInTime,
                bt.ShiftOutTime
            })
            .ToListAsync();

        var holidayRecords = await _context.HolidayMasters
            .Where(hm => hm.CreatedUtc.Month == month &&
                         hm.CreatedUtc.Year == year)
            .Select(hm => new
            {
                hm.CreatedUtc,
                hm.HolidayName
            })
            .ToListAsync();

        return new
        {
            staff.Id,
            staff.FullName,
            StaffCreationId = $"{staff.OrgShortName}{staff.Id}",
            Month = new DateTime(year, month, 1).ToString("MMMM"),
            Year = year,
            LeaveRequisition = leaveRecords,
            WorkFromHome = workFromHomeRecords,
            OnDuty = onDutyRecords,
            BusinessTravel = businessTravelRecords,
            CompOff = compOffRecords,
            WeeklyOffHolidayWorking = weeklyOffRecords,
            Holidays = holidayRecords
        };
    }
    public async Task<IEnumerable<CompOffCreditResponseDto>> GetCompOffCreditAllAsync()
    {
        var compOffCredits = await _context.CompOffCredits.ToListAsync();

        return compOffCredits.Select(c => new CompOffCreditResponseDto
        {
            Id = c.Id,
            WorkedDate = c.WorkedDate,
            TotalDays = c.TotalDays,
            Reason = c.Reason,
        }).ToList();
    }
    public async Task<string> CreateAsync(CompOffCreditDto compOffCreditDto)
    {
        var message = "CompOff Credit Successfully";
        var lastBalance = await _context.CompOffCredits
            .Where(c => c.CreatedBy == compOffCreditDto.CreatedBy)
            .OrderByDescending(c => c.Id) 
            .Select(c => c.Balance)
            .FirstOrDefaultAsync(); 
        var compOffCredit = new CompOffCredit
        {
            WorkedDate = compOffCreditDto.WorkedDate,
            StaffId = compOffCreditDto.StaffId,
            TotalDays = compOffCreditDto.TotalDays,
            ApplicationTypeId = compOffCreditDto.ApplicationTypeId,
            Reason = compOffCreditDto.Reason,
            IsActive = true,
            CreatedBy = compOffCreditDto.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            Balance = lastBalance 
        };
        _context.CompOffCredits.Add(compOffCredit);
        await _context.SaveChangesAsync();
        return message;
    }
    public async Task<string> CreateAsync(CompOffAvailRequest request)
    {
        var message = "CompOff Avail Successfully";

        var isHolidayWorkingExists = await _context.CompOffCredits
            .AnyAsync(h => h.WorkedDate == request.WorkedDate
            && h.CreatedBy == request.CreatedBy
            && h.Status == true);

        if (!isHolidayWorkingExists)
        {
            throw new Exception("WorkedDate does not match the date in CompOffCredit or the record is not active.");
        }

        var lastCompOffCredit = await _context.CompOffCredits
            .Where(c => c.CreatedBy == request.CreatedBy)
            .OrderByDescending(c => c.CreatedUtc) 
            .FirstOrDefaultAsync();

        if (lastCompOffCredit == null)
        {
            throw new Exception("No previous Comp-Off Credit record found for this user.");
        }

        if (lastCompOffCredit.Balance == 0)
        {
            throw new Exception("Insufficient balance. Cannot create Comp-Off Avail.");
        }

        var compOff = new CompOffAvail
        {
            WorkedDate = request.WorkedDate,
            StaffId = request.StaffId,
            ApplicationTypeId = request.ApplicationTypeId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            FromDuration = request.FromDuration,
            ToDuration = request.ToDuration,
            Reason = request.Reason,
            TotalDays = request.TotalDays,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            IsActive = true,
        };

        _context.CompOffAvails.Add(compOff);
        await _context.SaveChangesAsync();

        return message;
    }
    public async Task<List<CompOffAvailDto>> GetAllAsync()
    {
        var compOffs = await (from compOff in _context.CompOffAvails
                              select new CompOffAvailDto
                              {
                                  Id = compOff.Id,
                                  staffId = compOff.StaffId,
                                  ApplicationTypeId = compOff.ApplicationTypeId,
                                  WorkedDate = compOff.WorkedDate,
                                  FromDate = compOff.FromDate,
                                  ToDate = compOff.ToDate,
                                  FromDuration = compOff.FromDuration,
                                  ToDuration = compOff.ToDuration,
                                  Reason = compOff.Reason,
                                  TotalDays = compOff.TotalDays,
                              }).ToListAsync();

        if (compOffs.Count == 0)
        {
            throw new MessageNotFoundException("No CompOff records found");
        }
        return compOffs;
    }
    public async Task<IEnumerable<ApplicationTypeDto>> GetAllApplicationTypesAsync()
    {
        var application = await (from applicationType in _context.ApplicationTypes
                                 select new ApplicationTypeDto
                                 {
                                     ApplicationTypeId = applicationType.Id,
                                     ApplicationTypeName = applicationType.ApplicationTypeName
                                 })
                                 .ToListAsync();
        if (application.Count == 0)
        {
            throw new MessageNotFoundException("No application type found");
        }
        return application;
    }
    public async Task<List<object>> GetApplicationRequisition(int approverId, List<int>? staffIds, int? applicationTypeId, DateOnly? fromDate, DateOnly? toDate)
    {
        List<object> result = new List<object>();

        if (applicationTypeId.HasValue && applicationTypeId == 1)
        {
            var getLeaves = await (from staff in _context.StaffCreations
                                   join leave in _context.LeaveRequisitions on staff.Id equals leave.CreatedBy
                                   join leaveType in _context.LeaveTypes on leave.LeaveTypeId equals leaveType.Id
                                   join application in _context.ApplicationTypes on leave.ApplicationTypeId equals application.Id
                                   join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                   where leave.IsActive == true
                                   && staff.IsActive == true
                                   && leave.IsCancelled == null
                                   && (fromDate.HasValue ? leave.FromDate >= fromDate : true)
                                   && (toDate.HasValue ? leave.ToDate <= toDate : true)
                                   && (approverId < 0 || (staff.ApprovalLevel1 == approverId && leave.Status1 == null))
                                   && (staffIds != null && staffIds.Any() ? staffIds.Contains(leave.CreatedBy) : true)
                                   select new
                                   {
                                       leave.Id,
                                       leave.ApplicationTypeId,
                                       ApplicationType = application.ApplicationTypeName,
                                       StaffCreationId = $"{org.ShortName}{staff.Id}",
                                       StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                       leave.StartDuration,
                                       leave.EndDuration,
                                       LeaveType = leaveType.Name,
                                       leave.FromDate,
                                       leave.ToDate,
                                       leave.TotalDays,
                                       leave.Reason,
                                       leave.CreatedBy
                                   }).ToListAsync();

            if (getLeaves.Count == 0)
            {
                throw new MessageNotFoundException("No leave requisition found");
            }
            result.AddRange(getLeaves.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 2)
        {
            var getCommonPermissions = await (from permission in _context.CommonPermissions
                                              join staff in _context.StaffCreations on permission.StaffId equals staff.Id
                                              join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                              where permission.IsActive == true && staff.IsActive == true
                                               && permission.IsCancelled== null
                                              && (staffIds != null && staffIds.Any() ? staffIds.Contains(permission.CreatedBy) : true)
                                              && (approverId < 0 || (staff.ApprovalLevel1 == approverId && permission.Status == null))
                                              && (fromDate.HasValue ? permission.PermissionDate >= fromDate : true)
                                              && (toDate.HasValue ? permission.PermissionDate <= toDate : true)
                                              select new
                                              {
                                                  permission.Id,
                                                  permission.ApplicationTypeId,
                                                  permission.PermissionType,
                                                  StaffCreationId = $"{org.ShortName}{staff.Id}",
                                                  StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                                  permission.PermissionDate,
                                                  permission.StartTime,
                                                  permission.EndTime,
                                                  permission.TotalHours,
                                                  permission.Remarks,
                                                  permission.Status,
                                                  permission.CreatedBy
                                              }).ToListAsync();
            if (getCommonPermissions.Count == 0)
            {
                throw new MessageNotFoundException("No common permission found");
            }
            result.AddRange(getCommonPermissions.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 3)
        {
            var getManualPunch = await (from punch in _context.ManualPunchRequistions
                                        join staff in _context.StaffCreations on punch.CreatedBy equals staff.Id
                                        join application in _context.ApplicationTypes on punch.ApplicationTypeId equals application.Id
                                        join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                        where punch.IsActive == true && staff.IsActive == true
                                        && punch.IsCancelled == null
                                        && (approverId < 0 || (staff.ApprovalLevel1 == approverId && punch.Status1 == null))
                                        && (staffIds != null && staffIds.Any() ? staffIds.Contains(punch.CreatedBy) : true)
                                        select new
                                        {
                                            punch.ApplicationTypeId,
                                            punch.Id,
                                            punch = application.ApplicationTypeName,
                                            punch.SelectPunch,
                                            punch.InPunch,
                                            punch.OutPunch,
                                            punch.Remarks,
                                            punch.Status1,
                                            punch.Status2,
                                            punch.IsActive,
                                            punch.CreatedBy
                                        }).ToListAsync();
            if (getManualPunch.Count == 0)
            {
                throw new MessageNotFoundException("No manual punch requisition found");
            }
            result.AddRange(getManualPunch.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 4)
        {
            var getOnDutyRequisitions = await (from duty in _context.OnDutyRequisitions
                                               join staff in _context.StaffCreations on duty.CreatedBy equals staff.Id
                                               join application in _context.ApplicationTypes on duty.ApplicationTypeId equals application.Id
                                               join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                               where duty.IsActive == true && staff.IsActive == true
                                               && duty.IsCancelled== null
                                               && (staffIds != null && staffIds.Any() ? staffIds.Contains(duty.CreatedBy) : true)
                                               && (fromDate.HasValue ? duty.StartDate >= fromDate : true)
                                               && (toDate.HasValue ? duty.EndDate <= toDate : true)
                                               && (approverId < 0 || (staff.ApprovalLevel1 == approverId && duty.Status1 == null))
                                               select new
                                               {
                                                   duty.Id,
                                                   duty.ApplicationTypeId,
                                                   ApplicationType = application.ApplicationTypeName,
                                                   StaffCreationId = $"{org.ShortName}{staff.Id}",
                                                   StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                                   duty.StartDuration,
                                                   duty.EndDuration,
                                                   duty.StartDate,
                                                   duty.EndDate,
                                                   duty.StartTime,
                                                   duty.EndTime,
                                                   duty.Reason,
                                                   duty.Status1,
                                                   duty.Status2,
                                                   duty.CreatedBy
                                               }).ToListAsync();
            if (getOnDutyRequisitions.Count == 0)
            {
                throw new MessageNotFoundException("No on-duty requisition found");
            }
            result.AddRange(getOnDutyRequisitions.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 5)
        {
            var getBusinessTravels = await (from travel in _context.BusinessTravels
                                            join staff in _context.StaffCreations on travel.CreatedBy equals staff.Id
                                            join application in _context.ApplicationTypes on travel.ApplicationTypeId equals application.Id
                                            join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                            where travel.IsActive == true && staff.IsActive == true
                                            && travel.IsCancelled == null
                                            && (staffIds != null && staffIds.Any() ? staffIds.Contains(travel.CreatedBy) : true)
                                            && (approverId < 0 || (staff.ApprovalLevel1 == approverId && travel.Status1 == null))
                                            && (fromDate.HasValue ? travel.FromDate >= fromDate : true)
                                            && (toDate.HasValue ? travel.ToDate <= toDate : true)
                                            select new
                                            {
                                                travel.Id,
                                                travel.ApplicationTypeId,
                                                ApplicationType = application.ApplicationTypeName,
                                                StaffCreationId = $"{org.ShortName}{staff.Id}",
                                                StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                                travel.StartDuration,
                                                travel.EndDuration,
                                                travel.FromDate,
                                                travel.ToDate,
                                                travel.FromTime,
                                                travel.ToTime,
                                                travel.Reason,
                                                travel.Status1,
                                                travel.Status2,
                                                travel.CreatedBy
                                            }).ToListAsync();
            if (getBusinessTravels.Count == 0)
            {
                throw new MessageNotFoundException("No business travel found");
            }
            result.AddRange(getBusinessTravels.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 6)
        {
            var getWorkFromHomes = await (from workFromHome in _context.WorkFromHomes
                                          join staff in _context.StaffCreations on workFromHome.CreatedBy equals staff.Id
                                          join application in _context.ApplicationTypes on workFromHome.ApplicationTypeId equals application.Id
                                          join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                          where workFromHome.IsActive == true && staff.IsActive == true
                                          && workFromHome.IsCancelled == null
                                          && (staffIds != null && staffIds.Any() ? staffIds.Contains(workFromHome.CreatedBy) : true)
                                          && (approverId < 0 || (staff.ApprovalLevel1 == approverId && workFromHome.Status1 == null))
                                          && (fromDate.HasValue ? workFromHome.FromDate >= fromDate : true)
                                          && (toDate.HasValue ? workFromHome.ToDate <= toDate : true)
                                          select new
                                          {
                                              workFromHome.Id,
                                              workFromHome.ApplicationTypeId,
                                              ApplicationType = application.ApplicationTypeName,
                                              StaffCreationId = $"{org.ShortName}{staff.Id}",
                                              StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                              workFromHome.StartDuration,
                                              workFromHome.EndDuration,
                                              workFromHome.FromDate,
                                              workFromHome.ToDate,
                                              workFromHome.FromTime,
                                              workFromHome.ToTime,
                                              workFromHome.Reason,
                                              workFromHome.Status1,
                                              workFromHome.Status2,
                                              workFromHome.CreatedBy
                                          }).ToListAsync();
            if (getWorkFromHomes.Count == 0)
            {
                throw new MessageNotFoundException("No work from home found");
            }
            result.AddRange(getWorkFromHomes.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 7)
        {
            var getShiftChanges = await (from shiftChange in _context.ShiftChanges
                                         join staff in _context.StaffCreations on shiftChange.CreatedBy equals staff.Id
                                         join application in _context.ApplicationTypes on shiftChange.ApplicationTypeId equals application.Id
                                         join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                         join shift in _context.Shifts on shiftChange.ShiftId equals shift.Id
                                         where shiftChange.IsActive == true && staff.IsActive == true
                                         && shiftChange.IsCancelled == null
                                         && (staffIds != null && staffIds.Any() ? staffIds.Contains(shiftChange.CreatedBy) : true)
                                         && (approverId < 0 || (staff.ApprovalLevel1 == approverId && shiftChange.Status1 == null))
                                         && (fromDate.HasValue ? shiftChange.FromDate >= fromDate : true)
                                         && (toDate.HasValue ? shiftChange.ToDate <= toDate : true)
                                         select new
                                         {
                                             shiftChange.Id,
                                             shiftChange.ApplicationTypeId,
                                             ApplicationType = application.ApplicationTypeName,
                                             StaffCreationId = $"{org.ShortName}{staff.Id}",
                                             StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                             shiftChange.FromDate,
                                             shiftChange.ToDate,
                                             shiftChange.Reason,
                                             shiftChange.Status1,
                                             shiftChange.Status2,
                                             shiftChange.CreatedBy,
                                             ShiftName = shift.ShiftName
                                         }).ToListAsync();
            if (getShiftChanges.Count == 0)
            {
                throw new MessageNotFoundException("No shift change found");
            }
            result.AddRange(getShiftChanges.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 8)
        {
            var getShiftExtensions = await (from shiftExtension in _context.ShiftExtensions
                                            join staff in _context.StaffCreations on shiftExtension.CreatedBy equals staff.Id
                                            join application in _context.ApplicationTypes on shiftExtension.ApplicationTypeId equals application.Id
                                            join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                            where shiftExtension.IsActive == true && staff.IsActive == true
                                            && shiftExtension.IsCancelled == null
                                            && (staffIds != null && staffIds.Any() ? staffIds.Contains(shiftExtension.CreatedBy) : true)
                                            && (approverId < 0 || (staff.ApprovalLevel1 == approverId && shiftExtension.Status1 == null))
                                            && (fromDate.HasValue ? shiftExtension.TransactionDate >= fromDate : true)
                                            && (toDate.HasValue ? shiftExtension.TransactionDate <= toDate : true)
                                            select new
                                            {
                                                shiftExtension.Id,
                                                shiftExtension.ApplicationTypeId,
                                                ApplicationType = application.ApplicationTypeName,
                                                StaffCreationId = $"{org.ShortName}{staff.Id}",
                                                StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                                shiftExtension.TransactionDate,
                                                shiftExtension.DurationHours,
                                                shiftExtension.BeforeShiftHours,
                                                shiftExtension.AfterShiftHours,
                                                shiftExtension.Remarks,
                                                shiftExtension.Status1,
                                                shiftExtension.Status2,
                                                shiftExtension.CreatedBy
                                            }).ToListAsync();
            if (getShiftExtensions.Count == 0)
            {
                throw new MessageNotFoundException("No shift extension found");
            }
            result.AddRange(getShiftExtensions.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 9)
        {
            var getWeeklyOffHolidayWorking = await (from holidayWorking in _context.WeeklyOffHolidayWorkings
                                                    join staff in _context.StaffCreations on holidayWorking.CreatedBy equals staff.Id
                                                    join application in _context.ApplicationTypes on holidayWorking.ApplicationTypeId equals application.Id
                                                    join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                                    where holidayWorking.IsActive == true && staff.IsActive == true
                                                    && holidayWorking.IsCancelled == null
                                                    && (staffIds != null && staffIds.Any() ? staffIds.Contains(holidayWorking.CreatedBy) : true)
                                                    && (approverId < 0 || (staff.ApprovalLevel1 == approverId && holidayWorking.Status1 == null))
                                                    && (fromDate.HasValue ? holidayWorking.TxnDate >= fromDate : true)
                                                    && (toDate.HasValue ? holidayWorking.TxnDate <= toDate : true)
                                                    select new
                                                    {
                                                        holidayWorking.Id,
                                                        holidayWorking.ApplicationTypeId,
                                                        ApplicationType = application.ApplicationTypeName,
                                                        StaffCreationId = $"{org.ShortName}{staff.Id}",
                                                        StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                                        holidayWorking.TxnDate,
                                                        holidayWorking.SelectShiftType,
                                                        holidayWorking.ShiftId,
                                                        holidayWorking.ShiftInTime,
                                                        holidayWorking.ShiftOutTime,
                                                        holidayWorking.Status1,
                                                        holidayWorking.Status2,
                                                        holidayWorking.CreatedBy
                                                    }).ToListAsync();
            if (getWeeklyOffHolidayWorking.Count == 0)
            {
                throw new MessageNotFoundException("No weekly off holiday working found");
            }
            result.AddRange(getWeeklyOffHolidayWorking.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 10)
        {
            var getCompOffAvail = await (from compOff in _context.CompOffAvails
                                         join staff in _context.StaffCreations on compOff.CreatedBy equals staff.Id
                                         join application in _context.ApplicationTypes on compOff.ApplicationTypeId equals application.Id
                                         join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                         where compOff.IsActive == true && staff.IsActive == true
                                         && compOff.IsCancelled == null
                                         && (staffIds != null && staffIds.Any() ? staffIds.Contains(compOff.CreatedBy) : true)
                                         && (approverId < 0 || (staff.ApprovalLevel1 == approverId && compOff.Status == null))
                                         && (fromDate.HasValue ? compOff.FromDate >= fromDate : true)
                                         && (toDate.HasValue ? compOff.ToDate <= toDate : true)
                                         select new
                                         {
                                             compOff.Id,
                                             compOff.ApplicationTypeId,
                                             ApplicationType = application.ApplicationTypeName,
                                             StaffCreationId = $"{org.ShortName}{staff.Id}",
                                             StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                             compOff.WorkedDate,
                                             compOff.FromDate,
                                             compOff.ToDate,
                                             compOff.FromDuration,
                                             compOff.ToDuration,
                                             compOff.Reason,
                                             compOff.TotalDays,
                                             compOff.Status,
                                             compOff.CreatedBy
                                         })
                                         .ToListAsync();
            if (getCompOffAvail.Count == 0)
            {
                throw new MessageNotFoundException("No Comp off Avail found");
            }
            result.AddRange(getCompOffAvail.Cast<object>());
        }
        else if (applicationTypeId.HasValue && applicationTypeId == 11)
        {
            var getCompOffCredit = await (from compOff in _context.CompOffCredits
                                          join staff in _context.StaffCreations on compOff.CreatedBy equals staff.Id
                                          join application in _context.ApplicationTypes on compOff.ApplicationTypeId equals application.Id
                                          join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                                          where compOff.IsActive == true && staff.IsActive == true
                                          && compOff.IsCancelled == null
                                          && (staffIds != null && staffIds.Any() ? staffIds.Contains(compOff.CreatedBy) : true)
                                          && (approverId < 0 || (staff.ApprovalLevel1 == approverId && compOff.Status == null))
                                          select new
                                          {
                                              compOff.Id,
                                              compOff.ApplicationTypeId,
                                              ApplicationType = application.ApplicationTypeName,
                                              StaffCreationId = $"{org.ShortName}{staff.Id}",
                                              StaffName = staff.FirstName + " " + (staff.LastName ?? ""),
                                              compOff.WorkedDate,
                                              compOff.TotalDays,
                                              compOff.Reason,
                                              compOff.Status,
                                              compOff.CreatedBy
                                          }).ToListAsync();
            if (getCompOffCredit.Count == 0)
            {
                throw new MessageNotFoundException("No Comp off Credit found");
            }
            result.AddRange(getCompOffCredit.Cast<object>());
        }
        return result;
    }
    public async Task<string> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest)
    {
        var message = "";
        var selectedRows = approveLeaveRequest.SelectedRows;
        var staff = _context.StaffCreations.Where(s => s.Id == approveLeaveRequest.ApprovedBy && s.IsActive == true).Select(s => $"{s.FirstName}{s.LastName}").FirstOrDefault();
        string approvedDateTime = DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        foreach (var item in selectedRows)
        {
            if (approveLeaveRequest.ApplicationTypeId == 1)
            {
                if (approveLeaveRequest.IsApproved)
                {
                    var leave = await _context.LeaveRequisitions.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                    if (leave != null)
                    {
                        var staffName = _context.StaffCreations
                        .Where(s => s.Id == leave.CreatedBy && s.IsActive == true)
                        .Select(s => $"{s.FirstName} {s.LastName}")
                        .FirstOrDefault();

                        if (leave.Status1 == null)
                        {
                            if (leave.ApplicationTypeId == 1)
                            {
                                var individualLeave = await _context.IndividualLeaveCreditDebits
                                    .Where(l => l.StaffCreationId == leave.CreatedBy
                                                && l.LeaveTypeId == leave.LeaveTypeId
                                                && l.IsActive == true)
                                    .OrderByDescending(l => l.Id)
                                    .FirstOrDefaultAsync();

                                if (individualLeave != null && individualLeave.AvailableBalance > 0 && individualLeave.AvailableBalance >= leave.TotalDays)
                                {
                                    individualLeave.AvailableBalance = decimal.Subtract(individualLeave.AvailableBalance, leave.TotalDays);
                                    individualLeave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                    individualLeave.UpdatedUtc = DateTime.UtcNow;
                                }
                                else
                                {
                                    throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
                                }
                            }
                            leave.Status1 = true;
                            leave.IsActive = false;
                            leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            leave.UpdatedUtc = DateTime.UtcNow;
                        }
                        await _context.SaveChangesAsync();

                        var notification = new ApprovalNotification
                        {
                            StaffId = leave.CreatedBy,
                            Message = $"Your leave request has been approved. Approved by - {staff} on {approvedDateTime}",
                            IsActive = true,
                            CreatedBy = approveLeaveRequest.ApprovedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        _context.ApprovalNotifications.Add(notification);
                        await _context.SaveChangesAsync();
                        leave.ApprovalNotificationId = notification.Id;
                        await _context.SaveChangesAsync();
                    }
                    message = "Leave request approved successfully";
                }
                else if (!approveLeaveRequest.IsApproved)
                {
                    var leave = await _context.LeaveRequisitions.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                    if (leave != null)
                    {
                        if (leave.Status1 == null)
                        {
                            leave.Status1 = false;
                            leave.IsActive = false;
                            leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            leave.UpdatedUtc = DateTime.UtcNow;
                        }
                        await _context.SaveChangesAsync();

                        var notification = new ApprovalNotification
                        {
                            StaffId = leave.CreatedBy,
                            Message = $"Your leave request has been rejected. Rejected by - {staff} on {approvedDateTime}",
                            IsActive = true,
                            CreatedBy = approveLeaveRequest.ApprovedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        _context.ApprovalNotifications.Add(notification);
                        await _context.SaveChangesAsync();
                        leave.ApprovalNotificationId = notification.Id;
                        await _context.SaveChangesAsync();
                    }
                    message = "Leave request rejected successfully";
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 2)
            {
                var leaveRequest = await _context.CommonPermissions
                    .Where(l => l.Id == item.Id && l.IsActive == true)
                    .FirstOrDefaultAsync();

                if (leaveRequest != null && leaveRequest.Status == null)
                {
                    leaveRequest.Status = approveLeaveRequest.IsApproved ? true : false;
                    leaveRequest.IsActive = false;
                    leaveRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    leaveRequest.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved
                        ? "Common Permission request approved successfully"
                        : "Common Permission request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your common permission request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your common permission request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = leaveRequest.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    leaveRequest.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }

            else if (approveLeaveRequest.ApplicationTypeId == 3)
            {
                var manualPunch = await _context.ManualPunchRequistions
                    .Where(m => m.Id == item.Id && m.IsActive == true)
                    .FirstOrDefaultAsync();

                if (manualPunch != null && manualPunch.Status1 == null)
                {
                    manualPunch.Status1 = approveLeaveRequest.IsApproved;
                    manualPunch.IsActive = false;
                    manualPunch.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    manualPunch.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved
                        ? "Manual punch request approved successfully"
                        : "Manual punch request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your manual punch request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your manual punch request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = manualPunch.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    manualPunch.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 4)
            {
                var onDuty = await _context.OnDutyRequisitions
                    .Where(o => o.Id == item.Id && o.IsActive == true)
                    .FirstOrDefaultAsync();

                if (onDuty != null && onDuty.Status1 == null)
                {
                    onDuty.Status1 = approveLeaveRequest.IsApproved;
                    onDuty.IsActive = false;
                    onDuty.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    onDuty.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved ? "On-duty request approved successfully" : "On-duty request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your on-duty request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your on-duty request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = onDuty.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    onDuty.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 5)
            {
                var businessTravel = await _context.BusinessTravels
                    .Where(b => b.Id == item.Id && b.IsActive)
                    .FirstOrDefaultAsync();

                if (businessTravel != null && businessTravel.Status1 == null)
                {
                    businessTravel.Status1 = approveLeaveRequest.IsApproved;
                    businessTravel.IsActive = false;
                    businessTravel.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    businessTravel.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved ? "Business travel request approved successfully" : "Business travel request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your business travel request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your business travel request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = businessTravel.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    businessTravel.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 6)
            {
                var workFromHome = await _context.WorkFromHomes
                    .Where(w => w.Id == item.Id && w.IsActive)
                    .FirstOrDefaultAsync();

                if (workFromHome != null && workFromHome.Status1 == null)
                {
                    workFromHome.Status1 = approveLeaveRequest.IsApproved;
                    workFromHome.IsActive = false;
                    workFromHome.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    workFromHome.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved ? "Work from home request approved successfully" : "Work from home request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your work from home request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your work from home request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = workFromHome.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    workFromHome.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 7)
            {
                var shiftChange = await _context.ShiftChanges
                    .Where(s => s.Id == item.Id && s.IsActive)
                    .FirstOrDefaultAsync();

                if (shiftChange != null && shiftChange.Status1 == null)
                {
                    shiftChange.Status1 = approveLeaveRequest.IsApproved;
                    shiftChange.IsActive = false;
                    shiftChange.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    shiftChange.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved ? "Shift change request approved successfully" : "Shift change request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your shift change request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your shift change request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = shiftChange.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    shiftChange.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 8)
            {
                var shiftExtension = await _context.ShiftExtensions
                    .Where(s => s.Id == item.Id && s.IsActive)
                    .FirstOrDefaultAsync();

                if (shiftExtension != null && shiftExtension.Status1 == null)
                {
                    shiftExtension.Status1 = approveLeaveRequest.IsApproved;
                    shiftExtension.IsActive = false;
                    shiftExtension.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    shiftExtension.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved ? "Shift extension request approved successfully" : "Shift extension request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your shift extension request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your shift extension request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = shiftExtension.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    shiftExtension.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 9)
            {
                var weeklyOffHoliday = await _context.WeeklyOffHolidayWorkings
                    .Where(w => w.Id == item.Id && w.IsActive)
                    .FirstOrDefaultAsync();

                if (weeklyOffHoliday != null && weeklyOffHoliday.Status1 == null)
                {
                    weeklyOffHoliday.Status1 = approveLeaveRequest.IsApproved;
                    weeklyOffHoliday.IsActive = false;
                    weeklyOffHoliday.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    weeklyOffHoliday.UpdatedUtc = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved ? "Weekly off/holiday working request approved successfully" : "Weekly off/holiday working request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your weekly off/holiday working request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your weekly off/holiday working request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = weeklyOffHoliday.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    weeklyOffHoliday.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 10)
            {
                var compOffAvail = await _context.CompOffAvails
                    .Where(c => c.Id == item.Id && c.IsActive)
                    .FirstOrDefaultAsync();

                if (compOffAvail != null && compOffAvail.Status == null)
                {
                    compOffAvail.Status = approveLeaveRequest.IsApproved;
                    compOffAvail.IsActive = false;
                    compOffAvail.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    compOffAvail.UpdatedUtc = DateTime.UtcNow;

                    if (approveLeaveRequest.IsApproved)
                    {
                        var lastCompOffCredit = await _context.CompOffCredits
                            .Where(c => c.CreatedBy == compOffAvail.CreatedBy)
                            .OrderByDescending(c => c.CreatedUtc) 
                            .FirstOrDefaultAsync();

                        if (lastCompOffCredit != null && lastCompOffCredit.Balance > 0)
                        {
                            lastCompOffCredit.Balance -= 1; 
                            lastCompOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            lastCompOffCredit.UpdatedUtc = DateTime.UtcNow;
                        }
                    }

                    await _context.SaveChangesAsync();

                    message = approveLeaveRequest.IsApproved
                        ? "Comp-Off Avail request approved successfully"
                        : "Comp-Off Avail request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your Comp-Off avail request has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your Comp-Off avail request has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = compOffAvail.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    compOffAvail.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }

            else if (approveLeaveRequest.ApplicationTypeId == 11)
            {
                var compOffCredit = await _context.CompOffCredits
                    .Where(c => c.Id == item.Id && c.Status == null)
                    .FirstOrDefaultAsync();

                if (compOffCredit != null)
                {
                    compOffCredit.Status = approveLeaveRequest.IsApproved;
                    compOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                    compOffCredit.UpdatedUtc = DateTime.UtcNow;

                    if (approveLeaveRequest.IsApproved)
                    {
                        compOffCredit.Balance = (compOffCredit.Balance) + 1;
                    }
                    await _context.SaveChangesAsync();
                    message = approveLeaveRequest.IsApproved
                        ? "Comp-Off Credit approved successfully"
                        : "Comp-Off Credit rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your Comp-Off credit has been approved. Approved by - {staff} on {approvedDateTime}"
                        : $"Your Comp-Off credit has been rejected. Rejected by - {staff} on {approvedDateTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = compOffCredit.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();
                    compOffCredit.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                }
            }
        }

        return message;
    }
    public async Task<List<ApprovalNotificationResponse>> GetApprovalNotifications(int staffId)
    {
        var profile = await _context.StaffCreations.Where(s => s.Id == staffId && s.IsActive == true).Select(s => s.ProfilePhoto).FirstOrDefaultAsync();
        var notifications = await (from notification in _context.ApprovalNotifications
                                   where notification.StaffId == staffId && notification.IsActive
                                   select new ApprovalNotificationResponse
                                   {
                                       Id = notification.Id,
                                       StaffId = notification.StaffId,
                                       ProfilePhoto = profile,
                                       Message = notification.Message,
                                       CreatedBy = notification.CreatedBy
                                   }).ToListAsync();
        if (notifications.Count == 0)
        {
            throw new MessageNotFoundException("No notifications found");
        }
        return notifications;
    }

    public async Task<string> UpdateApprovalNotifications(int staffId, int notificationId)
    {
        var message = "";
        var notification = await _context.ApprovalNotifications
            .Where(n => n.StaffId == staffId && n.Id == notificationId && n.IsActive)
            .FirstOrDefaultAsync();
        if (notification != null)
        {
            notification.UpdatedBy = staffId;
            notification.UpdatedUtc = DateTime.UtcNow;
            notification.IsActive = false;
            await _context.SaveChangesAsync();
            message = "Notification updated successfully";
        }
        return message;
    }

    public async Task<string> CreateLeaveRequisitionAsync(LeaveRequisitionRequest leaveRequisitionRequest)
    {
        var message = "Leave requisition added successfully.";
        LeaveRequisition leaveRequisition = new LeaveRequisition();
        leaveRequisition.StartDuration = leaveRequisitionRequest.StartDuration;
        leaveRequisition.StaffId = leaveRequisitionRequest.StaffId;
        leaveRequisition.EndDuration = leaveRequisitionRequest.EndDuration;
        leaveRequisition.FromDate = leaveRequisitionRequest.FromDate;
        leaveRequisition.ToDate = leaveRequisitionRequest.ToDate;
        leaveRequisition.LeaveTypeId = leaveRequisitionRequest.LeaveTypeId;
        leaveRequisition.Reason = leaveRequisitionRequest.Reason;
        leaveRequisition.ApplicationTypeId = leaveRequisitionRequest.ApplicationTypeId;
        leaveRequisition.TotalDays = leaveRequisitionRequest.TotalDays;
        leaveRequisition.CreatedBy = leaveRequisitionRequest.CreatedBy;
        leaveRequisition.CreatedUtc = DateTime.UtcNow;
        leaveRequisition.IsActive = true;
        _context.LeaveRequisitions.Add(leaveRequisition);
        await _context.SaveChangesAsync();
        return message;
    }
    public async Task<string> AddCommonPermissionAsync(CommonPermissionRequest commonPermissionRequest)
    {
        try
        {
            var message = "Common permission added successfully.";
            CommonPermission commonPermission = new CommonPermission();
            var permissionDate = commonPermissionRequest.PermissionDate;
            var dayOfWeek = permissionDate.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday)
            {
                throw new InvalidOperationException("Permission is not allowed on Saturdays.");
            }
            var startOfMonth = new DateOnly(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month, 1);
            var endOfMonth = new DateOnly(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month,
                DateTime.DaysInMonth(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month));
            var monthName = commonPermissionRequest.PermissionDate.ToString("MMMM");
            var existingPermissionOnDate = await _context.CommonPermissions
                .AnyAsync(p => p.StaffId == commonPermissionRequest.CreatedBy &&
                               p.PermissionDate == commonPermissionRequest.PermissionDate);
            if (existingPermissionOnDate)
            {
                throw new InvalidOperationException($"Permission for the date {commonPermissionRequest.PermissionDate:yyyy-MM-dd} already exists.");
            }
            var permissionsThisMonth = await _context.CommonPermissions
                .Where(p => p.StaffId == commonPermissionRequest.CreatedBy &&
                            p.PermissionDate >= startOfMonth &&
                            p.PermissionDate <= endOfMonth)
                .ToListAsync();
            if (permissionsThisMonth.Count >= 2)
            {
                throw new InvalidOperationException($"You cannot apply for permission more than twice in {monthName}.");
            }
            var duration = commonPermissionRequest.EndTime - commonPermissionRequest.StartTime;
            if (duration.TotalMinutes <= 0)
                throw new InvalidOperationException("End time must be greater than start time.");
            if (duration.TotalMinutes > 120)
            {
                throw new InvalidOperationException("Permission duration cannot exceed 2 hours.");
            }
            var totalMinutesThisMonth = permissionsThisMonth.Sum(p => TimeSpan.Parse(p.TotalHours).TotalMinutes);
            if (totalMinutesThisMonth + duration.TotalMinutes > 120)
            {
                throw new InvalidOperationException($"Cumulative permission time for {monthName} cannot exceed 2 hours.");
            }
            var formattedDuration = $"{duration.Hours:D2}:{duration.Minutes:D2}";
            commonPermission.StaffId = commonPermissionRequest.CreatedBy;
            commonPermission.StaffId = commonPermissionRequest.StaffId;
            commonPermission.ApplicationTypeId = commonPermissionRequest.ApplicationTypeId;
            commonPermission.PermissionType = commonPermissionRequest.PermissionType;
            commonPermission.StartTime = commonPermissionRequest.StartTime;
            commonPermission.EndTime = commonPermissionRequest.EndTime;
            commonPermission.PermissionDate = commonPermissionRequest.PermissionDate;
            commonPermission.TotalHours = formattedDuration;
            commonPermission.Remarks = commonPermissionRequest.Remarks;
            commonPermission.CreatedBy = commonPermissionRequest.CreatedBy;
            commonPermission.CreatedUtc = DateTime.UtcNow;
            commonPermission.IsActive = true;
            _context.CommonPermissions.Add(commonPermission);
            await _context.SaveChangesAsync();
            return message;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}", ex);
        }
    }

    public async Task<List<StaffPermissionResponse>> GetStaffCommonPermission(int staffId)
    {
        var permission = await (from staffPermission in _context.CommonPermissions
                                where staffPermission.StaffId == staffId && staffPermission.IsActive == true
                                select new StaffPermissionResponse
                                {
                                    Id = staffPermission.Id,
                                    StartTime = staffPermission.StartTime,
                                    EndTime = staffPermission.EndTime,
                                    TotalHours = staffPermission.TotalHours,
                                    PermissionDate = staffPermission.PermissionDate,
                                    PermissionType = staffPermission.PermissionType,
                                    Status = staffPermission.Status,
                                    Remarks = staffPermission.Remarks,
                                    StaffId = staffPermission.StaffId,
                                    ApplicationTypeId = staffPermission.ApplicationTypeId,
                                    IsActive = staffPermission.IsActive,
                                    CreatedBy = staffPermission.CreatedBy
                                })
                                .ToListAsync();
        if (permission.Count == 0) throw new MessageNotFoundException("No staff permissions found");
        return permission;
    }
    public async Task<List<CommonPermissionResponse>> GetStaffPermissions(GetCommonStaff getStaff)
    {
        var parameters = new[]
        {
            new SqlParameter("@CompanyName", string.IsNullOrWhiteSpace(getStaff.CompanyName) ? (object)DBNull.Value : getStaff.CompanyName),
            new SqlParameter("@CategoryName", string.IsNullOrWhiteSpace(getStaff.CategoryName) ? (object)DBNull.Value : getStaff.CategoryName),
            new SqlParameter("@CostCentreName", string.IsNullOrWhiteSpace(getStaff.CostCentreName) ? (object)DBNull.Value : getStaff.CostCentreName),
            new SqlParameter("@OrganizationTypeName", string.IsNullOrWhiteSpace(getStaff.OrganizationTypeName) ? (object)DBNull.Value : getStaff.OrganizationTypeName),
            new SqlParameter("@BranchName", string.IsNullOrWhiteSpace(getStaff.BranchName) ? (object)DBNull.Value : getStaff.BranchName),
            new SqlParameter("@DepartmentName", string.IsNullOrWhiteSpace(getStaff.DepartmentName) ? (object)DBNull.Value : getStaff.DepartmentName),
            new SqlParameter("@DesignationName", string.IsNullOrWhiteSpace(getStaff.DesignationName) ? (object)DBNull.Value : getStaff.DesignationName),
            new SqlParameter("@StaffName", string.IsNullOrWhiteSpace(getStaff.StaffName) ? (object)DBNull.Value : getStaff.StaffName),
            new SqlParameter("@LocationName", string.IsNullOrWhiteSpace(getStaff.LocationName) ? (object)DBNull.Value : getStaff.LocationName),
            new SqlParameter("@GradeName", string.IsNullOrWhiteSpace(getStaff.GradeName) ? (object)DBNull.Value : getStaff.GradeName),
        };

        var permissionList = await _storedProcedureDbContext.CommonPermissionResponses
            .FromSqlRaw("EXEC GetStaffPermissions @CompanyName, @CategoryName, @CostCentreName, @OrganizationTypeName, @BranchName, @DepartmentName, @DesignationName, @StaffName, @LocationName, @GradeName", parameters)
            .ToListAsync();
        if(permissionList.Count == 0) throw new MessageNotFoundException("No staff permissions found");
        return permissionList;
    }

    public async Task<List<object>> GetLeaveDetailsWithDefaultsAsync(int StaffCreationId)
    {
        var allLeaveTypes = await _context.LeaveTypes
            .Where(lt => lt.IsActive)
            .Select(lt => new { lt.Id, lt.Name })
            .ToListAsync();
        var userLeaveRecords = await _context.IndividualLeaveCreditDebits
            .Where(l => l.StaffCreationId == StaffCreationId && l.IsActive)
            .GroupBy(l => l.LeaveTypeId)
            .Select(g => new
            {
                LeaveTypeId = g.Key,
                ActualBalance = g.OrderByDescending(l => l.UpdatedUtc).First().ActualBalance,
                AvailableBalance = g.OrderByDescending(l => l.UpdatedUtc).First().AvailableBalance
            })
            .ToListAsync();

        var leaveDetails = allLeaveTypes.Select(lt =>
        {
            var record = userLeaveRecords.FirstOrDefault(r => r.LeaveTypeId == lt.Id);
            return new
            {
                LeaveTypeId = lt.Id,
                LeaveTypeName = lt.Name,
                ActualBalance = record?.ActualBalance ?? 0,
                AvailableBalance = record?.AvailableBalance ?? 0
            };
        }).ToList();
        if (leaveDetails.Count == 0)
        {
            throw new MessageNotFoundException("No leave details found for the given user.");
        }

        return leaveDetails.Cast<object>().ToList();
    }
    public async Task<ManualPunchRequistion> CreateManualPunchAsync(ManualPunchRequestDto request)
    {
        var manualPunch = new ManualPunchRequistion
        {
            ApplicationTypeId = request.ApplicationTypeId,
            SelectPunch = request.SelectPunch,
            InPunch = request.InPunch,
            StaffId = request.StaffId,
            OutPunch = request.OutPunch,
            Remarks = request.Remarks,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };

        _context.ManualPunchRequistions.Add(manualPunch);
        await _context.SaveChangesAsync();
        return manualPunch;
    }
    public async Task<OnDutyRequisition> CreateOnDutyRequisitionAsync(OnDutyRequisitionRequest request)
    {
        var onDutyRequisition = new OnDutyRequisition
        {
            ApplicationTypeId = request.ApplicationTypeId,
            StartDuration = request.StartDuration,
            EndDuration = request.EndDuration,
            StartDate = request.StartDate,
            StaffId = request.StaffId,
            EndDate = request.EndDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Reason = request.Reason,
            TotalHours = request.TotalHours,
            TotalDays = request.TotalDays,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.OnDutyRequisitions.Add(onDutyRequisition);
        await _context.SaveChangesAsync();
        return onDutyRequisition;
    }
    public async Task<BusinessTravel> CreateBusinessTravelAsync(BusinessTravelRequestDto request)
    {
        var businessTravel = new BusinessTravel
        {
            ApplicationTypeId = request.ApplicationTypeId,
            StartDuration = request.StartDuration,
            EndDuration = request.EndDuration,
            FromTime = request.FromTime,
            StaffId = request.StaffId,
            ToTime = request.ToTime,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Reason = request.Reason,
            TotalHours = request.TotalHours,
            TotalDays = request.TotalDays,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.BusinessTravels.Add(businessTravel);
        await _context.SaveChangesAsync();
        return businessTravel;
    }
    public async Task<WorkFromHome> CreateWorkFromHomeAsync(WorkFromHomeDto request)
    {
        var workFromHome = new WorkFromHome
        {
            ApplicationTypeId = request.ApplicationTypeId,
            StaffId = request.StaffId,
            StartDuration = request.StartDuration,
            EndDuration = request.EndDuration,
            FromTime = request.FromTime,
            ToTime = request.ToTime,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Reason = request.Reason,
            TotalHours = request.TotalHours,
            TotalDays = request.TotalDays,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.WorkFromHomes.Add(workFromHome);
        await _context.SaveChangesAsync();
        return workFromHome;
    }
    public List<object> GetShiftsByStaffAndDateRange(int staffId, DateOnly fromDate, DateOnly toDate)
    {
        var shifts = _context.AssignShifts
            .Include(a => a.Shift)
            .Where(a => a.StaffId == staffId &&
                        a.FromDate >= fromDate &&
                        a.ToDate <= toDate &&
                        a.IsActive)
            .Select(a => new
            {
                a.FromDate,
                a.ToDate,
                ShiftName = a.Shift.ShiftName,
                StartTime = a.Shift.StartTime,
                EndTime = a.Shift.EndTime
            })
            .ToList<object>();
        if (shifts.Count == 0) throw new MessageNotFoundException("Shifts not found between the date range for the staff");
        return shifts;
    }
    public async Task<ShiftChange> CreateShiftChangeAsync(ShiftChangeDto request)
    {
        var shiftChange = new ShiftChange
        {
            ApplicationTypeId = request.ApplicationTypeId,
            ShiftId = request.ShiftId,
            StaffId = request.StaffId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Reason = request.Reason,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.ShiftChanges.Add(shiftChange);
        await _context.SaveChangesAsync();
        return shiftChange;
    }
    public async Task<ShiftExtension> CreateShiftExtensionAsync(ShiftExtensionDto request)
    {
        var shiftExtension = new ShiftExtension
        {
            ApplicationTypeId = request.ApplicationTypeId,
            StaffId = request.StaffId,
            TransactionDate = request.TransactionDate,
            DurationHours = request.DurationHours,
            BeforeShiftHours = request.BeforeShiftHours,
            AfterShiftHours = request.AfterShiftHours,
            Remarks = request.Remarks,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.ShiftExtensions.Add(shiftExtension);
        await _context.SaveChangesAsync();
        return shiftExtension;
    }
    public async Task<WeeklyOffHolidayWorking> CreateWeeklyOffHolidayWorkingAsync(WeeklyOffHolidayWorkingDto request)
    {
        var weeklyOffHolidayWorking = new WeeklyOffHolidayWorking
        {
            ApplicationTypeId = request.ApplicationTypeId,
            SelectShiftType = request.SelectShiftType,
            StaffId = request.StaffId,
            TxnDate = request.TxnDate,
            ShiftId = request.ShiftId,
            ShiftInTime = request.ShiftInTime,
            ShiftOutTime = request.ShiftOutTime,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.WeeklyOffHolidayWorkings.Add(weeklyOffHolidayWorking);
        await _context.SaveChangesAsync();
        return weeklyOffHolidayWorking;
    }
}