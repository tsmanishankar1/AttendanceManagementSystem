using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class LeaveGroupTransaction
{
    public int Id { get; set; }

    public int LeaveTypeId { get; set; }

    public int LeaveGroupId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual LeaveGroup LeaveGroup { get; set; } = null!;

    public virtual LeaveType LeaveType { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
