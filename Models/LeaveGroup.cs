using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class LeaveGroup
{
    public int Id { get; set; }

    public string LeaveGroupName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<LeaveGroupTransaction> LeaveGroupTransactions { get; set; } = new List<LeaveGroupTransaction>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
