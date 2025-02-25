using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class EducationalCertificate
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public string Degree { get; set; } = null!;

    public string FieldOfStudy { get; set; } = null!;

    public string InstituteName { get; set; } = null!;

    public int YearOfCompletion { get; set; }

    public string? DocumentPath { get; set; }

    public string? DocumentContent { get; set; }

    public bool IsVerified { get; set; }

    public DateTime? VerificationDate { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
