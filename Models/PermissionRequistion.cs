using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class PermissionRequistion
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public int ApplicationTypeId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string PermissionType { get; set; } = null!;

    public bool? Status1 { get; set; }

    public bool? Status2 { get; set; }

    public string? Remarks { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public bool? IsEmailSent { get; set; }

    public virtual ApplicationType ApplicationType { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
