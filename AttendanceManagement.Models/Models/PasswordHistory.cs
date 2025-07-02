using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class PasswordHistory
{
    public int Id { get; set; }

    public string OldPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual UserManagement CreatedByNavigation { get; set; } = null!;

    public virtual UserManagement? UpdatedByNavigation { get; set; }
}
