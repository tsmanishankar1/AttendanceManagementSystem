using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ApprovalStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<AbsenceApproval> AbsenceApprovals { get; set; } = new List<AbsenceApproval>();

    public virtual ICollection<ApplicationApproval> ApplicationApprovals { get; set; } = new List<ApplicationApproval>();
}
