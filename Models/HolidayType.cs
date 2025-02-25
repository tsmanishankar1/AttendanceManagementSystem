using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class HolidayType
{
    public int Id { get; set; }

    public string HolidayName { get; set; } = null!;

    public virtual ICollection<HolidayMaster> HolidayMasters { get; set; } = new List<HolidayMaster>();
}
