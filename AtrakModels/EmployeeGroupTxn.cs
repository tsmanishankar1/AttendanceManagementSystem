using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmployeeGroupTxn
{
    public string Id { get; set; } = null!;

    public string? EmployeeGroupId { get; set; }

    public string? StaffId { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public string? LastUpdatedShiftId { get; set; }

    public bool IsActive { get; set; }

    public virtual EmployeeGroup? EmployeeGroup { get; set; }

    public virtual Staff? Staff { get; set; }
}
