using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class OrganizationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public string ShortName { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<AssignLeaveType> AssignLeaveTypes { get; set; } = new List<AssignLeaveType>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
