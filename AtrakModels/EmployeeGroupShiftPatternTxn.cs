using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmployeeGroupShiftPatternTxn
{
    public long Id { get; set; }

    public string? EmployeeGroupId { get; set; }

    public int ShiftPatternId { get; set; }

    public bool IsActive { get; set; }

    public virtual EmployeeGroup? EmployeeGroup { get; set; }

    public virtual ShiftPattern ShiftPattern { get; set; } = null!;
}
