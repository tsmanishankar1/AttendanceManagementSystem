using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class PrefixSuffixSetting
{
    public int Id { get; set; }

    public string? LeaveTypeId { get; set; }

    public string? PrefixLeaveTypeId { get; set; }

    public string? SuffixLeaveTypeId { get; set; }

    public bool IsActive { get; set; }

    public virtual LeaveType? LeaveType { get; set; }

    public virtual LeaveType? PrefixLeaveType { get; set; }

    public virtual LeaveType? SuffixLeaveType { get; set; }
}
