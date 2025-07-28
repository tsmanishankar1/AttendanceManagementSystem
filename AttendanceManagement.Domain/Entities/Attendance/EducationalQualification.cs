using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class EducationalQualification
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public string Qualification { get; set; } = null!;

    public string Specilization { get; set; } = null!;

    public string University { get; set; } = null!;

    public string Institute { get; set; } = null!;

    public string MediumOfInstruction { get; set; } = null!;

    public string CourseType { get; set; } = null!;

    public int YearOfPassing { get; set; }

    public string CourseAppraisal { get; set; } = null!;

    public int? Score { get; set; }

    public int? OutOf { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
