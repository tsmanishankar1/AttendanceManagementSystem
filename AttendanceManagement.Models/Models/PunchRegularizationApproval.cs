using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class PunchRegularizationApproval
{
    public int Id { get; set; }

    public int AttendanceId { get; set; }

    public int UserManagementId { get; set; }

    public string Name { get; set; } = null!;

    public string PunchType { get; set; } = null!;

    public decimal? InTime { get; set; }

    public decimal? OutTime { get; set; }

    public string Reason { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual Attendance Attendance { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual UserManagement UserManagement { get; set; } = null!;
}
