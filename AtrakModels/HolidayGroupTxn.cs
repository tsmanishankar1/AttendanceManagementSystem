using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class HolidayGroupTxn
{
    public long Id { get; set; }

    public string? HolidayGroupId { get; set; }

    public int HolidayId { get; set; }

    public DateTime HolidayDateFrom { get; set; }

    public DateTime HolidayDateTo { get; set; }

    public bool IsActive { get; set; }

    public virtual Holiday Holiday { get; set; } = null!;

    public virtual HolidayGroup? HolidayGroup { get; set; }
}
