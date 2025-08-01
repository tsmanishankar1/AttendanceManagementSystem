using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AttendanceManagement.Infrastructure.Infra;

public class ApplicationInfra : IApplicationInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IEmailInfra _emailService;
    private readonly StoredProcedureDbContext _storedProcedureDbContext;

    public ApplicationInfra(AttendanceManagementSystemContext context, IEmailInfra emailService, StoredProcedureDbContext storedProcedureDbContext)
    {
        _context = context;
        _emailService = emailService;
        _storedProcedureDbContext = storedProcedureDbContext;
    }
    public async Task<bool> CancelAppliedLeave(CancelAppliedLeave cancel)
    {
        await NotFoundMethod(cancel.ApplicationTypeId);
        object? entity = null;
        switch (cancel.ApplicationTypeId)
        {
            case 1:
                entity = await _context.LeaveRequisitions.FirstOrDefaultAsync(l => l.Id == cancel.Id && l.IsActive);
                break;
            case 2:
                entity = await _context.CommonPermissions.FirstOrDefaultAsync(p => p.Id == cancel.Id && p.IsActive);
                break;
            case 3:
                entity = await _context.ManualPunchRequistions.FirstOrDefaultAsync(m => m.Id == cancel.Id && m.IsActive);
                break;
            case 4:
                entity = await _context.OnDutyRequisitions.FirstOrDefaultAsync(o => o.Id == cancel.Id && o.IsActive);
                break;
            case 5:
                entity = await _context.BusinessTravels.FirstOrDefaultAsync(b => b.Id == cancel.Id && b.IsActive);
                break;
            case 6:
                entity = await _context.WorkFromHomes.FirstOrDefaultAsync(w => w.Id == cancel.Id && w.IsActive);
                break;
            case 7:
                entity = await _context.ShiftChanges.AsNoTracking().FirstOrDefaultAsync(s => s.Id == cancel.Id && s.IsActive);
                break;
            case 8:
                entity = await _context.ShiftExtensions.FirstOrDefaultAsync(se => se.Id == cancel.Id && se.IsActive);
                break;
            case 9:
                entity = await _context.WeeklyOffHolidayWorkings.FirstOrDefaultAsync(wh => wh.Id == cancel.Id && wh.IsActive);
                break;
            case 10:
                entity = await _context.CompOffAvails.FirstOrDefaultAsync(ca => ca.Id == cancel.Id && ca.IsActive);
                break;
            case 11:
                entity = await _context.CompOffCredits.FirstOrDefaultAsync(cc => cc.Id == cancel.Id && cc.IsActive);
                break;
            case 12:
                entity = await _context.Reimbursements.FirstOrDefaultAsync(cc => cc.Id == cancel.Id && cc.IsActive);
                break;
            default:
                return false;
        }

        if (entity == null)
        {
            throw new MessageNotFoundException($"Active entity not found for ApplicationTypeId: {cancel.ApplicationTypeId}, Id: {cancel.Id}");
        }

        if (cancel.ApplicationTypeId == 1)
        {
            var leave = (LeaveRequisition)entity;
            var staffOrCreatorId = leave.StaffId ?? leave.CreatedBy;
            var individualLeave = await _context.IndividualLeaveCreditDebits
                .Where(l => l.StaffCreationId == staffOrCreatorId &&
                            l.LeaveTypeId == leave.LeaveTypeId &&
                            l.IsActive == true)
                .OrderByDescending(l => l.Id)
                .FirstOrDefaultAsync();
            if (individualLeave != null)
            {
                individualLeave.AvailableBalance = decimal.Add(individualLeave.AvailableBalance, leave.TotalDays);
                individualLeave.UpdatedBy = cancel.UpdatedBy;
                individualLeave.UpdatedUtc = DateTime.UtcNow;
                _context.Entry(individualLeave).State = EntityState.Modified;
            }
        }

        if (cancel.ApplicationTypeId == 10)
        {
            var compOffAvail = (CompOffAvail)entity;
            var lastCompOffCredit = await _context.CompOffCredits
                .Where(c => c.CreatedBy == compOffAvail.CreatedBy && c.IsActive == true)
                .OrderByDescending(c => c.CreatedUtc)
                .FirstOrDefaultAsync();

            if (lastCompOffCredit == null)
            {
                throw new MessageNotFoundException("Insufficient CompOff balance found.");
            }

            lastCompOffCredit.Balance = (lastCompOffCredit.Balance ?? 0) + (int)compOffAvail.TotalDays;
            lastCompOffCredit.UpdatedBy = cancel.UpdatedBy;
            lastCompOffCredit.UpdatedUtc = DateTime.UtcNow;

            _context.Entry(lastCompOffCredit).State = EntityState.Modified;
        }

        var entityType = entity.GetType();
        var isCancelledProperty = entityType.GetProperty("IsCancelled");
        var cancelledOnProperty = entityType.GetProperty("CancelledOn");
        var updatedByProperty = entityType.GetProperty("CancellededBy");
        var isActiveProperty = entityType.GetProperty("IsActive");

        if (isCancelledProperty == null)
        {
            throw new MessageNotFoundException($"IsCancelled property not found in entity type: {entityType.Name}");
        }

        bool isAlreadyCancelled = (bool)(isCancelledProperty.GetValue(entity) ?? false);
        if (isAlreadyCancelled)
        {
            throw new ConflictException("Application already cancelled");
        }
        isCancelledProperty.SetValue(entity, cancel.IsCancelled);
        cancelledOnProperty?.SetValue(entity, DateTime.UtcNow);
        updatedByProperty?.SetValue(entity, cancel.UpdatedBy);
        isActiveProperty?.SetValue(entity, false);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RevokeAppliedLeave(CancelAppliedLeave cancel)
    {
        await NotFoundMethod(cancel.ApplicationTypeId);
        object? entity = null;
        switch (cancel.ApplicationTypeId)
        {
            case 1:
                entity = await _context.LeaveRequisitions.FirstOrDefaultAsync(l => l.Id == cancel.Id);
                break;
            case 2:
                entity = await _context.CommonPermissions.FirstOrDefaultAsync(p => p.Id == cancel.Id);
                break;
            case 3:
                entity = await _context.ManualPunchRequistions.FirstOrDefaultAsync(m => m.Id == cancel.Id);
                break;
            case 4:
                entity = await _context.OnDutyRequisitions.FirstOrDefaultAsync(o => o.Id == cancel.Id);
                break;
            case 5:
                entity = await _context.BusinessTravels.FirstOrDefaultAsync(b => b.Id == cancel.Id);
                break;
            case 6:
                entity = await _context.WorkFromHomes.FirstOrDefaultAsync(w => w.Id == cancel.Id);
                break;
            case 7:
                entity = await _context.ShiftChanges.FirstOrDefaultAsync(s => s.Id == cancel.Id);
                break;
            case 8:
                entity = await _context.ShiftExtensions.FirstOrDefaultAsync(se => se.Id == cancel.Id);
                break;
            case 9:
                entity = await _context.WeeklyOffHolidayWorkings.FirstOrDefaultAsync(wh => wh.Id == cancel.Id);
                break;
            case 10:
                entity = await _context.CompOffAvails.FirstOrDefaultAsync(ca => ca.Id == cancel.Id);
                break;
            case 11:
                entity = await _context.CompOffCredits.FirstOrDefaultAsync(cc => cc.Id == cancel.Id && cc.IsActive);
                break;
            case 12:
                entity = await _context.Reimbursements.FirstOrDefaultAsync(cc => cc.Id == cancel.Id);
                break;
            default:
                return false;
        }

        if (entity == null)
        {
            throw new MessageNotFoundException($"Active entity not found for ApplicationTypeId: {cancel.ApplicationTypeId}, Id: {cancel.Id}");
        }

        if (cancel.ApplicationTypeId == 1)
        {
            var leave = (LeaveRequisition)entity;
            var staffOrCreatorId = leave.StaffId ?? leave.CreatedBy;
            var individualLeave = await _context.IndividualLeaveCreditDebits
                .Where(l => l.StaffCreationId == staffOrCreatorId &&
                            l.LeaveTypeId == leave.LeaveTypeId &&
                            l.IsActive == true)
                .OrderByDescending(l => l.Id)
                .FirstOrDefaultAsync();
            if (individualLeave != null)
            {
                individualLeave.AvailableBalance = decimal.Add(individualLeave.AvailableBalance, leave.TotalDays);
                individualLeave.UpdatedBy = cancel.UpdatedBy;
                individualLeave.UpdatedUtc = DateTime.UtcNow;
                _context.Entry(individualLeave).State = EntityState.Modified;
            }
        }

        if (cancel.ApplicationTypeId == 10)
        {
            var compOffAvail = (CompOffAvail)entity;
            var lastCompOffCredit = await _context.CompOffCredits
                .Where(c => c.CreatedBy == compOffAvail.CreatedBy && c.IsActive == true)
                .OrderByDescending(c => c.CreatedUtc)
                .FirstOrDefaultAsync();

            if (lastCompOffCredit == null)
            {
                throw new MessageNotFoundException("Insufficient CompOff balance found.");
            }

            lastCompOffCredit.Balance = (lastCompOffCredit.Balance ?? 0) + (int)compOffAvail.TotalDays;
            lastCompOffCredit.UpdatedBy = cancel.UpdatedBy;
            lastCompOffCredit.UpdatedUtc = DateTime.UtcNow;

            _context.Entry(lastCompOffCredit).State = EntityState.Modified;
        }

        var entityType = entity.GetType();
        var isCancelledProperty = entityType.GetProperty("IsCancelled");
        var cancelledOnProperty = entityType.GetProperty("CancelledOn");
        var updatedByProperty = entityType.GetProperty("CancellededBy");
        var isActiveProperty = entityType.GetProperty("IsActive");

        if (isCancelledProperty == null)
        {
            throw new MessageNotFoundException($"IsCancelled property not found in entity type: {entityType.Name}");
        }

        bool isAlreadyCancelled = (bool)(isCancelledProperty.GetValue(entity) ?? false);
        if (isAlreadyCancelled)
        {
            throw new ConflictException("Application already cancelled");
        }
        isCancelledProperty.SetValue(entity, cancel.IsCancelled);
        cancelledOnProperty?.SetValue(entity, DateTime.UtcNow);
        updatedByProperty?.SetValue(entity, cancel.UpdatedBy);
        isActiveProperty?.SetValue(entity, false);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<object>> GetApplicationDetails(int staffId, int applicationTypeId)
    {
        await NotFoundMethod(applicationTypeId);
        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
        if (staff == null) throw new MessageNotFoundException("Staff not found");
        var application = applicationTypeId switch
        {
            1 => await _context.LeaveRequisitions
                .Where(lr => (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .Join(_context.ApplicationTypes, lr => lr.ApplicationTypeId, at => at.Id,
                    (lr, at) => new { lr, at })
                .Join(_context.LeaveTypes, temp => temp.lr.LeaveTypeId, lt => lt.Id,
                    (temp, lt) => new { temp.lr, temp.at, lt })
                .GroupJoin(_context.StaffCreations, temp => temp.lr.StaffId ?? temp.lr.CreatedBy, sc => sc.Id,
                    (temp, scGroup) => new { temp, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new LeaveReq
                    {
                        Id = tempWithSc.temp.lr.Id,
                        ApplicationTypeId = tempWithSc.temp.lr.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.temp.at.Name,
                        Status1 = tempWithSc.temp.lr.IsCancelled == true ? "Cancelled" :
                            (sc != null)
                                ? (tempWithSc.temp.lr.Status1.HasValue
                                    ? (tempWithSc.temp.lr.Status1.Value ? "Approved" : "Rejected")
                                    : "Pending")
                                : (tempWithSc.temp.lr.StaffId == null ? "Pending" : null),
                        Status2 = tempWithSc.temp.lr.IsCancelled == true ? "Cancelled" :
                            (sc != null && sc.ApprovalLevel2 != null)
                                ? (tempWithSc.temp.lr.Status2.HasValue
                                    ? (tempWithSc.temp.lr.Status2.Value ? "Approved" : "Rejected")
                                    : "Pending")
                                : null,
                        StartDuration = tempWithSc.temp.lr.StartDuration,
                        EndDuration = tempWithSc.temp.lr.EndDuration,
                        LeaveTypeName = tempWithSc.temp.lt.Name,
                        FromDate = tempWithSc.temp.lr.FromDate,
                        ToDate = tempWithSc.temp.lr.ToDate,
                        TotalDays = tempWithSc.temp.lr.TotalDays,
                        Reason = tempWithSc.temp.lr.Reason
                    })
                .OrderByDescending(x => x.Id)
                .ToListAsync(),
            2 => await _context.CommonPermissions
                .Where(cp => (cp.StaffId.HasValue ? cp.StaffId == staffId : cp.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, cp => cp.StaffId ?? cp.CreatedBy, sc => sc.Id,
                    (cp, scGroup) => new { cp, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new PermissionDto
                    {
                        Id = tempWithSc.cp.Id,
                        ApplicationTypeId = tempWithSc.cp.ApplicationTypeId,
                        ApplicationTypeName = _context.ApplicationTypes
                            .Where(at => at.Id == tempWithSc.cp.ApplicationTypeId)
                            .Select(at => at.Name)
                            .FirstOrDefault(),
                        Status1 = tempWithSc.cp.IsCancelled == true ? "Cancelled" :
                            (sc != null)
                                ? (tempWithSc.cp.Status1.HasValue
                                    ? (tempWithSc.cp.Status1.Value ? "Approved" : "Rejected")
                                    : "Pending")
                                : (tempWithSc.cp.StaffId == null ? "Pending" : null),
                        Status2 = tempWithSc.cp.IsCancelled == true ? "Cancelled" :
                            (sc != null && sc.ApprovalLevel2 != null)
                                ? (tempWithSc.cp.Status2.HasValue
                                    ? (tempWithSc.cp.Status2.Value ? "Approved" : "Rejected")
                                    : "Pending")
                                : null,
                        PermissionDate = tempWithSc.cp.PermissionDate,
                        TotalHours = tempWithSc.cp.TotalHours,
                        StartTime = tempWithSc.cp.FromDate != null
                        ? tempWithSc.cp.FromDate.Value.ToDateTime(tempWithSc.cp.StartTime)
                        : (DateTime?)null,
                        EndTime = tempWithSc.cp.ToDate != null
                        ? tempWithSc.cp.ToDate.Value.ToDateTime(tempWithSc.cp.EndTime)
                        : (DateTime?)null,
                        PermissionType = tempWithSc.cp.PermissionType,
                        Remarks = tempWithSc.cp.Remarks
                    })
                .OrderByDescending(x => x.Id)
                .ToListAsync(),
            3 => await _context.ManualPunchRequistions
                 .Where(mp => (mp.StaffId.HasValue ? mp.StaffId == staffId : mp.CreatedBy == staffId))
                 .GroupJoin(_context.StaffCreations, mp => mp.StaffId ?? mp.CreatedBy, sc => sc.Id,
                     (mp, scGroup) => new { mp, scGroup })
                 .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                     (tempWithSc, sc) => new ManualPunch
                     {
                         Id = tempWithSc.mp.Id,
                         ApplicationTypeId = tempWithSc.mp.ApplicationTypeId,
                         ApplicationTypeName = tempWithSc.mp.ApplicationType.Name,
                         Status1 = tempWithSc.mp.IsCancelled == true ? "Cancelled" :
                             (sc != null)
                                 ? (tempWithSc.mp.Status1.HasValue
                                     ? (tempWithSc.mp.Status1.Value ? "Approved" : "Rejected")
                                     : "Pending")
                                 : (tempWithSc.mp.StaffId == null ? "Pending" : null),
                         Status2 = tempWithSc.mp.IsCancelled == true ? "Cancelled" :
                             (sc != null && sc.ApprovalLevel2 != null)
                                 ? (tempWithSc.mp.Status2.HasValue
                                     ? (tempWithSc.mp.Status2.Value ? "Approved" : "Rejected")
                                     : "Pending")
                                 : null,
                         SelectPunch = tempWithSc.mp.SelectPunch,
                         InPunch = tempWithSc.mp.InPunch,
                         OutPunch = tempWithSc.mp.OutPunch,
                         Remarks = tempWithSc.mp.Remarks
                     })
                 .OrderByDescending(x => x.Id)
                 .ToListAsync(),
            4 => await _context.OnDutyRequisitions
                .Where(od => (od.StaffId.HasValue ? od.StaffId == staffId : od.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, od => od.StaffId ?? od.CreatedBy, sc => sc.Id,
                    (od, scGroup) => new { od, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new OnDutyRequest
                    {
                        Id = tempWithSc.od.Id,
                        ApplicationTypeId = tempWithSc.od.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.od.ApplicationType.Name,
                        Status1 = tempWithSc.od.IsCancelled == true ? "Cancelled" :
                            (sc != null)
                                ? (tempWithSc.od.Status1.HasValue
                                    ? (tempWithSc.od.Status1.Value ? "Approved" : "Rejected")
                                    : "Pending")
                                : (tempWithSc.od.StaffId == null ? "Pending" : null),
                        Status2 = tempWithSc.od.IsCancelled == true ? "Cancelled" :
                            (sc != null && sc.ApprovalLevel2 != null)
                                ? (tempWithSc.od.Status2.HasValue
                                    ? (tempWithSc.od.Status2.Value ? "Approved" : "Rejected")
                                    : "Pending")
                                : null,
                        StartDuration = tempWithSc.od.StartDuration,
                        EndDuration = tempWithSc.od.EndDuration,
                        StartDate = tempWithSc.od.StartDate,
                        EndDate = tempWithSc.od.EndDate,
                        StartTime = tempWithSc.od.StartTime,
                        EndTime = tempWithSc.od.EndTime,
                        TotalDays = tempWithSc.od.TotalDays,
                        TotalHours = tempWithSc.od.TotalHours,
                        Reason = tempWithSc.od.Reason
                    })
                .OrderByDescending(x => x.Id)
                .ToListAsync(),
            5 => await _context.BusinessTravels
                 .Where(bt => (bt.StaffId.HasValue ? bt.StaffId == staffId : bt.CreatedBy == staffId))
                 .GroupJoin(_context.StaffCreations, bt => bt.StaffId ?? bt.CreatedBy, sc => sc.Id,
                     (bt, scGroup) => new { bt, scGroup })
                 .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                     (tempWithSc, sc) => new Business
                     {
                         Id = tempWithSc.bt.Id,
                         ApplicationTypeId = tempWithSc.bt.ApplicationTypeId,
                         ApplicationTypeName = tempWithSc.bt.ApplicationType.Name,
                         Status1 = tempWithSc.bt.IsCancelled == true ? "Cancelled" :
                             (sc != null)
                                 ? (tempWithSc.bt.Status1.HasValue
                                     ? (tempWithSc.bt.Status1.Value ? "Approved" : "Rejected")
                                     : "Pending")
                                 : (tempWithSc.bt.StaffId == null ? "Pending" : null),

                         Status2 = tempWithSc.bt.IsCancelled == true ? "Cancelled" :
                             (sc != null && sc.ApprovalLevel2 != null)
                                 ? (tempWithSc.bt.Status2.HasValue
                                     ? (tempWithSc.bt.Status2.Value ? "Approved" : "Rejected")
                                     : "Pending")
                                 : null,
                         StartDuration = tempWithSc.bt.StartDuration,
                         EndDuration = tempWithSc.bt.EndDuration,
                         FromTime = tempWithSc.bt.FromTime,
                         ToTime = tempWithSc.bt.ToTime,
                         FromDate = tempWithSc.bt.FromDate,
                         ToDate = tempWithSc.bt.ToDate,
                         TotalDays = tempWithSc.bt.TotalDays,
                         TotalHours = tempWithSc.bt.TotalHours,
                         Reason = tempWithSc.bt.Reason
                     })
                 .OrderByDescending(x => x.Id)
                 .ToListAsync(),
            6 => await _context.WorkFromHomes
                   .Where(wfh => (wfh.StaffId.HasValue ? wfh.StaffId == staffId : wfh.CreatedBy == staffId))
                   .GroupJoin(_context.StaffCreations, wfh => wfh.StaffId ?? wfh.CreatedBy, sc => sc.Id,
                       (wfh, scGroup) => new { wfh, scGroup })
                   .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                       (tempWithSc, sc) => new WorkFrom
                       {
                           Id = tempWithSc.wfh.Id,
                           ApplicationTypeId = tempWithSc.wfh.ApplicationTypeId,
                           ApplicationTypeName = tempWithSc.wfh.ApplicationType.Name,
                           Status1 = tempWithSc.wfh.IsCancelled == true ? "Cancelled" :
                               (sc != null)
                                   ? (tempWithSc.wfh.Status1.HasValue
                                       ? (tempWithSc.wfh.Status1.Value ? "Approved" : "Rejected")
                                       : "Pending")
                                   : (tempWithSc.wfh.StaffId == null ? "Pending" : null),
                           Status2 = tempWithSc.wfh.IsCancelled == true ? "Cancelled" :
                               (sc != null && sc.ApprovalLevel2 != null)
                                   ? (tempWithSc.wfh.Status2.HasValue
                                       ? (tempWithSc.wfh.Status2.Value ? "Approved" : "Rejected")
                                       : "Pending")
                                   : null,
                           StartDuration = tempWithSc.wfh.StartDuration,
                           EndDuration = tempWithSc.wfh.EndDuration,
                           FromTime = tempWithSc.wfh.FromTime,
                           ToTime = tempWithSc.wfh.ToTime,
                           FromDate = tempWithSc.wfh.FromDate,
                           ToDate = tempWithSc.wfh.ToDate,
                           TotalDays = tempWithSc.wfh.TotalDays,
                           TotalHours = tempWithSc.wfh.TotalHours,
                           Reason = tempWithSc.wfh.Reason
                       })
                   .OrderByDescending(x => x.Id)
                   .ToListAsync(),
            7 => await _context.ShiftChanges
                  .Where(lr => (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                  .Join(_context.ApplicationTypes, sc => sc.ApplicationTypeId, at => at.Id,
                        (sc, at) => new { sc, at })
                  .Join(_context.Shifts, temp => temp.sc.ShiftId, s => s.Id,
                        (temp, s) => new { temp, s })
                  .GroupJoin(_context.StaffCreations, tempWithS => tempWithS.temp.sc.StaffId ?? tempWithS.temp.sc.CreatedBy,
                             sc => sc.Id, (tempWithS, scGroup) => new { tempWithS, scGroup })
                  .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                      (tempWithSc, sc) => new ShiftChan
                      {
                          Id = tempWithSc.tempWithS.temp.sc.Id,
                          ApplicationTypeId = tempWithSc.tempWithS.temp.sc.ApplicationTypeId,
                          ApplicationTypeName = tempWithSc.tempWithS.temp.at.Name,
                          ShiftName = tempWithSc.tempWithS.s.Name,
                          FromDate = tempWithSc.tempWithS.temp.sc.FromDate,
                          ToDate = tempWithSc.tempWithS.temp.sc.ToDate,
                          Reason = tempWithSc.tempWithS.temp.sc.Reason,
                          Status1 = tempWithSc.tempWithS.temp.sc.IsCancelled == true ? "Cancelled" :
                                    (sc != null)
                                        ? (tempWithSc.tempWithS.temp.sc.Status1.HasValue
                                            ? (tempWithSc.tempWithS.temp.sc.Status1.Value ? "Approved" : "Rejected")
                                            : "Pending")
                                        : (tempWithSc.tempWithS.temp.sc.StaffId == null ? "Pending" : null),
                          Status2 = tempWithSc.tempWithS.temp.sc.IsCancelled == true ? "Cancelled" :
                                    (sc != null && sc.ApprovalLevel2 != null)
                                        ? (tempWithSc.tempWithS.temp.sc.Status2.HasValue
                                            ? (tempWithSc.tempWithS.temp.sc.Status2.Value ? "Approved" : "Rejected")
                                            : "Pending")
                                        : null
                      })
                  .OrderByDescending(x => x.Id)
                  .ToListAsync(),
            8 => await _context.ShiftExtensions
                 .Where(lr => (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                 .GroupJoin(_context.StaffCreations, se => se.StaffId ?? se.CreatedBy,
                     sc => sc.Id, (se, scGroup) => new { se, scGroup })
                 .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                     (tempWithSc, sc) => new ShiftExte
                     {
                         Id = tempWithSc.se.Id,
                         ApplicationTypeId = tempWithSc.se.ApplicationTypeId,
                         ApplicationTypeName = tempWithSc.se.ApplicationType.Name,
                         TransactionDate = tempWithSc.se.TransactionDate,
                         DurationHours = tempWithSc.se.DurationHours,
                         BeforeShiftHours = tempWithSc.se.BeforeShiftHours,
                         AfterShiftHours = tempWithSc.se.AfterShiftHours,
                         Remarks = tempWithSc.se.Remarks,
                         Status1 = tempWithSc.se.IsCancelled == true ? "Cancelled" :
                                   (sc != null)
                                       ? (tempWithSc.se.Status1.HasValue
                                           ? (tempWithSc.se.Status1.Value ? "Approved" : "Rejected")
                                           : "Pending")
                                       : (tempWithSc.se.StaffId == null ? "Pending" : null),

                         Status2 = tempWithSc.se.IsCancelled == true ? "Cancelled" :
                                   (sc != null && sc.ApprovalLevel2 != null)
                                       ? (tempWithSc.se.Status2.HasValue
                                           ? (tempWithSc.se.Status2.Value ? "Approved" : "Rejected")
                                           : "Pending")
                                       : null
                     })
                 .OrderByDescending(x => x.Id)
                 .ToListAsync(),
            9 => await _context.WeeklyOffHolidayWorkings
                .Where(lr => (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, wh => wh.StaffId ?? wh.CreatedBy,
                    sc => sc.Id, (wh, scGroup) => new { wh, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new { tempWithSc.wh, sc })
                .Join(_context.ApplicationTypes,
                    temp => temp.wh.ApplicationTypeId, at => at.Id, (temp, at) => new { temp.wh, temp.sc, at })
                .Join(_context.Shifts,
                    temp => temp.wh.ShiftId, s => s.Id, (temp, s) => new WeeklyOffHoliday
                    {
                        Id = temp.wh.Id,
                        ApplicationTypeId = temp.wh.ApplicationTypeId,
                        ApplicationTypeName = temp.at.Name,
                        SelectShiftType = temp.wh.SelectShiftType,
                        TxnDate = temp.wh.TxnDate,
                        ShiftName = s.Name,
                        ShiftInTime = temp.wh.ShiftInTime,
                        ShiftOutTime = temp.wh.ShiftOutTime,
                        Status1 = temp.wh.IsCancelled == true ? "Cancelled" :
                                  (temp.sc != null)
                                      ? (temp.wh.Status1.HasValue
                                          ? (temp.wh.Status1.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : (temp.wh.StaffId == null ? "Pending" : null),
                        Status2 = temp.wh.IsCancelled == true ? "Cancelled" :
                                  (temp.sc != null && temp.sc.ApprovalLevel2 != null)
                                      ? (temp.wh.Status2.HasValue
                                          ? (temp.wh.Status2.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : null
                    })
                .OrderByDescending(x => x.Id)
                .ToListAsync(),
            10 => await _context.CompOffAvails
                .Where(lr => (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, coa => coa.StaffId ?? coa.CreatedBy,
                    sc => sc.Id, (coa, scGroup) => new { coa, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new CompOffAvai
                    {
                        Id = tempWithSc.coa.Id,
                        ApplicationTypeId = tempWithSc.coa.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.coa.ApplicationType.Name,
                        WorkedDate = tempWithSc.coa.WorkedDate,
                        FromDate = tempWithSc.coa.FromDate,
                        ToDate = tempWithSc.coa.ToDate,
                        FromDuration = tempWithSc.coa.FromDuration,
                        ToDuration = tempWithSc.coa.ToDuration,
                        Reason = tempWithSc.coa.Reason,
                        TotalDays = tempWithSc.coa.TotalDays,
                        Status1 = tempWithSc.coa.IsCancelled == true ? "Cancelled" :
                                  (sc != null)
                                      ? (tempWithSc.coa.Status1.HasValue
                                          ? (tempWithSc.coa.Status1.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : "Pending",
                        Status2 = tempWithSc.coa.IsCancelled == true ? "Cancelled" :
                                  (sc != null && sc.ApprovalLevel2 != null)
                                      ? (tempWithSc.coa.Status2.HasValue
                                          ? (tempWithSc.coa.Status2.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : null
                    })
                .OrderByDescending(x => x.Id)
                .ToListAsync(),
            11 => await _context.CompOffCredits
                .Where(lr => (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, coc => coc.StaffId ?? coc.CreatedBy,
                    sc => sc.Id, (coc, scGroup) => new { coc, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new CompOffCred
                    {
                        Id = tempWithSc.coc.Id,
                        ApplicationTypeId = tempWithSc.coc.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.coc.ApplicationType.Name,
                        WorkedDate = tempWithSc.coc.WorkedDate,
                        TotalDays = tempWithSc.coc.TotalDays,
                        Reason = tempWithSc.coc.Reason,
                        Status1 = tempWithSc.coc.IsCancelled == true ? "Cancelled" :
                                  (sc != null)
                                      ? (tempWithSc.coc.Status1.HasValue
                                          ? (tempWithSc.coc.Status1.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : "Pending",
                        Status2 = tempWithSc.coc.IsCancelled == true ? "Cancelled" :
                                  (sc != null && sc.ApprovalLevel2 != null)
                                      ? (tempWithSc.coc.Status2.HasValue
                                          ? (tempWithSc.coc.Status2.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : null
                    })
                .OrderByDescending(x => x.Id)
                .ToListAsync(),
            18 => await _context.Reimbursements
                .Where(r => (r.StaffId.HasValue ? r.StaffId == staffId : r.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, r => r.StaffId ?? r.CreatedBy,
                    sc => sc.Id, (r, scGroup) => new { r, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new Reimbursements
                    {
                        Id = tempWithSc.r.Id,
                        BillDate = tempWithSc.r.BillDate,
                        BillNo = tempWithSc.r.BillNo,
                        Description = tempWithSc.r.Description,
                        BillPeriod = tempWithSc.r.BillPeriod,
                        Amount = tempWithSc.r.Amount,
                        ReimbursementTypeName = tempWithSc.r.ReimbursementType.Name,
                        UploadFilePath = tempWithSc.r.UploadFilePath,
                        Status1 = tempWithSc.r.CancelledOn.HasValue ? "Cancelled" :
                                  (sc != null)
                                      ? (tempWithSc.r.Status1.HasValue
                                          ? (tempWithSc.r.Status1.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : "Pending",
                        Status2 = tempWithSc.r.CancelledOn.HasValue ? "Cancelled" :
                                  (sc != null && sc.ApprovalLevel2 != null)
                                      ? (tempWithSc.r.Status2.HasValue
                                          ? (tempWithSc.r.Status2.Value ? "Approved" : "Rejected")
                                          : "Pending")
                                      : null
                    })
                .OrderByDescending(x => x.Id)
                .ToListAsync(),
            _ => Enumerable.Empty<object>()
        };
        return application;
    }

    public async Task<object> GetMonthlyDetailsAsync(int staffId, int month, int year)
    {
        var staff = await _context.StaffCreations
            .Where(s => s.Id == staffId && s.IsActive == true)
            .Select(s => new
            {
                s.Id,
                s.StaffId,
                StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}"
            })
            .FirstOrDefaultAsync();
        if (staff == null) throw new MessageNotFoundException("Staff not found");
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                  .Select(offset => startDate.AddDays(offset))
                                  .ToList();
        var attendanceRecords = await (from a in _context.AttendanceRecords
                                       join s in _context.Shifts on a.ShiftId equals s.Id into shiftGroup
                                       from shift in shiftGroup.DefaultIfEmpty()
                                       where a.StaffId == staffId && a.FirstIn.HasValue && !a.IsDeleted &&
                                             a.FirstIn.Value.Month == month && a.FirstIn.Value.Year == year
                                       select new
                                       {
                                           a.FirstIn,
                                           LoginTime = a.FirstIn,
                                           LogoutTime = a.LastOut,
                                           a.ShiftId,
                                           TotalHoursWorked = a.LastOut.HasValue && a.FirstIn.HasValue
    ? ((a.LastOut.Value < a.FirstIn.Value)
        ? (a.LastOut.Value.AddDays(1) - a.FirstIn.Value).TotalHours
        : (a.LastOut.Value - a.FirstIn.Value).TotalHours)
    : 0
                                       })
                                   .ToListAsync();
        var leaveRecords = await _context.LeaveRequisitions
            .Where(lr => (lr.StaffId != null ? lr.StaffId == staffId : lr.CreatedBy == staffId) &&
                         (lr.FromDate <= DateOnly.FromDateTime(endDate) &&
                          lr.ToDate >= DateOnly.FromDateTime(startDate)) && !lr.IsActive &&
                         ((lr.Staff.ApprovalLevel2 != null && lr.Status1 == true && lr.Status2 == true) ||
                          (lr.Staff.ApprovalLevel2 == null && lr.Status1 == true)))
            .Select(lr => new
            {
                lr.FromDate,
                lr.ToDate,
                lr.LeaveTypeId,
                lr.LeaveType.Name,
                lr.StartDuration,
                lr.EndDuration,
                lr.Reason
            })
            .ToListAsync();
        var assignedShift = await (from sh in _context.AssignShifts
                                   where sh.StaffId == staffId && sh.IsActive &&
                                         sh.FromDate >= DateOnly.FromDateTime(startDate) &&
                                         sh.FromDate <= DateOnly.FromDateTime(endDate)
                                   select new
                                   {
                                       Date = sh.FromDate,
                                       ShiftName = sh.Shift.Name,
                                       ShiftStartTime = sh.Shift.StartTime,
                                       ShiftEndTime = sh.Shift.EndTime
                                   }).ToListAsync();

        var workFromHomeRecords = await _context.WorkFromHomes
            .Where(wfh => (wfh.StaffId != null ? wfh.StaffId == staffId : wfh.CreatedBy == staffId) &&
                          ((wfh.FromDate.HasValue && wfh.FromDate.Value <= DateOnly.FromDateTime(endDate)) &&
                           (wfh.ToDate.HasValue && wfh.ToDate.Value >= DateOnly.FromDateTime(startDate))) && !wfh.IsActive &&
                          ((wfh.Staff.ApprovalLevel2 != null && wfh.Status1 == true && wfh.Status2 == true) || (wfh.Staff.ApprovalLevel2 == null && wfh.Status1 == true)))
            .Select(wfh => new
            {
                wfh.FromDate,
                wfh.ToDate,
                wfh.StartDuration,
                wfh.EndDuration,
                wfh.Reason
            })
            .ToListAsync();
        var startDateOnly = DateOnly.FromDateTime(startDate);
        var endDateOnly = DateOnly.FromDateTime(endDate);
        var onDutyRecords = await _context.OnDutyRequisitions
            .Where(od => (od.StaffId != null ? od.StaffId == staffId : od.CreatedBy == staffId) &&
                         od.StartDate <= endDateOnly && od.EndDate >= startDateOnly && !od.IsActive &&
                         ((od.Staff.ApprovalLevel2 != null && od.Status1 == true && od.Status2 == true) || (od.Staff.ApprovalLevel2 == null && od.Status1 == true)))
            .Select(od => new { od.StartDate, od.EndDate, od.StartDuration, od.EndDuration, od.Reason })
            .ToListAsync();
        var businessTravelRecords = await _context.BusinessTravels
            .Where(bt => (bt.StaffId != null ? bt.StaffId == staffId : bt.CreatedBy == staffId) &&
                         bt.FromDate <= endDateOnly && bt.ToDate >= startDateOnly && !bt.IsActive &&
                         ((bt.Staff.ApprovalLevel2 != null && bt.Status1 == true && bt.Status2 == true) || (bt.Staff.ApprovalLevel2 == null && bt.Status1 == true)))
            .Select(bt => new { bt.FromDate, bt.ToDate, bt.StartDuration, bt.EndDuration, bt.Reason })
            .ToListAsync();
        var compOffRecords = await _context.CompOffAvails
            .Where(co => (co.StaffId != null ? co.StaffId == staffId : co.CreatedBy == staffId) &&
                         co.FromDate <= endDateOnly && co.ToDate >= startDateOnly && !co.IsActive &&
                         ((co.Staff.ApprovalLevel2 != null && co.Status1 == true && co.Status2 == true) || (co.Staff.ApprovalLevel2 == null && co.Status1 == true)))
            .Select(co => new { co.FromDate, co.ToDate, co.FromDuration, co.ToDuration })
            .ToListAsync();
        var holidayWorkingRecords = await _context.WeeklyOffHolidayWorkings
    .Where(co => (co.StaffId != null ? co.StaffId == staffId : co.CreatedBy == staffId) &&
                 co.TxnDate <= endDateOnly && co.TxnDate >= startDateOnly && !co.IsActive &&
                 ((co.Staff.ApprovalLevel2 != null && co.Status1 == true && co.Status2 == true) || (co.Staff.ApprovalLevel2 == null && co.Status1 == true)))
    .Select(co => new { co.TxnDate })
    .ToListAsync();
        var weeklyOffRecords = await _context.AssignShifts
       .Include(x => x.Shift)
       .Where(x => x.StaffId == staffId && x.IsActive && x.Shift.IsActive && x.FromDate >= startDateOnly && x.FromDate <= endDateOnly && x.ShiftId == 18)
       .Select(x => new
       {
           Date = x.FromDate,
           x.Shift.Name,
           x.Shift.StartTime,
           x.Shift.EndTime
       })
       .ToListAsync();
        var shiftTypeIds = await (
            from a in _context.AssignShifts
            join s in _context.Shifts on a.ShiftId equals s.Id
            where a.StaffId == staffId && a.IsActive && a.FromDate >= startDateOnly && a.FromDate <= endDateOnly
            select s.ShiftTypeId
        ).Distinct().ToListAsync();
        var holidayCalendarIds = await (
            from hcc in _context.HolidayCalendarConfigurations
            where hcc.IsActive && hcc.CalendarYear == year && shiftTypeIds.Contains((int)hcc.ShiftTypeId!)
            select hcc.Id
        ).ToListAsync();
        var holidayRecords = await (
            from hc in _context.HolidayCalendarTransactions
            join hm in _context.HolidayMasters on hc.HolidayMasterId equals hm.Id
            where hc.IsActive && hm.IsActive &&
                  holidayCalendarIds.Contains(hc.HolidayCalendarId) &&
                  (
                    (hc.FromDate.Month == month && hc.FromDate.Year == year) ||
                    (hc.ToDate.Month == month && hc.ToDate.Year == year)
                  )
            select new
            {
                HolidayName = hm.Name,
                Date = hc.FromDate
            }
        ).ToListAsync();
        var assignedShifts = await _context.AssignShifts
       .Include(x => x.Shift)
       .Where(x => x.StaffId == staffId && x.IsActive && x.Shift.IsActive &&
                   x.FromDate >= startDateOnly && x.FromDate <= endDateOnly)
       .Select(x => new
       {
           Date = x.FromDate,
           x.Shift.Name,
           x.Shift.StartTime,
           x.Shift.EndTime,
           ShiftTypeId = x.Shift.ShiftTypeId
       })
       .ToListAsync();
        var statusColors = await (
            from sd in _context.StatusDropdowns
            where sd.IsActive
            select new
            {
                StatusId = sd.Id,
                StatusName = sd.Name,
                ShortName = sd.ShortName,
                ColorCode = sd.ColorCode.ColourCode,
            }).ToListAsync();
        var result = new List<object>();
        foreach (var date in allDates)
        {
            var dateOnly = DateOnly.FromDateTime(date);
            var shiftForDate = assignedShifts.FirstOrDefault(s => s.Date == dateOnly);
            var attendance = attendanceRecords.FirstOrDefault(a => a.LoginTime.HasValue && a.LogoutTime.HasValue && a.LoginTime.Value.Date == date);
            var todayDateOnly = DateOnly.FromDateTime(DateTime.Today);
            var attendanceRecord = await _context.AttendanceRecords.FirstOrDefaultAsync(a => !a.IsDeleted && a.AttendanceDate == dateOnly && a.StaffId == staffId);
            string statusName = "";
            string colorCode = "";
            string shortName = "";
            var leave = leaveRecords.FirstOrDefault(l => dateOnly >= l.FromDate && dateOnly <= l.ToDate);
            var workFromHomeAny = workFromHomeRecords.FirstOrDefault(wfh => wfh.FromDate.HasValue && wfh.ToDate.HasValue && dateOnly >= wfh.FromDate.Value && dateOnly <= wfh.ToDate.Value);
            var workFromHome = workFromHomeRecords.Any(wfh => wfh.FromDate.HasValue && wfh.ToDate.HasValue && dateOnly >= wfh.FromDate.Value && dateOnly <= wfh.ToDate.Value);
            var onDutyAny = onDutyRecords.FirstOrDefault(od => dateOnly >= od.StartDate && dateOnly <= od.EndDate);
            var onDuty = onDutyRecords.Any(od => dateOnly >= od.StartDate && dateOnly <= od.EndDate);
            var businessTravelAny = businessTravelRecords.FirstOrDefault(bt => dateOnly >= bt.FromDate && dateOnly <= bt.ToDate);
            var businessTravel = businessTravelRecords.Any(bt => dateOnly >= bt.FromDate && dateOnly <= bt.ToDate);
            var compOffAny = compOffRecords.FirstOrDefault(co => dateOnly >= co.FromDate && dateOnly <= co.ToDate);
            var compOff = compOffRecords.Any(co => dateOnly >= co.FromDate && dateOnly <= co.ToDate);
            var weeklyOff = weeklyOffRecords.FirstOrDefault(wo => wo.Date == dateOnly);
            var holidayWorking = holidayWorkingRecords.FirstOrDefault(wo => wo.TxnDate >= dateOnly && wo.TxnDate <= dateOnly);
            var holiday = holidayRecords.FirstOrDefault(h => dateOnly == h.Date);
            var shift = assignedShift.FirstOrDefault(s => s.Date >= startDateOnly && s.Date <= endDateOnly);
            if (leave != null)
            {
                int leaveStatusId = GetLeaveStatusId(leave.LeaveTypeId, leave.StartDuration, leave?.EndDuration);
                var leaveStatus = await _context.StatusDropdowns
                    .Where(sd => sd.Id == leaveStatusId && sd.IsActive)
                    .Join(_context.AttendanceStatusColors,
                          sd => sd.ColorCodeId,
                          asc => asc.Id,
                          (sd, asc) => new { sd.Name, sd.ShortName, asc.ColourCode })
                    .FirstOrDefaultAsync();
                if (leaveStatus != null)
                {
                    statusName = leaveStatus.Name;
                    shortName = leaveStatus.ShortName;
                    colorCode = leaveStatus?.ColourCode ?? "#f99da3";
                }
            }
            else if (workFromHomeAny != null)
            {
                int compOffStatusId = GetCompOffStatusId(workFromHomeAny.StartDuration, workFromHomeAny.EndDuration);
                var wfh = await _context.StatusDropdowns
                    .Where(sd => sd.Id == compOffStatusId && sd.IsActive)
                    .Join(_context.AttendanceStatusColors,
                          sd => sd.ColorCodeId,
                          asc => asc.Id,
                          (sd, asc) => new { sd.Name, sd.ShortName, asc.ColourCode })
                    .FirstOrDefaultAsync();
                if (wfh != null)
                {
                    statusName = wfh.Name;
                    shortName = wfh.ShortName;
                    colorCode = wfh?.ColourCode ?? "#ede7f6";
                }
            }
            else if (onDutyAny != null)
            {
                int compOffStatusId = GetCompOffStatusId(onDutyAny.StartDuration, onDutyAny.EndDuration);
                var onDutyStatus = await _context.StatusDropdowns
                    .Where(sd => sd.Id == compOffStatusId && sd.IsActive)
                    .Join(_context.AttendanceStatusColors,
                          sd => sd.ColorCodeId,
                          asc => asc.Id,
                          (sd, asc) => new { sd.Name, sd.ShortName, asc.ColourCode })
                    .FirstOrDefaultAsync();
                if (onDutyStatus != null)
                {
                    statusName = onDutyStatus.Name;
                    shortName = onDutyStatus.ShortName;
                    colorCode = onDutyStatus?.ColourCode ?? "rgb(134,194,230)";
                }
            }
            else if (businessTravelAny != null)
            {
                int compOffStatusId = GetCompOffStatusId(businessTravelAny.StartDuration, businessTravelAny.EndDuration);
                var businessTravelStatus = await _context.StatusDropdowns
                    .Where(sd => sd.Id == compOffStatusId && sd.IsActive)
                    .Join(_context.AttendanceStatusColors,
                          sd => sd.ColorCodeId,
                          asc => asc.Id,
                          (sd, asc) => new { sd.Name, sd.ShortName, asc.ColourCode })
                    .FirstOrDefaultAsync();
                if (businessTravelStatus != null)
                {
                    statusName = businessTravelStatus.Name;
                    shortName = businessTravelStatus.ShortName;
                    colorCode = businessTravelStatus?.ColourCode ?? "#e3f2fd";
                }
            }
            else if (compOffAny != null)
            {
                int compOffStatusId = GetCompOffStatusId(compOffAny.FromDuration, compOffAny.ToDuration);
                var compOffStatus = await _context.StatusDropdowns
                    .Where(sd => sd.Id == compOffStatusId && sd.IsActive)
                    .Join(_context.AttendanceStatusColors,
                          sd => sd.ColorCodeId,
                          asc => asc.Id,
                          (sd, asc) => new { sd.Name, sd.ShortName, asc.ColourCode })
                    .FirstOrDefaultAsync();
                if (compOffStatus != null)
                {
                    statusName = compOffStatus.Name;
                    shortName = compOffStatus.ShortName;
                    colorCode = compOffStatus.ColourCode ?? "#e0f7fa";
                }
            }
            else if (holiday != null)
            {
                var notUpdated = await _context.AttendanceStatusColors.FirstOrDefaultAsync(a => a.Id == 5);
                if (notUpdated != null)
                {
                    statusName = notUpdated.Name;
                    shortName = notUpdated.ShortName;
                    colorCode = notUpdated?.ColourCode ?? "#d9e3be";
                }
            }
            else if (weeklyOff != null)
            {
                var notUpdated = await _context.AttendanceStatusColors.FirstOrDefaultAsync(a => a.Id == 9);
                if (notUpdated != null)
                {
                    statusName = notUpdated.Name;
                    shortName = notUpdated.ShortName;
                    colorCode = notUpdated?.ColourCode ?? "#f5e6c8";
                }
            }
            else if (attendanceRecord != null)
            {
                var status = await _context.StatusDropdowns.FirstOrDefaultAsync(s => s.Id == attendanceRecord.StatusId && s.IsActive);
                statusName = status?.Name ?? "";

                var colorEntry = statusColors.FirstOrDefault(c => c.StatusName == statusName);
                if (colorEntry != null)
                {
                    colorCode = colorEntry.ColorCode;
                    shortName = colorEntry.ShortName;
                }
            }
            else if (dateOnly == DateOnly.FromDateTime(DateTime.Today))
            {
                var notUpdated = await _context.AttendanceStatusColors.FirstOrDefaultAsync(a => a.Id == 10);
                if (notUpdated != null)
                {
                    statusName = notUpdated.Name;
                    shortName = notUpdated.ShortName;
                    colorCode = notUpdated?.ColourCode ?? "#f0f0f0";
                }
            }
            result.Add(new
            {
                date = date.ToString("yyyy-MM-dd"),
                day = date.DayOfWeek.ToString(),
                status = statusName,
                shortName = shortName,
                colorCode = colorCode,
                login = attendance?.LoginTime?.ToString("hh:mm tt") ?? "00:00:000",
                logout = attendance?.LogoutTime?.ToString("hh:mm tt") ?? "00:00:000",
                totalHoursWorked = attendance?.LoginTime != null && attendance.LogoutTime != null
    ? CalculateTotalHoursWorked(attendance.LoginTime.Value, attendance.LogoutTime.Value)
    : "0.00",
                shiftName = shiftForDate?.Name,
                workFromHome,
                leaveTypeName = leave?.Name,
                onDuty,
                businessTravel,
                compOff,
                holidayWorking,
                weeklyOff = weeklyOff != null,
                holidayName = holiday?.HolidayName,
                assignedShiftStart = shiftForDate?.StartTime ?? "00:00",
                assignedShiftEnd = shiftForDate?.EndTime ?? "00:00"
            });
        }
        return new
        {
            staff.Id,
            staff.StaffId,
            staff.StaffName,
            startDate = startDate.ToString("yyyy-MM-dd"),
            endDate = endDate.ToString("yyyy-MM-dd"),
            records = result
        };
    }

    private string CalculateTotalHoursWorked(DateTime login, DateTime logout)
    {
        if (logout < login)
        {
            logout = logout.AddDays(1);
        }

        TimeSpan totalWorked = logout - login;

        int totalHours = (int)totalWorked.TotalHours;
        int totalMinutes = totalWorked.Minutes;

        return $"{totalHours}.{totalMinutes:D2}";
    }

    private int GetCompOffStatusId(string fromDuration, string? toDuration)
    {
        if (fromDuration == "First Half" || toDuration == "First Half")
            return 15;
        else if (fromDuration == "Second Half" || toDuration == "Second Half")
            return 16;
        else if (fromDuration == "Full Day" || toDuration == "Full Day")
            return 14;
        else
            return 37;
    }

    private int GetLeaveStatusId(int leaveTypeId, string startDuration, string? endDuration)
    {
        switch (leaveTypeId)
        {
            case 5:
                if (startDuration == "Full Day" && endDuration == "Full Day") return 11;
                if (startDuration == "First Half" || endDuration == "First Half") return 12;
                if (startDuration == "Second Half" || endDuration == "Second Half") return 13;
                return 37;
            case 6:
                if (startDuration == "Full Day" && endDuration == "Full Day") return 17;
                if (startDuration == "First Half" || endDuration == "First Half") return 18;
                if (startDuration == "Second Half" || endDuration == "Second Half") return 19;
                return 37;

            case 18: return 20;
            case 43: return 23;
            case 15: return 21;
            case 39: return 31;

            case 40:
                if (startDuration == "Full Day" && endDuration == "Full Day") return 39;
                if (startDuration == "First Half" || endDuration == "First Half") return 40;
                if (startDuration == "Second Half" || endDuration == "Second Half") return 41;
                return 37;

            default: return 37;
        }
    }

    public async Task<List<CompOffCreditResponseDto>> GetCompOffCreditAllAsync()
    {
        var compOffCredits = await (from c in _context.CompOffCredits
                                    where c.IsActive
                                    select new CompOffCreditResponseDto
                                    {
                                        Id = c.Id,
                                        StaffId = c.StaffId ?? c.CreatedBy,
                                        ApplicationTypeId = c.ApplicationTypeId,
                                        WorkedDate = c.WorkedDate,
                                        TotalDays = c.TotalDays,
                                        Reason = c.Reason,
                                    }).ToListAsync();
        if (compOffCredits.Count == 0) throw new MessageNotFoundException("No CompOff Credits found");
        return compOffCredits;
    }

    public async Task<string> CreateAsync(CompOffCreditDto compOffCreditDto)
    {
        var message = "CompOff Credit request submitted successfully";
        var staffOrCreatorId = compOffCreditDto.StaffId ?? compOffCreditDto.CreatedBy;
        await AttendanceFreezeDate(staffOrCreatorId, compOffCreditDto.WorkedDate);
        await NotFoundMethod(compOffCreditDto.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var existingLeaves = await _context.CompOffCredits
        .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) && lr.WorkedDate <= compOffCreditDto.WorkedDate)
        .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.WorkedDate == compOffCreditDto.WorkedDate;
            if (sameStartDate)
            {
                throw new ConflictException("CompOff Credit request already exists");
            }
        }
        var lastBalance = await _context.CompOffCredits
            .Where(c => (c.StaffId ?? c.CreatedBy) == staffOrCreatorId)
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
        await _context.CompOffCredits.AddAsync(compOffCredit);
        await _context.SaveChangesAsync();

        string requestDateTime = compOffCredit.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == compOffCredit.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = compOffCredit.CreatedBy,
            Message = $"{requestedBy} has submitted a CompOff Credit request on {requestDateTime}",
            ApplicationTypeId = compOffCredit.ApplicationTypeId,
            IsActive = true,
            CreatedBy = compOffCredit.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        compOffCredit.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();

        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendCompOffCreditRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    staffName: staffName,
                    id: compOffCredit.Id,
                    applicationTypeId: compOffCreditDto.ApplicationTypeId,
                    workedDate: compOffCreditDto.WorkedDate,
                    totalDays: compOffCreditDto.TotalDays,
                    balance: compOffCredit.Balance,
                    reason: compOffCreditDto.Reason,
                    requestDate: requestDateTime,
                    createdBy: staffOrCreatorId
                );
            }
        }
        return message;
    }

    public async Task<string> CreateAsync(CompOffAvailRequest request)
    {
        var message = "CompOff Avail request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        await AttendanceFreeze(staffOrCreatorId, request.FromDate, request.ToDate);
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var isHolidayWorkingExists = await _context.CompOffCredits.AnyAsync(h => h.WorkedDate == request.WorkedDate && (h.StaffId ?? h.CreatedBy) == staffOrCreatorId && !h.IsActive);
        if (!isHolidayWorkingExists)
        {
            throw new ConflictException("WorkedDate does not match the date in CompOffCredit");
        }
        var isHolidayWorkingExist = await _context.CompOffAvails.AnyAsync(h => h.WorkedDate == request.WorkedDate && (h.StaffId ?? h.CreatedBy) == staffOrCreatorId);
        if (isHolidayWorkingExist)
        {
            throw new ConflictException("CompOff Avail request already exists for the requested worked date");
        }
        var existingLeaves = await _context.CompOffAvails
        .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                     (lr.FromDate <= request.ToDate &&
                      lr.ToDate >= request.FromDate))
        .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.FromDate == request.FromDate;
            bool sameEndDate = existingLeave.ToDate == request.ToDate;
            bool sameDay = request.FromDate == request.ToDate &&
                           existingLeave.FromDate == existingLeave.ToDate &&
                           existingLeave.FromDate == request.FromDate;

            // Allow complementary half-days on the same start date
            if (sameStartDate &&
                ((existingLeave.FromDuration == "First Half" && request.FromDuration == "Second Half") ||
                 (existingLeave.FromDuration == "Second Half" && request.FromDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-days on the same end date
            if (sameEndDate &&
                ((existingLeave.ToDuration == "First Half" && request.ToDuration == "Second Half") ||
                 (existingLeave.ToDuration == "Second Half" && request.ToDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-day request on same single day
            if (sameDay)
            {
                if ((existingLeave.FromDuration == "First Half" && request.FromDuration == "Second Half") ||
                    (existingLeave.FromDuration == "Second Half" && request.FromDuration == "First Half"))
                {
                    continue;
                }

                if (existingLeave.FromDuration == request.FromDuration)
                {
                    throw new ConflictException("CompOff Avail request already exists");
                }
            }

            // Prevent full day overlap
            if ((existingLeave.FromDuration == "Full Day" || request.FromDuration == "Full Day") ||
                (existingLeave.ToDuration == "Full Day" || request.ToDuration == "Full Day"))
            {
                throw new ConflictException("CompOff Avail request already exists");
            }

            // Prevent general date overlap
            if (existingLeave.FromDate <= request.ToDate &&
                existingLeave.ToDate >= request.FromDate)
            {
                throw new ConflictException("CompOff Avail request already exists");
            }
        }
        var lastCompOffCredit = await _context.CompOffCredits
            .Where(c => (c.StaffId ?? c.CreatedBy) == staffOrCreatorId)
            .OrderByDescending(c => c.CreatedUtc)
            .FirstOrDefaultAsync();
        if (lastCompOffCredit == null)
        {
            throw new MessageNotFoundException("No CompOff Credit record found");
        }
        if (lastCompOffCredit.Balance == 0)
        {
            throw new ConflictException("No CompOff Credit balance found");
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
        await _context.CompOffAvails.AddAsync(compOff);
        await _context.SaveChangesAsync();

        if (lastCompOffCredit.TotalDays == 0) throw new MessageNotFoundException("Insufficient CompOff balance found");
        if (lastCompOffCredit != null && lastCompOffCredit.Balance > 0)
        {
            lastCompOffCredit.Balance = (lastCompOffCredit.Balance ?? 0) - (int)compOff.TotalDays;
            lastCompOffCredit.UpdatedBy = staffOrCreatorId;
            lastCompOffCredit.UpdatedUtc = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();
        string requestDateTime = compOff.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == request.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = request.CreatedBy,
            Message = $"{requestedBy} has submitted a CompOff Avail request on {requestDateTime}",
            ApplicationTypeId = compOff.ApplicationTypeId,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        compOff.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendCompOffApprovalRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    staffName: staffName,
                    id: compOff.Id,
                    applicationTypeId: compOff.ApplicationTypeId,
                    workedDate: request.WorkedDate,
                    fromDate: request.FromDate,
                    toDate: request.ToDate,
                    totalDays: request.TotalDays,
                    reason: request.Reason,
                    requestDate: requestDateTime,
                    createdBy: staffOrCreatorId
                );
            }
        }
        return message;
    }

    public async Task<List<CompOffAvailDto>> GetCompOffAvail()
    {
        var compOffs = await (from compOff in _context.CompOffAvails
                              where compOff.IsActive
                              select new CompOffAvailDto
                              {
                                  Id = compOff.Id,
                                  staffId = compOff.StaffId ?? compOff.CreatedBy,
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

    public async Task<List<CompOffCreditResponseDto>> GetCompOffCredit()
    {
        var compOffs = await (from compOff in _context.CompOffCredits
                              where compOff.IsActive
                              select new CompOffCreditResponseDto
                              {
                                  Id = compOff.Id,
                                  StaffId = compOff.StaffId ?? compOff.CreatedBy,
                                  ApplicationTypeId = compOff.ApplicationTypeId,
                                  WorkedDate = compOff.WorkedDate,
                                  Reason = compOff.Reason,
                                  TotalDays = compOff.TotalDays,
                              }).ToListAsync();
        if (compOffs.Count == 0)
        {
            throw new MessageNotFoundException("No CompOff records found");
        }
        return compOffs;
    }

    public async Task<List<object>> GetApplicationRequisition(int approverId, List<int>? staffIds, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate)
    {
        await NotFoundMethod(applicationTypeId);
        var approver = await _context.StaffCreations
            .Where(x => x.Id == approverId && x.IsActive == true)
            .Select(x => x.AccessLevel)
            .FirstOrDefaultAsync();
        bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
        List<object> result = new List<object>();
        if (applicationTypeId == 1)
        {
            var leaveQuery =
                from leave in _context.LeaveRequisitions
                join leaveType in _context.LeaveTypes on leave.LeaveTypeId equals leaveType.Id
                let staffIdToUse = leave.StaffId ?? leave.CreatedBy
                join application in _context.ApplicationTypes on leave.ApplicationTypeId equals application.Id
                join leaveStaff in _context.StaffCreations on leave.StaffId equals leaveStaff.Id into leaveStaffJoin
                from leaveStaff in leaveStaffJoin.DefaultIfEmpty()
                join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                where leave.IsActive == true
                      && (leave.IsCancelled == null || leave.IsCancelled == false)
                      && creatorStaff.IsActive == true
                      && (leave.StaffId == null || leaveStaff.IsActive == true)
                      && (!fromDate.HasValue || leave.FromDate >= fromDate)
                      && (!toDate.HasValue || leave.ToDate <= toDate)
                      && (isSuperAdmin
                          || (
                              (
                                  leave.StaffId.HasValue &&
                                  leaveStaff.ApprovalLevel1 == approverId &&
                                  leave.Status1 == null
                              ) ||
                              (
                                  !leave.StaffId.HasValue &&
                                  creatorStaff.ApprovalLevel1 == approverId &&
                                  leave.Status1 == null
                              ) ||
                              (
                                  leave.StaffId.HasValue &&
                                  leaveStaff.ApprovalLevel2 == approverId &&
                                  leave.Status1 == true &&
                                  leave.Status2 == null
                              ) ||
                              (
                                  !leave.StaffId.HasValue &&
                                  creatorStaff.ApprovalLevel2 == approverId &&
                                  leave.Status1 == true &&
                                  leave.Status2 == null
                              )
                          ))
                      && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                orderby leave.Id descending
                select new
                {
                    leave.Id,
                    leave.ApplicationTypeId,
                    ApplicationType = application.Name,
                    StaffId = staffIdToUse,
                    StaffName = leave.StaffId.HasValue
                        ? $"{leaveStaff.FirstName}{(string.IsNullOrWhiteSpace(leaveStaff.LastName) ? "" : " " + leaveStaff.LastName)}"
                        : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                    leave.StartDuration,
                    leave.EndDuration,
                    LeaveType = leaveType.Name,
                    leave.FromDate,
                    leave.ToDate,
                    leave.TotalDays,
                    leave.Reason
                };
            var getLeaves = await leaveQuery.ToListAsync();
            if (!getLeaves.Any()) throw new MessageNotFoundException("No Leave requisitions found");
            result.AddRange(getLeaves.Cast<object>());
        }
        else if (applicationTypeId == 2)
        {
#pragma warning disable CS8629 // Nullable value type may be null.
            var getCommonPermissions = await (from permission in _context.CommonPermissions
                                              let staffIdToUse = permission.StaffId ?? permission.CreatedBy
                                              join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                              join staff in _context.StaffCreations on permission.StaffId equals staff.Id into staffJoin
                                              from staff in staffJoin.DefaultIfEmpty()
                                              join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                              where permission.IsActive == true
                                                    && (permission.StaffId == null || staff.IsActive == true)
                                                    && creatorStaff.IsActive == true
                                                    && (permission.IsCancelled == null || permission.IsCancelled == false)
                                                    && (isSuperAdmin || approverId < 0
                                                        || (
                                                            (
                                                                permission.StaffId.HasValue &&
                                                                staff.ApprovalLevel1 == approverId &&
                                                                permission.Status1 == null &&
                                                                permission.ApplicationTypeId == 2
                                                            ) ||
                                                            (
                                                                !permission.StaffId.HasValue &&
                                                                creatorStaff.ApprovalLevel1 == approverId &&
                                                                permission.Status1 == null &&
                                                                permission.ApplicationTypeId == 2
                                                            ) ||
                                                            (
                                                                permission.StaffId.HasValue &&
                                                                staff.ApprovalLevel2 == approverId &&
                                                                permission.Status1 == true &&
                                                                permission.Status2 == null
                                                            ) ||
                                                            (
                                                                !permission.StaffId.HasValue &&
                                                                creatorStaff.ApprovalLevel2 == approverId &&
                                                                permission.Status1 == true &&
                                                                permission.Status2 == null
                                                            )
                                                        ))
                                                    && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                              orderby permission.Id descending
                                              select new
                                              {
                                                  permission.Id,
                                                  permission.ApplicationTypeId,
                                                  permission.PermissionType,
                                                  StaffId = staffIdToUse,
                                                  StaffName = permission.StaffId.HasValue
                                                      ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                      : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                                  permission.PermissionDate,
                                                  StartTime = permission.FromDate != null
                                                    ? permission.FromDate.Value.ToDateTime(permission.StartTime)
                                                    : (DateTime?)null,
                                                  EndTime = permission.ToDate != null
                                                    ? permission.ToDate.Value.ToDateTime(permission.EndTime)
                                                    : (DateTime?)null,
                                                  permission.TotalHours,
                                                  permission.Remarks
                                              }).ToListAsync();

            if (!getCommonPermissions.Any())
            {
                throw new MessageNotFoundException("No Common Permission requisitions found");
            }

            result.AddRange(getCommonPermissions.Cast<object>());
        }
        else if (applicationTypeId == 3)
        {
            var getManualPunch = await (from punch in _context.ManualPunchRequistions
                                        let staffIdToUse = punch.StaffId ?? punch.CreatedBy
                                        join application in _context.ApplicationTypes on punch.ApplicationTypeId equals application.Id
                                        join staff in _context.StaffCreations on punch.StaffId equals staff.Id into staffJoin
                                        from staff in staffJoin.DefaultIfEmpty()
                                        join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                        join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                        where punch.IsActive == true
                                              && (punch.StaffId == null || staff.IsActive == true)
                                              && creatorStaff.IsActive == true
                                              && (punch.IsCancelled == null || punch.IsCancelled == false)
                                              && (isSuperAdmin || approverId < 0 ||
                                                  (
                                                      (
                                                          punch.StaffId.HasValue &&
                                                          staff.ApprovalLevel1 == approverId &&
                                                          punch.Status1 == null
                                                      ) ||
                                                      (
                                                          !punch.StaffId.HasValue &&
                                                          creatorStaff.ApprovalLevel1 == approverId &&
                                                          punch.Status1 == null
                                                      ) ||
                                                      (
                                                          punch.StaffId.HasValue &&
                                                          staff.ApprovalLevel2 == approverId &&
                                                          punch.Status1 == true &&
                                                          punch.Status2 == null
                                                      ) ||
                                                      (
                                                          !punch.StaffId.HasValue &&
                                                          creatorStaff.ApprovalLevel2 == approverId &&
                                                          punch.Status1 == true &&
                                                          punch.Status2 == null
                                                      )
                                                  ))
                                              && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                        orderby punch.Id descending
                                        select new
                                        {
                                            punch.Id,
                                            punch.ApplicationTypeId,
                                            ApplicationType = application.Name,
                                            StaffId = staffIdToUse,
                                            StaffName = punch.StaffId.HasValue
                                                ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                            punch.SelectPunch,
                                            punch.InPunch,
                                            punch.OutPunch,
                                            punch.Remarks,
                                            punch.IsActive,
                                            punch.CreatedBy
                                        }).ToListAsync();

            if (!getManualPunch.Any())
            {
                throw new MessageNotFoundException("No Manual Punch requisitions found");
            }

            result.AddRange(getManualPunch.Cast<object>());
        }
        else if (applicationTypeId == 4)
        {
            var getOnDutyRequisitions = await (from duty in _context.OnDutyRequisitions
                                               let staffIdToUse = duty.StaffId ?? duty.CreatedBy
                                               join application in _context.ApplicationTypes on duty.ApplicationTypeId equals application.Id
                                               join staff in _context.StaffCreations on duty.StaffId equals staff.Id into dutyStaffJoin
                                               from staff in dutyStaffJoin.DefaultIfEmpty()
                                               join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                               join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                               where duty.IsActive == true
                                                     && (duty.StaffId == null || staff.IsActive == true)
                                                     && creatorStaff.IsActive == true
                                                     && (duty.IsCancelled == null || duty.IsCancelled == false)
                                                     && (!fromDate.HasValue || duty.StartDate >= fromDate)
                                                     && (!toDate.HasValue || duty.EndDate <= toDate)
                                                     && (isSuperAdmin || approverId < 0 ||
                                                         (
                                                             (
                                                                 duty.StaffId.HasValue &&
                                                                 staff.ApprovalLevel1 == approverId &&
                                                                 duty.Status1 == null
                                                             ) ||
                                                             (
                                                                 !duty.StaffId.HasValue &&
                                                                 creatorStaff.ApprovalLevel1 == approverId &&
                                                                 duty.Status1 == null
                                                             ) ||
                                                             (
                                                                 duty.StaffId.HasValue &&
                                                                 staff.ApprovalLevel2 == approverId &&
                                                                 duty.Status1 == true &&
                                                                 duty.Status2 == null
                                                             ) ||
                                                             (
                                                                 !duty.StaffId.HasValue &&
                                                                 creatorStaff.ApprovalLevel2 == approverId &&
                                                                 duty.Status1 == true &&
                                                                 duty.Status2 == null
                                                             )
                                                         ))
                                                     && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                               orderby duty.Id descending
                                               select new
                                               {
                                                   duty.Id,
                                                   duty.ApplicationTypeId,
                                                   ApplicationType = application.Name,
                                                   StaffId = staffIdToUse,
                                                   StaffName = duty.StaffId.HasValue
                                                       ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                       : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                                   duty.StartDuration,
                                                   duty.EndDuration,
                                                   duty.StartDate,
                                                   duty.EndDate,
                                                   duty.StartTime,
                                                   duty.EndTime,
                                                   duty.Reason,
                                                   duty.CreatedBy
                                               }).ToListAsync();

            if (!getOnDutyRequisitions.Any())
            {
                throw new MessageNotFoundException("No On Duty requisitions found");
            }

            result.AddRange(getOnDutyRequisitions.Cast<object>());
        }
        else if (applicationTypeId == 5)
        {
            var getBusinessTravels = await (from travel in _context.BusinessTravels
                                            let staffIdToUse = travel.StaffId ?? travel.CreatedBy
                                            join application in _context.ApplicationTypes on travel.ApplicationTypeId equals application.Id
                                            join staff in _context.StaffCreations on travel.StaffId equals staff.Id into travelStaffJoin
                                            from staff in travelStaffJoin.DefaultIfEmpty()
                                            join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                            join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                            where travel.IsActive == true
                                                  && (travel.StaffId == null || staff.IsActive == true)
                                                  && creatorStaff.IsActive == true
                                                  && (travel.IsCancelled == null || travel.IsCancelled == false)
                                                  && (!fromDate.HasValue || travel.FromDate >= fromDate)
                                                  && (!toDate.HasValue || travel.ToDate <= toDate)
                                                  && (isSuperAdmin || approverId < 0 ||
                                                      (
                                                          (
                                                              travel.StaffId.HasValue &&
                                                              staff.ApprovalLevel1 == approverId &&
                                                              travel.Status1 == null
                                                          ) ||
                                                          (
                                                              !travel.StaffId.HasValue &&
                                                              creatorStaff.ApprovalLevel1 == approverId &&
                                                              travel.Status1 == null
                                                          ) ||
                                                          (
                                                              travel.StaffId.HasValue &&
                                                              staff.ApprovalLevel2 == approverId &&
                                                              travel.Status1 == true &&
                                                              travel.Status2 == null
                                                          ) ||
                                                          (
                                                              !travel.StaffId.HasValue &&
                                                              creatorStaff.ApprovalLevel2 == approverId &&
                                                              travel.Status1 == true &&
                                                              travel.Status2 == null
                                                          )
                                                      ))
                                                  && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                            orderby travel.Id descending
                                            select new
                                            {
                                                travel.Id,
                                                travel.ApplicationTypeId,
                                                ApplicationType = application.Name,
                                                StaffId = staffIdToUse,
                                                StaffName = travel.StaffId.HasValue
                                                    ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                    : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                                travel.StartDuration,
                                                travel.EndDuration,
                                                travel.FromDate,
                                                travel.ToDate,
                                                travel.FromTime,
                                                travel.ToTime,
                                                travel.Reason,
                                                travel.CreatedBy
                                            }).ToListAsync();

            if (!getBusinessTravels.Any())
            {
                throw new MessageNotFoundException("No Business Travel requisitions found");
            }

            result.AddRange(getBusinessTravels.Cast<object>());
        }
        else if (applicationTypeId == 6)
        {
            var getWorkFromHomes = await (from workFromHome in _context.WorkFromHomes
                                          let staffIdToUse = workFromHome.StaffId ?? workFromHome.CreatedBy
                                          join application in _context.ApplicationTypes on workFromHome.ApplicationTypeId equals application.Id
                                          join staff in _context.StaffCreations on workFromHome.StaffId equals staff.Id into workFromHomeStaffJoin
                                          from staff in workFromHomeStaffJoin.DefaultIfEmpty()
                                          join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                          join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                          where workFromHome.IsActive == true
                                                && (workFromHome.StaffId == null || staff.IsActive == true)
                                                && creatorStaff.IsActive == true
                                                && (workFromHome.IsCancelled == null || workFromHome.IsCancelled == false)
                                                && (!fromDate.HasValue || workFromHome.FromDate >= fromDate)
                                                && (!toDate.HasValue || workFromHome.ToDate <= toDate)
                                                && (isSuperAdmin || approverId < 0 ||
                                                    (
                                                        (
                                                            workFromHome.StaffId.HasValue &&
                                                            staff.ApprovalLevel1 == approverId &&
                                                            workFromHome.Status1 == null
                                                        ) ||
                                                        (
                                                            !workFromHome.StaffId.HasValue &&
                                                            creatorStaff.ApprovalLevel1 == approverId &&
                                                            workFromHome.Status1 == null
                                                        ) ||
                                                        (
                                                            workFromHome.StaffId.HasValue &&
                                                            staff.ApprovalLevel2 == approverId &&
                                                            workFromHome.Status1 == true &&
                                                            workFromHome.Status2 == null
                                                        ) ||
                                                        (
                                                            !workFromHome.StaffId.HasValue &&
                                                            creatorStaff.ApprovalLevel2 == approverId &&
                                                            workFromHome.Status1 == true &&
                                                            workFromHome.Status2 == null
                                                        )
                                                    ))
                                                && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                          orderby workFromHome.Id descending
                                          select new
                                          {
                                              workFromHome.Id,
                                              workFromHome.ApplicationTypeId,
                                              ApplicationType = application.Name,
                                              StaffId = staffIdToUse,
                                              StaffName = workFromHome.StaffId.HasValue
                                                  ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                  : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                              workFromHome.StartDuration,
                                              workFromHome.EndDuration,
                                              workFromHome.FromDate,
                                              workFromHome.ToDate,
                                              workFromHome.FromTime,
                                              workFromHome.ToTime,
                                              workFromHome.Reason,
                                              workFromHome.CreatedBy
                                          }).ToListAsync();

            if (!getWorkFromHomes.Any())
            {
                throw new MessageNotFoundException("No Work From Home requisitions found");
            }

            result.AddRange(getWorkFromHomes.Cast<object>());
        }
        else if (applicationTypeId == 7)
        {
            var getShiftChanges = await (from shiftChange in _context.ShiftChanges
                                         join application in _context.ApplicationTypes on shiftChange.ApplicationTypeId equals application.Id
                                         let staffIdToUse = shiftChange.StaffId ?? shiftChange.CreatedBy
                                         join shift in _context.Shifts on shiftChange.ShiftId equals shift.Id
                                         join staff in _context.StaffCreations on shiftChange.StaffId equals staff.Id into staffJoin
                                         from staff in staffJoin.DefaultIfEmpty()
                                         join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                         join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                         where shiftChange.IsActive == true
                                               && (shiftChange.StaffId == null || staff.IsActive == true)
                                               && creatorStaff.IsActive == true
                                               && (shiftChange.IsCancelled == null || shiftChange.IsCancelled == false)
                                               && (!fromDate.HasValue || shiftChange.FromDate >= fromDate)
                                               && (!toDate.HasValue || shiftChange.ToDate <= toDate)
                                               && (isSuperAdmin || approverId < 0 ||
                                                   (
                                                       (
                                                           shiftChange.StaffId.HasValue &&
                                                           staff.ApprovalLevel1 == approverId &&
                                                           shiftChange.Status1 == null
                                                       ) ||
                                                       (
                                                           !shiftChange.StaffId.HasValue &&
                                                           creatorStaff.ApprovalLevel1 == approverId &&
                                                           shiftChange.Status1 == null
                                                       ) ||
                                                       (
                                                           shiftChange.StaffId.HasValue &&
                                                           staff.ApprovalLevel2 == approverId &&
                                                           shiftChange.Status1 == true &&
                                                           shiftChange.Status2 == null
                                                       ) ||
                                                       (
                                                           !shiftChange.StaffId.HasValue &&
                                                           creatorStaff.ApprovalLevel2 == approverId &&
                                                           shiftChange.Status1 == true &&
                                                           shiftChange.Status2 == null
                                                       )
                                                   ))
                                               && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                         orderby shiftChange.Id descending
                                         select new
                                         {
                                             shiftChange.Id,
                                             shiftChange.ApplicationTypeId,
                                             ApplicationType = application.Name,
                                             StaffId = staffIdToUse,
                                             StaffName = shiftChange.StaffId.HasValue
                                                 ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                 : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                             shiftChange.FromDate,
                                             shiftChange.ToDate,
                                             shiftChange.Reason,
                                             shiftChange.CreatedBy,
                                             ShiftName = shift.Name
                                         }).ToListAsync();

            if (!getShiftChanges.Any())
            {
                throw new MessageNotFoundException("No Shift Change requisitions found");
            }

            result.AddRange(getShiftChanges.Cast<object>());
        }
        else if (applicationTypeId == 8)
        {
            var getShiftExtensions = await (from shiftExtension in _context.ShiftExtensions
                                            join staff in _context.StaffCreations on shiftExtension.StaffId equals staff.Id into staffJoin
                                            let staffIdToUse = shiftExtension.StaffId ?? shiftExtension.CreatedBy
                                            from staff in staffJoin.DefaultIfEmpty()
                                            join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                            join application in _context.ApplicationTypes on shiftExtension.ApplicationTypeId equals application.Id
                                            join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                            where shiftExtension.IsActive == true
                                                  && (shiftExtension.StaffId == null || staff.IsActive == true)
                                                  && creatorStaff.IsActive == true
                                                  && (shiftExtension.IsCancelled == null || shiftExtension.IsCancelled == false)
                                                  && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                                  && (isSuperAdmin || approverId < 0 ||
                                                      (
                                                          (
                                                              shiftExtension.StaffId.HasValue &&
                                                              staff.ApprovalLevel1 == approverId &&
                                                              shiftExtension.Status1 == null
                                                          ) ||
                                                          (
                                                              !shiftExtension.StaffId.HasValue &&
                                                              creatorStaff.ApprovalLevel1 == approverId &&
                                                              shiftExtension.Status1 == null
                                                          ) ||
                                                          (
                                                              shiftExtension.StaffId.HasValue &&
                                                              staff.ApprovalLevel2 == approverId &&
                                                              shiftExtension.Status1 == true &&
                                                              shiftExtension.Status2 == null
                                                          ) ||
                                                          (
                                                              !shiftExtension.StaffId.HasValue &&
                                                              creatorStaff.ApprovalLevel2 == approverId &&
                                                              shiftExtension.Status1 == true &&
                                                              shiftExtension.Status2 == null
                                                          )
                                                      ))
                                                  && (!fromDate.HasValue || shiftExtension.TransactionDate >= fromDate)
                                                  && (!toDate.HasValue || shiftExtension.TransactionDate <= toDate)
                                            orderby shiftExtension.Id descending
                                            select new
                                            {
                                                shiftExtension.Id,
                                                shiftExtension.ApplicationTypeId,
                                                ApplicationType = application.Name,
                                                StaffId = staffIdToUse,
                                                StaffName = shiftExtension.StaffId.HasValue
                                                    ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                    : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                                shiftExtension.TransactionDate,
                                                shiftExtension.DurationHours,
                                                shiftExtension.BeforeShiftHours,
                                                shiftExtension.AfterShiftHours,
                                                shiftExtension.Remarks,
                                                shiftExtension.CreatedBy
                                            }).ToListAsync();

            if (!getShiftExtensions.Any())
            {
                throw new MessageNotFoundException("No Shift Extension requisitions found");
            }

            result.AddRange(getShiftExtensions.Cast<object>());
        }
        else if (applicationTypeId == 9)
        {
            var getWeeklyOffHolidayWorking = await (from holidayWorking in _context.WeeklyOffHolidayWorkings
                                                    join staff in _context.StaffCreations on holidayWorking.StaffId equals staff.Id into staffJoin
                                                    let staffIdToUse = holidayWorking.StaffId ?? holidayWorking.CreatedBy
                                                    from staff in staffJoin.DefaultIfEmpty()
                                                    join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                                    join application in _context.ApplicationTypes on holidayWorking.ApplicationTypeId equals application.Id
                                                    join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                                    where holidayWorking.IsActive == true
                                                          && (holidayWorking.StaffId == null || staff.IsActive == true)
                                                          && creatorStaff.IsActive == true
                                                          && holidayWorking.IsCancelled == null
                                                          && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                                          && (isSuperAdmin || approverId < 0 ||
                                                              (
                                                                  (
                                                                      holidayWorking.StaffId.HasValue &&
                                                                      staff.ApprovalLevel1 == approverId &&
                                                                      holidayWorking.Status1 == null
                                                                  ) ||
                                                                  (
                                                                      !holidayWorking.StaffId.HasValue &&
                                                                      creatorStaff.ApprovalLevel1 == approverId &&
                                                                      holidayWorking.Status1 == null
                                                                  ) ||
                                                                  (
                                                                      holidayWorking.StaffId.HasValue &&
                                                                      staff.ApprovalLevel2 == approverId &&
                                                                      holidayWorking.Status1 == true &&
                                                                      holidayWorking.Status2 == null
                                                                  ) ||
                                                                  (
                                                                      !holidayWorking.StaffId.HasValue &&
                                                                      creatorStaff.ApprovalLevel2 == approverId &&
                                                                      holidayWorking.Status1 == true &&
                                                                      holidayWorking.Status2 == null
                                                                  )
                                                              ))
                                                          && (!fromDate.HasValue || holidayWorking.TxnDate >= fromDate)
                                                          && (!toDate.HasValue || holidayWorking.TxnDate <= toDate)
                                                    orderby holidayWorking.Id descending
                                                    select new
                                                    {
                                                        holidayWorking.Id,
                                                        holidayWorking.ApplicationTypeId,
                                                        ApplicationType = application.Name,
                                                        StaffId = staffIdToUse,
                                                        StaffName = holidayWorking.StaffId.HasValue
                                                            ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                            : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                                        holidayWorking.TxnDate,
                                                        holidayWorking.SelectShiftType,
                                                        holidayWorking.ShiftId,
                                                        holidayWorking.ShiftInTime,
                                                        holidayWorking.ShiftOutTime,
                                                        holidayWorking.CreatedBy
                                                    }).ToListAsync();

            if (!getWeeklyOffHolidayWorking.Any())
            {
                throw new MessageNotFoundException("No Weekly Off/ Holiday Working requisitions found");
            }

            result.AddRange(getWeeklyOffHolidayWorking.Cast<object>());
        }
        else if (applicationTypeId == 10)
        {
            var getCompOffAvail = await (from compOff in _context.CompOffAvails
                                         join staff in _context.StaffCreations on compOff.StaffId equals staff.Id into staffJoin
                                         let staffIdToUse = compOff.StaffId ?? compOff.CreatedBy
                                         from staff in staffJoin.DefaultIfEmpty()
                                         join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                         join application in _context.ApplicationTypes on compOff.ApplicationTypeId equals application.Id
                                         join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                         where compOff.IsActive == true
                                               && (compOff.StaffId == null || staff.IsActive == true)
                                               && creatorStaff.IsActive == true
                                               && compOff.IsCancelled == null
                                               && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                               && (isSuperAdmin || approverId < 0 ||
                                                   (
                                                       (
                                                           compOff.StaffId.HasValue &&
                                                           staff.ApprovalLevel1 == approverId &&
                                                           compOff.Status1 == null
                                                       ) ||
                                                       (
                                                           !compOff.StaffId.HasValue &&
                                                           creatorStaff.ApprovalLevel1 == approverId &&
                                                           compOff.Status1 == null
                                                       ) ||
                                                       (
                                                           compOff.StaffId.HasValue &&
                                                           staff.ApprovalLevel2 == approverId &&
                                                           compOff.Status1 == true &&
                                                           compOff.Status2 == null
                                                       ) ||
                                                       (
                                                           !compOff.StaffId.HasValue &&
                                                           creatorStaff.ApprovalLevel2 == approverId &&
                                                           compOff.Status1 == true &&
                                                           compOff.Status2 == null
                                                       )
                                                   ))
                                               && (!fromDate.HasValue || compOff.FromDate >= fromDate)
                                               && (!toDate.HasValue || compOff.ToDate <= toDate)
                                         orderby compOff.Id descending
                                         select new
                                         {
                                             compOff.Id,
                                             compOff.ApplicationTypeId,
                                             ApplicationType = application.Name,
                                             StaffId = staffIdToUse,
                                             StaffName = compOff.StaffId.HasValue
                                                 ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                 : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                             compOff.WorkedDate,
                                             compOff.FromDate,
                                             compOff.ToDate,
                                             compOff.FromDuration,
                                             compOff.ToDuration,
                                             compOff.Reason,
                                             compOff.TotalDays,
                                             compOff.CreatedBy
                                         }).ToListAsync();

            if (!getCompOffAvail.Any())
            {
                throw new MessageNotFoundException("No Comp Off Avail requisitions found");
            }

            result.AddRange(getCompOffAvail.Cast<object>());
        }
        else if (applicationTypeId == 11)
        {
            var getCompOffCredit = await (from compOff in _context.CompOffCredits
                                          join staff in _context.StaffCreations on compOff.StaffId equals staff.Id into staffJoin
                                          let staffIdToUse = compOff.StaffId ?? compOff.CreatedBy
                                          from staff in staffJoin.DefaultIfEmpty()
                                          join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                          join application in _context.ApplicationTypes on compOff.ApplicationTypeId equals application.Id
                                          join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                          where compOff.IsActive == true
                                                && (compOff.StaffId == null || staff.IsActive == true)
                                                && creatorStaff.IsActive == true
                                                && compOff.IsCancelled == null
                                                && (staffIds == null || !staffIds.Any() || staffIds.Contains(staffIdToUse))
                                                && (isSuperAdmin || approverId < 0 ||
                                                    (
                                                        (
                                                            compOff.StaffId.HasValue &&
                                                            staff.ApprovalLevel1 == approverId &&
                                                            compOff.Status1 == null
                                                        ) ||
                                                        (
                                                            !compOff.StaffId.HasValue &&
                                                            creatorStaff.ApprovalLevel1 == approverId &&
                                                            compOff.Status1 == null
                                                        ) ||
                                                        (
                                                            compOff.StaffId.HasValue &&
                                                            staff.ApprovalLevel2 == approverId &&
                                                            compOff.Status1 == true &&
                                                            compOff.Status2 == null
                                                        ) ||
                                                        (
                                                            !compOff.StaffId.HasValue &&
                                                            creatorStaff.ApprovalLevel2 == approverId &&
                                                            compOff.Status1 == true &&
                                                            compOff.Status2 == null
                                                        )
                                                    ))
                                          orderby compOff.Id descending
                                          select new
                                          {
                                              compOff.Id,
                                              compOff.ApplicationTypeId,
                                              ApplicationType = application.Name,
                                              StaffId = staffIdToUse,
                                              StaffName = compOff.StaffId.HasValue
                                                  ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                                  : $"{creatorStaff.FirstName}{(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                              compOff.WorkedDate,
                                              compOff.TotalDays,
                                              compOff.Reason,
                                              compOff.CreatedBy
                                          }).ToListAsync();

            if (!getCompOffCredit.Any())
            {
                throw new MessageNotFoundException("No Comp Off Credit requisitions found");
            }

            result.AddRange(getCompOffCredit.Cast<object>());
        }
        else if (applicationTypeId == 18)
        {
            var getReimbursements = await (from reimbursement in _context.Reimbursements
                                           join staff in _context.StaffCreations on reimbursement.StaffId equals staff.Id into staffJoin
                                           let staffIdToUse = reimbursement.StaffId ?? reimbursement.CreatedBy
                                           from staff in staffJoin.DefaultIfEmpty()
                                           join creatorStaff in _context.StaffCreations on staffIdToUse equals creatorStaff.Id
                                           join reimbursementType in _context.ReimbursementTypes on reimbursement.ReimbursementTypeId equals reimbursementType.Id
                                           where reimbursement.IsActive == true
                                                 && reimbursement.IsCancelled == null
                                                 && (fromDate == null || reimbursement.BillDate >= fromDate)
                                                 && (toDate == null || reimbursement.BillDate <= toDate)
                                                 && (staffIds == null || !staffIds.Any() || (staffIds.Contains(reimbursement.StaffId ?? reimbursement.CreatedBy)))
                                                 && (isSuperAdmin || approverId < 0 ||
                                                     ((
                                                         (reimbursement.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                          reimbursement.Status1 == null && reimbursement.ApplicationTypeId == 18) ||
                                                         (!reimbursement.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                          reimbursement.Status1 == null && reimbursement.ApplicationTypeId == 18)
                                                     ) ||
                                                     (
                                                         (reimbursement.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                          reimbursement.Status1 == true && reimbursement.Status2 == null &&
                                                          reimbursement.ApplicationTypeId == 18) ||
                                                         (!reimbursement.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                          reimbursement.Status1 == true && reimbursement.Status2 == null &&
                                                          reimbursement.ApplicationTypeId == 18)
                                                     )))
                                           orderby reimbursement.Id descending
                                           select new
                                           {
                                               reimbursement.Id,
                                               reimbursement.BillDate,
                                               reimbursement.BillNo,
                                               reimbursement.Description,
                                               reimbursement.BillPeriod,
                                               reimbursement.Amount,
                                               reimbursement.UploadFilePath,
                                               reimbursementType.Name,
                                               StaffId = reimbursement.StaffId ?? reimbursement.CreatedBy,
                                               StaffName = reimbursement.StaffId.HasValue ? $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}"
                                               : $"{creatorStaff.FirstName} {(string.IsNullOrWhiteSpace(creatorStaff.LastName) ? "" : " " + creatorStaff.LastName)}",
                                               reimbursement.Status1,
                                               reimbursement.Status2,
                                               reimbursement.CreatedUtc,
                                               reimbursement.CreatedBy
                                           }).ToListAsync();

            if (!getReimbursements.Any())
            {
                throw new MessageNotFoundException("No Reimbursement requisitions found");
            }
            result.AddRange(getReimbursements.Cast<object>());
        }
        return result;
    }

    public async Task<List<ApprovalNotificationResponse>> GetApprovalNotifications(int staffId)
    {
        var profile = await _context.StaffCreations
            .Where(s => s.Id == staffId && s.IsActive == true)
            .Select(s => s.ProfilePhoto)
            .FirstOrDefaultAsync();
        var tenDaysAgo = DateTime.UtcNow.AddDays(-10);
        var notifications = await (from notification in _context.ApprovalNotifications
                                   where notification.StaffId == staffId && notification.IsActive && notification.CreatedUtc >= tenDaysAgo && notification.ApplicationTypeId == null
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
            throw new MessageNotFoundException("No approval notifications found");
        }
        return notifications;
    }

    public async Task<List<RequestNotificationResponse>> GetRequestNotifications(int staffId)
    {
        var approverRole = await _context.StaffCreations
            .Where(x => x.Id == staffId && x.IsActive == true)
            .Select(x => x.AccessLevel)
            .FirstOrDefaultAsync();
        var results = new List<RequestNotificationResponse>();
        async Task<int> GetCount<T>(DbSet<T> dbSet, int appTypeId) where T : class
        {
            var query = dbSet
                .Join(_context.StaffCreations,
                      r => EF.Property<int?>(r, "StaffId") ?? EF.Property<int?>(r, "CreatedBy"),
                      s => s.Id,
                      (r, s) => new { Requisition = r, Staff = s })
                .Where(x =>
                    EF.Property<bool>(x.Requisition, "IsActive") &&
                    EF.Property<int>(x.Requisition, "ApplicationTypeId") == appTypeId &&
                    (
                        (EF.Property<bool?>(x.Requisition, "Status1") == null && (x.Staff.ApprovalLevel1 == staffId))
                        || (EF.Property<bool?>(x.Requisition, "Status1") == true &&
                            EF.Property<bool?>(x.Requisition, "Status2") == null &&
                            x.Staff.ApprovalLevel2 == staffId)
                    ));

            return await query.CountAsync();
        }

        results.Add(new RequestNotificationResponse { ApplicationTypeId = 1, PendingCount = await GetCount(_context.LeaveRequisitions, 1) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 2, PendingCount = await GetCount(_context.CommonPermissions, 2) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 3, PendingCount = await GetCount(_context.ManualPunchRequistions, 3) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 4, PendingCount = await GetCount(_context.OnDutyRequisitions, 4) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 5, PendingCount = await GetCount(_context.BusinessTravels, 5) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 6, PendingCount = await GetCount(_context.WorkFromHomes, 6) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 7, PendingCount = await GetCount(_context.ShiftChanges, 7) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 8, PendingCount = await GetCount(_context.ShiftExtensions, 8) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 9, PendingCount = await GetCount(_context.WeeklyOffHolidayWorkings, 9) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 10, PendingCount = await GetCount(_context.CompOffAvails, 10) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 11, PendingCount = await GetCount(_context.CompOffCredits, 11) });
        results.Add(new RequestNotificationResponse { ApplicationTypeId = 18, PendingCount = await GetCount(_context.Reimbursements, 18) });

        if (results.All(r => r.PendingCount == 0)) throw new MessageNotFoundException("No pending approvals found");
        return results.ToList();
    }

    public async Task<string> UpdateApprovalNotifications(int staffId, int notificationId)
    {
        var message = "";
        var notification = await _context.ApprovalNotifications.FirstOrDefaultAsync(n => n.StaffId == staffId && n.Id == notificationId && n.IsActive);
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
        var message = "Leave request submitted successfully.";
        var staffOrCreatorId = leaveRequisitionRequest.StaffId ?? leaveRequisitionRequest.CreatedBy;
        await AttendanceFreeze(staffOrCreatorId, leaveRequisitionRequest.FromDate, leaveRequisitionRequest.ToDate);
        await NotFoundMethod(leaveRequisitionRequest.ApplicationTypeId);
        var individualLeave = await _context.IndividualLeaveCreditDebits
        .Where(l => l.StaffCreationId == staffOrCreatorId
                    && l.LeaveTypeId == leaveRequisitionRequest.LeaveTypeId
                    && l.IsActive == true)
        .OrderByDescending(l => l.Id)
        .FirstOrDefaultAsync();
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var leaveType = await _context.LeaveTypes.Where(l => l.Id == leaveRequisitionRequest.LeaveTypeId && l.IsActive).Select(l => l.Name).FirstOrDefaultAsync();
        if (leaveType == null) throw new MessageNotFoundException("Leave type not found");
        if (individualLeave == null || individualLeave.AvailableBalance == 0)
        {
            if (leaveRequisitionRequest.StaffId != null) throw new MessageNotFoundException($"No leave balance found for Staff {staffName}");
            else throw new MessageNotFoundException("No leave balance found");
        }
        if (individualLeave != null && (individualLeave.AvailableBalance > 0 && individualLeave.AvailableBalance < leaveRequisitionRequest.TotalDays))
        {
            if (leaveRequisitionRequest.StaffId != null) throw new ConflictException($"Insufficient leave balance found for Staff {staffName}");
            else throw new ConflictException("Insufficient leave balance found");
        }
        var existingLeaves = await _context.LeaveRequisitions
         .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                      (lr.FromDate <= leaveRequisitionRequest.ToDate &&
                       lr.ToDate >= leaveRequisitionRequest.FromDate))
         .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.FromDate == leaveRequisitionRequest.FromDate;
            bool sameEndDate = existingLeave.ToDate == leaveRequisitionRequest.ToDate;
            bool sameDay = leaveRequisitionRequest.FromDate == leaveRequisitionRequest.ToDate &&
                           existingLeave.FromDate == existingLeave.ToDate &&
                           existingLeave.FromDate == leaveRequisitionRequest.FromDate;

            // Allow complementary half-days on the same start date
            if (sameStartDate &&
                ((existingLeave.StartDuration == "First Half" && leaveRequisitionRequest.StartDuration == "Second Half") ||
                 (existingLeave.StartDuration == "Second Half" && leaveRequisitionRequest.StartDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-days on the same end date
            if (sameEndDate &&
                ((existingLeave.EndDuration == "First Half" && leaveRequisitionRequest.EndDuration == "Second Half") ||
                 (existingLeave.EndDuration == "Second Half" && leaveRequisitionRequest.EndDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-day request on same single day
            if (sameDay)
            {
                if ((existingLeave.StartDuration == "First Half" && leaveRequisitionRequest.StartDuration == "Second Half") ||
                    (existingLeave.StartDuration == "Second Half" && leaveRequisitionRequest.StartDuration == "First Half"))
                {
                    continue;
                }

                if (existingLeave.StartDuration == leaveRequisitionRequest.StartDuration)
                {
                    throw new ConflictException("Leave request already exists");
                }
            }

            // Prevent full day overlap
            if ((existingLeave.StartDuration == "Full Day" || leaveRequisitionRequest.StartDuration == "Full Day") ||
                (existingLeave.EndDuration == "Full Day" || leaveRequisitionRequest.EndDuration == "Full Day"))
            {
                throw new ConflictException("Leave request already exists");
            }

            // Prevent general date overlap
            if (existingLeave.FromDate <= leaveRequisitionRequest.ToDate &&
                existingLeave.ToDate >= leaveRequisitionRequest.FromDate)
            {
                throw new ConflictException("Leave request already exists");
            }
        }

        LeaveRequisition leaveRequisition = new LeaveRequisition
        {
            ApplicationTypeId = leaveRequisitionRequest.ApplicationTypeId,
            StartDuration = leaveRequisitionRequest.StartDuration,
            EndDuration = leaveRequisitionRequest.EndDuration,
            StaffId = leaveRequisitionRequest.StaffId,
            FromDate = leaveRequisitionRequest.FromDate,
            ToDate = leaveRequisitionRequest.ToDate,
            LeaveTypeId = leaveRequisitionRequest.LeaveTypeId,
            TotalDays = leaveRequisitionRequest.TotalDays,
            Reason = leaveRequisitionRequest.Reason,
            IsActive = true,
            CreatedBy = leaveRequisitionRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.LeaveRequisitions.AddAsync(leaveRequisition);
        await _context.SaveChangesAsync();

        if (individualLeave != null && individualLeave.AvailableBalance >= leaveRequisitionRequest.TotalDays)
        {
            individualLeave.AvailableBalance = decimal.Subtract(individualLeave.AvailableBalance, leaveRequisitionRequest.TotalDays);
            individualLeave.UpdatedBy = staffOrCreatorId;
            individualLeave.UpdatedUtc = DateTime.UtcNow;
        }
        else
        {
            if (leaveRequisition.StaffId != null) throw new ConflictException($"Insufficient leave balance found for Staff {staffName}");
            else throw new ConflictException("Insufficient leave balance found");
        }
        await _context.SaveChangesAsync();
        string requestDateTime = leaveRequisition.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == leaveRequisition.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = leaveRequisition.CreatedBy,
            Message = $"{requestedBy} has submitted a {leaveType} request on {requestDateTime}",
            ApplicationTypeId = leaveRequisitionRequest.ApplicationTypeId,
            IsActive = true,
            CreatedBy = leaveRequisition.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        leaveRequisition.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendLeaveRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    applicationTypeId: leaveRequisition.ApplicationTypeId,
                    id: leaveRequisition.Id,
                    leaveType: leaveType,
                    fromDate: leaveRequisitionRequest.FromDate,
                    toDate: leaveRequisitionRequest.ToDate,
                    fromDuration: leaveRequisitionRequest.StartDuration,
                    toDuration: leaveRequisitionRequest.EndDuration,
                    totalDays: leaveRequisitionRequest.TotalDays,
                    reason: leaveRequisitionRequest.Reason,
                    createdBy: staffOrCreatorId,
                    creatorName: staffName,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task<string> AddCommonPermissionAsync(CommonPermissionRequest commonPermissionRequest)
    {
        var message = "Common Permission request submitted successfully.";
        var staffOrCreatorId = commonPermissionRequest.StaffId ?? commonPermissionRequest.CreatedBy;
        await AttendanceFreezeDate(staffOrCreatorId, commonPermissionRequest.PermissionDate);
        await NotFoundMethod(commonPermissionRequest.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        CommonPermission commonPermission = new CommonPermission();
        var permissionDate = commonPermissionRequest.PermissionDate;
        var dayOfWeek = permissionDate.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Saturday) throw new ConflictException("Permission is not allowed on Saturdays");
        var hasLeaveOnDate = await _context.LeaveRequisitions.AnyAsync(l => ((commonPermissionRequest.StaffId != null && l.StaffId == (int?)commonPermissionRequest.StaffId) ||
            (commonPermissionRequest.StaffId != null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.StaffId) ||
            (commonPermissionRequest.StaffId == null && l.StaffId == (int?)commonPermissionRequest.CreatedBy) ||
            (commonPermissionRequest.StaffId == null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.CreatedBy)) &&
            permissionDate >= l.FromDate && permissionDate <= l.ToDate && (l.Status1 == null || l.Status1 == true) &&
            l.IsActive == true && (l.IsCancelled == false || l.IsCancelled == null));
        if (hasLeaveOnDate) throw new ConflictException($"Cannot apply for permission on {permissionDate:yyyy-MM-dd}, as leave is already taken");
        var startOfMonth = new DateOnly(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month, 1);
        var endOfMonth = new DateOnly(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month,
            DateTime.DaysInMonth(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month));
        var monthName = commonPermissionRequest.PermissionDate.ToString("MMMM");
        var list = await _context.CommonPermissions.ToListAsync();
        var existingPermissionOnDate = await _context.CommonPermissions
        .AnyAsync(l => ((commonPermissionRequest.StaffId != null && l.StaffId == (int?)commonPermissionRequest.StaffId) ||
            (commonPermissionRequest.StaffId != null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.StaffId) ||
            (commonPermissionRequest.StaffId == null && l.StaffId == (int?)commonPermissionRequest.CreatedBy) ||
            (commonPermissionRequest.StaffId == null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.CreatedBy)) &&
            l.PermissionDate == commonPermissionRequest.PermissionDate);
        if (existingPermissionOnDate) throw new ConflictException($"Permission for the date {commonPermissionRequest.PermissionDate:yyyy-MM-dd} already exists");
        var permissionsThisMonth = await _context.CommonPermissions
            .Where(l => ((commonPermissionRequest.StaffId != null && l.StaffId == (int?)commonPermissionRequest.StaffId) ||
                (commonPermissionRequest.StaffId != null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.StaffId) ||
                (commonPermissionRequest.StaffId == null && l.StaffId == (int?)commonPermissionRequest.CreatedBy) ||
                (commonPermissionRequest.StaffId == null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.CreatedBy)) &&
                                l.PermissionDate >= startOfMonth &&
                                l.PermissionDate <= endOfMonth)
            .ToListAsync();
        if (permissionsThisMonth.Count >= 2) throw new ConflictException($"You cannot apply for permission more than twice in {monthName}.");
        var duration = commonPermissionRequest.EndTime - commonPermissionRequest.StartTime;
        if (duration.TotalMinutes > 120) throw new ConflictException("Permission duration cannot exceed 2 hours");
        var totalMinutesThisMonth = permissionsThisMonth.Sum(p => TimeSpan.Parse(p.TotalHours).TotalMinutes);
        if (totalMinutesThisMonth + duration.TotalMinutes > 120) throw new ConflictException($"Cumulative permission time for {monthName} cannot exceed 2 hours.");
        var formattedDuration = $"{duration.Hours:D2}:{duration.Minutes:D2}";
        commonPermission.StaffId = commonPermissionRequest.CreatedBy;
        commonPermission.StaffId = commonPermissionRequest.StaffId;
        commonPermission.ApplicationTypeId = commonPermissionRequest.ApplicationTypeId;
        commonPermission.PermissionType = commonPermissionRequest.PermissionType;
        commonPermission.FromDate = DateOnly.FromDateTime(commonPermissionRequest.StartTime);
        commonPermission.ToDate = DateOnly.FromDateTime(commonPermissionRequest.EndTime);
        commonPermission.StartTime = TimeOnly.FromDateTime(commonPermissionRequest.StartTime);
        commonPermission.EndTime = TimeOnly.FromDateTime(commonPermissionRequest.EndTime);
        commonPermission.PermissionDate = commonPermissionRequest.PermissionDate;
        commonPermission.TotalHours = formattedDuration;
        commonPermission.Remarks = commonPermissionRequest.Remarks;
        commonPermission.CreatedBy = commonPermissionRequest.CreatedBy;
        commonPermission.CreatedUtc = DateTime.UtcNow;
        commonPermission.IsActive = true;
        await _context.CommonPermissions.AddAsync(commonPermission);
        await _context.SaveChangesAsync();

        string requestDateTime = commonPermission.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == commonPermission.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = commonPermission.CreatedBy,
            Message = $"{requestedBy} has submitted a {commonPermission.PermissionType} request on {requestDateTime}",
            ApplicationTypeId = commonPermissionRequest.ApplicationTypeId,
            IsActive = true,
            CreatedBy = commonPermission.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        commonPermission.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();

        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendCommonPermissionRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    applicationTypeId: commonPermissionRequest.ApplicationTypeId,
                    id: commonPermission.Id,
                    permissionType: commonPermissionRequest.PermissionType,
                    permissionDate: commonPermissionRequest.PermissionDate,
                    startTime: commonPermissionRequest.StartTime,
                    endTime: commonPermissionRequest.EndTime,
                    duration: formattedDuration,
                    remarks: commonPermissionRequest.Remarks,
                    createdBy: staffOrCreatorId,
                    creatorName: staffName,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task<List<StaffPermissionResponse>> GetStaffCommonPermission(int? staffId)
    {
        var permission = await (from staffPermission in _context.CommonPermissions
                                join staff in _context.StaffCreations on staffPermission.StaffId equals staff.Id into staffJoin
                                from staff in staffJoin.DefaultIfEmpty()
                                join creatorStaff in _context.StaffCreations on staffPermission.CreatedBy equals creatorStaff.Id
                                where (staffId == null || staffPermission.StaffId == staffId || staffPermission.CreatedBy == staffId)
                                      && staffPermission.IsActive == true
                                select new StaffPermissionResponse
                                {
                                    Id = staffPermission.Id,
                                    StartTime = staffPermission.StartTime,
                                    EndTime = staffPermission.EndTime,
                                    TotalHours = staffPermission.TotalHours,
                                    PermissionDate = staffPermission.PermissionDate,
                                    PermissionType = staffPermission.PermissionType,
                                    Status = staffPermission.Status1,
                                    Remarks = staffPermission.Remarks,
                                    StaffId = staffPermission.StaffId ?? staffPermission.CreatedBy,
                                    ApplicationTypeId = staffPermission.ApplicationTypeId,
                                    IsActive = staffPermission.IsActive,
                                    CreatedBy = staffPermission.CreatedBy
                                })
                                .ToListAsync();

        if (!permission.Any()) throw new MessageNotFoundException("No staff permissions found");
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
        if (permissionList.Count == 0) throw new MessageNotFoundException("No staff permissions found");
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
        if (leaveDetails.Count == 0) throw new MessageNotFoundException("No leave details found for the given user.");
        return leaveDetails.Cast<object>().ToList();
    }

    public async Task<string> CreateManualPunchAsync(ManualPunchRequestDto request)
    {
        var message = "Manual Punch request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        //await AttendanceFreeze(staffOrCreatorId);
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
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
        await _context.ManualPunchRequistions.AddAsync(manualPunch);
        await _context.SaveChangesAsync();

        string requestDateTime = manualPunch.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == manualPunch.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = manualPunch.CreatedBy,
            Message = $"{requestedBy} has submitted a Manual Punch request on {requestDateTime}",
            ApplicationTypeId = manualPunch.ApplicationTypeId,
            IsActive = true,
            CreatedBy = manualPunch.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        manualPunch.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendManualPunchRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    staffName: staffName,
                    id: manualPunch.Id,
                    applicationTypeId: request.ApplicationTypeId,
                    selectPunch: request.SelectPunch,
                    inPunch: request.InPunch,
                    outPunch: request.OutPunch,
                    remarks: request.Remarks,
                    createdBy: staffOrCreatorId,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task AttendanceFreeze(int staffId, DateOnly startDate, DateOnly endDate)
    {
        var hasUnfreezed = await _context.AttendanceRecords.AnyAsync(f => f.IsFreezed == true && f.StaffId == staffId && f.AttendanceDate >= startDate && f.AttendanceDate <= endDate);
        if (hasUnfreezed) throw new InvalidOperationException("Your request cannot proceed attendance records are frozen");
    }

    public async Task AttendanceFreezeDate(int staffId, DateOnly date)
    {
        var hasUnfreezed = await _context.AttendanceRecords.AnyAsync(f => f.IsFreezed == true && f.StaffId == staffId && f.AttendanceDate == date);
        if (hasUnfreezed) throw new InvalidOperationException("Your request cannot proceed attendance records are frozen");
    }

    private async Task NotFoundMethod(int applicationTypeId)
    {
        var application = await _context.ApplicationTypes.AnyAsync(a => a.Id == applicationTypeId && a.IsActive);
        if (!application) throw new MessageNotFoundException("Application type not found");
    }

    public async Task<string> CreateOnDutyRequisitionAsync(OnDutyRequisitionRequest request)
    {
        var message = "On Duty request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        if (request.StartDate != null && request.EndDate != null)
        {
            await AttendanceFreeze(staffOrCreatorId, (DateOnly)request.StartDate, (DateOnly)request.EndDate);
        }
        if (request.StartTime != null && request.EndTime != null)
        {
            await AttendanceFreeze(staffOrCreatorId, DateOnly.FromDateTime(request.StartTime.Value), DateOnly.FromDateTime(request.EndTime.Value));
        }
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var existingLeaves = await _context.OnDutyRequisitions
         .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                      (lr.StartDate <= request.EndDate &&
                       lr.EndDate >= request.StartDate))
         .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.StartDate == request.StartDate;
            bool sameEndDate = existingLeave.EndDate == request.EndDate;
            bool sameDay = request.StartDate == request.EndDate &&
                           existingLeave.StartDate == existingLeave.EndDate &&
                           existingLeave.StartDate == request.StartDate;

            // Allow complementary half-days on the same start date
            if (sameStartDate &&
                ((existingLeave.StartDuration == "First Half" && request.StartDuration == "Second Half") ||
                 (existingLeave.StartDuration == "Second Half" && request.StartDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-days on the same end date
            if (sameEndDate &&
                ((existingLeave.EndDuration == "First Half" && request.EndDuration == "Second Half") ||
                 (existingLeave.EndDuration == "Second Half" && request.EndDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-day request on same single day
            if (sameDay)
            {
                if ((existingLeave.StartDuration == "First Half" && request.StartDuration == "Second Half") ||
                    (existingLeave.StartDuration == "Second Half" && request.StartDuration == "First Half"))
                {
                    continue;
                }

                if (existingLeave.StartDuration == request.StartDuration)
                {
                    throw new ConflictException("On Duty request already exists");
                }
            }

            // Prevent full day overlap
            if ((existingLeave.StartDuration == "Full Day" || request.StartDuration == "Full Day") ||
                (existingLeave.EndDuration == "Full Day" || request.EndDuration == "Full Day"))
            {
                throw new ConflictException("On Duty request already exists");
            }

            // Prevent general date overlap
            if (existingLeave.StartDate <= request.EndDate &&
                existingLeave.EndDate >= request.StartDate)
            {
                throw new ConflictException("On Duty request already exists");
            }
        }
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
        await _context.OnDutyRequisitions.AddAsync(onDutyRequisition);
        await _context.SaveChangesAsync();

        string requestDateTime = onDutyRequisition.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == onDutyRequisition.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = onDutyRequisition.CreatedBy,
            Message = $"{requestedBy} has submitted a On Duty request on {requestDateTime}",
            ApplicationTypeId = onDutyRequisition.ApplicationTypeId,
            IsActive = true,
            CreatedBy = onDutyRequisition.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        onDutyRequisition.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendOnDutyRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    id: onDutyRequisition.Id,
                    applicationTypeId: request.ApplicationTypeId,
                    startDate: request.StartDate,
                    endDate: request.EndDate,
                    startTime: request.StartTime,
                    endTime: request.EndTime,
                    totalDays: request.TotalDays,
                    totalHours: request.TotalHours,
                    reason: request.Reason,
                    createdBy: staffOrCreatorId,
                    creatorName: staffName,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task<string> CreateBusinessTravelAsync(BusinessTravelRequestDto request)
    {
        var message = "Business Travel request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        if (request.FromDate != null && request.ToDate != null)
        {
            await AttendanceFreeze(staffOrCreatorId, (DateOnly)request.FromDate, (DateOnly)request.ToDate);
        }
        if (request.FromTime != null && request.ToTime != null)
        {
            await AttendanceFreeze(staffOrCreatorId, DateOnly.FromDateTime(request.FromTime.Value), DateOnly.FromDateTime(request.ToTime.Value));
        }
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var existingLeaves = await _context.BusinessTravels
         .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                      (lr.FromDate <= request.ToDate &&
                       lr.ToDate >= request.FromDate))
         .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.FromDate == request.FromDate;
            bool sameEndDate = existingLeave.ToDate == request.ToDate;
            bool sameDay = request.FromDate == request.ToDate &&
                           existingLeave.FromDate == existingLeave.ToDate &&
                           existingLeave.FromDate == request.FromDate;

            // Allow complementary half-days on the same start date
            if (sameStartDate &&
                ((existingLeave.StartDuration == "First Half" && request.StartDuration == "Second Half") ||
                 (existingLeave.StartDuration == "Second Half" && request.StartDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-days on the same end date
            if (sameEndDate &&
                ((existingLeave.EndDuration == "First Half" && request.EndDuration == "Second Half") ||
                 (existingLeave.EndDuration == "Second Half" && request.EndDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-day request on same single day
            if (sameDay)
            {
                if ((existingLeave.StartDuration == "First Half" && request.StartDuration == "Second Half") ||
                    (existingLeave.StartDuration == "Second Half" && request.StartDuration == "First Half"))
                {
                    continue;
                }

                if (existingLeave.StartDuration == request.StartDuration)
                {
                    throw new ConflictException("Business Travel request already exists");
                }
            }

            // Prevent full day overlap
            if ((existingLeave.StartDuration == "Full Day" || request.StartDuration == "Full Day") ||
                (existingLeave.EndDuration == "Full Day" || request.EndDuration == "Full Day"))
            {
                throw new ConflictException("Business Travel request already exists");
            }

            // Prevent general date overlap
            if (existingLeave.FromDate <= request.ToDate &&
                existingLeave.ToDate >= request.FromDate)
            {
                throw new ConflictException("Business Travel request already exists");
            }
        }
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

        string requestDateTime = businessTravel.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == businessTravel.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = businessTravel.CreatedBy,
            Message = $"{requestedBy} has submitted a Business Travel request on {requestDateTime}",
            ApplicationTypeId = businessTravel.ApplicationTypeId,
            IsActive = true,
            CreatedBy = businessTravel.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        businessTravel.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendBusinessTravelRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    id: businessTravel.Id,
                    applicationTypeId: request.ApplicationTypeId,
                    fromDate: request.FromDate,
                    toDate: request.ToDate,
                    fromTime: request.FromTime,
                    toTime: request.ToTime,
                    totalDays: request.TotalDays,
                    totalHours: request.TotalHours,
                    reason: request.Reason,
                    createdBy: staffOrCreatorId,
                    creatorName: staffName,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task<string> CreateWorkFromHomeAsync(WorkFromHomeDto request)
    {
        var message = "Work From Home request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        if (request.FromDate != null && request.ToDate != null)
        {
            await AttendanceFreeze(staffOrCreatorId, (DateOnly)request.FromDate, (DateOnly)request.ToDate);
        }
        if (request.FromTime != null && request.ToTime != null)
        {
            await AttendanceFreeze(staffOrCreatorId, DateOnly.FromDateTime(request.FromTime.Value), DateOnly.FromDateTime(request.ToTime.Value));
        }
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var existingLeaves = await _context.WorkFromHomes
         .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                      (lr.FromDate <= request.ToDate &&
                       lr.ToDate >= request.FromDate))
         .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.FromDate == request.FromDate;
            bool sameEndDate = existingLeave.ToDate == request.ToDate;
            bool sameDay = request.FromDate == request.ToDate &&
                           existingLeave.FromDate == existingLeave.ToDate &&
                           existingLeave.FromDate == request.FromDate;

            // Allow complementary half-days on the same start date
            if (sameStartDate &&
                ((existingLeave.StartDuration == "First Half" && request.StartDuration == "Second Half") ||
                 (existingLeave.StartDuration == "Second Half" && request.StartDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-days on the same end date
            if (sameEndDate &&
                ((existingLeave.EndDuration == "First Half" && request.EndDuration == "Second Half") ||
                 (existingLeave.EndDuration == "Second Half" && request.EndDuration == "First Half")))
            {
                continue;
            }

            // Allow complementary half-day request on same single day
            if (sameDay)
            {
                if ((existingLeave.StartDuration == "First Half" && request.StartDuration == "Second Half") ||
                    (existingLeave.StartDuration == "Second Half" && request.StartDuration == "First Half"))
                {
                    continue;
                }

                if (existingLeave.StartDuration == request.StartDuration)
                {
                    throw new ConflictException("Work From Home request already exists");
                }
            }

            // Prevent full day overlap
            if ((existingLeave.StartDuration == "Full Day" || request.StartDuration == "Full Day") ||
                (existingLeave.EndDuration == "Full Day" || request.EndDuration == "Full Day"))
            {
                throw new ConflictException("Work From Home request already exists");
            }

            // Prevent general date overlap
            if (existingLeave.FromDate <= request.ToDate &&
                existingLeave.ToDate >= request.FromDate)
            {
                throw new ConflictException("Work From Home request already exists");
            }
        }
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
        await _context.WorkFromHomes.AddAsync(workFromHome);
        await _context.SaveChangesAsync();

        string requestDateTime = workFromHome.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == workFromHome.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = workFromHome.CreatedBy,
            Message = $"{requestedBy} has submitted a Work From Home request on {requestDateTime}",
            ApplicationTypeId = workFromHome.ApplicationTypeId,
            IsActive = true,
            CreatedBy = workFromHome.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        workFromHome.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendWorkFromHomeRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    id: workFromHome.Id,
                    applicationTypeId: request.ApplicationTypeId,
                    fromDate: request.FromDate,
                    toDate: request.ToDate,
                    fromTime: request.FromTime,
                    toTime: request.ToTime,
                    totalDays: request.TotalDays,
                    totalHours: request.TotalHours,
                    reason: request.Reason,
                    createdBy: staffOrCreatorId,
                    creatorName: staffName,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task<List<object>> GetShiftsByStaffAndDateRange(int staffId, DateOnly fromDate, DateOnly toDate)
    {
        var shifts = await (from asg in _context.AssignShifts
                            join sh in _context.Shifts on asg.ShiftId equals sh.Id
                            where asg.StaffId == staffId &&
                                  asg.IsActive &&
                                  sh.IsActive &&
                                  asg.FromDate >= fromDate && asg.FromDate <= toDate
                            select new
                            {
                                Date = asg.FromDate,
                                ShortName = sh.ShortName,
                                StartTime = sh.StartTime,
                                EndTime = sh.EndTime
                            }).ToListAsync();
        if (shifts.Count == 0) throw new MessageNotFoundException("Shifts not found between the date range for the staff");
        var result = shifts.Select(shift => new
        {
            Date = shift.Date.ToString("dd/MM/yyyy"),
            Shift = shift.ShortName,
            Time = $"{shift.StartTime} {shift.EndTime}"
        }).Cast<object>().ToList(); return result;
    }

    public async Task<string> CreateShiftChangeAsync(ShiftChangeDto request)
    {
        var message = "Shift Change request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        await AttendanceFreeze(staffOrCreatorId, request.FromDate, request.ToDate);
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var shiftName = await _context.Shifts.Where(s => s.Id == request.ShiftId && s.IsActive).Select(s => s.Name).FirstOrDefaultAsync();
        if (shiftName == null) throw new MessageNotFoundException("Shift not found");
        var assignedShifts = await _context.AssignShifts
            .Where(a => a.StaffId == staffOrCreatorId &&
                        a.FromDate >= request.FromDate &&
                        a.FromDate <= request.ToDate &&
                        a.IsActive)
            .ToListAsync();
        if (!assignedShifts.Any())
        {
            throw new MessageNotFoundException("No assigned shifts found for the selected date range");
        }
        var existingLeaves = await _context.ShiftChanges
         .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                      (lr.FromDate <= request.ToDate &&
                       lr.ToDate >= request.FromDate) &&
                      lr.IsActive)
         .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            if (existingLeave.FromDate <= request.ToDate && existingLeave.ToDate >= request.FromDate)
            {
                throw new ConflictException("Shift Change request already exists");
            }
        }
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
        await _context.ShiftChanges.AddAsync(shiftChange);
        await _context.SaveChangesAsync();

        /*        foreach (var assignShift in assignedShifts)
                {
                    assignShift.ShiftId = request.ShiftId;
                }
                await _context.SaveChangesAsync();
        */
        string requestDateTime = shiftChange.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == shiftChange.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = shiftChange.CreatedBy,
            Message = $"{requestedBy} has submitted a Shift Change request on {requestDateTime}",
            ApplicationTypeId = shiftChange.ApplicationTypeId,
            IsActive = true,
            CreatedBy = shiftChange.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        shiftChange.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendShiftChangeRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    id: shiftChange.Id,
                    applicationTypeId: request.ApplicationTypeId,
                    shiftName: shiftName,
                    fromDate: request.FromDate,
                    toDate: request.ToDate,
                    reason: request.Reason,
                    createdBy: staffOrCreatorId,
                    creatorName: staffName,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task<string> CreateShiftExtensionAsync(ShiftExtensionDto request)
    {
        var message = "Shift Extension request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        await AttendanceFreezeDate(staffOrCreatorId, request.TransactionDate);
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var assignedShifts = await _context.AssignShifts.FirstOrDefaultAsync(a => a.StaffId == staffOrCreatorId && a.FromDate == request.TransactionDate && a.IsActive);
        if (assignedShifts == null)
        {
            throw new MessageNotFoundException("No assigned shifts found for the selected date range");
        }
        var existingLeaves = await _context.ShiftExtensions
         .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                      (lr.TransactionDate == request.TransactionDate))
         .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.TransactionDate == request.TransactionDate;
            if (sameStartDate)
            {
                throw new ConflictException("Shift Extension request already exists");
            }
        }
        var shiftExtension = new ShiftExtension
        {
            ApplicationTypeId = request.ApplicationTypeId,
            StaffId = request.StaffId,
            ShiftId = assignedShifts.ShiftId,
            TransactionDate = request.TransactionDate,
            DurationHours = request.DurationHours,
            BeforeShiftHours = request.BeforeShiftHours,
            AfterShiftHours = request.AfterShiftHours,
            Remarks = request.Remarks,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ShiftExtensions.AddAsync(shiftExtension);
        await _context.SaveChangesAsync();
        string requestDateTime = shiftExtension.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == shiftExtension.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = shiftExtension.CreatedBy,
            Message = $"{requestedBy} has submitted a Shift Extension request on {requestDateTime}",
            ApplicationTypeId = shiftExtension.ApplicationTypeId,
            IsActive = true,
            CreatedBy = shiftExtension.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        shiftExtension.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendShiftExtensionRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    id: shiftExtension.Id,
                    applicationTypeId: request.ApplicationTypeId,
                    transactionDate: request.TransactionDate,
                    durationHours: request.DurationHours,
                    beforeShiftHours: request.BeforeShiftHours,
                    afterShiftHours: request.AfterShiftHours,
                    remarks: request.Remarks,
                    createdBy: staffOrCreatorId,
                    creatorName: staffName,
                    requestDate: requestDateTime
                );
            }
        }
        return message;
    }

    public async Task<string> CreateWeeklyOffHolidayWorkingAsync(WeeklyOffHolidayWorkingDto request)
    {
        var message = "Weekly Off/ Holiday Working request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        await AttendanceFreezeDate(staffOrCreatorId, request.TxnDate);
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        var shiftName = await _context.Shifts.Where(s => s.Id == request.ShiftId && s.IsActive).Select(s => s.Name).FirstOrDefaultAsync();
        if (shiftName == null) throw new MessageNotFoundException("Shift not found");
        var existingLeaves = await _context.WeeklyOffHolidayWorkings
        .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                     (lr.TxnDate == request.TxnDate))
        .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.TxnDate == request.TxnDate;
            if (sameStartDate)
            {
                throw new ConflictException("WeeklyOff/ Holiday Working request already exists");
            }
        }
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
        await _context.WeeklyOffHolidayWorkings.AddAsync(weeklyOffHolidayWorking);
        await _context.SaveChangesAsync();
        string requestDateTime = weeklyOffHolidayWorking.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == weeklyOffHolidayWorking.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = weeklyOffHolidayWorking.CreatedBy,
            Message = $"{requestedBy} has submitted a WeeklyOff/ Holiday Working request on {requestDateTime}",
            ApplicationTypeId = weeklyOffHolidayWorking.ApplicationTypeId,
            IsActive = true,
            CreatedBy = weeklyOffHolidayWorking.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        weeklyOffHolidayWorking.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();

        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendWeeklyOffHolidayWorkingRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    staffName: staffName,
                    selectShiftType: request.SelectShiftType,
                    id: weeklyOffHolidayWorking.Id,
                    applicationTypeId: request.ApplicationTypeId,
                    txnDate: request.TxnDate,
                    shiftName: shiftName,
                    shiftInTime: request.ShiftInTime,
                    shiftOutTime: request.ShiftOutTime,
                    requestDate: requestDateTime,
                    createdBy: staffOrCreatorId
                );
            }
        }
        return message;
    }

    public async Task<string> AddReimbursement(ReimbursementRequestModel request)
    {
        var message = "Reimbursement request submitted successfully";
        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        await AttendanceFreezeDate(staffOrCreatorId, request.BillDate);
        await NotFoundMethod(request.ApplicationTypeId);
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName}{(string.IsNullOrWhiteSpace(staffId.LastName) ? "" : " " + staffId.LastName)}";
        bool reimbursementExists = await _context.Reimbursements.AnyAsync(r => r.BillNo == request.BillNo);
        if (reimbursementExists) throw new ConflictException($"Reimbursement with Bill No {request.BillNo} already exists.");
        var existingLeaves = await _context.Reimbursements
        .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) && lr.BillNo == request.BillNo)
        .ToListAsync();
        foreach (var existingLeave in existingLeaves)
        {
            bool sameStartDate = existingLeave.BillNo == request.BillNo;
            if (sameStartDate)
            {
                throw new ConflictException("Reimbursement request already exists");
            }
        }
        string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        string fileUploadPath = string.Empty;
        var reimbursementType = await _context.ReimbursementTypes.Where(s => s.Id == request.ReimbursementTypeId && s.IsActive).Select(s => s.Name).FirstOrDefaultAsync();
        async Task<string> SaveFile(IFormFile file, string folderName)
        {
            if (file == null) throw new ArgumentNullException(nameof(file), "File cannot be null.");
            if (file.Length == 0) throw new InvalidOperationException("Uploaded file is empty.");
            string directoryPath = Path.Combine(baseDirectory, folderName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(directoryPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"/{folderName}/{fileName}";
        }
        fileUploadPath = await SaveFile(request.File, "Reimbursement");
        var reimbursement = new Reimbursement
        {
            BillDate = request.BillDate,
            BillNo = request.BillNo,
            Description = request.Description,
            BillPeriod = request.BillPeriod,
            Amount = request.Amount,
            ApplicationTypeId = request.ApplicationTypeId,
            UploadFilePath = fileUploadPath ?? string.Empty,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            IsActive = true,
            StaffId = request.StaffId,
            ReimbursementTypeId = request.ReimbursementTypeId
        };
        await _context.Reimbursements.AddAsync(reimbursement);
        await _context.SaveChangesAsync();
        string requestDateTime = reimbursement.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 == null) throw new MessageNotFoundException("Approver not found");
        var requestedBy = await _context.StaffCreations
            .Where(s => s.Id == reimbursement.CreatedBy && s.IsActive == true)
            .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}")
            .FirstOrDefaultAsync();
        var notification = new ApprovalNotification
        {
            StaffId = reimbursement.CreatedBy,
            Message = $"{requestedBy} has submitted a {reimbursementType} request on {requestDateTime}",
            ApplicationTypeId = reimbursement.ApplicationTypeId,
            IsActive = true,
            CreatedBy = reimbursement.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.ApprovalNotifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        reimbursement.ApprovalNotificationId = notification.Id;
        await _context.SaveChangesAsync();
        if (approver1 != null)
        {
            if (!string.IsNullOrEmpty(approver1.OfficialEmail))
            {
                await _emailService.SendReimbursementRequestEmail(
                    id: reimbursement.Id,
                    applicationTypeId: reimbursement.ApplicationTypeId,
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}",
                    staffName: staffName,
                    requestDate: requestDateTime,
                    billDate: reimbursement.BillDate,
                    billNo: reimbursement.BillNo,
                    description: reimbursement.Description,
                    billPeriod: reimbursement.BillPeriod,
                    amount: reimbursement.Amount,
                    createdBy: staffOrCreatorId
                );
            }
        }
        return message;
    }
}