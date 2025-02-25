using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ManualPunch
{
    public string Id { get; set; } = null!;

    public string? StaffId { get; set; }

    public DateTime InDateTime { get; set; }

    public DateTime OutDateTime { get; set; }

    public string Reason { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? PunchType { get; set; }

    public virtual Staff? Staff { get; set; }
}
