using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AssignDepartment
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string NewDepartmentId { get; set; } = null!;

    public DateTime? EffectFromDate { get; set; }

    public DateTime? EffectToDate { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Department NewDepartment { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
