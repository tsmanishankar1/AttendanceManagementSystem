using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class AttendanceStatus
{
    public int Id { get; set; }

    public int UserManagementId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string Duration { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual UserManagement UserManagement { get; set; } = null!;
}
