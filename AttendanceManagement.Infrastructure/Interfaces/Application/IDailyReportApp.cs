using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IDailyReportApp
    {
        Task<List<ReportTypeResponse>> GetReportType();
        Task<object> GetDailyReports(DailyReportRequest request);
        Task<string> AddWorkingTypeAmount(WorkingTypeAmountRequest request);
        Task<List<WorkingTypeAmountResponse>> GetWorkingTypeAmount();
        Task<string> UpdateWorkingTypeAmount(UpdateWorkingTypeAmount request);
    }
}
