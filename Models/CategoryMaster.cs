using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class CategoryMaster
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DailyReport> DailyReports { get; set; } = new List<DailyReport>();

    public virtual ICollection<HeadCount> HeadCounts { get; set; } = new List<HeadCount>();

    public virtual ICollection<ManualAttendanceProcessing> ManualAttendanceProcessings { get; set; } = new List<ManualAttendanceProcessing>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual ICollection<StatutoryReport> StatutoryReports { get; set; } = new List<StatutoryReport>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
