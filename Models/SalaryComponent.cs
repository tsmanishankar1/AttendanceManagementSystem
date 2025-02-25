using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class SalaryComponent
{
    public int Id { get; set; }

    public string ComponentName { get; set; } = null!;

    public int ComponentTypeId { get; set; }

    public bool IsTaxable { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual SalaryComponentType ComponentType { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<StaffSalaryBreakdown> StaffSalaryBreakdowns { get; set; } = new List<StaffSalaryBreakdown>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
