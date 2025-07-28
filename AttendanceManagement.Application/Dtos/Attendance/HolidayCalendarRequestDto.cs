using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class HolidayCalendarRequestDto
    {
        [MaxLength(255)]
        public string GroupName { get; set; } = null!;
        public int CalendarYear { get; set; }
        public bool Currents { get; set; }
        public int ShiftTypeId { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public List<HolidayCalendarTransactionDto>? Transactions { get; set; } = new List<HolidayCalendarTransactionDto>();
    }

    public class UpdateHolidayCalanderDto
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string GroupName { get; set; } = null!;
        public int CalendarYear { get; set; }
        public bool Currents { get; set; }
        public int ShiftTypeId { get; set; }
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