using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class DepartmentMaster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public long Phone { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<HeadCount> HeadCounts { get; set; } = new List<HeadCount>();

    public virtual ICollection<PaySheet> PaySheets { get; set; } = new List<PaySheet>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
