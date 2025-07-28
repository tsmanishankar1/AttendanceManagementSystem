using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IPerformanceReviewApp
    {
        Task<List<MonthlyPerformanceResponse>> GetMonthlyPerformance(int year, int month);
        Task<List<QuarterlyPerformanceResponse>> GetQuarterlyPerformance(int year, string quarterType);
        Task<List<YearlyPerformanceResponse>> GetYearlyPerformance(int year);
    }
}
