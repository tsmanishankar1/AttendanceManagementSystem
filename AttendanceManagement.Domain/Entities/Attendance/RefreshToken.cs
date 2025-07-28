using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation? CreatedByNavigation { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual StaffCreation User { get; set; } = null!;
}
