using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class OnBehalfApplicationApproval
{
    public int Id { get; set; }

    public string Criteria { get; set; } = null!;

    public DateOnly? FromDate { get; set; }

    public int ApplicationTypeId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int StaffCreationId { get; set; }

    public virtual ApplicationType ApplicationType { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
