using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveDuration
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();
}
