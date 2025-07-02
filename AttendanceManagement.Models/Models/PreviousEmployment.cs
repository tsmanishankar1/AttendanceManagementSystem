using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class PreviousEmployment
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public string? CompanyName { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? PreviousLocation { get; set; }

    public string? FunctionalArea { get; set; }

    public decimal? LastGrossSalary { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
