using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class PerformanceUploadType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<MonthlyPerformance> MonthlyPerformances { get; set; } = new List<MonthlyPerformance>();

    public virtual ICollection<PerformanceReport> PerformanceReports { get; set; } = new List<PerformanceReport>();

    public virtual ICollection<QuarterlyPerformance> QuarterlyPerformances { get; set; } = new List<QuarterlyPerformance>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
