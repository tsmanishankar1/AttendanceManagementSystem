using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class EmploymentHistory
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string JobTitle { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? LastDrawnSalary { get; set; }

    public string? JobLocation { get; set; }

    public string EmploymentType { get; set; } = null!;

    public string? ReasonForLeaving { get; set; }

    public string? ReferenceContact { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
