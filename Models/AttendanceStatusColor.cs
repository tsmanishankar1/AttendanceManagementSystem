using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class AttendanceStatusColor
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string? ColourCode { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
