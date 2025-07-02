using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ComplianceDocument
{
    public int Id { get; set; }

    public string DocumentName { get; set; } = null!;

    public string DocumentType { get; set; } = null!;

    public int? StaffCreationId { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? IssuedBy { get; set; }

    public string DocumentPath { get; set; } = null!;

    public string Status { get; set; } = null!;

    public bool IsMandatory { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? StaffCreation { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
