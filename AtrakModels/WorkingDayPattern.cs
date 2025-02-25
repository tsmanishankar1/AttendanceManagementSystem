using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class WorkingDayPattern
{
    public int Id { get; set; }

    public string? PsCode { get; set; }

    public double WorkingPattern { get; set; }

    public bool IsActive { get; set; }

    public string PatternDesc { get; set; } = null!;

    public virtual ICollection<StaffOfficial> StaffOfficials { get; set; } = new List<StaffOfficial>();
}
