using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IWeeklyOffApp
    {
        Task<string> CreateWeeklyOffAsync(WeeklyOffRequest weeklyOffRequest);
        Task<List<WeeklyOffResponse>> GetAllWeeklyOffsAsync();
        Task<string> UpdateWeeklyOffAsync(UpdateWeeklyOff updatedWeeklyOff);
    }
}
