using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ApplicationApproval
{
    public string Id { get; set; } = null!;

    public string ParentId { get; set; } = null!;

    public int ApprovalStatusId { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public string? Comment { get; set; }

    public string ApprovalOwner { get; set; } = null!;

    public string ParentType { get; set; } = null!;

    public int ForwardCounter { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string? Approval2Owner { get; set; }

    public int Approval2statusId { get; set; }

    public DateTime? Approval2On { get; set; }

    public string? Approval2By { get; set; }

    public virtual ApprovalStatus ApprovalStatus { get; set; } = null!;
}
