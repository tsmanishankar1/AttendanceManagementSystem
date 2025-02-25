using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class TeamHierarchy
{
    public int Id { get; set; }

    public string ReportingManagerId { get; set; } = null!;

    public bool IsActive { get; set; }

    public string StaffId { get; set; } = null!;

    public virtual Staff ReportingManager { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
