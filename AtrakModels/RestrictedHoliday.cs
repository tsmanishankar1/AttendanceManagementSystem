using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class RestrictedHoliday
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Rhdate { get; set; }

    public int Rhyear { get; set; }

    public string CompanyId { get; set; } = null!;

    public string LeaveId { get; set; } = null!;

    public DateTime ImportDate { get; set; }

    public string ImportedBy { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual LeaveType Leave { get; set; } = null!;
}
