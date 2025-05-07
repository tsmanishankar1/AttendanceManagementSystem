using AttendanceManagement.DTOs;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class HolidayConfigurationResponse
    {
        public int HolidayCalendarId { get; set; }
        [MaxLength(255)]
        public string GroupName { get; set; } = null!;
        public int CalendarYear { get; set; }
        public bool Currents { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public List<HolidayCalendarTransactionDto> Transactions { get; set; } = new List<HolidayCalendarTransactionDto>();
    }

    public class HolidayZoneResponse
    {
        public int HolidayZoneId { get; set; }
        public string HolidayZoneName { get; set; } = null!;
        public int HolidayCalanderId { get; set; }
        public string HolidayCalendarName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class HolidayZoneRequest
    {
        [MaxLength(255)]
        public string HolidayZoneName { get; set; } = null!;
        public int HolidayCalendarId { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateHolidayZone
    {
        public int HolidayZoneId { get; set; }
        public int HolidayCalendarId {  get; set; }
        [MaxLength(255)]
        public string HolidayZoneName { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}