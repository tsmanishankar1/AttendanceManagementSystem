using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Workstation
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public virtual ICollection<WorkstationAllocation> WorkstationAllocations { get; set; } = new List<WorkstationAllocation>();
}
