using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveApplicationDetail
{
    public string? ApplicationId { get; set; }

    public string? StaffId { get; set; }

    public DateOnly? LeaveDate { get; set; }

    public bool? IsApproved { get; set; }

    public bool? IsRejected { get; set; }

    public bool? IsCancelled { get; set; }
}
