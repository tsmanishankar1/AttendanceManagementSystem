using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class WeeklyOffApp : IWeeklyOffApp
    {
        private readonly IWeeklyOffInfra _weeklyOffInfra;
        public WeeklyOffApp(IWeeklyOffInfra weeklyOffInfra)
        {
            _weeklyOffInfra = weeklyOffInfra;
        }

        public async Task<string> CreateWeeklyOffAsync(WeeklyOffRequest weeklyOffRequest)
            => await _weeklyOffInfra.CreateWeeklyOffAsync(weeklyOffRequest);

        public async Task<List<WeeklyOffResponse>> GetAllWeeklyOffsAsync()
            => await _weeklyOffInfra.GetAllWeeklyOffsAsync();

        public async Task<string> UpdateWeeklyOffAsync(UpdateWeeklyOff updatedWeeklyOff)
            => await _weeklyOffInfra.UpdateWeeklyOffAsync(updatedWeeklyOff);
    }
}