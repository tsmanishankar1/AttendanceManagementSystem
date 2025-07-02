using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IPerformanceReview
    {
        Task<List<MonthlyPerformanceResponse>> GetMonthlyPerformance(int year, int month);
        Task<List<QuarterlyPerformanceResponse>> GetQuarterlyPerformance(int year, string quarterType);
        Task<List<YearlyPerformanceResponse>> GetYearlyPerformance(int year);
    }
}
