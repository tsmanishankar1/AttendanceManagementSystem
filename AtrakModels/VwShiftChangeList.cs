using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwShiftChangeList
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? DeptName { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public string? NewShiftId { get; set; }

    public string? ShiftName { get; set; }

    public string? Remarks { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string IsCancelled { get; set; } = null!;

    public string ApproverStatus1 { get; set; } = null!;

    public string ApproverStatus2 { get; set; } = null!;
}
