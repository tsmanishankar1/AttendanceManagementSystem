using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int UserManagementId { get; set; }

    public int DepartmentId { get; set; }

    public decimal PunchInTime { get; set; }

    public decimal PunchOutTime { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual DepartmentMaster Department { get; set; } = null!;

    public virtual ICollection<PunchRegularizationApproval> PunchRegularizationApprovals { get; set; } = new List<PunchRegularizationApproval>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual UserManagement UserManagement { get; set; } = null!;
}
