using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class PerformanceReviewApp : IPerformanceReviewApp
    {
        private readonly IPerformanceReviewInfra _performanceReviewInfra;
        public PerformanceReviewApp(IPerformanceReviewInfra performanceReviewInfra)
        {
            _performanceReviewInfra = performanceReviewInfra;
        }

        public async Task<List<MonthlyPerformanceResponse>> GetMonthlyPerformance(int year, int month)
            => await _performanceReviewInfra.GetMonthlyPerformance(year, month);

        public async Task<List<QuarterlyPerformanceResponse>> GetQuarterlyPerformance(int year, string quarterType)
            => await _performanceReviewInfra.GetQuarterlyPerformance(year, quarterType);

        public async Task<List<YearlyPerformanceResponse>> GetYearlyPerformance(int year)
            => await _performanceReviewInfra.GetYearlyPerformance(year);
    }
}
