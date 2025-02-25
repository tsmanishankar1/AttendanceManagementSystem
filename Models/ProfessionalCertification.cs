using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ProfessionalCertification
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public string? CertificationName { get; set; }

    public string? IssuingOrganization { get; set; }

    public string? CertificationCode { get; set; }

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public string? CertificationStatus { get; set; }

    public string? CertificationType { get; set; }

    public DateOnly? RenewalDate { get; set; }

    public string? RenewalStatus { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
