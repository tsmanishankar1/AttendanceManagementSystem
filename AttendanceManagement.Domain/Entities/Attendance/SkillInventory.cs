using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class SkillInventory
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public string Name { get; set; } = null!;

    public int LevelOfProficiency { get; set; }

    public string Notes { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
