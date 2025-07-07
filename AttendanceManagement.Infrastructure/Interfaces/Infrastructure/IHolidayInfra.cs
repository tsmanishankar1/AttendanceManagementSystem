using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IHolidayInfra
    {
        Task<string> CreateHoliday(HolidayRequest holidayRequest);
        Task<List<HolidyTypeRequest>> GetAllHolidayType();
        Task<List<HolidayResponse>> GetAllHolidaysAsync();
        Task<string> UpdateHoliday(UpdateHoliday updatedHoliday);
        Task<string> CreateHolidayCalendar(HolidayCalendarRequestDto request);
        Task<List<HolidayConfigurationResponse>> GetAllHolidayCalendarsAsync();
        Task<string> UpdateHolidayCalendar(UpdateHolidayCalanderDto request);
        Task<List<HolidayZoneResponse>> GetAllHolidayZonesAsync();
        Task<string> CreateHolidayZoneAsync(HolidayZoneRequest holidayZoneRequest);
        Task<string> UpdateHolidayZoneAsync(UpdateHolidayZone holidayZone);
    }
}
