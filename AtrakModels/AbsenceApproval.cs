using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AbsenceApproval
{
    public int Id { get; set; }

    public string? AbsenceId { get; set; }

    public int ApprovalStatusId { get; set; }

    public string? ApprovedById { get; set; }

    public DateTime ApprovedOn { get; set; }

    public string Comment { get; set; } = null!;

    public virtual LeaveApplication? Absence { get; set; }

    public virtual ApprovalStatus ApprovalStatus { get; set; } = null!;

    public virtual Staff? ApprovedBy { get; set; }
}
