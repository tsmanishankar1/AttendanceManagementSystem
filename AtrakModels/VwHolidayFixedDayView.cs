using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwHolidayFixedDayView
{
    public int Id { get; set; }

    public int HolidayId { get; set; }

    public DateTime HolidayDateFrom { get; set; }

    public DateTime HolidayDateTo { get; set; }

    public int Isfixed { get; set; }
}
