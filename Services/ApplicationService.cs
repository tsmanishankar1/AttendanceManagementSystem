using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Web;
using System.Text.Json;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AttendanceManagement.Services;

public class ApplicationService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly StoredProcedureDbContext _storedProcedureDbContext;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public ApplicationService(AttendanceManagementSystemContext context, StoredProcedureDbContext storedProcedureDbContext, IConfiguration configuration, EmailService emailService)
    {
        _context = context;
        _storedProcedureDbContext = storedProcedureDbContext;
        _configuration = configuration;
        _emailService = emailService;
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
                case 12:
                    entity = await _context.Reimbursements.FirstOrDefaultAsync(cc => cc.Id == id && cc.IsActive);
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
            var cancelledOnProperty = entityType.GetProperty("CancelledOn");
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
            cancelledOnProperty?.SetValue(entity, DateTime.UtcNow);
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
                .Where(lr => lr.IsCancelled == null &&
                    (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .Join(_context.ApplicationTypes, lr => lr.ApplicationTypeId, at => at.Id,
                    (lr, at) => new { lr, at })
                .Join(_context.LeaveTypes, temp => temp.lr.LeaveTypeId, lt => lt.Id,
                    (temp, lt) => new { temp.lr, temp.at, lt })
                .GroupJoin(_context.StaffCreations, temp => temp.lr.StaffId ?? temp.lr.CreatedBy, sc => sc.Id,
                    (temp, scGroup) => new { temp, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new LeaveReq
                    {
                        ApplicationTypeId = tempWithSc.temp.lr.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.temp.at.ApplicationTypeName,
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
                .ToListAsync(),
            2 => await _context.CommonPermissions
                .Where(cp => cp.IsCancelled == null &&
                    (cp.StaffId.HasValue ? cp.StaffId == staffId : cp.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, cp => cp.StaffId ?? cp.CreatedBy, sc => sc.Id,
                    (cp, scGroup) => new { cp, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new PermissionDto
                    {
                        ApplicationTypeId = tempWithSc.cp.ApplicationTypeId,
                        ApplicationTypeName = _context.ApplicationTypes
                            .Where(at => at.Id == tempWithSc.cp.ApplicationTypeId)
                            .Select(at => at.ApplicationTypeName)
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
                        StartTime = tempWithSc.cp.StartTime,
                        EndTime = tempWithSc.cp.EndTime,
                        PermissionType = tempWithSc.cp.PermissionType,
                        Remarks = tempWithSc.cp.Remarks
                    })
                .ToListAsync(),
            3 => await _context.ManualPunchRequistions
                 .Where(mp => mp.IsCancelled == null && (mp.StaffId.HasValue ? mp.StaffId == staffId : mp.CreatedBy == staffId))
                 .GroupJoin(_context.StaffCreations, mp => mp.StaffId ?? mp.CreatedBy, sc => sc.Id,
                     (mp, scGroup) => new { mp, scGroup })
                 .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                     (tempWithSc, sc) => new ManualPunch
                     {
                         ApplicationTypeId = tempWithSc.mp.ApplicationTypeId,
                         ApplicationTypeName = tempWithSc.mp.ApplicationType.ApplicationTypeName,
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
                 .ToListAsync(),

            4 => await _context.OnDutyRequisitions
                .Where(od => od.IsCancelled == null && (od.StaffId.HasValue ? od.StaffId == staffId : od.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, od => od.StaffId ?? od.CreatedBy, sc => sc.Id,
                    (od, scGroup) => new { od, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new OnDutyRequest
                    {
                        ApplicationTypeId = tempWithSc.od.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.od.ApplicationType.ApplicationTypeName,
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
                        Reason = tempWithSc.od.Reason
                    })
                .ToListAsync(),

            5 => await _context.BusinessTravels
                 .Where(bt => bt.IsCancelled == null &&
                     (bt.StaffId.HasValue ? bt.StaffId == staffId : bt.CreatedBy == staffId))
                 .GroupJoin(_context.StaffCreations, bt => bt.StaffId ?? bt.CreatedBy, sc => sc.Id,
                     (bt, scGroup) => new { bt, scGroup })
                 .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                     (tempWithSc, sc) => new Business
                     {
                         ApplicationTypeId = tempWithSc.bt.ApplicationTypeId,
                         ApplicationTypeName = tempWithSc.bt.ApplicationType.ApplicationTypeName,

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
                         Reason = tempWithSc.bt.Reason
                     })
                 .ToListAsync(),


            6 => await _context.WorkFromHomes
                   .Where(wfh => wfh.IsCancelled == null && (wfh.StaffId.HasValue ? wfh.StaffId == staffId : wfh.CreatedBy == staffId))
                   .GroupJoin(_context.StaffCreations, wfh => wfh.StaffId ?? wfh.CreatedBy, sc => sc.Id,
                       (wfh, scGroup) => new { wfh, scGroup })
                   .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                       (tempWithSc, sc) => new WorkFrom
                       {
                           ApplicationTypeId = tempWithSc.wfh.ApplicationTypeId,
                           ApplicationTypeName = tempWithSc.wfh.ApplicationType.ApplicationTypeName,

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
                           Reason = tempWithSc.wfh.Reason
                       })
                   .ToListAsync(),

            7 => await _context.ShiftChanges
                  .Where(lr => lr.IsCancelled == null &&
                        (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                  .Join(_context.ApplicationTypes, sc => sc.ApplicationTypeId, at => at.Id,
                        (sc, at) => new { sc, at })
                  .Join(_context.Shifts, temp => temp.sc.ShiftId, s => s.Id,
                        (temp, s) => new { temp, s })
                  .GroupJoin(_context.StaffCreations, tempWithS => tempWithS.temp.sc.StaffId ?? tempWithS.temp.sc.CreatedBy,
                             sc => sc.Id, (tempWithS, scGroup) => new { tempWithS, scGroup })
                  .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                      (tempWithSc, sc) => new ShiftChan
                      {
                          ApplicationTypeId = tempWithSc.tempWithS.temp.sc.ApplicationTypeId,
                          ApplicationTypeName = tempWithSc.tempWithS.temp.at.ApplicationTypeName,
                          ShiftName = tempWithSc.tempWithS.s.ShiftName,
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
                  .ToListAsync(),

            8 => await _context.ShiftExtensions
                 .Where(lr => lr.IsCancelled == null &&
                     (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                 .GroupJoin(_context.StaffCreations, se => se.StaffId ?? se.CreatedBy,
                     sc => sc.Id, (se, scGroup) => new { se, scGroup })
                 .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                     (tempWithSc, sc) => new ShiftExte
                     {
                         ApplicationTypeId = tempWithSc.se.ApplicationTypeId,
                         ApplicationTypeName = tempWithSc.se.ApplicationType.ApplicationTypeName,
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
                 .ToListAsync(),

            9 => await _context.WeeklyOffHolidayWorkings
                .Where(lr => lr.IsCancelled == null &&
                    (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, wh => wh.StaffId ?? wh.CreatedBy,
                    sc => sc.Id, (wh, scGroup) => new { wh, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new { tempWithSc.wh, sc })
                .Join(_context.ApplicationTypes,
                    temp => temp.wh.ApplicationTypeId, at => at.Id, (temp, at) => new { temp.wh, temp.sc, at })
                .Join(_context.Shifts,
                    temp => temp.wh.ShiftId, s => s.Id, (temp, s) => new WeeklyOffHoliday
                    {
                        ApplicationTypeId = temp.wh.ApplicationTypeId,
                        ApplicationTypeName = temp.at.ApplicationTypeName,
                        SelectShiftType = temp.wh.SelectShiftType,
                        TxnDate = temp.wh.TxnDate,
                        ShiftName = s.ShiftName,  
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
                .ToListAsync(),

            10 => await _context.CompOffAvails
                .Where(lr => lr.IsCancelled == null &&
                    (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, coa => coa.StaffId ?? coa.CreatedBy,
                    sc => sc.Id, (coa, scGroup) => new { coa, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new CompOffAvai
                    {
                        ApplicationTypeId = tempWithSc.coa.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.coa.ApplicationType.ApplicationTypeName,
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
                .ToListAsync(),

            11 => await _context.CompOffCredits
                .Where(lr => lr.IsCancelled == null &&
                    (lr.StaffId.HasValue ? lr.StaffId == staffId : lr.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, coc => coc.StaffId ?? coc.CreatedBy,
                    sc => sc.Id, (coc, scGroup) => new { coc, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new CompOffCred
                    {
                        ApplicationTypeId = tempWithSc.coc.ApplicationTypeId,
                        ApplicationTypeName = tempWithSc.coc.ApplicationType.ApplicationTypeName,
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
                .ToListAsync(),

            18 => await _context.Reimbursements
                .Where(r => r.CancelledOn == null &&
                    (r.StaffId.HasValue ? r.StaffId == staffId : r.CreatedBy == staffId))
                .GroupJoin(_context.StaffCreations, r => r.StaffId ?? r.CreatedBy,
                    sc => sc.Id, (r, scGroup) => new { r, scGroup })
                .SelectMany(tempWithSc => tempWithSc.scGroup.DefaultIfEmpty(),
                    (tempWithSc, sc) => new Reimbursements
                    {
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
                .ToListAsync(),

            _ => Enumerable.Empty<object>()
        };
        return application;
    }
    public async Task<object> GetMonthlyDetailsAsync(int staffId, int month, int year)
    {
        var staff = await _context.StaffCreations
            .Where(s => s.Id == staffId)
            .Select(s => new
            {
                s.Id,
                s.StaffId,
                StaffName = $"{s.FirstName} {s.LastName}"
            })
            .FirstOrDefaultAsync();

        if (staff == null)
        {
            throw new ArgumentException("Staff not found.");
        }

        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                 .Select(offset => startDate.AddDays(offset))
                                 .ToList();

        var attendanceRecords = await (from a in _context.AttendanceRecords
                                       join s in _context.Shifts on a.ShiftId equals s.Id into shiftGroup
                                       from shift in shiftGroup.DefaultIfEmpty()
                                       where a.StaffId == staffId && a.FirstIn.HasValue &&
                                             a.FirstIn.Value.Month == month && a.FirstIn.Value.Year == year
                                       select new
                                       {
                                           a.FirstIn,
                                           LoginTime = a.FirstIn,
                                           LogoutTime = a.LastOut,
                                           ShiftId = a.ShiftId,
                                           ShiftName = shift != null ? shift.ShiftName : "No Shift Assigned",
                                           TotalHoursWorked = a.LastOut.HasValue && a.FirstIn.HasValue
                                               ? (a.LastOut.Value - a.FirstIn.Value).TotalHours
                                               : 0
                                       })
                              .ToListAsync();

        var leaveRecords = await _context.LeaveRequisitions
            .Where(lr => (staffId == null ? lr.CreatedBy != null : lr.StaffId == staffId || lr.CreatedBy == staffId) &&
                         (lr.FromDate <= DateOnly.FromDateTime(endDate) &&
                          lr.ToDate >= DateOnly.FromDateTime(startDate)) && !lr.IsActive)
            .Select(lr => new
            {
                lr.FromDate,
                lr.ToDate,
                LeaveTypeName = lr.LeaveType.Name,
                lr.Reason
            })
            .ToListAsync();

        var workFromHomeRecords = await _context.WorkFromHomes
             .Where(wfh => (staffId == null ? wfh.CreatedBy != null : wfh.StaffId == staffId || wfh.CreatedBy == staffId) &&
                           ((wfh.FromDate.HasValue && wfh.FromDate.Value <= DateOnly.FromDateTime(endDate)) &&
                            (wfh.ToDate.HasValue && wfh.ToDate.Value >= DateOnly.FromDateTime(startDate))) && !wfh.IsActive)
             .Select(wfh => new
             {
                 wfh.FromDate,
                 wfh.ToDate,
                 wfh.Reason
             })
             .ToListAsync();

        var startDateOnly = DateOnly.FromDateTime(startDate);
        var endDateOnly = DateOnly.FromDateTime(endDate);

        var onDutyRecords = await _context.OnDutyRequisitions
            .Where(od => (staffId == null ? od.CreatedBy != null : od.StaffId == staffId || od.CreatedBy == staffId) &&
                         (od.StartDate <= endDateOnly && od.EndDate >= startDateOnly) && !od.IsActive)
            .Select(od => new
            {
                od.StartDate,
                od.EndDate,
                od.Reason
            })
            .ToListAsync();

        var businessTravelRecords = await _context.BusinessTravels
            .Where(bt => (staffId == null ? bt.CreatedBy != null : bt.StaffId == staffId || bt.CreatedBy == staffId) &&
                         (bt.FromDate <= endDateOnly && bt.ToDate >= startDateOnly) && !bt.IsActive)
            .Select(bt => new
            {
                bt.FromDate,
                bt.ToDate,
                bt.Reason
            })
            .ToListAsync();

        var compOffRecords = await _context.CompOffAvails
            .Where(co => (staffId == null ? co.CreatedBy != null : co.StaffId == staffId || co.CreatedBy == staffId) &&
                         (co.FromDate <= endDateOnly && co.ToDate >= startDateOnly) && !co.IsActive)
            .Select(co => new
            {
                co.FromDate,
                co.ToDate
            })
            .ToListAsync();

        var weeklyOffRecords = await _context.WeeklyOffHolidayWorkings
            .Where(wo => (staffId == null ? wo.CreatedBy != null : wo.StaffId == staffId || wo.CreatedBy == staffId) && wo.TxnDate.Month == month && wo.TxnDate.Year == year)
            .Select(wo => new
            {
                wo.TxnDate,
                wo.ShiftInTime,
                wo.ShiftOutTime
            })
            .ToListAsync();

        var holidayRecords = await _context.HolidayMasters
            .Include(hm => hm.HolidayCalendarTransactions)
            .Where(hm => hm.HolidayCalendarTransactions
                .Any(x => ((x.FromDate.Month == month && x.FromDate.Year == year) ||
                           (x.ToDate.Month == month && x.ToDate.Year == year))))
            .Select(hm => new
            {
                HolidayName = hm.HolidayName,
                Transactions = hm.HolidayCalendarTransactions
                    .Where(x => (x.FromDate.Month == month && x.FromDate.Year == year) ||
                                (x.ToDate.Month == month && x.ToDate.Year == year))
                    .Select(x => new
                    {
                        x.FromDate,
                        x.ToDate
                    })
                    .ToList()
            })
            .ToListAsync();

        var result = allDates.Select(date =>
        {
            var dateOnly = DateOnly.FromDateTime(date);
            var attendance = attendanceRecords
                .FirstOrDefault(a => a.LoginTime.HasValue && a.LogoutTime.HasValue && a.LoginTime.Value.Date == date);
            var todayDateOnly = DateOnly.FromDateTime(DateTime.Today);
            var statusId = _context.AttendanceRecords
                .Where(a => !a.IsDeleted && a.AttendanceDate == dateOnly)
                .Select(a => a.StatusId)
                .FirstOrDefault();

            string statusName = _context.StatusDropdowns.Where(s => s.Id == statusId).Select(s => s.Name).FirstOrDefault();

            // Load all colors once for mapping
            var statusColors = _context.AttendanceStatusColors
                .Where(c => c.IsActive)
                .Select(c => new { c.Id, c.StatusName })
                .ToList();

            int? color = null;

            color = statusColors
            .Where(c => c.StatusName == "Absent")
            .Select(c => c.Id)
            .FirstOrDefault();
            if (dateOnly > todayDateOnly && statusName == null)
            {
                statusName = "Unprocessed";
                // Future date – Unprocessed
                color = statusColors
                    .Where(c => c.StatusName == "Unprocessed")
                    .Select(c => c.Id)
                    .FirstOrDefault();
            }
            else if (dateOnly == todayDateOnly && statusName == null)
            {
                statusName = "Not Updated";
                // Today and not updated – Not Updated
                color = statusColors
                    .Where(c => c.StatusName == "Not Updated")
                    .Select(c => c.Id)
                    .FirstOrDefault();
            }
            else if (statusName != null)
            {
                string name = statusName;

                // Handle grouped types for consistent color mapping
                if (new[]
                {
        "Casual Leave", "First Half Casual Leave", "Second Half Casual Leave",
        "Sick Leave", "First Half Sick Leave", "Second Half Sick Leave",
        "Paternity Leave", "Marriage Leave", "Non Confirmed Leave",
        "First Half Non Confirmed Leave", "Second Half Non Confirmed Leave",
        "Medical Leave", "Bereavement Leave", "Maternity Leave"
    }.Contains(name))
                {
                    name = "Leave";
                }
                else if (new[]
                {
        "Work From Home", "First Half Work From Home", "Second Half Work From Home"
    }.Contains(name))
                {
                    name = "Work From Home";
                }
                else if (new[]
                {
        "On Duty", "First Half On Duty", "Second Half On Duty"
    }.Contains(name))
                {
                    name = "On Duty";
                }
                else if (new[]
                {
        "Comp-Off", "First Half Comp-Off", "Second Half Comp-Off"
    }.Contains(name))
                {
                    name = "Comp Off";
                }

                color = statusColors
                    .Where(c => c.StatusName == name)
                    .Select(c => c.Id)
                    .FirstOrDefault();
            }
            else if(statusName == null)
            {
                statusName = "Absent";
            }
            else if (statusName != null)
            {
                // Valid status found
                var matchedColor = statusColors
                    .Where(c => c.StatusName == statusName)
                    .Select(c => c.Id)
                    .FirstOrDefault();

                if (matchedColor != 0)
                    color = matchedColor;
            }
            var leave = leaveRecords.FirstOrDefault(l => dateOnly >= l.FromDate && dateOnly <= l.ToDate);
            var workFromHome = workFromHomeRecords.Any(wfh => wfh.FromDate.HasValue && wfh.ToDate.HasValue && dateOnly >= wfh.FromDate.Value && dateOnly <= wfh.ToDate.Value);
            var onDuty = onDutyRecords.Any(od => dateOnly >= od.StartDate && dateOnly <= od.EndDate);
            var businessTravel = businessTravelRecords.Any(bt => dateOnly >= bt.FromDate && dateOnly <= bt.ToDate);
            var compOff = compOffRecords.Any(co => dateOnly >= co.FromDate && dateOnly <= co.ToDate);
            var weeklyOff = weeklyOffRecords.FirstOrDefault(wo => wo.TxnDate == dateOnly);
            var holiday = holidayRecords.FirstOrDefault(h => h.Transactions.Any(t => dateOnly >= t.FromDate && dateOnly <= t.ToDate));
            return new
            {
                date = date.ToString("yyyy-MM-dd"),
                day = date.DayOfWeek.ToString(),
                status = statusName,
                statusColorId = color,
                login = attendance?.LoginTime.HasValue == true ? attendance.LoginTime.Value.ToString("hh:mm tt") : "00:00:000",
                logout = attendance?.LogoutTime.HasValue == true ? attendance.LogoutTime.Value.ToString("hh:mm tt") : "00:00:000",
                //totalHoursWorked = attendance?.TotalHoursWorked ?? 0,
                totalHoursWorked = attendance?.TotalHoursWorked != null ? Math.Round(attendance.TotalHoursWorked, 2) : 0.00,
                shiftName = attendance?.ShiftName,
                workFromHome = workFromHome,
                leaveTypeName = leave?.LeaveTypeName,
                onDuty = onDuty,
                businessTravel = businessTravel,
                compOff = compOff,
                weeklyOff = weeklyOff != null,
                holidayName = holiday?.HolidayName
            };
        }).ToList();

        return new
        {
            staff.Id,
            StaffId = staff.StaffId,
            staff.StaffName,
            Month = startDate.ToString("MMMM"),
            Year = year,
            Details = result
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

        var staffOrCreatorId = compOffCreditDto.StaffId ?? compOffCreditDto.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = compOffCredit.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendCompOffCreditRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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
        return message;
    }

    public async Task<string> CreateAsync(CompOffAvailRequest request)
    {
        var message = "CompOff Avail Successfully";

        var isHolidayWorkingExists = await _context.CompOffCredits
      .AnyAsync(h => h.WorkedDate == request.WorkedDate
          && (h.StaffId != null ? h.StaffId == request.StaffId : h.CreatedBy == request.StaffId)
          && h.Status1 == true);

        if (!isHolidayWorkingExists)
        {
            throw new Exception("WorkedDate does not match the date in CompOffCredit or the record is not active.");
        }

        var lastCompOffCredit = await _context.CompOffCredits
            .Where(c => (c.StaffId != null ? c.StaffId == request.StaffId : c.CreatedBy == request.StaffId))
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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = compOff.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendCompOffApprovalRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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
            var getLeaves = await (from leave in _context.LeaveRequisitions
                                   join leaveType in _context.LeaveTypes on leave.LeaveTypeId equals leaveType.Id
                                   join application in _context.ApplicationTypes on leave.ApplicationTypeId equals application.Id
                                   join leaveStaff in _context.StaffCreations on leave.StaffId equals leaveStaff.Id into leaveStaffJoin
                                   from leaveStaff in leaveStaffJoin.DefaultIfEmpty()
                                   join creatorStaff in _context.StaffCreations on leave.CreatedBy equals creatorStaff.Id
                                   join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                   where leave.IsActive == true
                                         && (leave.StaffId == null || leaveStaff.IsActive == true)
                                         && creatorStaff.IsActive == true
                                         && leave.IsCancelled == null
                                         && (!fromDate.HasValue || leave.FromDate >= fromDate)
                                         && (!toDate.HasValue || leave.ToDate <= toDate)

                                         // Check approval logic
                                         && (
                                             approverId < 0 || // No filter
                                             (
                                                 // Approver 1 Logic
                                                 (
                                                     leave.StaffId.HasValue &&
                                                     leaveStaff.ApprovalLevel1 == approverId &&
                                                     leave.Status1 == null &&
                                                     leave.ApplicationTypeId == 1
                                                 ) ||
                                                 (
                                                     !leave.StaffId.HasValue &&
                                                     creatorStaff.ApprovalLevel1 == approverId &&
                                                     leave.Status1 == null &&
                                                     leave.ApplicationTypeId == 1
                                                 )
                                             ) ||
                                             (
                                                 // Approver 2 Logic
                                                 (
                                                     leave.StaffId.HasValue &&
                                                     leaveStaff.ApprovalLevel2 == approverId &&
                                                     leave.Status1 == true &&
                                                     leave.Status2 == null &&
                                                     leave.ApplicationTypeId == 1 &&
                                                     leave.Status1 != false
                                                 ) ||
                                                 (
                                                     !leave.StaffId.HasValue &&
                                                     creatorStaff.ApprovalLevel2 == approverId &&
                                                     leave.Status1 == true &&
                                                     leave.Status2 == null &&
                                                     leave.ApplicationTypeId == 1 &&
                                                     leave.Status1 != false
                                                 )
                                             )
                                         )

                                         // Staff filter (optional)
                                         && (
                                             staffIds == null || !staffIds.Any() ||
                                             (
                                                 (leave.StaffId.HasValue && staffIds.Contains(leave.StaffId.Value)) ||
                                                 (!leave.StaffId.HasValue && staffIds.Contains(leave.CreatedBy))
                                             )
                                         )
                                   select new
                                   {
                                       leave.Id,
                                       leave.ApplicationTypeId,
                                       ApplicationType = application.ApplicationTypeName,
                                       StaffId = leave.StaffId ?? leave.CreatedBy,
                                       StaffName = leave.StaffId.HasValue
                                           ? leaveStaff.FirstName + " " + (leaveStaff.LastName ?? "")
                                           : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                       leave.StartDuration,
                                       leave.EndDuration,
                                       LeaveType = leaveType.Name,
                                       leave.FromDate,
                                       leave.ToDate,
                                       leave.TotalDays,
                                       leave.Reason
                                   }).ToListAsync();

            if (!getLeaves.Any())
            {
                throw new MessageNotFoundException("No leave requisition found");
            }
            result.AddRange(getLeaves.Cast<object>());
        }

        else if (applicationTypeId.HasValue && applicationTypeId == 2)
        {
            var getCommonPermissions = await (from permission in _context.CommonPermissions
                                              join staff in _context.StaffCreations on permission.StaffId equals staff.Id into staffJoin
                                              from staff in staffJoin.DefaultIfEmpty()
                                              join creatorStaff in _context.StaffCreations on permission.CreatedBy equals creatorStaff.Id
                                              join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                              where permission.IsActive == true
                                                    && (permission.StaffId == null || staff.IsActive == true)
                                                    && creatorStaff.IsActive == true
                                                    && permission.IsCancelled == null

                                                    // Approver logic
                                                    && (
                                                        approverId < 0 ||
                                                        (
                                                            // Approver 1
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
                                                            )
                                                        ) ||
                                                        (
                                                            // Approver 2
                                                            (
                                                                permission.StaffId.HasValue &&
                                                                staff.ApprovalLevel2 == approverId &&
                                                                permission.Status1 == true &&
                                                                permission.Status2 == null &&
                                                                permission.ApplicationTypeId == 2 &&
                                                                permission.Status1 != false
                                                            ) ||
                                                            (
                                                                !permission.StaffId.HasValue &&
                                                                creatorStaff.ApprovalLevel2 == approverId &&
                                                                permission.Status1 == true &&
                                                                permission.Status2 == null &&
                                                                permission.ApplicationTypeId == 2 &&
                                                                permission.Status1 != false
                                                            )
                                                        )
                                                    )

                                                    // Staff filter (optional)
                                                    && (
                                                        staffIds == null || !staffIds.Any() ||
                                                        (
                                                            (permission.StaffId.HasValue && staffIds.Contains(permission.StaffId.Value)) ||
                                                            (!permission.StaffId.HasValue && staffIds.Contains(permission.CreatedBy))
                                                        )
                                                    )
                                              select new
                                              {
                                                  permission.Id,
                                                  permission.ApplicationTypeId,
                                                  permission.PermissionType,
                                                  StaffId = permission.StaffId ?? permission.CreatedBy,
                                                  StaffName = permission.StaffId.HasValue
                                                      ? staff.FirstName + " " + (staff.LastName ?? "")
                                                      : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                                  permission.PermissionDate,
                                                  permission.StartTime,
                                                  permission.EndTime,
                                                  permission.TotalHours,
                                                  permission.Remarks,
                                                  permission.Status1,
                                              }).ToListAsync();

            if (!getCommonPermissions.Any())
            {
                throw new MessageNotFoundException("No common permission found");
            }
            result.AddRange(getCommonPermissions.Cast<object>());
        }



        else if (applicationTypeId.HasValue && applicationTypeId == 3)
        {
            var getManualPunch = await (from punch in _context.ManualPunchRequistions
                                        join application in _context.ApplicationTypes on punch.ApplicationTypeId equals application.Id
                                        join staff in _context.StaffCreations on punch.StaffId equals staff.Id into staffJoin
                                        from staff in staffJoin.DefaultIfEmpty() // Left join for StaffId
                                        join creatorStaff in _context.StaffCreations on punch.CreatedBy equals creatorStaff.Id
                                        join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                        where punch.IsActive == true
                                              && (punch.StaffId == null || staff.IsActive == true)
                                              && creatorStaff.IsActive == true
                                              && punch.IsCancelled == null

                                              // Approver logic
                                              && (
                                                  approverId < 0 ||
                                                  (
                                                      // Approver 1
                                                      (
                                                          punch.StaffId.HasValue &&
                                                          staff.ApprovalLevel1 == approverId &&
                                                          punch.Status1 == null &&
                                                          punch.ApplicationTypeId == 3
                                                      ) ||
                                                      (
                                                          !punch.StaffId.HasValue &&
                                                          creatorStaff.ApprovalLevel1 == approverId &&
                                                          punch.Status1 == null &&
                                                          punch.ApplicationTypeId == 3
                                                      )
                                                  ) ||
                                                  (
                                                      // Approver 2
                                                      (
                                                          punch.StaffId.HasValue &&
                                                          staff.ApprovalLevel2 == approverId &&
                                                          punch.Status1 == true &&
                                                          punch.Status2 == null &&
                                                          punch.ApplicationTypeId == 3 &&
                                                          punch.Status1 != false
                                                      ) ||
                                                      (
                                                          !punch.StaffId.HasValue &&
                                                          creatorStaff.ApprovalLevel2 == approverId &&
                                                          punch.Status1 == true &&
                                                          punch.Status2 == null &&
                                                          punch.ApplicationTypeId == 3 &&
                                                          punch.Status1 != false
                                                      )
                                                  )
                                              )

                                              // StaffId filter
                                              && (
                                                  staffIds == null || !staffIds.Any() ||
                                                  (
                                                      (punch.StaffId.HasValue && staffIds.Contains(punch.StaffId.Value)) ||
                                                      (!punch.StaffId.HasValue && staffIds.Contains(punch.CreatedBy))
                                                  )
                                              )
                                        select new
                                        {
                                            punch.Id,
                                            punch.ApplicationTypeId,
                                            ApplicationType = application.ApplicationTypeName,
                                            StaffId = punch.StaffId ?? punch.CreatedBy,
                                            StaffName = punch.StaffId.HasValue
                                                ? staff.FirstName + " " + (staff.LastName ?? "")
                                                : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                            punch.SelectPunch,
                                            punch.InPunch,
                                            punch.OutPunch,
                                            punch.Remarks,
                                            punch.Status1,
                                            punch.Status2,
                                            punch.IsActive,
                                            punch.CreatedBy
                                        }).ToListAsync();

            if (!getManualPunch.Any())
            {
                throw new MessageNotFoundException("No manual punch requisition found");
            }
            result.AddRange(getManualPunch.Cast<object>());
        }

        else if (applicationTypeId.HasValue && applicationTypeId == 4)
        {
            var getOnDutyRequisitions = await (from duty in _context.OnDutyRequisitions
                                               join application in _context.ApplicationTypes on duty.ApplicationTypeId equals application.Id
                                               join staff in _context.StaffCreations on duty.StaffId equals staff.Id into dutyStaffJoin
                                               from staff in dutyStaffJoin.DefaultIfEmpty()
                                               join creatorStaff in _context.StaffCreations on duty.CreatedBy equals creatorStaff.Id
                                               join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                               where duty.IsActive == true
                                                     && (duty.StaffId == null || staff.IsActive == true)
                                                     && creatorStaff.IsActive == true
                                                     && duty.IsCancelled == null
                                                     && (!fromDate.HasValue || duty.StartDate >= fromDate)
                                                     && (!toDate.HasValue || duty.EndDate <= toDate)
                                                     && (
                                                         approverId < 0 ||                                                    
                                                         (
                                                             (duty.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                              duty.Status1 == null && duty.ApplicationTypeId == 4) ||
                                                             (!duty.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                              duty.Status1 == null && duty.ApplicationTypeId == 4)
                                                         ) ||
                                                         (
                                                             (duty.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                              duty.Status1 == true && duty.Status2 == null &&
                                                              duty.Status1 != false && duty.ApplicationTypeId == 4) ||
                                                             (!duty.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                              duty.Status1 == true && duty.Status2 == null &&
                                                              duty.Status1 != false && duty.ApplicationTypeId == 4)
                                                         )
                                                     )

                                                     // Filter by staff IDs
                                                     && (
                                                         staffIds == null || !staffIds.Any() ||
                                                         (duty.StaffId.HasValue && staffIds.Contains(duty.StaffId.Value)) ||
                                                         (!duty.StaffId.HasValue && staffIds.Contains(duty.CreatedBy))
                                                     )
                                               select new
                                               {
                                                   duty.Id,
                                                   duty.ApplicationTypeId,
                                                   ApplicationType = application.ApplicationTypeName,
                                                   StaffId = duty.StaffId ?? duty.CreatedBy,
                                                   StaffName = duty.StaffId.HasValue
                                                       ? staff.FirstName + " " + (staff.LastName ?? "")
                                                       : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
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

            if (!getOnDutyRequisitions.Any())
            {
                throw new MessageNotFoundException("No on-duty requisition found");
            }

            result.AddRange(getOnDutyRequisitions.Cast<object>());
        }


        else if (applicationTypeId.HasValue && applicationTypeId == 5)
        {
            var getBusinessTravels = await (from travel in _context.BusinessTravels
                                            join application in _context.ApplicationTypes on travel.ApplicationTypeId equals application.Id
                                            join staff in _context.StaffCreations on travel.StaffId equals staff.Id into travelStaffJoin
                                            from staff in travelStaffJoin.DefaultIfEmpty()
                                            join creatorStaff in _context.StaffCreations on travel.CreatedBy equals creatorStaff.Id
                                            join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                            where travel.IsActive == true
                                                  && (travel.StaffId == null || staff.IsActive == true)
                                                  && creatorStaff.IsActive == true
                                                  && travel.IsCancelled == null
                                                  && (!fromDate.HasValue || travel.FromDate >= fromDate)
                                                  && (!toDate.HasValue || travel.ToDate <= toDate)
                                                  && (
                                                      approverId < 0 ||
                                                      (
                                                          (travel.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                           travel.Status1 == null && travel.ApplicationTypeId == 5) ||
                                                          (!travel.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                           travel.Status1 == null && travel.ApplicationTypeId == 5)
                                                      ) ||
                                                      (
                                                          (travel.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                           travel.Status1 == true && travel.Status2 == null &&
                                                           travel.Status1 != false && travel.ApplicationTypeId == 5) ||
                                                          (!travel.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                           travel.Status1 == true && travel.Status2 == null &&
                                                           travel.Status1 != false && travel.ApplicationTypeId == 5)
                                                      )
                                                  )
                                                  && (
                                                      staffIds == null || !staffIds.Any() ||
                                                      (travel.StaffId.HasValue && staffIds.Contains(travel.StaffId.Value)) ||
                                                      (!travel.StaffId.HasValue && staffIds.Contains(travel.CreatedBy))
                                                  )
                                            select new
                                            {
                                                travel.Id,
                                                travel.ApplicationTypeId,
                                                ApplicationType = application.ApplicationTypeName,
                                                StaffId = travel.StaffId ?? travel.CreatedBy,
                                                StaffName = travel.StaffId.HasValue
                                                    ? staff.FirstName + " " + (staff.LastName ?? "")
                                                    : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
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

            if (!getBusinessTravels.Any())
            {
                throw new MessageNotFoundException("No business travel found");
            }

            result.AddRange(getBusinessTravels.Cast<object>());
        }


        else if (applicationTypeId.HasValue && applicationTypeId == 6)
        {
            var getWorkFromHomes = await (from workFromHome in _context.WorkFromHomes
                                          join application in _context.ApplicationTypes on workFromHome.ApplicationTypeId equals application.Id
                                          join staff in _context.StaffCreations on workFromHome.StaffId equals staff.Id into workFromHomeStaffJoin
                                          from staff in workFromHomeStaffJoin.DefaultIfEmpty()
                                          join creatorStaff in _context.StaffCreations on workFromHome.CreatedBy equals creatorStaff.Id
                                          join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                          where workFromHome.IsActive == true
                                                && (workFromHome.StaffId == null || staff.IsActive == true)
                                                && creatorStaff.IsActive == true
                                                && workFromHome.IsCancelled == null
                                                && (!fromDate.HasValue || workFromHome.FromDate >= fromDate)
                                                && (!toDate.HasValue || workFromHome.ToDate <= toDate)             
                                                && (
                                                    approverId < 0 ||                                            
                                                    (
                                                        (workFromHome.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                         workFromHome.Status1 == null && workFromHome.ApplicationTypeId == 6) ||
                                                        (!workFromHome.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                         workFromHome.Status1 == null && workFromHome.ApplicationTypeId == 6)
                                                    ) ||
                                                    (
                                                        (workFromHome.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                         workFromHome.Status1 == true && workFromHome.Status2 == null &&
                                                         workFromHome.Status1 != false && workFromHome.ApplicationTypeId == 6) ||
                                                        (!workFromHome.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                         workFromHome.Status1 == true && workFromHome.Status2 == null &&
                                                         workFromHome.Status1 != false && workFromHome.ApplicationTypeId == 6)
                                                    )
                                                )

                                                // Staff filter
                                                && (
                                                    staffIds == null || !staffIds.Any() ||
                                                    (workFromHome.StaffId.HasValue && staffIds.Contains(workFromHome.StaffId.Value)) ||
                                                    (!workFromHome.StaffId.HasValue && staffIds.Contains(workFromHome.CreatedBy))
                                                )
                                          select new
                                          {
                                              workFromHome.Id,
                                              workFromHome.ApplicationTypeId,
                                              ApplicationType = application.ApplicationTypeName,
                                              StaffId = workFromHome.StaffId ?? workFromHome.CreatedBy,
                                              StaffName = workFromHome.StaffId.HasValue
                                                  ? staff.FirstName + " " + (staff.LastName ?? "")
                                                  : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
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

            if (!getWorkFromHomes.Any())
            {
                throw new MessageNotFoundException("No work from home found");
            }

            result.AddRange(getWorkFromHomes.Cast<object>());
        }


        else if (applicationTypeId.HasValue && applicationTypeId == 7)
        {
            var getShiftChanges = await (from shiftChange in _context.ShiftChanges
                                         join application in _context.ApplicationTypes on shiftChange.ApplicationTypeId equals application.Id
                                         join shift in _context.Shifts on shiftChange.ShiftId equals shift.Id
                                         join staff in _context.StaffCreations on shiftChange.StaffId equals staff.Id into staffJoin
                                         from staff in staffJoin.DefaultIfEmpty()  
                                         join creatorStaff in _context.StaffCreations on shiftChange.CreatedBy equals creatorStaff.Id
                                         join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                         where shiftChange.IsActive == true
                                               && (shiftChange.StaffId == null || staff.IsActive == true)
                                               && creatorStaff.IsActive == true
                                               && shiftChange.IsCancelled == null
                                               && (!fromDate.HasValue || shiftChange.FromDate >= fromDate)
                                               && (!toDate.HasValue || shiftChange.ToDate <= toDate)
                                            
                                               && (
                                                   approverId < 0 ||
                                                   (
                                                       (shiftChange.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                        shiftChange.Status1 == null && shiftChange.ApplicationTypeId == 7) ||
                                                       (!shiftChange.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                        shiftChange.Status1 == null && shiftChange.ApplicationTypeId == 7)
                                                   ) ||                                                
                                                   (
                                                       (shiftChange.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                        shiftChange.Status1 == true && shiftChange.Status2 == null &&
                                                        shiftChange.Status1 != false && shiftChange.ApplicationTypeId == 7) ||
                                                       (!shiftChange.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                        shiftChange.Status1 == true && shiftChange.Status2 == null &&
                                                        shiftChange.Status1 != false && shiftChange.ApplicationTypeId == 7)
                                                   )
                                               )            
                                               && (
                                                   staffIds == null || !staffIds.Any() ||
                                                   (shiftChange.StaffId.HasValue && staffIds.Contains(shiftChange.StaffId.Value)) ||
                                                   (!shiftChange.StaffId.HasValue && staffIds.Contains(shiftChange.CreatedBy))
                                               )
                                         select new
                                         {
                                             shiftChange.Id,
                                             shiftChange.ApplicationTypeId,
                                             ApplicationType = application.ApplicationTypeName,
                                             StaffId = shiftChange.StaffId ?? shiftChange.CreatedBy,
                                             StaffName = shiftChange.StaffId.HasValue
                                                 ? staff.FirstName + " " + (staff.LastName ?? "")
                                                 : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                             shiftChange.FromDate,
                                             shiftChange.ToDate,
                                             shiftChange.Reason,
                                             shiftChange.Status1,
                                             shiftChange.Status2,
                                             shiftChange.CreatedBy,
                                             ShiftName = shift.ShiftName
                                         }).ToListAsync();

            if (!getShiftChanges.Any())
            {
                throw new MessageNotFoundException("No shift change found");
            }

            result.AddRange(getShiftChanges.Cast<object>());
        }


        else if (applicationTypeId.HasValue && applicationTypeId == 8)
        {
            var getShiftExtensions = await (from shiftExtension in _context.ShiftExtensions
                                            join staff in _context.StaffCreations on shiftExtension.StaffId equals staff.Id into staffJoin
                                            from staff in staffJoin.DefaultIfEmpty()
                                            join creatorStaff in _context.StaffCreations on shiftExtension.CreatedBy equals creatorStaff.Id
                                            join application in _context.ApplicationTypes on shiftExtension.ApplicationTypeId equals application.Id
                                            join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                            where shiftExtension.IsActive == true
                                                  && (shiftExtension.StaffId == null || staff.IsActive == true)
                                                  && creatorStaff.IsActive == true
                                                  && shiftExtension.IsCancelled == null
                                                  && (staffIds == null || !staffIds.Any() ||
                                                      (shiftExtension.StaffId.HasValue
                                                          ? staffIds.Contains(shiftExtension.StaffId.Value)
                                                          : staffIds.Contains(shiftExtension.CreatedBy)))
                                                  && (
                                                      approverId < 0 ||
                                                      (
                                                          (shiftExtension.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                           shiftExtension.Status1 == null && shiftExtension.ApplicationTypeId == 8) ||
                                                          (!shiftExtension.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                           shiftExtension.Status1 == null && shiftExtension.ApplicationTypeId == 8)
                                                      ) ||
                                                      (
                                                          (shiftExtension.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                           shiftExtension.Status1 == true && shiftExtension.Status2 == null &&
                                                           shiftExtension.Status1 != false && shiftExtension.ApplicationTypeId == 8) ||
                                                          (!shiftExtension.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                           shiftExtension.Status1 == true && shiftExtension.Status2 == null &&
                                                           shiftExtension.Status1 != false && shiftExtension.ApplicationTypeId == 8)
                                                      )
                                                  )
                                                  && (!fromDate.HasValue || shiftExtension.TransactionDate >= fromDate)
                                                  && (!toDate.HasValue || shiftExtension.TransactionDate <= toDate)
                                            select new
                                            {
                                                shiftExtension.Id,
                                                shiftExtension.ApplicationTypeId,
                                                ApplicationType = application.ApplicationTypeName,
                                                StaffId = shiftExtension.StaffId ?? shiftExtension.CreatedBy,
                                                StaffName = shiftExtension.StaffId.HasValue
                                                    ? staff.FirstName + " " + (staff.LastName ?? "")
                                                    : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                                shiftExtension.TransactionDate,
                                                shiftExtension.DurationHours,
                                                shiftExtension.BeforeShiftHours,
                                                shiftExtension.AfterShiftHours,
                                                shiftExtension.Remarks,
                                                shiftExtension.Status1,
                                                shiftExtension.Status2,
                                                shiftExtension.CreatedBy
                                            }).ToListAsync();

            if (!getShiftExtensions.Any())
            {
                throw new MessageNotFoundException("No shift extension found");
            }

            result.AddRange(getShiftExtensions.Cast<object>());
        }


        else if (applicationTypeId.HasValue && applicationTypeId == 9)
        {
            var getWeeklyOffHolidayWorking = await (from holidayWorking in _context.WeeklyOffHolidayWorkings
                                                    join staff in _context.StaffCreations on holidayWorking.StaffId equals staff.Id into staffJoin
                                                    from staff in staffJoin.DefaultIfEmpty()
                                                    join creatorStaff in _context.StaffCreations on holidayWorking.CreatedBy equals creatorStaff.Id
                                                    join application in _context.ApplicationTypes on holidayWorking.ApplicationTypeId equals application.Id
                                                    join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                                    where holidayWorking.IsActive == true
                                                          && (holidayWorking.StaffId == null || staff.IsActive == true)
                                                          && creatorStaff.IsActive == true
                                                          && holidayWorking.IsCancelled == null
                                                          && (staffIds == null || !staffIds.Any() ||
                                                              (holidayWorking.StaffId.HasValue
                                                                  ? staffIds.Contains(holidayWorking.StaffId.Value)
                                                                  : staffIds.Contains(holidayWorking.CreatedBy)))
                                                          && (
                                                              approverId < 0 ||

                                                              (
                                                                  (holidayWorking.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                                   holidayWorking.Status1 == null && holidayWorking.ApplicationTypeId == 9) ||
                                                                  (!holidayWorking.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                                   holidayWorking.Status1 == null && holidayWorking.ApplicationTypeId == 9)
                                                              ) ||
                                                              (
                                                                  (holidayWorking.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                                   holidayWorking.Status1 == true && holidayWorking.Status2 == null &&
                                                                   holidayWorking.Status1 != false && holidayWorking.ApplicationTypeId == 9) ||
                                                                  (!holidayWorking.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                                   holidayWorking.Status1 == true && holidayWorking.Status2 == null &&
                                                                   holidayWorking.Status1 != false && holidayWorking.ApplicationTypeId == 9)
                                                              )
                                                          )                                                       
                                                          && (!fromDate.HasValue || holidayWorking.TxnDate >= fromDate)
                                                          && (!toDate.HasValue || holidayWorking.TxnDate <= toDate)
                                                    select new
                                                    {
                                                        holidayWorking.Id,
                                                        holidayWorking.ApplicationTypeId,
                                                        ApplicationType = application.ApplicationTypeName,
                                                        StaffId = holidayWorking.StaffId ?? holidayWorking.CreatedBy,
                                                        StaffName = holidayWorking.StaffId.HasValue
                                                            ? staff.FirstName + " " + (staff.LastName ?? "")
                                                            : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                                        holidayWorking.TxnDate,
                                                        holidayWorking.SelectShiftType,
                                                        holidayWorking.ShiftId,
                                                        holidayWorking.ShiftInTime,
                                                        holidayWorking.ShiftOutTime,
                                                        holidayWorking.Status1,
                                                        holidayWorking.Status2,
                                                        holidayWorking.CreatedBy
                                                    }).ToListAsync();

            if (!getWeeklyOffHolidayWorking.Any())
            {
                throw new MessageNotFoundException("No weekly off holiday working found");
            }

            result.AddRange(getWeeklyOffHolidayWorking.Cast<object>());
        }



        else if (applicationTypeId.HasValue && applicationTypeId == 10)
        {
            var getCompOffAvail = await (from compOff in _context.CompOffAvails
                                         join staff in _context.StaffCreations on compOff.StaffId equals staff.Id into staffJoin
                                         from staff in staffJoin.DefaultIfEmpty()
                                         join creatorStaff in _context.StaffCreations on compOff.CreatedBy equals creatorStaff.Id
                                         join application in _context.ApplicationTypes on compOff.ApplicationTypeId equals application.Id
                                         join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                         where compOff.IsActive == true
                                               && (compOff.StaffId == null || staff.IsActive == true)
                                               && creatorStaff.IsActive == true
                                               && compOff.IsCancelled == null

                                               // Staff filtering
                                               && (staffIds == null || !staffIds.Any() ||
                                                   (compOff.StaffId.HasValue && staffIds.Contains(compOff.StaffId.Value)) ||
                                                   (!compOff.StaffId.HasValue && staffIds.Contains(compOff.CreatedBy)))                                       
                                               && (
                                                   approverId < 0 ||
                                                   (
                                                       (compOff.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                        compOff.Status1 == null && compOff.ApplicationTypeId == 10) ||
                                                       (!compOff.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                        compOff.Status1 == null && compOff.ApplicationTypeId == 10)
                                                   ) || 
                                                   (
                                                       (compOff.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                        compOff.Status1 == true && compOff.Status2 == null &&
                                                        compOff.Status1 != false && compOff.ApplicationTypeId == 10) ||
                                                       (!compOff.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                        compOff.Status1 == true && compOff.Status2 == null &&
                                                        compOff.Status1 != false && compOff.ApplicationTypeId == 10)
                                                   )
                                               )
                                               && (!fromDate.HasValue || compOff.FromDate >= fromDate)
                                               && (!toDate.HasValue || compOff.ToDate <= toDate)
                                         select new
                                         {
                                             compOff.Id,
                                             compOff.ApplicationTypeId,
                                             ApplicationType = application.ApplicationTypeName,
                                             StaffId = compOff.StaffId ?? compOff.CreatedBy,
                                             StaffName = compOff.StaffId.HasValue
                                                 ? staff.FirstName + " " + (staff.LastName ?? "")
                                                 : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                             compOff.WorkedDate,
                                             compOff.FromDate,
                                             compOff.ToDate,
                                             compOff.FromDuration,
                                             compOff.ToDuration,
                                             compOff.Reason,
                                             compOff.TotalDays,
                                             compOff.Status1,
                                             compOff.Status2,
                                             compOff.CreatedBy
                                         }).ToListAsync();

            if (!getCompOffAvail.Any())
            {
                throw new MessageNotFoundException("No Comp Off Avail found");
            }

            result.AddRange(getCompOffAvail.Cast<object>());
        }


        else if (applicationTypeId.HasValue && applicationTypeId == 11)
        {
            var getCompOffCredit = await (from compOff in _context.CompOffCredits
                                          join staff in _context.StaffCreations on compOff.StaffId equals staff.Id into staffJoin
                                          from staff in staffJoin.DefaultIfEmpty()
                                          join creatorStaff in _context.StaffCreations on compOff.CreatedBy equals creatorStaff.Id
                                          join application in _context.ApplicationTypes on compOff.ApplicationTypeId equals application.Id
                                          join org in _context.OrganizationTypes on creatorStaff.OrganizationTypeId equals org.Id
                                          where compOff.IsActive == true
                                                && (compOff.StaffId == null || staff.IsActive == true)
                                                && creatorStaff.IsActive == true
                                                && compOff.IsCancelled == null
                                                && (staffIds == null || !staffIds.Any() ||
                                                    (compOff.StaffId.HasValue && staffIds.Contains(compOff.StaffId.Value)) ||
                                                    (!compOff.StaffId.HasValue && staffIds.Contains(compOff.CreatedBy)))
                                                && (
                                                    approverId < 0 ||
                                                    (
                                                        (compOff.StaffId.HasValue && staff.ApprovalLevel1 == approverId &&
                                                         compOff.Status1 == null && compOff.ApplicationTypeId == 11) ||
                                                        (!compOff.StaffId.HasValue && creatorStaff.ApprovalLevel1 == approverId &&
                                                         compOff.Status1 == null && compOff.ApplicationTypeId == 11)
                                                    ) ||
                                                    (
                                                        (compOff.StaffId.HasValue && staff.ApprovalLevel2 == approverId &&
                                                         compOff.Status1 == true && compOff.Status2 == null &&
                                                         compOff.ApplicationTypeId == 11) ||
                                                        (!compOff.StaffId.HasValue && creatorStaff.ApprovalLevel2 == approverId &&
                                                         compOff.Status1 == true && compOff.Status2 == null &&
                                                         compOff.ApplicationTypeId == 11)
                                                    )
                                                )

                                          select new
                                          {
                                              compOff.Id,
                                              compOff.ApplicationTypeId,
                                              ApplicationType = application.ApplicationTypeName,
                                              StaffId = compOff.StaffId ?? compOff.CreatedBy,
                                              StaffName = compOff.StaffId.HasValue
                                                  ? staff.FirstName + " " + (staff.LastName ?? "")
                                                  : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                              compOff.WorkedDate,
                                              compOff.TotalDays,
                                              compOff.Reason,
                                              compOff.Status1,
                                              compOff.Status2,
                                              compOff.CreatedBy
                                          }).ToListAsync();

            if (!getCompOffCredit.Any())
            {
                throw new MessageNotFoundException("No Comp Off Credit found");
            }

            result.AddRange(getCompOffCredit.Cast<object>());
        }

        else if (applicationTypeId.HasValue && applicationTypeId == 18)
        {
            var getReimbursements = await (from reimbursement in _context.Reimbursements
                                           join staff in _context.StaffCreations on reimbursement.StaffId equals staff.Id into staffJoin
                                           from staff in staffJoin.DefaultIfEmpty()
                                           join creatorStaff in _context.StaffCreations on reimbursement.CreatedBy equals creatorStaff.Id
                                           join reimbursementType in _context.ReimbursementTypes on reimbursement.ReimbursementTypeId equals reimbursementType.Id
                                           where reimbursement.IsActive == true
                                                 && reimbursement.CancelledOn == null
                                                 && (fromDate == null || reimbursement.BillDate >= fromDate)
                                                 && (toDate == null || reimbursement.BillDate <= toDate)
                                                 && (
                                                     staffIds == null || !staffIds.Any() ||
                                                     (reimbursement.StaffId.HasValue && staffIds.Contains(reimbursement.StaffId.Value)) ||
                                                     (!reimbursement.StaffId.HasValue && staffIds.Contains(reimbursement.CreatedBy))
                                                 )
                                                 &&
                                                 (
                                                     approverId < 0 ||
                                                     (
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
                                                     )
                                                 )
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
                                               StaffName = reimbursement.StaffId.HasValue
                                                   ? staff.FirstName + " " + (staff.LastName ?? "")
                                                   : creatorStaff.FirstName + " " + (creatorStaff.LastName ?? ""),
                                               reimbursement.Status1,
                                               reimbursement.Status2,
                                               Status = reimbursement.CancelledOn.HasValue ? "Cancelled" :
                                                        reimbursement.Status1.HasValue ? (reimbursement.Status1.Value ? "Approved" : "Rejected") : "Pending",
                                               reimbursement.CreatedUtc,
                                               reimbursement.CreatedBy
                                           }).ToListAsync();

            if (!getReimbursements.Any())
            {
                throw new MessageNotFoundException("No reimbursements found");
            }

            result.AddRange(getReimbursements.Cast<object>());
        }



        return result;
    }
    public async Task<string> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest)
    {
        var message = "";
        var selectedRows = approveLeaveRequest.SelectedRows;
        var approverName = _context.StaffCreations.Where(s => s.Id == approveLeaveRequest.ApprovedBy && s.IsActive == true).Select(s => $"{s.FirstName}{s.LastName}").FirstOrDefault();
        //string approvedDateTime = DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var applicationType = await _context.ApplicationTypes.Where(a => a.Id == approveLeaveRequest.ApplicationTypeId && a.IsActive).Select(a => a.ApplicationTypeName).FirstOrDefaultAsync();
        foreach (var item in selectedRows)
        {
            if (approveLeaveRequest.ApplicationTypeId == 1)
            {
                var leave = await _context.LeaveRequisitions.FirstOrDefaultAsync(l => l.Id == item.Id && l.IsActive);
                if (leave == null) throw new MessageNotFoundException("Leave request not found");
                var leave1 = await _context.LeaveRequisitions.Where(l => l.Id == item.Id && (l.Status1 == false || l.Status2 == false) && l.IsActive == true).FirstOrDefaultAsync();
                if (leave1 != null) throw new InvalidOperationException("Leave request already rejected");
                var leaveType = await _context.LeaveTypes.Where(l => l.Id == leave.LeaveTypeId && l.IsActive).Select(l => l.Name).FirstOrDefaultAsync();
                var staffOrCreatorId = leave.StaffId ?? leave.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                if (approveLeaveRequest.IsApproved)
                {
                    if (staff.ApprovalLevel2 == null)
                    {
                        if (leave.Status1 == null)
                        {
                            var individualLeave = await _context.IndividualLeaveCreditDebits
                                .Where(l => l.StaffCreationId == staffOrCreatorId
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
                                if (leave.StaffId != null)
                                {
                                    throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
                                }
                                else
                                {
                                    throw new MessageNotFoundException("Insufficient leave balance found");
                                }
                            }
                            leave.Status1 = true;
                            leave.IsActive = false;
                            leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            leave.UpdatedUtc = DateTime.UtcNow;
                        }
                        else if (leave.Status1 != null && leave.Status1 == true)
                        {
                            throw new InvalidOperationException("Leave request already approved");
                        }
                    }
                    else if (staff.ApprovalLevel2 != null)
                    {
                        if (leave.Status1 == null)
                        {
                            var individualLeave = await _context.IndividualLeaveCreditDebits
                                .Where(l => l.StaffCreationId == staffOrCreatorId
                                            && l.LeaveTypeId == leave.LeaveTypeId
                                            && l.IsActive == true)
                                .OrderByDescending(l => l.Id)
                                .FirstOrDefaultAsync();

                            if (individualLeave != null && individualLeave.AvailableBalance > 0 && individualLeave.AvailableBalance >= leave.TotalDays)
                            {
                                //individualLeave.AvailableBalance = decimal.Subtract(individualLeave.AvailableBalance, leave.TotalDays);
                                individualLeave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                individualLeave.UpdatedUtc = DateTime.UtcNow;
                            }
                            else
                            {
                                if (leave.StaffId != null)
                                {
                                    throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
                                }
                                else
                                {
                                    throw new MessageNotFoundException("Insufficient leave balance found");
                                }
                            }
                            leave.Status1 = true;
                            leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            leave.UpdatedUtc = DateTime.UtcNow;
                        }
                        else if (leave.Status1 == true && leave.Status2 == null)
                        {
                            var individualLeave = await _context.IndividualLeaveCreditDebits
                                .Where(l => l.StaffCreationId == staffOrCreatorId
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
                                if (leave.StaffId != null)
                                {
                                    throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
                                }
                                else
                                {
                                    throw new MessageNotFoundException("Insufficient leave balance found");
                                }
                            }
                            leave.Status2 = true;
                            leave.IsActive = false;
                            leave.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            leave.ApprovedOn = DateTime.UtcNow;
                        }
                        else if (leave.Status1 == true && leave.Status2 == true)
                        {
                            throw new InvalidOperationException("Leave request already approved");
                        }
                    }
                    await _context.SaveChangesAsync();

                    string requestedTime = leave.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = leave.UpdatedUtc.HasValue ? leave.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    var notification = new ApprovalNotification
                    {
                        StaffId = leave.CreatedBy,
                        Message = $"Your leave request has been approved. Approved by - {approverName} on {approvedTime}",
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();
                    leave.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();
                    if (approver1 != null)
                    {
                        await _emailService.SendLeaveApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            leaveType: leaveType,
                            fromDate: leave.FromDate,
                            toDate: leave.ToDate,
                            totalDays: leave.TotalDays,
                            reason: leave.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!leave.IsEmailSent.HasValue || leave.IsEmailSent == false))
                        {
                            await _emailService.SendLeaveRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                applicationTypeId: leave.ApplicationTypeId,
                                id: leave.Id,
                                leaveType: leaveType,
                                fromDate: leave.FromDate,
                                toDate: leave.ToDate,
                                totalDays: leave.TotalDays,
                                reason: leave.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            leave.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                    message = "Leave request approved successfully";
                }
                else if (!approveLeaveRequest.IsApproved)
                {
                    if (staff.ApprovalLevel2 == null)
                    {
                        if (leave.Status1 == null)
                        {
                            leave.Status1 = false;
                            leave.IsActive = false;
                            leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            leave.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    else if (staff.ApprovalLevel2 != null)
                    {
                        if (leave.Status1 == null)
                        {
                            leave.Status1 = false;
                            leave.IsActive = false;
                            leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            leave.UpdatedUtc = DateTime.UtcNow;
                        }
                        else if (leave.Status1 == true && leave.Status2 == null)
                        {
                            leave.Status2 = false;
                            leave.IsActive = false;
                            leave.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            leave.ApprovedOn = DateTime.UtcNow;
                        }
                    }
                    await _context.SaveChangesAsync();

                    string requestedTime = leave.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = leave.UpdatedUtc.HasValue ? leave.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    var notification = new ApprovalNotification
                    {
                        StaffId = leave.CreatedBy,
                        Message = $"Your leave request has been rejected. Rejected by - {approverName} on {approvedTime}",
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();
                    leave.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendLeaveApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            leaveType: leaveType,
                            totalDays: leave.TotalDays,
                            fromDate: leave.FromDate,
                            toDate: leave.ToDate,
                            reason: leave.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );

                        if (approver2 != null && (!leave.IsEmailSent.HasValue || leave.IsEmailSent == false))
                        {
                            await _emailService.SendLeaveRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                applicationTypeId: leave.ApplicationTypeId,
                                id: leave.Id,
                                leaveType: leaveType,
                                fromDate: leave.FromDate,
                                toDate: leave.ToDate,
                                totalDays: leave.TotalDays,
                                reason: leave.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            leave.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                    message = "Leave request rejected successfully";
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 2)
            {
                var permissionRequest = await _context.CommonPermissions.Where(l => l.Id == item.Id).FirstOrDefaultAsync();
                //if (permission == null) throw new MessageNotFoundException("Permission request not found");
                var permissionType = await _context.PermissionTypes.Where(l => l.Name == permissionRequest.PermissionType && l.IsActive).Select(l => l.Name).FirstOrDefaultAsync();
                var staffOrCreatorId = permissionRequest.StaffId ?? permissionRequest.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                if(approver2 == null)
                {
                    var permissionRequest1 = await _context.CommonPermissions
                        .Where(l => l.Id == item.Id && l.IsActive == false)
                        .FirstOrDefaultAsync();
                    if(permissionRequest1 != null)
                    {
                        if (permissionRequest1.Status1.HasValue && permissionRequest1.Status1 != null)
                        {
                            string statusMessage = permissionRequest.Status1 == true
                                ? "Common Permission request has already been approved."
                                : "Common Permission request has already been rejected.";

                            throw new MessageNotFoundException(statusMessage);
                        }
                    }

                    if (permissionRequest != null && permissionRequest.Status1 == null)
                    {
                        permissionRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                        permissionRequest.IsActive = false;
                        permissionRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        permissionRequest.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    var permissionRequest1 = await _context.CommonPermissions
                        .Where(l => l.Id == item.Id && l.Status1 == true && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == true)
                        .FirstOrDefaultAsync();
                    if(permissionRequest1 != null)
                    {
                        if (permissionRequest1.Status1.HasValue && permissionRequest1.Status1 != null)
                        {
                            string statusMessage = permissionRequest.Status1 == true
                                ? "Common Permission request has already been approved."
                                : "Common Permission request has already been rejected.";

                            throw new MessageNotFoundException(statusMessage);
                        }
                    }

                    if (permissionRequest != null && permissionRequest.Status1 == null)
                    {
                        permissionRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                        permissionRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        permissionRequest.UpdatedUtc = DateTime.UtcNow;
                    }

                    var permissionRequest2 = await _context.CommonPermissions
                        .Where(l => l.Id == item.Id && l.Status1 == true && l.Status2 == true && l.ApprovedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false)
                        .FirstOrDefaultAsync();
                    if(permissionRequest2 != null)
                    {
                        if (permissionRequest2.Status2.HasValue && permissionRequest2.Status2 != null)
                        {
                            string statusMessage = permissionRequest.Status2 == true
                                ? "Common Permission request has already been approved."
                                : "Common Permission request has already been rejected.";

                            throw new MessageNotFoundException(statusMessage);
                        }
                    }

                    if (permissionRequest != null && permissionRequest.Status1 == true && permissionRequest.Status2 == null)
                    {
                        permissionRequest.Status2 = approveLeaveRequest.IsApproved ? true : false;
                        permissionRequest.IsActive = false;
                        permissionRequest.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        permissionRequest.ApprovedOn = DateTime.UtcNow;
                    }
                }
                await _context.SaveChangesAsync();

                string approvedTime = permissionRequest.UpdatedUtc.HasValue ? permissionRequest.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                message = approveLeaveRequest.IsApproved
                    ? "Common Permission request approved successfully"
                    : "Common Permission request rejected successfully";

                var notificationMessage = approveLeaveRequest.IsApproved
                    ? $"Your common permission request has been approved. Approved by - {approverName} on {approvedTime}"
                    : $"Your common permission request has been rejected. Rejected by - {approverName} on {approvedTime}";

                var notification = new ApprovalNotification
                {
                    StaffId = permissionRequest.CreatedBy,
                    Message = notificationMessage,
                    IsActive = true,
                    CreatedBy = approveLeaveRequest.ApprovedBy,
                    CreatedUtc = DateTime.UtcNow
                };

                _context.ApprovalNotifications.Add(notification);
                await _context.SaveChangesAsync();

                permissionRequest.ApprovalNotificationId = notification.Id;
                await _context.SaveChangesAsync();

                var duration = permissionRequest.EndTime - permissionRequest.StartTime;
                var formattedDuration = $"{duration.Hours:D2}:{duration.Minutes:D2}";
                string requestDateTime = permissionRequest.CreatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                if (approver1 != null)
                {
                    await _emailService.SendCommonPermissionApprovalEmail(
                        recipientEmail: staff.OfficialEmail,
                        recipientId: staff.Id,
                        recipientName: staffName,
                        isApproved: approveLeaveRequest.IsApproved,
                        permissionType: permissionType,
                        permissionDate: permissionRequest.PermissionDate,
                        startTime: permissionRequest.StartTime,
                        endTime: permissionRequest.EndTime,
                        approvedBy: approveLeaveRequest.ApprovedBy,
                        approverName: approverName,
                        approvedTime: approvedTime
                    );
                    if (approver2 != null && (!permissionRequest.IsEmailSent.HasValue || permissionRequest.IsEmailSent == false))
                    {
                        await _emailService.SendCommonPermissionRequestEmail(
                            recipientEmail: approver2.OfficialEmail,
                            recipientId: approver2.Id,
                            recipientName: $"{approver2.FirstName} {approver2.LastName}",
                            applicationTypeId: permissionRequest.ApplicationTypeId,
                            id: permissionRequest.Id,
                            permissionType: permissionRequest.PermissionType,
                            permissionDate: permissionRequest.PermissionDate,
                            startTime: permissionRequest.StartTime,
                            endTime: permissionRequest.EndTime,
                            duration: formattedDuration,
                            remarks: permissionRequest.Remarks,
                            createdBy: staffOrCreatorId,
                            creatorName: staffName,
                            requestDate: requestDateTime
                        );
                        permissionRequest.IsEmailSent = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 3)
            {
                var manualPunch = await _context.ManualPunchRequistions
                    .Where(m => m.Id == item.Id && m.IsActive == true)
                    .FirstOrDefaultAsync();
                if (manualPunch == null) throw new MessageNotFoundException("Manual punch request not found");
                var punchType = manualPunch.SelectPunch;
                var staffOrCreatorId = manualPunch.StaffId ?? manualPunch.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                if(approver2 == null)
                {
                    if (manualPunch != null && manualPunch.Status1 == null)
                    {
                        manualPunch.Status1 = approveLeaveRequest.IsApproved;
                        manualPunch.IsActive = false;
                        manualPunch.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        manualPunch.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (manualPunch != null && manualPunch.Status1 == null)
                    {
                        manualPunch.Status1 = approveLeaveRequest.IsApproved;
                        manualPunch.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        manualPunch.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (manualPunch != null && manualPunch.Status1 == true && manualPunch.Status2 == null)
                    {
                        manualPunch.Status2 = approveLeaveRequest.IsApproved;
                        manualPunch.IsActive = false;
                        manualPunch.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        manualPunch.ApprovedOn = DateTime.UtcNow;
                    }
                }
                await _context.SaveChangesAsync();

                string requestedTime = manualPunch.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                string approvedTime = manualPunch.UpdatedUtc.HasValue ? manualPunch.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                message = approveLeaveRequest.IsApproved
                    ? "Manual punch request approved successfully"
                    : "Manual punch request rejected successfully";

                var notificationMessage = approveLeaveRequest.IsApproved
                    ? $"Your manual punch request has been approved. Approved by - {approverName} on {approvedTime}"
                    : $"Your manual punch request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                if (approver1 != null)
                {
                    await _emailService.SendManualPunchApprovalEmail(
                        recipientEmail: staff.OfficialEmail,
                        recipientId: staff.Id,
                        recipientName: staffName,
                        isApproved: approveLeaveRequest.IsApproved,
                        punchType: manualPunch.SelectPunch,
                        approvedBy: approveLeaveRequest.ApprovedBy,
                        approverName: approverName,
                        approvedTime: approvedTime
                    );
                    if (approver2 != null && (!manualPunch.IsEmailSent.HasValue || manualPunch.IsEmailSent == false))
                    {
                        await _emailService.SendManualPunchRequestEmail(
                            recipientEmail: approver2.OfficialEmail,
                            recipientId: approver2.Id,
                            recipientName: $"{approver2.FirstName} {approver2.LastName}",
                            staffName: staffName,
                            id: manualPunch.Id,
                            applicationTypeId: manualPunch.ApplicationTypeId,
                            selectPunch: manualPunch.SelectPunch,
                            inPunch: manualPunch.InPunch,
                            outPunch: manualPunch.OutPunch,
                            remarks: manualPunch.Remarks,
                            createdBy: staffOrCreatorId,
                            requestDate: requestedTime
                        );
                        manualPunch.IsEmailSent = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 4)
            {
                var onDuty = await _context.OnDutyRequisitions
                    .Where(o => o.Id == item.Id && o.IsActive == true)
                    .FirstOrDefaultAsync();
                if (onDuty == null) throw new MessageNotFoundException("On duty request not found");
                var staffOrCreatorId = onDuty.StaffId ?? onDuty.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                if(approver2 == null)
                {
                    if (onDuty != null && onDuty.Status1 == null)
                    {
                        onDuty.Status1 = approveLeaveRequest.IsApproved;
                        onDuty.IsActive = false;
                        onDuty.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        onDuty.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (onDuty != null && onDuty.Status1 == null)
                    {
                        onDuty.Status1 = approveLeaveRequest.IsApproved;
                        onDuty.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        onDuty.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (onDuty != null && onDuty.Status1 == true && onDuty.Status2 == null)
                    {
                        onDuty.Status2 = approveLeaveRequest.IsApproved;
                        onDuty.IsActive = false;
                        onDuty.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        onDuty.ApprovedOn = DateTime.UtcNow;
                    }
                }
                await _context.SaveChangesAsync();

                string requestedTime = onDuty.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                string approvedTime = onDuty.UpdatedUtc.HasValue ? onDuty.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                message = approveLeaveRequest.IsApproved ? "On-duty request approved successfully" : "On-duty request rejected successfully";

                var notificationMessage = approveLeaveRequest.IsApproved
                    ? $"Your on-duty request has been approved. Approved by - {approverName} on {approvedTime}"
                    : $"Your on-duty request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                if (approver1 != null)
                {
                    await _emailService.SendOnDutyApprovalEmail(
                        recipientEmail: staff.OfficialEmail,
                        recipientId: staff.Id,
                        recipientName: staffName,
                        isApproved: approveLeaveRequest.IsApproved,
                        startDate: onDuty.StartDate,
                        endDate: onDuty.EndDate,
                        startTime: onDuty.StartTime,
                        endTime: onDuty.EndTime,
                        totalDays: onDuty.TotalDays,
                        totalHours: onDuty.TotalHours,
                        reason: onDuty.Reason,
                        approvedBy: approveLeaveRequest.ApprovedBy,
                        approverName: approverName,
                        approvedTime: approvedTime
                    );
                    if (approver2 != null && (!onDuty.IsEmailSent.HasValue || onDuty.IsEmailSent == false))
                    {
                        await _emailService.SendOnDutyRequestEmail(
                            recipientEmail: approver2.OfficialEmail,
                            recipientId: approver2.Id,
                            recipientName: $"{approver2.FirstName} {approver2.LastName}",
                            id: onDuty.Id,
                            applicationTypeId: onDuty.ApplicationTypeId,
                            startDate: onDuty.StartDate,
                            endDate: onDuty.EndDate,
                            startTime: onDuty.StartTime,
                            endTime: onDuty.EndTime,
                            totalDays: onDuty.TotalDays,
                            totalHours: onDuty.TotalHours,
                            reason: onDuty.Reason,
                            createdBy: staffOrCreatorId,
                            creatorName: staffName,
                            requestDate: requestedTime
                        );
                        onDuty.IsEmailSent = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 5)
            {
                var businessTravel = await _context.BusinessTravels.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                if (businessTravel == null) throw new MessageNotFoundException("Business travel request not found");
                var staffOrCreatorId = businessTravel.StaffId ?? businessTravel.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                if(approver2 == null)
                {
                    if (businessTravel != null && businessTravel.Status1 == null)
                    {
                        businessTravel.Status1 = approveLeaveRequest.IsApproved;
                        businessTravel.IsActive = false;
                        businessTravel.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        businessTravel.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (businessTravel != null && businessTravel.Status1 == null)
                    {
                        businessTravel.Status1 = approveLeaveRequest.IsApproved;
                        businessTravel.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        businessTravel.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (businessTravel != null && businessTravel.Status1 == true && businessTravel.Status2 == null)
                    {
                        businessTravel.Status2 = approveLeaveRequest.IsApproved;
                        businessTravel.IsActive = false;
                        businessTravel.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        businessTravel.ApprovedOn = DateTime.UtcNow;
                    }
                }
                await _context.SaveChangesAsync();

                    string requestedTime = businessTravel.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = businessTravel.UpdatedUtc.HasValue ? businessTravel.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Business travel request approved successfully" : "Business travel request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your business travel request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your business travel request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                    if(approver1 != null)
                    {
                        await _emailService.SendBusinessTravelApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            fromDate: businessTravel.FromDate,
                            toDate: businessTravel.ToDate,
                            fromTime: businessTravel.FromTime,
                            toTime: businessTravel.ToTime,
                            reason: businessTravel.Reason,
                            totalDays: businessTravel.TotalDays,
                            totalHours: businessTravel.TotalHours,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!businessTravel.IsEmailSent.HasValue || businessTravel.IsEmailSent == false))
                        {
                            await _emailService.SendBusinessTravelRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: businessTravel.Id,
                                applicationTypeId: businessTravel.ApplicationTypeId,
                                fromDate: businessTravel.FromDate,
                                toDate: businessTravel.ToDate,
                                fromTime: businessTravel.FromTime,
                                toTime: businessTravel.ToTime,
                                totalDays: businessTravel.TotalDays,
                                totalHours: businessTravel.TotalHours,
                                reason: businessTravel.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            businessTravel.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 6)
            {
                var workFromHome = await _context.WorkFromHomes.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                if (workFromHome == null) throw new MessageNotFoundException("Work from home request not found");
                var staffOrCreatorId = workFromHome.StaffId ?? workFromHome.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                if(approver2 == null)
                {
                    if (workFromHome != null && workFromHome.Status1 == null)
                    {
                        workFromHome.Status1 = approveLeaveRequest.IsApproved;
                        workFromHome.IsActive = false;
                        workFromHome.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        workFromHome.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (workFromHome != null && workFromHome.Status1 == null)
                    {
                        workFromHome.Status1 = approveLeaveRequest.IsApproved;
                        workFromHome.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        workFromHome.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (workFromHome != null && workFromHome.Status1 == true && workFromHome.Status2 == null)
                    {
                        workFromHome.Status2 = approveLeaveRequest.IsApproved;
                        workFromHome.IsActive = false;
                        workFromHome.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        workFromHome.ApprovedOn = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();

                    string requestedTime = workFromHome.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = workFromHome.UpdatedUtc.HasValue ? workFromHome.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Work from home request approved successfully" : "Work from home request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your work from home request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your work from home request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                    if(approver1 != null)
                    {
                        await _emailService.SendWorkFromHomeApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            fromDate: workFromHome.FromDate,
                            toDate: workFromHome.ToDate,
                            fromTime: workFromHome.FromTime,
                            toTime: workFromHome.ToTime,
                            reason: workFromHome.Reason,
                            totalDays: workFromHome.TotalDays,
                            totalHours: workFromHome.TotalHours,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!workFromHome.IsEmailSent.HasValue || workFromHome.IsEmailSent == false))
                        {
                            await _emailService.SendWorkFromHomeRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: workFromHome.Id,
                                applicationTypeId: workFromHome.ApplicationTypeId,
                                fromDate: workFromHome.FromDate,
                                toDate: workFromHome.ToDate,
                                fromTime: workFromHome.FromTime,
                                toTime: workFromHome.ToTime,
                                totalDays: workFromHome.TotalDays,
                                totalHours: workFromHome.TotalHours,
                                reason: workFromHome.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            workFromHome.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 7)
            {
                var shiftChange = await _context.ShiftChanges.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                if (shiftChange == null) throw new MessageNotFoundException("Shift change request not found");
                var staffOrCreatorId = shiftChange.StaffId ?? shiftChange.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                var shiftName = await _context.Shifts.Where(s => s.Id == shiftChange.ShiftId && s.IsActive).Select(s => s.ShiftName).FirstOrDefaultAsync();

                if(approver2 == null)
                {
                    if (shiftChange != null && shiftChange.Status1 == null)
                    {
                        shiftChange.Status1 = approveLeaveRequest.IsApproved;
                        shiftChange.IsActive = false;
                        shiftChange.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        shiftChange.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (shiftChange != null && shiftChange.Status1 == null)
                    {
                        shiftChange.Status1 = approveLeaveRequest.IsApproved;
                        shiftChange.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        shiftChange.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (shiftChange != null && shiftChange.Status1 == true && shiftChange.Status2 == null)
                    {
                        shiftChange.Status2 = approveLeaveRequest.IsApproved;
                        shiftChange.IsActive = false;
                        shiftChange.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        shiftChange.ApprovedOn = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();

                    string requestedTime = shiftChange.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = shiftChange.UpdatedUtc.HasValue ? shiftChange.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Shift change request approved successfully" : "Shift change request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your shift change request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your shift change request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                    if(approver1 != null)
                    {
                        await _emailService.SendShiftChangeApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            shiftName: shiftName,
                            fromDate: shiftChange.FromDate,
                            toDate: shiftChange.ToDate,
                            reason: shiftChange.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!shiftChange.IsEmailSent.HasValue || shiftChange.IsEmailSent == false))
                        {
                            await _emailService.SendShiftChangeRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: shiftChange.Id,
                                applicationTypeId: shiftChange.ApplicationTypeId,
                                shiftName: shiftName,
                                fromDate: shiftChange.FromDate,
                                toDate: shiftChange.ToDate,
                                reason: shiftChange.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            shiftChange.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 8)
            {
                var shiftExtensionRequest = await _context.ShiftExtensions.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                if (shiftExtensionRequest == null) throw new MessageNotFoundException("Shift extension request not found");
                var staffOrCreatorId = shiftExtensionRequest.StaffId ?? shiftExtensionRequest.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                var shiftExtension = await _context.ShiftExtensions
                    .Where(s => s.Id == item.Id && s.IsActive)
                    .FirstOrDefaultAsync();

                if(approver2 == null)
                {
                    if (shiftExtension != null && shiftExtension.Status1 == null)
                    {
                        shiftExtension.Status1 = approveLeaveRequest.IsApproved;
                        shiftExtension.IsActive = false;
                        shiftExtension.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        shiftExtension.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (shiftExtension != null && shiftExtension.Status1 == null)
                    {
                        shiftExtension.Status1 = approveLeaveRequest.IsApproved;
                        shiftExtension.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        shiftExtension.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (shiftExtension != null && shiftExtension.Status1 == true && shiftExtension.Status2 == null)
                    {
                        shiftExtension.Status2 = approveLeaveRequest.IsApproved;
                        shiftExtension.IsActive = false;
                        shiftExtension.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        shiftExtension.ApprovedOn = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();

                    string requestedTime = shiftExtension.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = shiftExtension.UpdatedUtc.HasValue ? shiftExtension.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Shift extension request approved successfully" : "Shift extension request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your shift extension request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your shift extension request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                    if(approver1 != null)
                    {
                        await _emailService.SendShiftExtensionApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            transactionDate: shiftExtension.TransactionDate,
                            durationHours: shiftExtension.DurationHours,
                            beforeShiftHours: shiftExtension.BeforeShiftHours,
                            afterShiftHours: shiftExtension.AfterShiftHours,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!shiftExtension.IsEmailSent.HasValue || shiftExtension.IsEmailSent == false))
                        {
                            await _emailService.SendShiftExtensionRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: shiftExtension.Id,
                                applicationTypeId: shiftExtension.ApplicationTypeId,
                                transactionDate: shiftExtension.TransactionDate,
                                durationHours: shiftExtension.DurationHours,
                                beforeShiftHours: shiftExtension.BeforeShiftHours,
                                afterShiftHours: shiftExtension.AfterShiftHours,
                                remarks: shiftExtension.Remarks,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            shiftExtension.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 9)
            {
                var weeklyOffHolidayRequest = await _context.WeeklyOffHolidayWorkings.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                if (weeklyOffHolidayRequest == null) throw new MessageNotFoundException("Weekly off holiday working request not found");
                var staffOrCreatorId = weeklyOffHolidayRequest.StaffId ?? weeklyOffHolidayRequest.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var shiftName = await _context.Shifts.Where(s => s.Id == weeklyOffHolidayRequest.ShiftId && s.IsActive).Select(s => s.ShiftName).FirstOrDefaultAsync();
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                var weeklyOffHoliday = await _context.WeeklyOffHolidayWorkings
                    .Where(w => w.Id == item.Id && w.IsActive)
                    .FirstOrDefaultAsync();

                if(approver2 == null)
                {
                    if (weeklyOffHoliday != null && weeklyOffHoliday.Status1 == null)
                    {
                        weeklyOffHoliday.Status1 = approveLeaveRequest.IsApproved;
                        weeklyOffHoliday.IsActive = false;
                        weeklyOffHoliday.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        weeklyOffHoliday.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (weeklyOffHoliday != null && weeklyOffHoliday.Status1 == null)
                    {
                        weeklyOffHoliday.Status1 = approveLeaveRequest.IsApproved;
                        weeklyOffHoliday.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        weeklyOffHoliday.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (weeklyOffHoliday != null && weeklyOffHoliday.Status1 == true && weeklyOffHoliday.Status2 == null)
                    {
                        weeklyOffHoliday.Status2 = approveLeaveRequest.IsApproved;
                        weeklyOffHoliday.IsActive = false;
                        weeklyOffHoliday.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        weeklyOffHoliday.ApprovedOn = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();

                    string requestedTime = weeklyOffHoliday.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = weeklyOffHoliday.UpdatedUtc.HasValue ? weeklyOffHoliday.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Weekly off/holiday working request approved successfully" : "Weekly off/holiday working request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your weekly off/holiday working request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your weekly off/holiday working request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                    if(approver1 != null)
                    {
                        await _emailService.SendWeeklyOffHolidayApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            shiftSelectType: weeklyOffHoliday.SelectShiftType,
                            shiftName: shiftName,
                            shiftInTime: weeklyOffHoliday.ShiftInTime,
                            shiftOutTime: weeklyOffHoliday.ShiftOutTime,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!weeklyOffHoliday.IsEmailSent.HasValue || weeklyOffHoliday.IsEmailSent == false))
                        {
                            await _emailService.SendWeeklyOffHolidayWorkingRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                selectShiftType: weeklyOffHoliday.SelectShiftType,
                                id: weeklyOffHoliday.Id,
                                applicationTypeId: weeklyOffHoliday.ApplicationTypeId,
                                txnDate: weeklyOffHoliday.TxnDate,
                                shiftName: shiftName,
                                shiftInTime: weeklyOffHoliday.ShiftInTime,
                                shiftOutTime: weeklyOffHoliday.ShiftOutTime,
                                requestDate: requestedTime,
                                createdBy: staffOrCreatorId
                            );
                            weeklyOffHoliday.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 10)
            {
                var compOffAvailRequest = await _context.CompOffAvails.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                if (compOffAvailRequest == null) throw new MessageNotFoundException("Compoff avail request not found");
                var staffOrCreatorId = compOffAvailRequest.StaffId ?? compOffAvailRequest.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                var compOffAvail = await _context.CompOffAvails
                    .Where(c => c.Id == item.Id && c.IsActive)
                    .FirstOrDefaultAsync();

                if(approver2 == null)
                {
                    if (compOffAvail != null && compOffAvail.Status1 == null)
                    {
                        compOffAvail.Status1 = approveLeaveRequest.IsApproved;
                        compOffAvail.IsActive = false;
                        compOffAvail.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        compOffAvail.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if (approver2 != null)
                {
                    if (compOffAvail != null && compOffAvail.Status1 == null)
                    {
                        compOffAvail.Status1 = approveLeaveRequest.IsApproved;
                        compOffAvail.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        compOffAvail.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (compOffAvail != null && compOffAvail.Status1 == true && compOffAvail.Status2 == null)
                    {
                        compOffAvail.Status2 = approveLeaveRequest.IsApproved;
                        compOffAvail.IsActive = false;
                        compOffAvail.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        compOffAvail.ApprovedOn = DateTime.UtcNow;
                    }
                }

                if (approveLeaveRequest.IsApproved)
                {
                    if (approver2 == null)
                    {
                        if (compOffAvail != null && compOffAvail.Status1 == null)
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
                    }
                    if(approver2 != null)
                    {
                        if (compOffAvail != null && compOffAvail.Status1 == true && compOffAvail.Status2 == null)
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
                    }
                }

                    await _context.SaveChangesAsync();

                    string requestedTime = compOffAvail.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = compOffAvail.UpdatedUtc.HasValue ? compOffAvail.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved
                        ? "Comp-Off Avail request approved successfully"
                        : "Comp-Off Avail request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your Comp-Off avail request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your Comp-Off avail request has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                    if(approver1 != null)
                    {
                        await _emailService.SendCompOffAvailApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            workedDate: compOffAvail.WorkedDate,
                            fromDate: compOffAvail.FromDate,
                            toDate: compOffAvail.ToDate,
                            totalDays: compOffAvail.TotalDays,
                            reason: compOffAvail.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!compOffAvail.IsEmailSent.HasValue || compOffAvail.IsEmailSent == false))
                        {
                            await _emailService.SendCompOffApprovalRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                id: compOffAvail.Id,
                                applicationTypeId: compOffAvail.ApplicationTypeId,
                                workedDate: compOffAvail.WorkedDate,
                                fromDate: compOffAvail.FromDate,
                                toDate: compOffAvail.ToDate,
                                totalDays: compOffAvail.TotalDays,
                                reason: compOffAvail.Reason,
                                requestDate: requestedTime,
                                createdBy: staffOrCreatorId
                            );
                            compOffAvail.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 11)
            {
                var compOffCreditRequest = await _context.CompOffCredits.Where(l => l.Id == item.Id && l.IsActive == true).FirstOrDefaultAsync();
                if (compOffCreditRequest == null) throw new MessageNotFoundException("Compoff credit request not found");
                var staffOrCreatorId = compOffCreditRequest.StaffId ?? compOffCreditRequest.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                var compOffCredit = await _context.CompOffCredits
                    .Where(c => c.Id == item.Id && c.Status1 == null)
                    .FirstOrDefaultAsync();

                if(approver2 == null)
                {
                    if (compOffCredit != null && compOffCredit.Status1 == null)
                    {
                        compOffCredit.Status1 = approveLeaveRequest.IsApproved;
                        compOffCredit.IsActive = false;
                        compOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        compOffCredit.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if(approver2 != null)
                {
                    if (compOffCredit != null && compOffCredit.Status1 == null)
                    {
                        compOffCredit.Status1 = approveLeaveRequest.IsApproved;
                        compOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        compOffCredit.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (compOffCredit != null && compOffCredit.Status1 == true && compOffCredit.Status2 == null)
                    {
                        compOffCredit.Status2 = approveLeaveRequest.IsApproved;
                        compOffCredit.IsActive = false;
                        compOffCredit.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        compOffCredit.ApprovedOn = DateTime.UtcNow;
                    }
                }

                if (approveLeaveRequest.IsApproved)
                {
                    if (approver2 == null)
                    {
                        if (compOffCredit != null && compOffCredit.Status1 == null)
                        {
                            compOffCredit.Balance = (compOffCredit.Balance) + 1;
                        }
                    }

                    if(approver2 != null)
                    {
                        if (compOffCredit != null && compOffCredit.Status1 == true && compOffCredit.Status2 == null)
                        {
                            compOffCredit.Balance = (compOffCredit.Balance) + 1;
                        }
                    }
                }
                    await _context.SaveChangesAsync();

                    string requestedTime = compOffCredit.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = compOffCredit.UpdatedUtc.HasValue ? compOffCredit.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved
                        ? "Comp-Off Credit approved successfully"
                        : "Comp-Off Credit rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your Comp-Off credit has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your Comp-Off credit has been rejected. Rejected by - {approverName} on {approvedTime}";

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

                    if(approver1 != null)
                    {
                        await _emailService.SendCompOffCreditApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            workedDate: compOffCredit.WorkedDate,
                            balance: compOffCredit.Balance,
                            totalDays: compOffCredit.TotalDays,
                            reason: compOffCredit.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approver2 != null && (!compOffCredit.IsEmailSent.HasValue || compOffCredit.IsEmailSent == false))
                        {
                            await _emailService.SendCompOffCreditRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                id: compOffCredit.Id,
                                applicationTypeId: compOffCredit.ApplicationTypeId,
                                workedDate: compOffCredit.WorkedDate,
                                totalDays: compOffCredit.TotalDays,
                                balance: compOffCredit.Balance,
                                reason: compOffCredit.Reason,
                                requestDate: requestedTime,
                                createdBy: staffOrCreatorId
                            );
                            compOffCredit.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
            }
            else if (approveLeaveRequest.ApplicationTypeId == 18)
            {
                var reimbursementRequest = await _context.Reimbursements
                    .Include(r => r.ApplicationType)
                    .Where(r => r.Id == item.Id && r.IsActive == true)
                    .FirstOrDefaultAsync();

                if (reimbursementRequest == null)
                    throw new MessageNotFoundException("Reimbursement request not found");

                var staffOrCreatorId = reimbursementRequest.StaffId ?? reimbursementRequest.CreatedBy;
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                if (staff == null) throw new MessageNotFoundException("Staff not found");

                var staffName = $"{staff.FirstName} {staff.LastName}";
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                if(approver2 == null)
                {
                    if (reimbursementRequest != null && reimbursementRequest.Status1 == null)
                    {
                        reimbursementRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                        reimbursementRequest.IsActive = false;
                        reimbursementRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        reimbursementRequest.UpdatedUtc = DateTime.UtcNow;
                    }
                }
                if(approver2 != null)
                {
                    if (reimbursementRequest != null && reimbursementRequest.Status1 == null)
                    {
                        reimbursementRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                        reimbursementRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                        reimbursementRequest.UpdatedUtc = DateTime.UtcNow;
                    }
                    if (reimbursementRequest != null && reimbursementRequest.Status1 == true && reimbursementRequest.Status2 == null)
                    {
                        reimbursementRequest.Status2 = approveLeaveRequest.IsApproved ? true : false;
                        reimbursementRequest.IsActive = false;
                        reimbursementRequest.ApprovedBy = approveLeaveRequest.ApprovedBy;
                        reimbursementRequest.ApprovedOn = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();

                    string requestedTime = reimbursementRequest.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = reimbursementRequest.UpdatedUtc.HasValue ? reimbursementRequest.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = "Reimbursement request approved successfully";

                    var notificationMessage = $"Your reimbursement request has been approved. Approved by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = reimbursementRequest.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    reimbursementRequest.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendReimbursementApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: true,
                            billDate: reimbursementRequest.BillDate,
                            billNo: reimbursementRequest.BillNo,
                            description: reimbursementRequest.Description,
                            billPeriod: reimbursementRequest.BillPeriod,
                            amount: reimbursementRequest.Amount,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );

                        if (approver2 != null && (!reimbursementRequest.IsEmailSent.HasValue || reimbursementRequest.IsEmailSent == false))
                        {
                            await _emailService.SendReimbursementRequestEmail(
                                id: reimbursementRequest.Id,
                                applicationTypeId: reimbursementRequest.ApplicationTypeId,
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                requestDate: requestedTime,
                                billDate: reimbursementRequest.BillDate,
                                billNo: reimbursementRequest.BillNo,
                                description: reimbursementRequest.Description,
                                billPeriod: reimbursementRequest.BillPeriod,
                                amount: reimbursementRequest.Amount,
                                createdBy: staffOrCreatorId
                            );
                            reimbursementRequest.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
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
        var staffOrCreatorId = leaveRequisitionRequest.StaffId ?? leaveRequisitionRequest.CreatedBy;
        var individualLeave = await _context.IndividualLeaveCreditDebits
        .Where(l => l.StaffCreationId == staffOrCreatorId
                    && l.LeaveTypeId == leaveRequisitionRequest.LeaveTypeId
                    && l.IsActive == true)
        .OrderByDescending(l => l.Id)
        .FirstOrDefaultAsync();
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        var leaveType = await _context.LeaveTypes.Where(l => l.Id == leaveRequisitionRequest.LeaveTypeId && l.IsActive).Select(l => l.Name).FirstOrDefaultAsync();

        if (individualLeave == null || individualLeave.AvailableBalance == 0 || (individualLeave != null && (individualLeave.AvailableBalance > 0 && individualLeave.AvailableBalance < leaveRequisitionRequest.TotalDays)))
        {
            if (leaveRequisitionRequest.StaffId != null)
            {
                throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
            }
            else
            {
                throw new MessageNotFoundException("Insufficient leave balance found");
            }
        }

        var existingLeaves = await _context.LeaveRequisitions
        .Where(lr => ((lr.StaffId == staffOrCreatorId) || (lr.CreatedBy == staffOrCreatorId)) &&
                     (lr.FromDate <= leaveRequisitionRequest.ToDate && lr.ToDate >= leaveRequisitionRequest.FromDate))
        .ToListAsync();

        foreach (var existingLeave in existingLeaves)
        {
            bool isSameStartDate = existingLeave.FromDate == leaveRequisitionRequest.FromDate;
            bool isSameEndDate = existingLeave.ToDate == leaveRequisitionRequest.ToDate;

            // Prevent duplicate Full-Day leave
            if ((existingLeave.StartDuration == "Full Day" || leaveRequisitionRequest.StartDuration == "Full Day") ||
                (existingLeave.EndDuration == "Full Day" || leaveRequisitionRequest.EndDuration == "Full Day"))
            {
                throw new InvalidOperationException("A full-day leave is already applied within the selected date range.");
            }

            // **Allow Second Half if only First Half exists on the same date**
            if (isSameStartDate && existingLeave.StartDuration == "First Half" && leaveRequisitionRequest.StartDuration == "Second Half")
            {
                continue; // Allow the request
            }

            // **Allow First Half if only Second Half exists on the same date**
            if (isSameEndDate && existingLeave.EndDuration == "Second Half" && leaveRequisitionRequest.EndDuration == "First Half")
            {
                continue; // Allow the request
            }

            // Prevent duplicate leave for the same half-day slot
            if (isSameStartDate && isSameEndDate &&
                ((existingLeave.StartDuration == leaveRequisitionRequest.StartDuration) ||
                 (existingLeave.EndDuration == leaveRequisitionRequest.EndDuration)))
            {
                throw new InvalidOperationException("A leave requisition already exists within the selected date range.");
            }

            // Prevent overlapping leave
            if (existingLeave.FromDate <= leaveRequisitionRequest.ToDate && existingLeave.ToDate >= leaveRequisitionRequest.FromDate)
            {
                throw new InvalidOperationException("A leave requisition already exists within the selected date range.");
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

        _context.LeaveRequisitions.Add(leaveRequisition);
        await _context.SaveChangesAsync();
        string requestDateTime = leaveRequisition.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if(approver1 != null)
        {
            await _emailService.SendLeaveRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
                applicationTypeId: leaveRequisition.ApplicationTypeId,
                id: leaveRequisition.Id,
                leaveType: leaveType,
                fromDate: leaveRequisitionRequest.FromDate,
                toDate: leaveRequisitionRequest.ToDate,
                totalDays: leaveRequisitionRequest.TotalDays,
                reason: leaveRequisitionRequest.Reason,
                createdBy: staffOrCreatorId,
                creatorName: staffName,
                requestDate: requestDateTime
            );
        }
        return "Leave request submitted successfully.";
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
            var hasLeaveOnDate = await _context.LeaveRequisitions
            .AnyAsync(l =>((commonPermissionRequest.StaffId != null && l.StaffId == (int?)commonPermissionRequest.StaffId) ||
                (commonPermissionRequest.StaffId != null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.StaffId) ||
                (commonPermissionRequest.StaffId == null && l.StaffId == (int?)commonPermissionRequest.CreatedBy) ||
                (commonPermissionRequest.StaffId == null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.CreatedBy)) &&
                permissionDate >= l.FromDate && permissionDate <= l.ToDate && (l.Status1 == null || l.Status1 == true) &&
                l.IsActive == true && (l.IsCancelled == false || l.IsCancelled == null));
            if (hasLeaveOnDate)
            {
                throw new InvalidOperationException($"Cannot apply for permission on {permissionDate:yyyy-MM-dd}, as leave is already taken.");
            }
            var startOfMonth = new DateOnly(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month, 1);
            var endOfMonth = new DateOnly(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month,
                DateTime.DaysInMonth(commonPermissionRequest.PermissionDate.Year, commonPermissionRequest.PermissionDate.Month));
            var monthName = commonPermissionRequest.PermissionDate.ToString("MMMM");
            var list = _context.CommonPermissions.ToList();
            var existingPermissionOnDate = await _context.CommonPermissions
            .AnyAsync(l => ((commonPermissionRequest.StaffId != null && l.StaffId == (int?)commonPermissionRequest.StaffId) ||
                (commonPermissionRequest.StaffId != null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.StaffId) ||
                (commonPermissionRequest.StaffId == null && l.StaffId == (int?)commonPermissionRequest.CreatedBy) ||
                (commonPermissionRequest.StaffId == null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.CreatedBy)) &&
                l.PermissionDate == commonPermissionRequest.PermissionDate);
            if (existingPermissionOnDate)
            {
                throw new InvalidOperationException($"Permission for the date {commonPermissionRequest.PermissionDate:yyyy-MM-dd} already exists.");
            }
            var permissionsThisMonth = await _context.CommonPermissions
                .Where(l => ((commonPermissionRequest.StaffId != null && l.StaffId == (int?)commonPermissionRequest.StaffId) ||
                    (commonPermissionRequest.StaffId != null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.StaffId) ||
                    (commonPermissionRequest.StaffId == null && l.StaffId == (int?)commonPermissionRequest.CreatedBy) ||
                    (commonPermissionRequest.StaffId == null && l.StaffId == null && l.CreatedBy == (int?)commonPermissionRequest.CreatedBy)) &&
                                    l.PermissionDate >= startOfMonth &&
                                    l.PermissionDate <= endOfMonth)
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

            var staffOrCreatorId = commonPermissionRequest.StaffId ?? commonPermissionRequest.CreatedBy;
            var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
            if (staffId == null) throw new MessageNotFoundException("Staff not found");
            var staffName = $"{staffId.FirstName} {staffId.LastName}";
            string requestDateTime = commonPermission.CreatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
            var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
            if(approver1 != null)
            {
                await _emailService.SendCommonPermissionRequestEmail(
                    recipientEmail: approver1.OfficialEmail,
                    recipientId: approver1.Id,
                    recipientName: $"{approver1.FirstName} {approver1.LastName}",
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
            return message;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}", ex);
        }
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

        if (!permission.Any())
            throw new MessageNotFoundException("No staff permissions found");

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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = manualPunch.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendManualPunchRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = onDutyRequisition.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendOnDutyRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = businessTravel.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendBusinessTravelRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = workFromHome.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendWorkFromHomeRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        var shiftName = await _context.Shifts.Where(s => s.Id == request.ShiftId && s.IsActive).Select(s => s.ShiftName).FirstOrDefaultAsync();
        string requestDateTime = shiftChange.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendShiftChangeRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = shiftExtension.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendShiftExtensionRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = weeklyOffHolidayWorking.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var shiftName = await _context.Shifts.Where(s => s.Id == request.ShiftId && s.IsActive).Select(s => s.ShiftName).FirstOrDefaultAsync();
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if (approver1 != null)
        {
            await _emailService.SendWeeklyOffHolidayWorkingRequestEmail(
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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
        return weeklyOffHolidayWorking;
    }

    public async Task<string> AddReimbursement(ReimbursementRequestModel request)
    {
        bool reimbursementExists = await _context.Reimbursements.AnyAsync(r => r.BillNo == request.BillNo);
        if (reimbursementExists)
        {
            return $"Reimbursement with Bill No {request.BillNo} already exists.";
        }

        string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        string fileUploadPath = string.Empty;

        async Task<string> SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return null;

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
            ApplicationTypeId=request.ApplicationTypeId,
            UploadFilePath = fileUploadPath ?? string.Empty,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            IsActive = true,
            StaffId = request.StaffId,
            ReimbursementTypeId = request.ReimbursementTypeId
        };

        _context.Reimbursements.Add(reimbursement);
        await _context.SaveChangesAsync();

        var staffOrCreatorId = request.StaffId ?? request.CreatedBy;
        var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
        if (staffId == null) throw new MessageNotFoundException("Staff not found");
        var staffName = $"{staffId.FirstName} {staffId.LastName}";
        string requestDateTime = reimbursement.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
        var reimbursementType = await _context.ReimbursementTypes.Where(s => s.Id == request.ReimbursementTypeId && s.IsActive).Select(s => s.Name).FirstOrDefaultAsync();
        var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId.ApprovalLevel1 && s.IsActive == true);
        if(approver1 != null)
        {
            await _emailService.SendReimbursementRequestEmail(
                id: reimbursement.Id,
                applicationTypeId: reimbursement.ApplicationTypeId,
                recipientEmail: approver1.OfficialEmail,
                recipientId: approver1.Id,
                recipientName: $"{approver1.FirstName} {approver1.LastName}",
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

        return "Reimbursement submitted successfully.";
    }
}