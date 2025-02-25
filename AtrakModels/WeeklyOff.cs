using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class WeeklyOff
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Settings { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
