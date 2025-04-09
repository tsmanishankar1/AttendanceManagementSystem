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

    public async Task<List<TypesOfReport>> GetReportType()
    {
        var reportType = await _context.TypesOfReports.ToListAsync();
        if (reportType.Count == 0)
        {
            throw new MessageNotFoundException("No report type found");
        }
        return reportType;
    }

    public async Task<object> GetDailyReports(DailyReportRequest request)
    {
        var user = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == request.CreatedBy && s.IsActive == true);
        if (user == null)
        {
            throw new MessageNotFoundException("User not found");
        }

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
            throw new MessageNotFoundException("Report type not found");
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
        var userName = $"{user.FirstName} {user.LastName}";
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    WorkedDate = report.WorkedDate,
                    FromDate = report.FromDate,
                    ToDate = report.ToDate,
                    FromDuration = report.FromDuration,
                    ToDuration = report.ToDuration,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus1 = report.ApproverStatus1,
                    ApproverStatus2 = report.ApproverStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    WorkedDate = report.WorkedDate,
                    Credit = report.Credit,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus1 = report.ApproverStatus1,
                    ApproverStatus2 = report.ApproverStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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
                    BranchId = report.BranchId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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
                        DepartmentId = report.DepartmentId,
                        DesignationId = report.DesignationId,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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

            var finalResponse = new
            {
                ReportName = reportName1,
                FromDate = fromDate1,
                ToDate = toDate1,
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
                    ApproverStatus1 = report.ApproverStatus1,
                    ApproverStatus2 = report.ApproverStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    CategoryId = report.CategoryId,
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    PunchType = report.PunchType,
                    InTime = report.InTime,
                    OutTime = report.OutTime,
                    AppliedOn = report.AppliedOn,
                    ApprovalStatus1 = report.ApprovalStatus1,
                    ApprovalStatus2 = report.ApprovalStatus2,
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
                    ApproverStatus1 = report.ApproverStatus1,
                    ApproverStatus2 = report.ApproverStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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
                    SwipeDate = report.SwipeDate,
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
                    ApprovalStatus1 = report.ApprovalStatus1,
                    ApprovalStatus2 = report.ApprovalStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    ApprovalStatus1 = report.ApprovalStatus1,
                    ApprovalStatus2 = report.ApprovalStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    From = report.From,
                    To = report.To,
                    PermissionDate = report.PermissionDate,
                    PermissionType = report.PermissionType,
                    TotalHours = report.TotalHours,
                    Reason = report.Reason,
                    AppliedOn = report.AppliedOn,
                    ApproverStatus1 = report.ApproverStatus1,
                    ApproverStatus2 = report.ApproverStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    CLCredits = report.CLCredits,
                    PLCredits = report.PLCredits,
                    SLCredits = report.SLCredits,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    ShiftId = report.ShiftId,
                    TxnDate = report.TxnDate,
                    DurationOfHoursExtension = report.DurationOfHoursExtension,
                    HoursBeforeShift = report.HoursBeforeShift,
                    HoursAfterShift = report.HoursAfterShift,
                    Remarks = report.Remarks,
                    AppliedOn = report.AppliedOn,
                    ApprovalStatus1 = report.ApprovalStatus1,
                    ApprovalStatus2 = report.ApprovalStatus2,
                    ApprovedOn = report.ApprovedOn,
                    ApprovedBy = report.ApprovedBy,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn,
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    AttendanceDate = report.AttendanceDate,
                    ShiftIn = report.ShiftIn,
                    ShiftOut = report.ShiftOut,
                    ApprovalStatus1 = report.ApprovalStatus1,
                    ApprovalStatus2 = report.ApprovalStatus2,
                    AppliedBy = report.AppliedBy,
                    ApprovedBy = report.ApprovedBy,
                    AppliedOn = report.AppliedOn,
                    ApprovedOn = report.ApprovedOn,
                    IsCancelled = report.IsCancelled,
                    CancelledOn = report.CancelledOn
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
                    DepartmentId = report.DepartmentId,
                    DesignationId = report.DesignationId,
                    VaccinationDate = report.VaccinationDate,
                    SecondVaccinatedDate = report.SecondVaccinatedDate,
                    VaccinationNumber = report.VaccinationNumber,
                    IsExempted = report.IsExempted,
                    Comments = report.Comments,
                    ApprovedOn = report.ApprovedOn,
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

        throw new MessageNotFoundException("Report type not found");
    }
}
