using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ProfessionalTaxSlab
{
    public int Id { get; set; }

    public decimal MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public decimal Amount { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
