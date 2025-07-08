using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;
public class DailyReportsApp : IDailyReportApp
{
    private readonly IDailyReportInfra _dailyReportInfra;
    public DailyReportsApp(IDailyReportInfra dailyReportInfra)
    {
        _dailyReportInfra = dailyReportInfra;

    }

    public async Task<string> AddWorkingTypeAmount(WorkingTypeAmountRequest request)
        => await _dailyReportInfra.AddWorkingTypeAmount(request);

    public async Task<object> GetDailyReports(DailyReportRequest request)
        => await _dailyReportInfra.GetDailyReports(request);

    public async Task<List<ReportTypeResponse>> GetReportType()
        => await _dailyReportInfra.GetReportType();

    public async Task<List<WorkingTypeAmountResponse>> GetWorkingTypeAmount()
        => await _dailyReportInfra.GetWorkingTypeAmount();

    public async Task<string> UpdateWorkingTypeAmount(UpdateWorkingTypeAmount request)
        => await _dailyReportInfra.UpdateWorkingTypeAmount(request);
}