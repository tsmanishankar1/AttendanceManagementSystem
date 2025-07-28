using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IStatutoryReportApp
    {
        Task<object> GenerateStatutoryReport(StatutoryReportRequest request);
    }
}
