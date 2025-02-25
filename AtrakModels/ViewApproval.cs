using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ViewApproval
{
    public int Id { get; set; }

    public string? ApproverId { get; set; }

    public string? LeaveApplicationId { get; set; }

    public bool Viewable { get; set; }

    public virtual Staff? Approver { get; set; }

    public virtual LeaveApplication? LeaveApplication { get; set; }
}
