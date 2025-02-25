using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AttendanceControlTable
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? ApplicationType { get; set; }

    public string? ApplicationId { get; set; }
}
