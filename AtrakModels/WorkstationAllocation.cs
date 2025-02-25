using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class WorkstationAllocation
{
    public int Id { get; set; }

    public string Staffid { get; set; } = null!;

    public string WorkstationId { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual Workstation Workstation { get; set; } = null!;
}
