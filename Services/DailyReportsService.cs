using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DailyReportsService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly ILogger<DailyReportsService> _logger;
    private readonly StoredProcedureDbContext _storedProcedureDbContext;

    public DailyReportsService(AttendanceManagementSystemContext context, ILogger<DailyReportsService> logger, StoredProcedureDbContext storedProcedureDbContext)
    {
        _context = context;
        _logger = logger;
        _storedProcedureDbContext = storedProcedureDbContext;
    }

    public async Task<List<object>> GetDailyReports(DailyReportRequest request)
    {
        if (request.DailyReportsId == 10)
        {
            var staffIds = request.StaffIds != null && request.StaffIds.Any()
                ? string.Join(",", request.StaffIds)
                : (object)DBNull.Value;

            var parameters = new[]
            {
            new SqlParameter("@DailyReportsId", request.DailyReportsId),
            new SqlParameter("@StaffIds", staffIds), // Pass StaffIds as a comma-separated string
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
                .FromSqlRaw("EXEC GetLeaveRequisitions @DailyReportsId, @StaffIds, @FromDate, @ToDate, @CurrentMonth, @PreviousMonth, @FromMonth, @ToMonth, @FromDateTime, @ToDateTime, @IncludeTerminated, @TerminatedFrom, @TerminatedTo", parameters)
                .ToListAsync();

            if (!reportList.Any())
            {
                throw new MessageNotFoundException("No records found");
            }

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
                .ToDictionaryAsync(sc => sc.Id, sc => new { sc.OrganizationTypeId, sc.FirstName, sc.LastName, sc.Id });

            // Fetch Organization Type Short Names
            var organizationTypeDict = await _context.OrganizationTypes
                .ToDictionaryAsync(o => o.Id, o => o.ShortName);

            List<DailyReportLeaveRequisitionResponse> result = new List<DailyReportLeaveRequisitionResponse>();

            foreach (var report in reportList)
            {
                // Determine StaffCreationId
                string staffCreationId = "Unknown";
                int lookupId = report.StaffId ?? (int)report.CreatedBy; // Use StaffId if available, otherwise use CreatedBy

                if (staffCreationDict.TryGetValue(lookupId, out var staffInfo))
                {
                    string orgShortName = organizationTypeDict.GetValueOrDefault(staffInfo.OrganizationTypeId, "Unknown");
                    staffCreationId = $"{orgShortName}{staffInfo.Id}";
                }

                // Determine Approver Status
                string approverStatus = report.Status1 switch
                {
                    null => "Pending",
                    true => "Approved",
                    false => "Rejected"
                };

                // Determine Approved By
                string approvedByName = "Pending";
                if (report.UpdatedBy.HasValue && staffCreationDict.TryGetValue(report.UpdatedBy.Value, out var approverInfo))
                {
                    approvedByName = $"{approverInfo.FirstName} {approverInfo.LastName}";
                }

                // Determine Approved On
                string approvedOn = report.UpdatedUtc.HasValue ? report.UpdatedUtc.Value.ToString("yyyy-MM-dd HH:mm:ss") : "Pending";

                // Determine Is Cancelled
                string isCancelled = report.IsCancelled.HasValue && report.IsCancelled.Value ? "Yes" : "No";

                var responseItem = new DailyReportLeaveRequisitionResponse
                {
                    Id = report.Id,
                    StaffId = report.StaffId ?? report.CreatedBy,
                    LeaveTypeId = report.LeaveTypeId,
                    StaffCreationId = staffCreationId, // Updated to include OrganizationTypeShortName
                    LeaveTypeName = leaveTypeDict.TryGetValue(report.LeaveTypeId, out var leaveTypeName) ? leaveTypeName : "Unknown", // Add LeaveTypeName
                    StartDate = report.FromDate,
                    StartDuration = report.StartDuration,
                    EndDuration = report.EndDuration,
                    EndDate = report.ToDate,
                    TotalDays = report.TotalDays,
                    Reason = report.Reason,
                    ApproverStatus = approverStatus, // Updated Approver Status
                  
                    AppliedOn = report.CreatedUtc,
                    ApprovedOn = approvedOn, // Updated Approved On
                    ApprovedBy = approvedByName, // Updated Approved By
                    IsCancelled = isCancelled // Updated Is Cancelled
                };
                result.Add(responseItem);
            }

            return result.Cast<object>().ToList();
        }

        return new List<object>(); // Ensure method always returns a value
    }

}
