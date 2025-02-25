using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmployeeGroup
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeGroupShiftPatternTxn> EmployeeGroupShiftPatternTxns { get; set; } = new List<EmployeeGroupShiftPatternTxn>();

    public virtual ICollection<EmployeeGroupTxn> EmployeeGroupTxns { get; set; } = new List<EmployeeGroupTxn>();
}
