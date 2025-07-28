using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class DashboardApp : IDashboardApp
    {
        private readonly IDashboardInfra _dashboardInfra;

        public DashboardApp(IDashboardInfra dashboardInfra)
        {
            _dashboardInfra = dashboardInfra;
        }

        public async Task<string> CreateAnnouncement(AnnouncementDto announcementDto)
            => await _dashboardInfra.CreateAnnouncement(announcementDto);

        public async Task<List<AnnouncementResponse>> GetActiveAnnouncement()
            => await _dashboardInfra.GetActiveAnnouncement();

        public async Task<List<object>> GetAllHolidaysAsync(int staffId, int shiftTypeId)
            => await _dashboardInfra.GetAllHolidaysAsync(staffId, shiftTypeId);

        public async Task<List<AnnouncementResponse>> GetAnnouncement()
            => await _dashboardInfra.GetAnnouncement();

        public async Task<List<object>> GetHeadCountByDepartmentAsync()
            => await _dashboardInfra.GetHeadCountByDepartmentAsync();

        public async Task<List<object>> GetLeaveDetailsWithDefaultsAsync(int StaffCreationId)
            => await _dashboardInfra.GetLeaveDetailsWithDefaultsAsync(StaffCreationId);

        public async Task<List<NewJoinee>> GetNewJoinee()
            => await _dashboardInfra.GetNewJoinee();

        public async Task<List<object>> GetTodaysAnniversaries(int eventTypeId)
            => await _dashboardInfra.GetTodaysAnniversaries(eventTypeId);

        public async Task<List<object>> GetUpcomingShiftsForStaffAsync(int staffId)
            => await _dashboardInfra.GetUpcomingShiftsForStaffAsync(staffId);

        public async Task<string> UpdateAnnouncement(AnnouncementResponse announcementResponse)
            => await _dashboardInfra.UpdateAnnouncement(announcementResponse);
    }
}