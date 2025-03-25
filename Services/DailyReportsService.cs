using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
namespace AttendanceManagement.Services;
public class DailyReportsService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly StoredProcedureDbContext _storedProcedureDbContext;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DailyReportsService(AttendanceManagementSystemContext context, StoredProcedureDbContext storedProcedureDbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _storedProcedureDbContext = storedProcedureDbContext;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<object> GetDailyReports(DailyReportRequest request)
    {
        var staffIds = request.StaffIds != null && request.StaffIds.Any() ? string.Join(",", request.StaffIds) : (object)DBNull.Value;

        DateOnly fromDate = default, toDate = default;
        DateTime fromDateTime = default, toDateTime = default;

        if (request.FromDate.HasValue && request.ToDate.HasValue)
        {
            fromDate = request.FromDate.Value;
            toDate = request.ToDate.Value;
        }
        else if (request.CurrentMonth == true)
        {
            var now = DateTime.Now;
            fromDate = new DateOnly(now.Year, now.Month, 1);
            toDate = new DateOnly(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }
        else if (request.PreviousMonth == true)
        {
            var now = DateTime.Now.AddMonths(-1);
            fromDate = new DateOnly(now.Year, now.Month, 1);
            toDate = new DateOnly(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }
        //else if (request.FromDateTime.HasValue && request.ToDateTime.HasValue)
        //{
        //    fromDate = DateOnly.FromDateTime(request.FromDateTime.Value);
        //    toDate = DateOnly.FromDateTime(request.ToDateTime.Value);
        //}
      
        else if (request.FromDateTime.HasValue && request.ToDateTime.HasValue)
        {
            fromDateTime = request.FromDateTime.Value;
            toDateTime = request.ToDateTime.Value;
        }
        var reportName = await _context.TypesOfReports.Where(r => r.Id == request.DailyReportsId).Select(r => r.ReportName).FirstOrDefaultAsync();
        if (reportName == null)
        {
            throw new MessageNotFoundException("Report not found");
        }

        var user = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == request.CreatedBy && s.IsActive == true);
        var userName = "";
        var userCreationId = "";
        if (user != null)
        {
            userName = $"{user.FirstName}{user.LastName}";
            userCreationId = user.StaffId;
        }
        
        var reportName1 = reportName;
        var fromDate1 = fromDate != default
        ? fromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
        : fromDateTime.ToString("dd-MMM-yyyy h:mm tt", CultureInfo.InvariantCulture);

        var toDate1 = toDate != default
            ? toDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
            : toDateTime.ToString("dd-MMM-yyyy h:mm tt", CultureInfo.InvariantCulture);
        //var fromDate1 = fromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        //var toDate1 = toDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        var reportDate = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        var userId = request.CreatedBy;
        var userCreationId1 = userCreationId;
        var userName1 = userName;
        if (request.DailyReportsId == 3)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };

            var reportList = await _storedProcedureDbContext.CompOffAvailResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<CompOffAvailResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new CompOffAvailResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    WorkedDate = report.WorkedDate,
                    FromDate = report.FromDate,
                    ToDate = report.ToDate,
                    FromDuration = report.FromDuration,
                    ToDuration = report.ToDuration,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus = report.ApproverStatus,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
                    CancelledBy = report.CancelledBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No compoff avail requisitions found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                LeaveRequisitionRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 4)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };

            var reportList = await _storedProcedureDbContext.CompOffCreditResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<CompOffCreditResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new CompOffCreditResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    WorkedDate = report.WorkedDate,
                    Credit = report.Credit,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus = report.ApproverStatus,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
                    CancelledBy = report.CancelledBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No compoff credit requisitions found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                LeaveRequisitionRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 9)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value)
            };

            var reportList = await _storedProcedureDbContext.LeaveTakenResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<LeaveTakenResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new LeaveTakenResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    CLAvailed = report.CLAvailed,
                    SLAvailed = report.SLAvailed,
                    NCLTaken = report.NCLTaken,
                    PTLTaken = report.PTLTaken,
                    MGLTaken = report.MGLTaken
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No leave takens found");
            }
            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                LeaveTakenRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 10)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };

            var reportList = await _storedProcedureDbContext.LeaveRequisitionResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<LeaveRequisitionResponse>();


            foreach (var report in reportList)
            {
                var responseItem = new LeaveRequisitionResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    StartDuration = report.StartDuration,
                    EndDuration = report.EndDuration,
                    LeaveTypeId = report.LeaveTypeId,
                    StartDate = report.StartDate,
                    EndDate = report.EndDate,
                    TotalDays = report.TotalDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus = report.ApproverStatus,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
                    CancelledBy = report.CancelledBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No leave requisitions found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                LeaveRequisitionRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 12)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };
            var reportList = await _storedProcedureDbContext.ManualPunchResponse
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<ManualPunchResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new ManualPunchResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    PunchType = report.PunchType,
                    InTime = report.InTime,
                    OutTime = report.OutTime,
                    AppliedOn = report.AppliedOn,
                    ApprovalStatus = report.ApprovalStatus,
                    IsCancelled = report.IsCancelled,
                    AppliedBy = report.AppliedBy,
                    ApprovedBy = report.ApprovedBy,
                    ApprovedOn = report.ApprovedOn,
                    CancelledOn = report.CancelledOn,
                };
                result.Add(responseItem);
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                ManualPunchRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 13)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };

            var reportList = await _storedProcedureDbContext.OnDutyRequisitionResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<OnDutyRequisitionResponse>();


            foreach (var report in reportList)
            {
                var responseItem = new OnDutyRequisitionResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    FromDuration = report.FromDuration,
                    ToDuration = report.ToDuration,
                    From = report.From,
                    To = report.To,
                    Duration = report.Duration,
                    TotalHoursDays = report.TotalHoursDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus = report.ApproverStatus,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
                    CancelledBy = report.CancelledBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No leave requisitions found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                LeaveRequisitionRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 16)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };
            var reportList = await _storedProcedureDbContext.workFromHomeResponses
               .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
               .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<WorkFromHomeResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new WorkFromHomeResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    Duration = report.Duration,
                    From = report.From,
                    FromDuration = report.FromDuration,
                    To = report.To,
                    ToDuration = report.ToDuration,
                    TotalHoursOrDays = report.TotalHoursOrDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApprovalStatus = report.ApprovalStatus,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
                    CancelledBy = report.CancelledBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No business travel records found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                WorkFromHomeRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 17)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };
            var reportList = await _storedProcedureDbContext.BusinessTravelResponses
               .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
               .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<BusinessTravelResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new BusinessTravelResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    FromDuration = report.FromDuration,
                    ToDuration = report.ToDuration,
                    Duration = report.Duration,
                    From = report.From,
                    To = report.To,
                    TotalHoursDays = report.TotalHoursDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApprovalStatus = report.ApprovalStatus,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
                    CancelledBy = report.CancelledBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No business travel records found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                BusinessTravelRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 18)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };

            var reportList = await _storedProcedureDbContext.PermissionRequisitionResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<PermissionRequisitionResponse>();


            foreach (var report in reportList)
            {
                var responseItem = new PermissionRequisitionResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    From = report.From,
                    To = report.To,

                    PermissionDate = report.PermissionDate,
                    PermissionType = report.PermissionType,
                    TotalHours = report.TotalHours,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus = report.ApproverStatus,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
                    CancelledBy = report.CancelledBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No leave requisitions found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                LeaveRequisitionRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 19)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };
            var reportList = await _storedProcedureDbContext.LeaveBalanceResponses
               .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
               .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<LeaveBalanceResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new LeaveBalanceResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    CLBalance = report.CLBalance,
                    PLBalance = report.PLBalance,
                    SLBalance = report.SLBalance
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No business travel records found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                LeaveBalanceRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 22)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds ?? (object)DBNull.Value),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromDateTime", request.FromDateTime ?? (object)DBNull.Value),
                new SqlParameter("@ToDateTime", request.ToDateTime ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };

            var reportList = await _storedProcedureDbContext.WeeklyOffHolidayWorkingResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

            var result = new List<WeeklyOffHolidayWorkingResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new WeeklyOffHolidayWorkingResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    AttendanceDate = report.AttendanceDate,
                    ShiftIn = report.ShiftIn,
                    ShiftOut = report.ShiftOut,
                    ApprovalStatus = report.ApprovalStatus,
                    AppliedBy = report.AppliedBy,
                    ApprovedBy = report.ApprovedBy,
                    AppliedOn = report.AppliedOn,
                    ApprovedOn = report.ApprovedOn,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No records found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                WeeklyOffHolidayWorkingRecords = result
            };

            return finalResponse;
        }
        else if (request.DailyReportsId == 23)
        {
            var parameters = new[]
            {
                new SqlParameter("@DailyReportsId", request.DailyReportsId),
                new SqlParameter("@StaffIds", staffIds),
                new SqlParameter("@FromDate", request.FromDate ?? (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate ?? (object)DBNull.Value),
                new SqlParameter("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value),
                new SqlParameter("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value),
                new SqlParameter("@FromMonth", request.FromMonth ?? (object)DBNull.Value),
                new SqlParameter("@ToMonth", request.ToMonth ?? (object)DBNull.Value),
                new SqlParameter("@IncludeTerminated", request.IncludeTerminated ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedFrom", request.TerminatedFromDate ?? (object)DBNull.Value),
                new SqlParameter("@TerminatedTo", request.TerminatedToDate ?? (object)DBNull.Value),
            };

            var reportList = await _storedProcedureDbContext.VaccinationReportResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No vaccination records found");
            }

            var result = new List<VaccinationReportResponse>();

            foreach (var report in reportList)
            {
                var responseItem = new VaccinationReportResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    VaccinationDate = report.VaccinationDate,
                    SecondVaccinationDate = report.SecondVaccinationDate,
                    VaccinationNumber = report.VaccinationNumber,
                    IsExempted = report.IsExempted,
                    Comments = report.Comments,
                    ApprovedOn = report.ApprovedOn,
                    AppliedBy = report.AppliedBy,
                    ApprovedBy = report.ApprovedBy
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                throw new MessageNotFoundException("No vaccination records found");
            }

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId1,
                UserName = userName1,
                VaccinationRecords = result
            };

            return finalResponse;
        }

        throw new MessageNotFoundException("Report type not found");
    }
}
