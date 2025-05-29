using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class PerformanceReviewCycle
{
    public int Id { get; set; }

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<PerformanceReviewEmployee> PerformanceReviewEmployees { get; set; } = new List<PerformanceReviewEmployee>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
