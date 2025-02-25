using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveDebit
{
    public int Id { get; set; }

    public string ParentId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string LeaveTypeId { get; set; } = null!;

    public string LeaveType { get; set; } = null!;

    public decimal TotalDays { get; set; }

    public string Dcflag { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime? ProcessedOn { get; set; }
}
