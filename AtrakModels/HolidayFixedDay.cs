using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class HolidayFixedDay
{
    public int Id { get; set; }

    public int HolidayId { get; set; }

    public DateTime HolidayDateFrom { get; set; }

    public DateTime HolidayDateTo { get; set; }

    public virtual Holiday Holiday { get; set; } = null!;
}
