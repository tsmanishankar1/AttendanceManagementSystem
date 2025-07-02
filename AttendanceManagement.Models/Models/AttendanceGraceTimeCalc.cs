using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class AttendanceGraceTimeCalc
{
    public int Id { get; set; }

    public int GraceTimeId { get; set; }

    public int Value { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual GraceTimeDropdown GraceTime { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
