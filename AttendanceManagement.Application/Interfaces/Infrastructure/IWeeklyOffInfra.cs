using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IWeeklyOffInfra
    {
        Task<string> CreateWeeklyOffAsync(WeeklyOffRequest weeklyOffRequest);
        Task<List<WeeklyOffResponse>> GetAllWeeklyOffsAsync();
        Task<string> UpdateWeeklyOffAsync(UpdateWeeklyOff updatedWeeklyOff);
    }
}
