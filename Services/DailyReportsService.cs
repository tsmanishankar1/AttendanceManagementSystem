using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
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

    public async Task<List<object>> GetDailyReports(DailyReportRequest request)
    {
        var staffIds = request.StaffIds != null && request.StaffIds.Any() ? string.Join(",", request.StaffIds) : (object)DBNull.Value;

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

        var reportList = await _storedProcedureDbContext.DailyReportResponses
            .FromSqlRaw("EXEC DailyReport @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
            .ToListAsync();

        if (!reportList.Any())
        {
            throw new MessageNotFoundException("No records found");
        }
        var reportName = await _context.TypesOfReports.Where(r => r.Id == request.DailyReportsId).Select(r => r.ReportName).FirstOrDefaultAsync();
        if(reportName == null)
        {
            throw new MessageNotFoundException("Report not found");
        }
        var user = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == request.CreatedBy && s.IsActive == true);
        var userName = "";
        var userCreationId = "";
        if (user != null)
        {
            userName = $"{user.FirstName}{user.LastName}";
            var org = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.Id == user.OrganizationTypeId && o.IsActive);
            userCreationId = $"{org.ShortName}{user.Id}";
        }

        if (request.DailyReportsId == 10)
        {
            // Extract LeaveTypeIds and CreatedBy Ids
            var leaveTypeIds = reportList.Select(r => r.LeaveTypeId).Distinct().ToList();
            var createdByIds = reportList.Select(r => r.CreatedBy).Distinct().ToList();
            var approvedByIds = reportList.Where(r => r.UpdatedBy.HasValue).Select(r => r.UpdatedBy.Value).Distinct().ToList();

            // Fetch LeaveTypeNames
            var leaveTypeDict = await _context.LeaveTypes
                .Where(lt => leaveTypeIds.Contains(lt.Id))
                .ToDictionaryAsync(lt => lt.Id, lt => lt.Name);

            // Fetch Staff Creation Details
            var staffCreationDict = await _context.StaffCreations
                .Where(sc => createdByIds.Contains(sc.CreatedBy) || approvedByIds.Contains(sc.Id))
                .ToDictionaryAsync(sc => sc.Id, sc => new { sc.OrganizationTypeId, sc.FirstName, sc.LastName, sc.Id, sc.DepartmentId, sc.DesignationId });

            // Fetch Organization Type Short Names
            var organizationTypeDict = await _context.OrganizationTypes
                .ToDictionaryAsync(o => o.Id, o => o.ShortName);

            List<DailyReportLeaveRequisitionResponse> result = new List<DailyReportLeaveRequisitionResponse>();

            foreach (var report in reportList)
            {
                // Determine StaffCreationId
                var staffCreationId = "";
                var staffName = "";
                int department = 0;
                int designation = 0;
                int lookupId = report.StaffId ?? report.CreatedBy;
                if (staffCreationDict.TryGetValue(lookupId, out var staffInfo))
                {
                    string orgShortName = organizationTypeDict.GetValueOrDefault(staffInfo.OrganizationTypeId, "");

                    staffCreationId = $"{orgShortName}{staffInfo.Id}";
                    staffName = $"{staffInfo.FirstName}{staffInfo.LastName}";
                    department = staffInfo.DepartmentId;
                    designation = staffInfo.DesignationId;
                }

                string approverStatus = report.Status1 switch
                {
                    null => "Pending",
                    true => "Approved",
                    false => "Rejected"
                };

                string approvedByName = "";
                if (report.UpdatedBy.HasValue && staffCreationDict.TryGetValue(report.UpdatedBy.Value, out var approverInfo))
                {
                    approvedByName = $"{approverInfo.FirstName} {approverInfo.LastName}";
                }

                //string approvedOn = report.UpdatedUtc.HasValue ? report.UpdatedUtc.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";

                // Determine Is Cancelled
                string isCancelled = report.IsCancelled == true ? "Yes" : "No";

                DateOnly fromDate = default, toDate = default;

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
                else if (request.FromDateTime.HasValue && request.ToDateTime.HasValue)
                {
                    fromDate = DateOnly.FromDateTime(request.FromDateTime.Value);
                    toDate = DateOnly.FromDateTime(request.ToDateTime.Value);
                }

                var responseItem = new DailyReportLeaveRequisitionResponse
                {
                    Id = report.Id,
                    ReportName = reportName,
                    StaffId = report.StaffId ?? report.CreatedBy,
                    StaffCreationId = staffCreationId,
                    StaffName = staffName,
                    UserId = request.CreatedBy,
                    UserCreationId = userCreationId,
                    UserName = userName,
                    FromDate = fromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    ToDate = toDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    DepartmentId = department,
                    DesignationId = designation,
                    LeaveTypeId = report.LeaveTypeId,
                    LeaveTypeName = leaveTypeDict.TryGetValue(report.LeaveTypeId, out var leaveTypeName) ? leaveTypeName : "Unknown",
                    StartDate = report.FromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    StartDuration = report.StartDuration,
                    EndDate = report.ToDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    EndDuration = report.EndDuration,
                    TotalDays = report.TotalDays,
                    Reason = report.Reason,
                    AppliedOn = report.CreatedUtc.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    ApproverStatus = approverStatus,
                    ApprovedOn = report.UpdatedUtc.HasValue ? report.UpdatedUtc.Value.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture) : null,
                    ApprovedBy = approvedByName,
                    IsCancelled = isCancelled,
                    CancelledOn = report.CancelledOn.HasValue ? report.CancelledOn.Value.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture) : null,
                    CancelledBy = report.IsCancelled.GetValueOrDefault() ? report.CreatedBy : null,
                    ReportDate = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                };
                result.Add(responseItem);
            }

            var res = result.Cast<object>().ToList();
            if (res.Count == 0)
            {
                return new List<object>
                {
                    new DailyReportLeaveRequisitionResponse
                    {
                        Id = 0,
                        StaffId = 0,
                        StaffCreationId = "",
                        StaffName = "",
                        UserId = request.CreatedBy,
                        UserCreationId = userCreationId,
                        UserName = userName,
                        FromDate = "",
                        ToDate = "",
                        DepartmentId = 0,
                        DesignationId = 0,
                        LeaveTypeId = 0,
                        LeaveTypeName = "",
                        StartDate = "",
                        StartDuration = "",
                        EndDate = "",
                        EndDuration = "",
                        TotalDays = 0,
                        Reason = "",
                        AppliedOn = "",
                        ApproverStatus = "",
                        ApprovedOn = "",
                        ApprovedBy = "",
                        IsCancelled = "",
                        CancelledOn = "",
                        CancelledBy = null,
                        ReportDate = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                    }
                };
            }
            return res;
        }
        throw new MessageNotFoundException("Reports not found");
    }
}
