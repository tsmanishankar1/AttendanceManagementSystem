using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class Shift
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string StartTime { get; set; } = null!;

    public string EndTime { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public string ShortName { get; set; } = null!;

    public int ShiftTypeId { get; set; }

    public int? DivisionId { get; set; }

    public virtual ICollection<AssignShift> AssignShifts { get; set; } = new List<AssignShift>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual DivisionMaster? Division { get; set; }

    public virtual ICollection<HeadCount> HeadCounts { get; set; } = new List<HeadCount>();

    public virtual ICollection<RegularShift> RegularShifts { get; set; } = new List<RegularShift>();

    public virtual ICollection<ShiftChange> ShiftChanges { get; set; } = new List<ShiftChange>();

    public virtual ICollection<ShiftExtension> ShiftExtensions { get; set; } = new List<ShiftExtension>();

    public virtual ShiftTypeDropDown ShiftType { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
