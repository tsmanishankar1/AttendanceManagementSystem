using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class FamilyDetail
{
    public int Id { get; set; }

    public string MemberName { get; set; } = null!;

    public string Relationship { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public decimal? IncomePerAnnum { get; set; }

    public string? Occupation { get; set; }

    public bool NomineeForPf { get; set; }

    public decimal? PfsharePercentage { get; set; }

    public bool NomineeForGratuity { get; set; }

    public decimal? GratuitySharePercentage { get; set; }

    public int StaffCreationId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
