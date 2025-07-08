using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IDashboardApp
    {
        Task<List<object>> GetTodaysAnniversaries(int eventTypeId);
        Task<List<NewJoinee>> GetNewJoinee();
        Task<List<object>> GetAllHolidaysAsync(int staffId, int shiftTypeId);
        Task<List<object>> GetLeaveDetailsWithDefaultsAsync(int StaffCreationId);
        Task<List<object>> GetHeadCountByDepartmentAsync();
        Task<List<object>> GetUpcomingShiftsForStaffAsync(int staffId);
        Task<string> CreateAnnouncement(AnnouncementDto announcementDto);
        Task<List<AnnouncementResponse>> GetAnnouncement();
        Task<List<AnnouncementResponse>> GetActiveAnnouncement();
        Task<string> UpdateAnnouncement(AnnouncementResponse announcementResponse);
    }
}
