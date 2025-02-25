using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class DepartmentWiseLeaveRequisitionLimit
{
    public string? DepartmentId { get; set; }

    public int? LimitPercentage { get; set; }
}
