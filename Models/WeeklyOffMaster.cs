using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class WeeklyOffMaster
{
    public int Id { get; set; }

    public string WeeklyOffName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual ICollection<WeeklyOffDetail> WeeklyOffDetails { get; set; } = new List<WeeklyOffDetail>();
}
