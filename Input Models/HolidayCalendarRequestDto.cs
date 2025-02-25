using System;
using System.Collections.Generic;

namespace AttendanceManagement.DTOs
{
    public class HolidayCalendarRequestDto
    {
        public string GroupName { get; set; } = null!;
        public int CalendarYear { get; set; }
        public bool Currents { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public List<HolidayCalendarTransactionDto>? Transactions { get; set; } = new List<HolidayCalendarTransactionDto>();
    }
    public class UpdateHolidayCalanderDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = null!;
        public int CalendarYear { get; set; }
        public bool Currents { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public List<HolidayCalendarTransactionDto>? Transactions { get; set; } = new List<HolidayCalendarTransactionDto>();
    }
    public class HolidayCalendarTransactionDto
    {
        public int HolidayMasterId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
    }
    public class HolidayDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int HolidayMasterId { get; set; }
        public bool IsActive { get; set; }
    }
}

