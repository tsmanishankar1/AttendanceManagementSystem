using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AttendanceStatusChange
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? Duration { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
