using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class MyApplication
{
    public int Id { get; set; }

    public int UserManagementId { get; set; }

    public int LeaveAvailabilityId { get; set; }

    public int LeaveTypeId { get; set; }

    public decimal TotalDays { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual LeaveAvailability LeaveAvailability { get; set; } = null!;

    public virtual LeaveType LeaveType { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual UserManagement UserManagement { get; set; } = null!;
}
