using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class Hrgoal
{
    public int GoalId { get; set; }

    public string GoalName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<StaffKpi> StaffKpis { get; set; } = new List<StaffKpi>();
}
