using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceManagement.Infrastructure.Infra;
public class DailyReportsInfra : IDailyReportInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly StoredProcedureDbContext _storedProcedureDbContext;

    public DailyReportsInfra(AttendanceManagementSystemContext context, StoredProcedureDbContext storedProcedureDbContext)
    {
        _context = context;
        _storedProcedureDbContext = storedProcedureDbContext;
    }

    public async Task<List<ReportTypeResponse>> GetReportType()
    {
        var reportType = await (from report in _context.TypesOfReports
                                where report.IsActive
                                select new ReportTypeResponse
                                {
                                    Id = report.Id,
                                    Name = report.Name,
                                    CreatedBy = report.CreatedBy
                                }).ToListAsync();
        if (reportType.Count == 0) throw new MessageNotFoundException("No report types found");
        return reportType;
    }

    public async Task<object> GetDailyReports(DailyReportRequest request)
    {
        var reportName = await _context.TypesOfReports.FirstOrDefaultAsync(s => s.Id == request.DailyReportsId && s.IsActive == true);
        if (reportName == null) throw new MessageNotFoundException("Report type not found");
        var user = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == request.CreatedBy && s.IsActive == true);
        if (user == null) throw new MessageNotFoundException("User not found");
        var staffIds = request.StaffIds != null && request.StaffIds.Any() ? string.Join(",", request.StaffIds) : (object)DBNull.Value;
        DateOnly fromDate = default, toDate = default;
        DateTime fromDateTime = default, toDateTime = default;
        if (request.FromDate.HasValue && request.ToDate.HasValue)
        {
            fromDate = request.FromDate.Value;
            toDate = request.ToDate.Value;
        }
        else if (request.FromMonth.HasValue && request.ToMonth.HasValue)
        {
            fromDate = request.FromMonth.Value;
            toDate = request.ToMonth.Value;
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

        var reportName1 = reportName.Name;
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
        var userName = $"{user.FirstName}{(string.IsNullOrWhiteSpace(user.LastName) ? "" : " " + user.LastName)}";
        var userCreationId = user.StaffId;

        if (request.DailyReportsId == 1)
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
            var reportList = await _storedProcedureDbContext.AbsentListResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<AbsentListResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new AbsentListResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    TransactionDate = report.TransactionDate,
                    AttendanceStatus = report.AttendanceStatus
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 2)
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
            var reportList = await _storedProcedureDbContext.AttendanceResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<AttendanceResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new AttendanceResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    ShiftInDate = report.ShiftInDate,
                    ShiftName = report.ShiftName,
                    InTime = report.InTime,
                    OutTime = report.OutTime,
                    TotalHoursWorked = report.TotalHoursWorked,
                    AttendanceStatus = report.AttendanceStatus
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 3)
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    WorkedDate = report.WorkedDate,
                    FromDate = report.FromDate,
                    ToDate = report.ToDate,
                    FromDuration = report.FromDuration,
                    ToDuration = report.ToDuration,
                    AppliedOn = report.AppliedOn.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    WorkedDate = report.WorkedDate,
                    Credit = report.Credit,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 5)
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
            var reportList = await _storedProcedureDbContext.ContinuousAbsentListResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<ContinuousAbsentListResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new ContinuousAbsentListResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    FromDate = report.FromDate,
                    ToDate = report.ToDate,
                    TotalDays = report.TotalDays
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 6)
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
            var reportList = await _storedProcedureDbContext.CurrentDaySwipeInResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<CurrentDaySwipeInResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new CurrentDaySwipeInResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Branch = report.Branch,
                    Department = report.Department,
                    Designation = report.Designation,
                    Shift = report.Shift,
                    InTime = report.InTime
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 7)
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
            var reportList = await _storedProcedureDbContext.DailyPerformanceResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<DailyPerformanceResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new DailyPerformanceResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    Date = report.Date,
                    ShiftName = report.ShiftName,
                    InTime = report.InTime,
                    OutTime = report.OutTime,
                    TotalHoursWorked = report.TotalHoursWorked,
                    EarlyEntry = report.EarlyEntry,
                    LateEntry = report.LateEntry,
                    EarlyExit = report.EarlyExit,
                    BreakHours = report.BreakHours,
                    IsBreakHoursExceeded = report.IsBreakHoursExceeded,
                    ExtraHoursWorked = report.ExtraHoursWorked,
                    ProductiveHours = report.ProductiveHours,
                    AttendanceStatus = report.AttendanceStatus
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 8)
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
            var reportList = await _storedProcedureDbContext.FirstInLastOutResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<FirstInLastOutResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new FirstInLastOutResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    SwipeDate = report.SwipeDate,
                    Shift = report.Shift,
                    SwipeIn = report.SwipeIn,
                    SwipeOut = report.SwipeOut,
                    TotalHoursWorked = report.TotalHoursWorked
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    CLAvailed = report.CLAvailed,
                    SLAvailed = report.SLAvailed,
                    NCLTaken = report.NCLTaken,
                    PTLTaken = report.PTLTaken,
                    MGLTaken = report.MGLTaken
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    StartDuration = report.StartDuration,
                    EndDuration = report.EndDuration,
                    LeaveTypeName = report.LeaveTypeName,
                    StartDate = report.StartDate,
                    EndDate = report.EndDate,
                    TotalDays = report.TotalDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 11)
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
            var reportList = await _storedProcedureDbContext.NightShiftCountResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<NightShiftCountResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new NightShiftCountResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Category = report.Category,
                    Department = report.Department,
                    Designation = report.Designation,
                    Plant = report.Plant,
                    Date = report.Date,
                    NightShiftCount = report.NightShiftCount
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    PunchType = report.PunchType,
                    InTime = report.InTime?.ToUniversalTime(),
                    OutTime = report.OutTime?.ToUniversalTime(),
                    AppliedOn = report.AppliedOn?.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledBy = report.CancelledBy,
                    CancelledOn = report.CancelledOn?.ToUniversalTime()
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    FromDuration = report.FromDuration,
                    ToDuration = report.ToDuration,
                    From = report.From.ToUniversalTime(),
                    To = report.To?.ToUniversalTime(),
                    Duration = report.Duration,
                    TotalHoursDays = report.TotalHoursDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 14)
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
            var reportList = await _storedProcedureDbContext.PresentListResponses
                .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<PresentListResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new PresentListResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    Date = report.Date,
                    AttendanceStatus = report.AttendanceStatus
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 15)
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
            var reportList = await _storedProcedureDbContext.RawPunchResponses
               .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
               .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<RawPunchResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new RawPunchResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    SwipeDate = report.SwipeDate?.ToUniversalTime(),
                    SwipeTime = report.SwipeTime,
                    ReaderName = report.ReaderName,
                    PunchType = report.PunchType,
                    SwipeLocation = report.SwipeLocation
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    Duration = report.Duration,
                    From = report.From.ToUniversalTime(),
                    FromDuration = report.FromDuration,
                    To = report.To.ToUniversalTime(),
                    ToDuration = report.ToDuration,
                    TotalHoursOrDays = report.TotalHoursOrDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    FromDuration = report.FromDuration,
                    ToDuration = report.ToDuration,
                    Duration = report.Duration,
                    From = report.From?.ToUniversalTime(),
                    To = report.To?.ToUniversalTime(),
                    TotalHoursDays = report.TotalHoursDays,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn?.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    From = report.From.ToUniversalTime(),
                    To = report.To.ToUniversalTime(),
                    PermissionDate = report.PermissionDate,
                    PermissionType = report.PermissionType,
                    TotalHours = report.TotalHours,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    CLBalance = report.CLBalance,
                    PLBalance = report.PLBalance,
                    SLBalance = report.SLBalance
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 20)
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
            var reportList = await _storedProcedureDbContext.MonthlyReportResponse
               .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
               .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<MonthlyReportResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new MonthlyReportResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    OpeningCl = report.OpeningCl,
                    OpeningPl = report.OpeningPl,
                    OpeningSl = report.OpeningSl,
                    CLCredits = report.CLCredits,
                    PLCredits = report.PLCredits,
                    SLCredits = report.SLCredits,
                    CLDebit = report.CLDebit,
                    PLDebit = report.PLDebit,
                    SLDebit = report.SLDebit,
                    CLAvailed = report.CLAvailed,
                    PLAvailed = report.PLAvailed,
                    SLAvailed = report.SLAvailed,
                    CLClosingBalance = report.CLClosingBalance,
                    PLClosingBalance = report.PLClosingBalance,
                    SLClosingBalance = report.SLClosingBalance
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 21)
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
            var reportList = await _storedProcedureDbContext.shiftExtensionResponses
               .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
               .ToListAsync();
            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<ShiftExtensionResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new ShiftExtensionResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    ShiftName = report.ShiftName,
                    TxnDate = report.TxnDate,
                    DurationOfHoursExtension = report.DurationOfHoursExtension,
                    HoursBeforeShift = report.HoursBeforeShift?.ToUniversalTime(),
                    HoursAfterShift = report.HoursAfterShift?.ToUniversalTime(),
                    Remarks = report.Remarks,
                    AppliedOn = report.AppliedOn,
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    AttendanceDate = report.AttendanceDate?.ToUniversalTime(),
                    ShiftIn = report.ShiftIn?.ToUniversalTime(),
                    ShiftOut = report.ShiftOut?.ToUniversalTime(),
                    Approval1Status = report.Approval1Status,
                    Approval2Status = report.Approval2Status,
                    Approved1By = report.Approved1By,
                    Approved1On = report.Approved1On?.ToUniversalTime(),
                    Approved2By = report.Approved2By,
                    Approved2On = report.Approved2On?.ToUniversalTime(),
                    RejectedStatus = report.RejectedStatus,
                    RejectedBy = report.RejectedBy,
                    RejectedOn = report.RejectedOn?.ToUniversalTime(),
                    CancelledBy = report.CancelledBy,
                    AppliedOn = report.AppliedOn?.ToUniversalTime(),
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn?.ToUniversalTime()
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
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
                throw new MessageNotFoundException("No records found");
            }
            var result = new List<VaccinationReportResponse>();
            foreach (var report in reportList)
            {
                var responseItem = new VaccinationReportResponse
                {
                    StaffId = report.StaffId,
                    StaffCreationId = report.StaffCreationId,
                    Name = report.Name,
                    Department = report.Department,
                    Designation = report.Designation,
                    VaccinationDate = report.VaccinationDate?.ToUniversalTime(),
                    SecondVaccinatedDate = report.SecondVaccinatedDate?.ToUniversalTime(),
                    VaccinationNumber = report.VaccinationNumber,
                    IsExempted = report.IsExempted,
                    Comments = report.Comments,
                    ApprovedOn = report.ApprovedOn?.ToUniversalTime(),
                    AppliedBy = report.AppliedBy,
                    ApprovedBy = report.ApprovedBy
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
                UserCreationId = userCreationId,
                UserName = userName,
                Records = result
            };
            return finalResponse;
        }
        else if (request.DailyReportsId == 24)
        {
            var finalResponse = new object();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(@" EXEC DailyReport  @DailyReportId = @DailyReportId, @StaffIds = @StaffIds, @FromDate = @FromDate, @ToDate = @ToDate, @CurrentMonth = @CurrentMonth,
                @PreviousMonth = @PreviousMonth, @FromMonth = @FromMonth, @ToMonth = @ToMonth, @IncludeTerminated = @IncludeTerminated, @TerminatedFrom = @TerminatedFrom, @TerminatedTo = @TerminatedTo", connection))
                {
                    command.Parameters.AddWithValue("@DailyReportId", request.DailyReportsId);
                    command.Parameters.AddWithValue("@StaffIds", staffIds);
                    command.Parameters.AddWithValue("@FromDate", request.FromDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", request.ToDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CurrentMonth", request.CurrentMonth ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PreviousMonth", request.PreviousMonth ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FromMonth", request.FromMonth ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ToMonth", request.ToMonth ?? (object)DBNull.Value);
                    command.Parameters.Add("@IncludeTerminated", SqlDbType.Bit).Value = (object?)request.IncludeTerminated ?? DBNull.Value;
                    command.Parameters.Add("@TerminatedFrom", SqlDbType.Date).Value = (object?)request.TerminatedFromDate ?? DBNull.Value;
                    command.Parameters.Add("@TerminatedTo", SqlDbType.Date).Value = (object?)request.TerminatedToDate ?? DBNull.Value;

                    var reader = await command.ExecuteReaderAsync();
                    var records = new List<Dictionary<string, object>>();
                    var result = new List<Dictionary<string, object>>();
                    Dictionary<string, object>? grandTotalRow = null;

                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? DBNull.Value : reader.GetValue(i);
                        }
                        if (row["Emp ID"]?.ToString() == "Grand Total")
                        {
                            grandTotalRow = row;
                        }
                        else
                        {
                            result.Add(row);
                        }
                    }
                    if (result.Count == 0 && grandTotalRow == null) throw new MessageNotFoundException("No records found");
                    var columnTotals = grandTotalRow != null ? grandTotalRow
                            .Where(kvp => DateTime.TryParse(kvp.Key, out _) || kvp.Key == "Grand Total")
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value) : new Dictionary<string, object>();

                    finalResponse = new
                    {
                        ReportName = reportName1,
                        FromDate = fromDate1,
                        ToDate = toDate1,
                        ReportDate = reportDate,
                        UserId = userId,
                        UserCreationId = userCreationId,
                        UserName = userName,
                        Records = result,
                        Column_Totals = columnTotals
                    };
                }
            }
            return finalResponse;
        }
        throw new MessageNotFoundException("Report type not found");
    }

    public async Task<string> AddWorkingTypeAmount(WorkingTypeAmountRequest request)
    {
        var typeName = await _context.WorkingTypes
            .Where(w => w.Id == request.WorkingTypeId && w.IsActive)
            .Select(w => w.Name)
            .FirstOrDefaultAsync();
        if (typeName == null) throw new MessageNotFoundException("Working type not found");
        var message = $"{typeName} amount added successfully";
        var existingActiveEntries = await _context.WorkingTypeAmounts.Where(x => x.WorkingTypeId == request.WorkingTypeId && x.IsActive).ToListAsync();
        foreach (var entry in existingActiveEntries)
        {
            entry.IsActive = false;
            entry.UpdatedBy = request.CreatedBy;
            entry.UpdatedUtc = DateTime.UtcNow;
        }
        var workingTypeAmount = new WorkingTypeAmount
        {
            WorkingTypeId = request.WorkingTypeId,
            Amount = request.Amount,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.WorkingTypeAmounts.AddAsync(workingTypeAmount);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<WorkingTypeAmountResponse>> GetWorkingTypeAmount()
    {
        var workingTypeAmount = await (from wta in _context.WorkingTypeAmounts
                                       where wta.IsActive
                                       select new WorkingTypeAmountResponse
                                       {
                                           Id = wta.Id,
                                           WorkingTypeId = wta.WorkingTypeId,
                                           Amount = wta.Amount
                                       }).ToListAsync();
        if (workingTypeAmount.Count == 0) throw new MessageNotFoundException("No working type amounts found");
        return workingTypeAmount;
    }

    public async Task<string> UpdateWorkingTypeAmount(UpdateWorkingTypeAmount request)
    {
        var typeName = await _context.WorkingTypes
            .Where(w => w.Id == request.WorkingTypeId && w.IsActive)
            .Select(w => w.Name)
            .FirstOrDefaultAsync();
        if (typeName == null) throw new MessageNotFoundException("Working type not found");
        var message = $"{typeName} amount updated successfully";
        var workingTypeAmount = await _context.WorkingTypeAmounts.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsActive);
        if (workingTypeAmount == null) throw new MessageNotFoundException("Working type amount not found");
        workingTypeAmount.WorkingTypeId = request.WorkingTypeId;
        workingTypeAmount.Amount = request.Amount;
        workingTypeAmount.UpdatedBy = request.UpdatedBy;
        workingTypeAmount.UpdatedUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return message;
    }
}