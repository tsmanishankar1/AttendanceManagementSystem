using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ReimbursementType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
