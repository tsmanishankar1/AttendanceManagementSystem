using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class RegularShift
{
    public int Id { get; set; }

    public string ShiftType { get; set; } = null!;

    public bool? WeeklyOffType { get; set; }

    public string? DayPattern { get; set; }

    public DateOnly ChangeEffectFrom { get; set; }

    public string? Reason { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? ShiftId { get; set; }

    public int StaffCreationId { get; set; }

    public string? ShiftPattern { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual Shift? Shift { get; set; }

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
