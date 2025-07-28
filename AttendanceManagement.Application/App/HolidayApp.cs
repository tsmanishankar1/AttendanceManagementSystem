using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class HolidayApp : IHolidayApp
    {
        private readonly IHolidayInfra _holidayInfra;

        public HolidayApp(IHolidayInfra holidayInfra)
        {
            _holidayInfra = holidayInfra;
        }

        public async Task<string> CreateHoliday(HolidayRequest holidayRequest)
            => await _holidayInfra.CreateHoliday(holidayRequest);

        public async Task<string> CreateHolidayCalendar(HolidayCalendarRequestDto request)
            => await _holidayInfra.CreateHolidayCalendar(request);

        public async Task<string> CreateHolidayZoneAsync(HolidayZoneRequest holidayZoneRequest)
            => await _holidayInfra.CreateHolidayZoneAsync(holidayZoneRequest);

        public async Task<List<HolidayConfigurationResponse>> GetAllHolidayCalendarsAsync()
            => await _holidayInfra.GetAllHolidayCalendarsAsync();

        public async Task<List<HolidayResponse>> GetAllHolidaysAsync()
            => await _holidayInfra.GetAllHolidaysAsync();

        public async Task<List<HolidyTypeRequest>> GetAllHolidayType()
            => await _holidayInfra.GetAllHolidayType();

        public async Task<List<HolidayZoneResponse>> GetAllHolidayZonesAsync()
            => await _holidayInfra.GetAllHolidayZonesAsync();

        public async Task<string> UpdateHoliday(UpdateHoliday updatedHoliday)
            => await _holidayInfra.UpdateHoliday(updatedHoliday);

        public async Task<string> UpdateHolidayCalendar(UpdateHolidayCalanderDto request)
            => await _holidayInfra.UpdateHolidayCalendar(request);

        public async Task<string> UpdateHolidayZoneAsync(UpdateHolidayZone holidayZone)
            => await _holidayInfra.UpdateHolidayZoneAsync(holidayZone);
    }
}