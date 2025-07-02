using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IDailyReport
    {
        Task<List<ReportTypeResponse>> GetReportType();
        Task<object> GetDailyReports(DailyReportRequest request);
        Task<string> AddWorkingTypeAmount(WorkingTypeAmountRequest request);
        Task<List<WorkingTypeAmountResponse>> GetWorkingTypeAmount();
        Task<string> UpdateWorkingTypeAmount(UpdateWorkingTypeAmount request);
    }
}
