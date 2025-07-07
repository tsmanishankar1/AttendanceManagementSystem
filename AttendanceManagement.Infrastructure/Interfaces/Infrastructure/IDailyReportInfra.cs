using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IDailyReportInfra
    {
        Task<List<ReportTypeResponse>> GetReportType();
        Task<object> GetDailyReports(DailyReportRequest request);
        Task<string> AddWorkingTypeAmount(WorkingTypeAmountRequest request);
        Task<List<WorkingTypeAmountResponse>> GetWorkingTypeAmount();
        Task<string> UpdateWorkingTypeAmount(UpdateWorkingTypeAmount request);
    }
}
