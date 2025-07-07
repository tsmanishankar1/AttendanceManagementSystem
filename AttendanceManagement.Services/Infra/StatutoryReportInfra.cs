using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class StatutoryReportInfra : IStatutoryReportInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly StoredProcedureDbContext _storedProcedureDbContext;
        public StatutoryReportInfra(AttendanceManagementSystemContext context, StoredProcedureDbContext storedProcedureDbContext)
        {
            _context = context;
            _storedProcedureDbContext = storedProcedureDbContext;
        }

        public async Task<object> GenerateStatutoryReport(StatutoryReportRequest request)
        {
            var fromDate1 = request.FromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            var toDate1 = request.ToDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            var reportDate = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            var user = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == request.CreatedBy && s.IsActive == true);
            if (user == null) throw new MessageNotFoundException("User not found");
            var userId = request.CreatedBy;
            var userName = $"{user.FirstName}{(string.IsNullOrWhiteSpace(user.LastName) ? "" : " " + user.LastName)}";
            var userCreationId = user.StaffId;
            var staffIdList = request.StaffIds ?? new List<int>();
            var staffIdParam = staffIdList.Any() ? string.Join(",", staffIdList) : (object)DBNull.Value;
            var parameters = new[]
            {
                new SqlParameter("@StaffIds", staffIdParam),
                new SqlParameter("@FromDate", request.FromDate),
                new SqlParameter("@ToDate", request.ToDate)
            };
            var reportList = await _storedProcedureDbContext.statutoryReportResponses
                .FromSqlRaw("EXEC usp_GenerateStatutoryReport @StaffIds, @FromDate, @ToDate", parameters)
                .ToListAsync();
            if (reportList.Count == 0) throw new MessageNotFoundException("Record not found");
            var summaries = reportList.Select(report => new StatutoryReportResponse
            {
                StaffId = report.StaffId,
                NoOfDaysInMonth = report.NoOfDaysInMonth,
                Lop = report.Lop,
                NumberOfDaysToBePaid = report.NumberOfDaysToBePaid,
                NightShiftPresentDays = report.NightShiftPresentDays,
                IsAttendanceBonus = report.IsAttendanceBonus,
                AttendanceBonusMonths = report.AttendanceBonusMonths,
                ReportFromDate = report.ReportFromDate,
                ReportToDate = report.ReportToDate
            }).ToList();
            var fromDate = request.FromDate;
            var toDate = request.ToDate;
            var attendanceRecords = await _context.AttendanceRecords
                .Where(ar => staffIdList.Contains(ar.StaffId) && ar.AttendanceDate >= fromDate && ar.AttendanceDate <= toDate)
                .ToListAsync();
            var staffDetailsList = await (from staff in _context.StaffCreations
                                          join dept in _context.DepartmentMasters on staff.DepartmentId equals dept.Id
                                          join desig in _context.DesignationMasters on staff.DesignationId equals desig.Id
                                          where staffIdList.Contains(staff.Id) && staff.IsActive == true && dept.IsActive && desig.IsActive
                                          select new
                                          {
                                              staff.Id,
                                              StaffCreationId = staff.StaffId,
                                              Name = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                                              Department = dept.Name,
                                              Designation = desig.Name
                                          }).ToListAsync();
            var statusDict = await _context.StatusDropdowns.ToDictionaryAsync(s => s.Id, s => s.ShortName);
            var defaultAbsentShortName = await _context.StatusDropdowns
                .Where(s => s.Id == 37 && s.IsActive)
                .Select(s => s.ShortName)
                .FirstOrDefaultAsync() ?? "AB";

            var dateRange = Enumerable.Range(0, toDate.DayNumber - fromDate.DayNumber + 1)
                .Select(offset => fromDate.AddDays(offset))
                .ToList();

            var reports = (from staff in staffDetailsList
                           let summary = summaries.FirstOrDefault(s => s.StaffId == staff.Id)
                           where summary != null
                           let records = attendanceRecords.Where(ar => ar.StaffId == staff.Id).ToList()
                           let dayWiseAttendance = dateRange
                           .Select(date =>
                           {
                               var attendance = records.FirstOrDefault(ar => ar.AttendanceDate == date);
                               var status = attendance != null && statusDict.TryGetValue(attendance.StatusId, out var shortName)
                                   ? shortName
                                   : defaultAbsentShortName;
                               return new KeyValuePair<string, string>(date.ToString("dd-MMM-yy"), status);
                           }).ToList()
                           select new StatutoryReportDto
                           {
                               StaffId = staff.Id,
                               StaffCreationId = staff.StaffCreationId,
                               Name = staff.Name,
                               Department = staff.Department,
                               Designation = staff.Designation,
                               DayWiseAttendance = dayWiseAttendance,
                               NoOfDaysInMonth = summary.NoOfDaysInMonth,
                               Lop = summary.Lop,
                               NumberOfDaysToBePaid = summary.NumberOfDaysToBePaid,
                               NightShiftPresentDays = summary.NightShiftPresentDays,
                               IsAttendanceBonus = summary.IsAttendanceBonus,
                               AttendanceBonusMonths = summary.AttendanceBonusMonths
                           }).ToList();
            if (reports.Count == 0) throw new MessageNotFoundException("No records found");
            var response = new
            {
                FromDate = fromDate1,
                ToDate = toDate1,
                ReportDate = reportDate,
                UserId = userId,
                UserCreationId = userCreationId,
                UserName = userName,
                Records = reports
            };
            return response;
        }
    }
}