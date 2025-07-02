using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IStatutoryReport
    {
        Task<object> GenerateStatutoryReport(StatutoryReportRequest request);
    }
}
