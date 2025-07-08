using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class StatutoryReportApp : IStatutoryReportApp
    {
        private readonly IStatutoryReportInfra _statutoryReportInfra;
        public StatutoryReportApp(IStatutoryReportInfra statutoryReportInfra)
        {
            _statutoryReportInfra = statutoryReportInfra;
        }

        public async Task<object> GenerateStatutoryReport(StatutoryReportRequest request)
            => await _statutoryReportInfra.GenerateStatutoryReport(request);
    }
}