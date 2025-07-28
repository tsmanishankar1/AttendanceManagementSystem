using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IStatutoryReportInfra
    {
        Task<object> GenerateStatutoryReport(StatutoryReportRequest request);
    }
}
