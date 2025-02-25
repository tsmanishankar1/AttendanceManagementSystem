using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveCreditDebitReason
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeLeaveAccount> EmployeeLeaveAccounts { get; set; } = new List<EmployeeLeaveAccount>();
}
